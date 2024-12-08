namespace CollectibleCardsTradingShopProject.Models
{
    public class Lot
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? ClosedUserId { get; set; }
        public virtual ICollection<CardInLot> CardInLot { get; set;} = new List<CardInLot>();
    }
}
