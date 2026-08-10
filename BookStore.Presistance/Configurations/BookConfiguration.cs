using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Presistance.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            builder.Property(b => b.Title).IsRequired().HasMaxLength(250);
            builder.Property(b => b.TotalCopies).HasDefaultValue(0);
            builder.Property(b => b.AvailableCopies).HasDefaultValue(0);
        }
    }
}
