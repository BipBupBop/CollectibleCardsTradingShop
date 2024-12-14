using CollectibleCardsTradingShopProject.Models;

namespace CollectibleCardsTradingShopProject.ViewModels
{
    public class LotViewModel
    {
        public int LotId { get; set; }
        public string OpenedByUserName { get; set; } = null!;
        public string OpenedByEmail { get; set; } = null!;

        public List<CardInfo> Cards { get; set; } = new List<CardInfo>();

        public string? ClosedByUserName { get; set; }
        public string? ClosedByEmail { get; set; }

        public string CardsSummary { get; set; } = null!;

        public class CardInfo
        {
            public string Name { get; set; } = null!;
            public string Image { get; set; } = null!;
            public string Franchise { get; set; } = null!;
            public string Rarity { get; set; } = null!;
            public string Status { get; set; } = null!;
            public int Quantity { get; set; }
        }
    }
}
