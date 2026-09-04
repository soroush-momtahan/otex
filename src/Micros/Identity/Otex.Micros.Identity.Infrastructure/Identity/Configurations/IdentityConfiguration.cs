using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otex.BuildingBlocks.Infrastructure.Converters;
using Otex.Micros.Identity.Domain.Identity.Models;
using Otex.Micros.Identity.Domain.Identity.ValueObjects;

namespace Otex.Micros.Identity.Infrastructure.Identity.Configurations;

public class IdentityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion<PrefixedGuidEfConverter<UserId>>();
        
        builder.Property(x => x.IdentityUserId)
            .HasConversion<PrefixedGuidEfConverter<IdentityUserId>>();
        
        builder.ComplexProperty(x => x.Mobile, mobileBuilder =>
        {
            mobileBuilder.Property(x => x.Value)
                .HasColumnName(nameof(User.Mobile));
        });
        
        builder.ComplexProperty(x => x.FullName, fullNameBuilder =>
        {
            fullNameBuilder.Property(x => x.Firstname)
                .HasColumnName(nameof(User.FullName.Firstname));
            fullNameBuilder.Property(x => x.Lastname)
                .HasColumnName(nameof(User.FullName.Lastname));
        });
    }
}