using CollectibleCardsTradingShopProject.Models;

namespace CollectibleCardsTradingShopProject.ViewModels
{
    public class CardsViewModel
    {
        public IEnumerable<Card> Cards { get; set; } = new List<Card>();
        public string Franchise { get; set; } = null!;
        public string Rarity { get; set; } = null!;
        public PageViewModel PageViewModel { get; set; } = null!;
        public SortViewModel SortViewModel { get; set; } = null!;
    }
}
