using CollectibleCardsTradingShopProject.ViewModels.Users;
using CollectibleCardsTradingShopProject.Models;
using System;
using System.Collections.Generic;

namespace CollectibleCardsTradingShopProject.ViewModels.CreateLot
{
    public class LotCardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Franchise { get; set; }
        public string Rarity { get; set; }
        public int Quantity { get; set; }
    }

    public class LotCreationViewModel
    {
        public PaginatedList<LotCardViewModel> UserCards { get; set; }
        public PaginatedList<LotCardViewModel> DatabaseCards { get; set; }
        public List<LotCardViewModel> OfferedCards { get; set; } = new List<LotCardViewModel>();
        public List<LotCardViewModel> RequiredCards { get; set; } = new List<LotCardViewModel>();

        public string UserSearchQuery { get; set; }      // Поиск для карт пользователя
        public string RequiredSearchQuery { get; set; }  // Поиск для требуемых карт
        public string DatabaseSearchQuery { get; set; }  // Поиск для карт из базы
    }


}

