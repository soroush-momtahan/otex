using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Otex.BuildingBlocks.Infrastructure.Converters;
using Otex.Micros.Applicants.Domain.Cooperation.ValueObjects;

namespace Otex.Micros.Applicants.Infrastructure.Cooperation.Configurations;

internal sealed class CooperationConfiguration : IEntityTypeConfiguration<Domain.Cooperation.Models.Cooperation>
{
    public void Configure(EntityTypeBuilder<Domain.Cooperation.Models.Cooperation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion<PrefixedGuidEfConverter<CooperationId>>();

        builder.ComplexProperty(x => x.Fullname, fullnameBuilder =>
        {
            fullnameBuilder.Property(x => x.Firstname)
                .HasColumnName(nameof(Fullname.Firstname));
            fullnameBuilder.Property(x => x.Lastname)
                .HasColumnName(nameof(Fullname.Lastname));
        });
        
        builder.ComplexProperty(x => x.Mobile, mobileBuilder =>
        {
            mobileBuilder.Property(x => x.Value)
                .HasColumnName(nameof(Mobile));
        });

        builder.ComplexProperty(x => x.Location, locationBuilder =>
        {
            locationBuilder.Property(x => x.Province)
                .HasColumnName(nameof(Location.Province));
            locationBuilder.Property(x => x.City)
                .HasColumnName(nameof(Location.City));
        });
        
        builder.Property(x => x.TypeOfActivity)
            .HasConversion<string>();
        
        builder.ComplexProperty(x => x.Description, descriptionBuilder =>
        {
            descriptionBuilder.IsRequired(false);
            descriptionBuilder.Property(x => x.Value)
                .HasColumnName(nameof(Description));
        });

        builder.ComplexProperty(x => x.ReserveDateTime, reserveDateTimeBuilder =>
        {
            reserveDateTimeBuilder.IsRequired(false);
            reserveDateTimeBuilder.Property(x => x.Value)
                .HasColumnName(nameof(ReservationDateTime));
        });
        
        builder.ComplexProperty(x => x.IsVerified, verifiedBuilder =>
        {
            verifiedBuilder.Property(x => x.Value)
                .HasColumnName(nameof(IsVerified));
        });
    }
}