using System.ComponentModel.DataAnnotations;
using CollectibleCardsTradingShopProject.Models;

namespace CollectibleCardsTradingShopProject.ViewModels.Users
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; } = null!;
        public string? UserName { get; set; }
        [EmailAddress(ErrorMessage = "Incorrect address")]
        public string? Email { get; set; }
        public string? RoleName { get; set; }
        public virtual ICollection<UserCardViewModel> Cards { get; set; } = new List<UserCardViewModel>();
    }
}
