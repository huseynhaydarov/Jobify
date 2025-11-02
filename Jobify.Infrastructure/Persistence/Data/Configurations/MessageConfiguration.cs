using Jobify.Domain.Entities;
using Jobify.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jobify.Infrastructure.Persistence.Data.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        builder.Property(u => u.MessageText)
            .HasMaxLength(500)
            .HasColumnName("Text")
            .IsRequired();

        builder.HasOne(m => m.Sender)
            .WithMany(m => m.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Receiver)
            .WithMany(m => m.ReceivedMessages)
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.JobListing)
            .WithMany(m => m.Messages)
            .HasForeignKey(m => m.JobId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
