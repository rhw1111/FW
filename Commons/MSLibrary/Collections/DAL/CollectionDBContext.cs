using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MSLibrary.Collections;
using MSLibrary.Collections.DAL.EntityTypeConfigurations;

namespace MSLibrary.Collections.DAL
{
    public class CollectionDBContext : DbContext
    {
        public CollectionDBContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<TreeEntity> TreeEntities { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilde)
        {
            modelBuilde.ApplyConfiguration<TreeEntity>(new TreeEntityConfig());
        }
    }
}
