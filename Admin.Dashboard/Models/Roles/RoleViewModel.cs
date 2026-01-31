using System.ComponentModel.DataAnnotations;

namespace Admin.Dashboard.Models.Roles
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Role name is required")]
        [StringLength(100, ErrorMessage = "Role name cannot exceed 100 characters")]
        public string Name { get; set; }
    }
}
