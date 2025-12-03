using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Service.Abstraction;
using ECommerce.Service.Specification;
using ECommerce.Shared.CommonResult;
using ECommerce.Shared.DTOS.OrderDTOs;

namespace ECommerce.Service
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;

        public OrderService(IMapper mapper, IUnitOfWork unitOfWork, IBasketRepository basketRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
        }

        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email)
        {
            // Goal => Create Order

            // 1- Map From AddressDTO to Address Entity
            var orderAddress = _mapper.Map<OrderAddress>(orderDTO.Address);

            // 2- Get Basket By Id From Basket Repository
            var basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId);
            if (basket == null)
                return Error.NotFound("Basket not found");


            // Payment
            ArgumentNullException.ThrowIfNullOrEmpty(basket.PaymentIntentId);
            var OrderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var Spec = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var ExistOrder = await OrderRepo.GetByIdAsync(Spec);
            if (ExistOrder is not null) OrderRepo.Remove(ExistOrder);


            // 3- Get Order Items From Basket Items
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (product is null)
                    return Error.NotFound($"Product with id {item.Id} not found");
                
                orderItems.Add(CreateOrderItem(item, product));
            }

            // 4- Get Delivery Method By Id
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);
            if (deliveryMethod is null)
                return Error.NotFound($"Delivery Method with id {orderDTO.DeliveryMethodId} not found");

            // 5- Calculate Subtotal
            var subtotal = orderItems.Sum(I => I.Price * I.Quantity);

            // 6- Create Order Entity
            var order = new Order()
            {
                Address = orderAddress,
                DeliveryMethod = deliveryMethod,
                Items = orderItems,
                SubTotal = subtotal,
                UserEmail = email,
                PaymentIntentId = basket.PaymentIntentId
            };

            // 7- Save Order To Database
            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(order);

            int Result = await _unitOfWork.SaveChangesAsync();
            if (Result == 0)
                return Error.Failure("Problem occurred while creating order");

            // 8- Return OrderToReturnDTO
            return _mapper.Map<OrderToReturnDTO>(order);
        }

        private static OrderItem CreateOrderItem(BasketItem item, Product? product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOrdered()
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    PictureUrl = product.PictureUrl
                },
                Price = product.Price,
                Quantity = item.Quantity
            };
        }

        public async Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string email)
        {
            var Spec = new OrderSpecification(email);

            var Orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(Spec);
            if (!Orders.Any())
                return Error.NotFound("No orders found for this user");

            var Data = _mapper.Map<IEnumerable<OrderToReturnDTO>>(Orders);
            return Result<IEnumerable<OrderToReturnDTO>>.Ok(Data);
        }
        
        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var DeliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync();
            if (!DeliveryMethods.Any())
                return Error.NotFound("No delivery methods found");

            var Data = _mapper.Map<IEnumerable<DeliveryMethod>, IEnumerable<DeliveryMethodDTO>>(DeliveryMethods);

            return Result<IEnumerable<DeliveryMethodDTO>>.Ok(Data);
        }

        public async Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid id, string email)
        {
            var Spec = new OrderSpecification(id, email);

            var Order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(Spec);
            if (Order is null)
                return Error.NotFound("Order not found");

            return _mapper.Map<OrderToReturnDTO>(Order);
        }
    }
}
