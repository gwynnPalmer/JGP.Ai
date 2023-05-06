using JGP.Telegram.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JGP.Telegram.Data.Mapping;

/// <summary>
///     Class chat log map
/// </summary>
/// <seealso cref="IEntityTypeConfiguration{ChatLog}" />
public class ChatLogMap : IEntityTypeConfiguration<ChatLog>
{
    /// <summary>
    ///     Configures the builder
    /// </summary>
    /// <param name="builder">The builder</param>
    public void Configure(EntityTypeBuilder<ChatLog> builder)
    {
        // Primary Key.
        builder.HasKey(x => x.Id);

        // Properties.
        builder.Property(x => x.ChatId)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.MessageDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        // Table & Column Mappings.
        builder.ToTable("ChatLogs", "dbo");
        builder.Property(x => x.Id).HasColumnName("Id").IsRequired();
        builder.Property(x => x.ChatId).HasColumnName("ChatId").IsRequired();
        builder.Property(x => x.MessageDate).HasColumnName("MessageDate").IsRequired();
        builder.Property(x => x.Request).HasColumnName("Request");
        builder.Property(x => x.Response).HasColumnName("Response");

        // Indexes.
        builder.HasIndex(x => x.ChatId)
            .HasDatabaseName("IX_ChatLogs_ChatId");
    }
}