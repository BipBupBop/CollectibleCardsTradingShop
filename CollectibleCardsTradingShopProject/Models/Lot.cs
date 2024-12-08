namespace CollectibleCardsTradingShopProject.Models
{
    public class Lot
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string? ClosedUserId { get; set; }
        public virtual User? User { get; set; }
        public virtual User? ClosedUser { get; set; }
        public virtual ICollection<CardInLot> CardInLot { get; set;} = new List<CardInLot>();
    }
}
