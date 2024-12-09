using System;
using System.ComponentModel.DataAnnotations;

namespace CollectibleCardsTradingShopProject.ViewModels.Users
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Login")]
        public string UserName { get; set; }
        [EmailAddress(ErrorMessage = "Incorrect address")]
        public string Email { get; set; }
        [Display(Name = "Role")]
        public string RoleName { get; set; }

    }
}
