using System.ComponentModel.DataAnnotations;
using CollectibleCardsTradingShopProject.Models;

namespace CollectibleCardsTradingShopProject.ViewModels
{
    public class CardInLotViewModel
    {
        [Required]
        public int CardId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity needs to be more than 0")]
        public int Quantity { get; set; }

        [Required]
        public LotCardStatusEnum Status { get; set; }

        public virtual Card? Card { get; set; }
    }
}

