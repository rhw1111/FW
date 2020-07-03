using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.DAL.EntityTypeConfigurations
{
    public class TestCaseHistoryConfig : IEntityTypeConfiguration<TestCaseHistory>
    {
        public void Configure(EntityTypeBuilder<TestCaseHistory> builder)
        {
            builder.ToTable("testcasehistory").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.Summary).IsRequired().HasColumnType("varchar(4000)");
            builder.Property((entity) => entity.CaseID).IsRequired().HasColumnType("char(36)");
            builder.HasOne((entity) => entity.Case).WithMany().HasForeignKey(entity => entity.CaseID);
            builder.Property((entity) => entity.CreateTime).HasColumnType("datetime").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property((entity) => entity.ModifyTime).HasColumnType("datetime");
            var sequenceProperty = builder.Property<long>("Sequence").HasColumnName("sequence").HasColumnType("bigint").Metadata;
            sequenceProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
            sequenceProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        }
    }
}
