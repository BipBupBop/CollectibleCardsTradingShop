namespace CollectibleCardsTradingShopProject.ViewModels
{
    public enum SortState
    {
        No,
        NameAsc,
        NameDesc,
        FranchiseAsc,
        FranchiseDesc,
        RarityAsc,
        RarityDesc,
    }
    public class SortViewModel
    {
        public SortState FranchiseSort { get; set; }
        public SortState RaritySort { get; set; }
        public SortState NameSort { get; set; }
        public SortState CurrentState { get; set; }

        public SortViewModel(SortState sortOrder)
        {
            FranchiseSort = sortOrder == SortState.FranchiseAsc ? SortState.FranchiseDesc : SortState.FranchiseAsc;
            RaritySort = sortOrder == SortState.RarityAsc ? SortState.RarityDesc : SortState.RarityAsc;
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            CurrentState = sortOrder;
        }
    }
}
