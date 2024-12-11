using Microsoft.AspNetCore.Identity;

namespace CollectibleCardsTradingShopProject.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<UserLot> UserLots { get; set; } = new List<UserLot>();
        public virtual ICollection<UserCard> UserCards { get; set; } = new List<UserCard>();
    }
}
