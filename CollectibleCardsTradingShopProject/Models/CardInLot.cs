namespace CollectibleCardsTradingShopProject.Models
{
    public class CardInLot
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int LotCardStatusId { get; set; }
        public virtual Card? Card { get; set; }
        public virtual LotCardStatus? LotCardStatus { get; set; }
    }
}