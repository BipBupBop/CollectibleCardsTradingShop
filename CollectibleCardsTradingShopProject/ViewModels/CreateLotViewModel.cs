using Microsoft.AspNetCore.Mvc.Rendering;

namespace CollectibleCardsTradingShopProject.ViewModels
{
    public class CreateLotViewModel
    {
        public int LotId { get; set; }
        public List<SelectListItem> Cards { get; set; }
        public List<SelectListItem> Statuses { get; set; }
        public List<int> SelectedCards { get; set; }
        public List<string> SelectedStatuses { get; set; }
    }
}
