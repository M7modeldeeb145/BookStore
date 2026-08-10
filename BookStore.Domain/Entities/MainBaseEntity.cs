namespace BookStore.Domain.Entities
{
    public class MainBaseEntity
    {
        public int Id { get; set; }

        // Soft Delete Fields
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        // Common Audit Timestamps
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
