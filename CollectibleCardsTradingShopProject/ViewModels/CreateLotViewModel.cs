using CollectibleCardsTradingShopProject.ViewModels.Users;
using CollectibleCardsTradingShopProject.Models;
using System;
using System.Collections.Generic;

namespace CollectibleCardsTradingShopProject.ViewModels
{
    public class CreateLotViewModel
    {
        public List<UserCardViewModel> UserCards { get; set; } = new List<UserCardViewModel>();
        public List<Card> OfferedCards { get; set; } = new List<Card>();

        public List<Card> WantedCards { get; set; } = new List<Card>();

        public List<Card> RequiredCards { get; set; } = new List<Card>();
    }

    public enum LotCardStatusEnum
    {
        Offered = 1,
        Wanted = 2,
        Required = 3
    }
}

