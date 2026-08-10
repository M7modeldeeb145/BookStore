namespace BookStore.Domain.Entities
{
    public class Reservation : MainBaseEntity
    {
        public int BookId { get; set; }
        public Book? Book { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public DateTime ReservedAt { get; set; } = DateTime.UtcNow;
    }
}
