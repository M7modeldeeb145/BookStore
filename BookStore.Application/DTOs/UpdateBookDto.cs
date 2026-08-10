namespace BookStore.Application.Dtos
{
    public class UpdateBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int TotalCopies { get; set; }
    }
}
