using Microsoft.AspNetCore.Identity;

namespace CollectibleCardsTradingShopProject.Models
{
    public class User : IdentityUser
    {
        public virtual ICollection<Lot> Lots { get; set; } = new List<Lot>();
    }
}
