using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace inventoryAppWebUi.Models
{
    public class UpdateUserRoleViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string UpdatedUserRole { get; set; }
        [Required]
        public string Email { get; set; }
        public List<String> Roles { get; set; }
    }
}