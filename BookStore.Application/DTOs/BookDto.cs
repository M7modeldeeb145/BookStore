namespace BookStore.Application.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public int WaitingListCount { get; set; }
    }
    public class CreateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public int TotalCopies { get; set; }
    }
}
