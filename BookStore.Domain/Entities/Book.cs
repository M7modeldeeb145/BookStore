using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Domain.Entities
{
    public class Book : MainBaseEntity
    {
        public string Title { get; set; } = string.Empty;
        // Total physical copies in library
        public int TotalCopies { get; set; }
        // Currently available copies for reservation
        public int AvailableCopies { get; set; }
    }
}
