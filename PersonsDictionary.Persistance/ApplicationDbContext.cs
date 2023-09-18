﻿using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Persistance.Configurations;

namespace Persistence.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<PersonRelation> PersonRelations { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<City> Cities { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration())
                       .ApplyConfiguration(new PersonRelationConfiguration())
                       .ApplyConfiguration(new PhoneNumberConfiguration())
                       .ApplyConfiguration(new CityConfiguration());
        }
    }
}
