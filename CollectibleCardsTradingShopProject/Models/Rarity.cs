namespace CollectibleCardsTradingShopProject.Models
{
    public class Rarity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Card> Cards { get; set; } = new List<Card>();
    }
}