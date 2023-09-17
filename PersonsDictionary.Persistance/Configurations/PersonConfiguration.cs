using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Persistance.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName)
                   .HasMaxLength(50);

            builder.Property(x => x.LastName)
                   .HasMaxLength(50);

            builder.Property(x => x.PersonalId)
                   .HasMaxLength(11);

            builder.HasMany(x => x.PhoneNumbers)
                   .WithOne(x => x.Person)
                   .HasForeignKey(x => x.PersonId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.RelatedPersons)
                   .WithOne(pr => pr.Person)
                   .HasForeignKey(pr => pr.PersonId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.RelatedToPersons)
                   .WithOne(pr => pr.RelatedPerson)
                   .HasForeignKey(pr => pr.RelatedPersonId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
