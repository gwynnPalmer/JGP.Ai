using JGP.Telegram.Core;
using JGP.Telegram.Data.Comparers;
using JGP.Telegram.Data.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JGP.Telegram.Data.Mapping;

/// <summary>
///     Class user map
/// </summary>
/// <seealso cref="IEntityTypeConfiguration{User}" />
internal class UserMap : IEntityTypeConfiguration<User>
{
    /// <summary>
    ///     Configures the builder
    /// </summary>
    /// <param name="builder">The builder</param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Primary Key.
        builder.HasKey(x => x.Id);

        // Properties.
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.ChatIds)
            .HasMaxLength(100)
            .HasConversion(new LongListToDelimitedStringConverter())
            .Metadata.SetValueComparer(new LongListValueComparer());

        builder.Property(x => x.Token)
            .IsRequired();

        builder.Property(x => x.IsEnabled)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(x => x.LastModifiedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        // Table & Column Mappings.
        builder.ToTable("Users");
        builder.Property(x => x.Id).HasColumnName("UserId");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.ChatIds).HasColumnName("ChatIds");
        builder.Property(x => x.Token).HasColumnName("Token");
        builder.Property(x => x.IsEnabled).HasColumnName("IsEnabled");
        builder.Property(x => x.CreatedDate).HasColumnName("CreatedDate");
        builder.Property(x => x.LastModifiedDate).HasColumnName("LastModifiedDate");
    }
}