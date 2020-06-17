using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.DAL.EntityTypeConfigurations
{
    public class TestCaseConfig : IEntityTypeConfiguration<TestCase>
    {
        public void Configure(EntityTypeBuilder<TestCase> builder)
        {
            builder.ToTable("testcase").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.Name).IsRequired().HasColumnType("varchar(150)");
            builder.Property((entity) => entity.EngineType).IsRequired().HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Configuration).IsRequired().HasColumnType("varchar(4000)");
            builder.Property((entity) => entity.Status).IsRequired().HasColumnType("int")
                .HasConversion<int>((v) => (int)v,
                (v) => (TestCaseStatus)v);

            builder.Property((entity) => entity.MasterHostID).IsRequired().HasColumnType("char(36)");
            builder.HasOne((entity) => entity.MasterHost).WithMany().HasForeignKey(entity => entity.MasterHostID);

            builder.Property((entity) => entity.OwnerID).IsRequired().HasColumnType("char(36)");
            builder.HasOne((entity) => entity.Owner).WithMany().HasForeignKey(entity => entity.OwnerID);

            builder.Property((entity) => entity.CreateTime).HasColumnType("datetime").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property((entity) => entity.ModifyTime).HasColumnType("datetime");
        }
    }
}
