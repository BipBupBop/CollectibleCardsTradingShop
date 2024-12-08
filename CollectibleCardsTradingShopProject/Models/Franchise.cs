namespace CollectibleCardsTradingShopProject.Models
{
    public class Franchise
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}