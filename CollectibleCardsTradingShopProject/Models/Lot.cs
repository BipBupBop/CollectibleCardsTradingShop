namespace CollectibleCardsTradingShopProject.Models
{
    public class Lot
    {
        public int Id { get; set; }
        public virtual ICollection<CardInLot> CardInLot { get; set;} = new List<CardInLot>();
        public virtual ICollection<UserLot> UsersLot { get; set; } = new List<UserLot>();
    }
}
