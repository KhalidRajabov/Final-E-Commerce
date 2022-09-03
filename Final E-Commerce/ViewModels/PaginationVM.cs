namespace Final_E_Commerce.ViewModels
{
    public class PaginationVM<T>
    {
        public List<T> Items { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }

        public PaginationVM(List<T> items, int pagecount, int currentpage)
        {
            Items = items;
            PageCount = pagecount;
            CurrentPage = currentpage;
        }
    }
}
