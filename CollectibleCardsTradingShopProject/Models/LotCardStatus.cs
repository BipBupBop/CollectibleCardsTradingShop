namespace CollectibleCardsTradingShopProject.Models
{
    public class LotCardStatus
    {
        public int Id { get; set; }
        public string Status { get; set; } = null!;
        public virtual ICollection<CardInLot> CardInLots { get; set;} = new List<CardInLot>();
    }
}