using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.DAL.EntityTypeConfigurations
{

    public class TestDataSourceConfig : IEntityTypeConfiguration<TestDataSource>
    {
        public void Configure(EntityTypeBuilder<TestDataSource> builder)
        {
            builder.ToTable("testdatasource").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.Name).IsRequired().HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Type).IsRequired().HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Data).IsRequired().HasColumnType("mediumtext");
            builder.Property((entity) => entity.CreateTime).HasColumnType("datetime").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property((entity) => entity.ModifyTime).HasColumnType("datetime");
            var sequenceProperty = builder.Property<long>("Sequence").HasColumnName("sequence").HasColumnType("bigint").Metadata;
            sequenceProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            sequenceProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        }
    }
}
