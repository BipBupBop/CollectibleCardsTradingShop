namespace CollectibleCardsTradingShopProject.Models
{
    public class UserLot
    {
        public int Id { get; set; }
        public bool DidCloseTheLot { get; set; }
        public string UserId { get; set; } = null!;
        public int LotId { get; set; }
        public virtual User? User { get; set; }
        public virtual Lot? Lot { get; set; }
    }
}
