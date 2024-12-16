using System.Collections.Generic;

namespace CollectibleCardsTradingShopProject.ViewModels
{
    public class LotFilterViewModel
    {
        public string Franchise { get; set; } = null!;
        public string CardName { get; set; } = null!;
        public string Rarity { get; set; } = null!;
        public bool SeekOpened { get; set; }
        public virtual PageViewModel PageViewModel { get; set; } = null!;
        public List<LotViewModel> Lots { get; set; } = new List<LotViewModel>();
    }
}
