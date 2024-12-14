namespace CollectibleCardsTradingShopProject.Models
{
    public class UserCard
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public int CardId { get; set; }
        public int Quantity { get; set; }
        public virtual User? User { get; set; }
        public virtual Card? Card { get; set; }
    }
}
