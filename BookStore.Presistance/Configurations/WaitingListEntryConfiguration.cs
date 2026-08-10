using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Presistance.Configurations
{
    public class WaitingListEntryConfiguration : IEntityTypeConfiguration<WaitingListEntry>
    {
        public void Configure(EntityTypeBuilder<WaitingListEntry> builder)
        {
            builder.ToTable("WaitingListEntries");
            builder.Property(w => w.CustomerName).IsRequired().HasMaxLength(200);
            builder.HasOne(w => w.Book)
                .WithMany()
                .HasForeignKey(w => w.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(w => w.JoinedAt).IsRequired();
        }
    }
}
