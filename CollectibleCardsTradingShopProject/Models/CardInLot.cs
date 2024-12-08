namespace CollectibleCardsTradingShopProject.Models
{
    public class CardInLot
    {
        public int Id { get; set; }
        public int CardId { get; set; }
        public int LotCardStatusId { get; set; }
        public int LotId { get; set; }
        public virtual Card? Card { get; set; }
        public virtual LotCardStatus? LotCardStatus { get; set; }
        public virtual Lot? Lot { get; set; }
    }
}