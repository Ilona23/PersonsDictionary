using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Persistance.Configurations
{
    public class PersonRelationConfiguration : IEntityTypeConfiguration<PersonRelation>
    {
        public void Configure(EntityTypeBuilder<PersonRelation> builder)
        {
            builder.HasKey(pr => new { pr.PersonId, pr.RelatedPersonId });

            builder.HasOne(pr => pr.Person)
                .WithMany(p => p.RelatedPersons)
                .HasForeignKey(pr => pr.PersonId);

            builder.HasOne(pr => pr.RelatedPerson)
                .WithMany(p => p.RelatedToPersons)
                .HasForeignKey(pr => pr.RelatedPersonId);
        }
    }
}
