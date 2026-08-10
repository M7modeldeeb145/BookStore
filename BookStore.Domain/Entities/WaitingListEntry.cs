namespace BookStore.Domain.Entities
{
    public class WaitingListEntry : MainBaseEntity
    {
        public int BookId { get; set; }
        public Book? Book { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}
