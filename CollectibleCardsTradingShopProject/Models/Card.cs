namespace CollectibleCardsTradingShopProject.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int FranchiseId { get; set; }
        public int RarityId { get; set; }        
        public virtual Rarity? Rarity { get; set; }
        public virtual Franchise? Franchise { get; set; }
        public virtual ICollection<CardInLot> CardInLots { get; set; } = new List<CardInLot>();
        public virtual ICollection<UserCard> UserCards { get; set; } = new List<UserCard>();
    }
}
