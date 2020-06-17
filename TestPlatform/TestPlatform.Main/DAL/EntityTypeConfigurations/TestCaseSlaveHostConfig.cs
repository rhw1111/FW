using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.DAL.EntityTypeConfigurations
{
    public class TestCaseSlaveHostConfig : IEntityTypeConfiguration<TestCaseSlaveHost>
    {
        public void Configure(EntityTypeBuilder<TestCaseSlaveHost> builder)
        {
            builder.ToTable("testcaseslavehost").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.HostID).IsRequired().HasColumnType("char(36)");
            builder.HasOne((entity) => entity.Host).WithMany().HasForeignKey(entity => entity.HostID);
            builder.Property((entity) => entity.TestCaseID).IsRequired().HasColumnType("char(36)");
            builder.HasOne((entity) => entity.TestCase).WithMany().HasForeignKey(entity => entity.TestCaseID);
            builder.Property((entity) => entity.Count).IsRequired().HasColumnType("int");
            builder.Property((entity) => entity.ExtensionInfo).IsRequired().HasColumnType("varchar(1000)");
            builder.Property((entity) => entity.CreateTime).HasColumnType("datetime").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property((entity) => entity.ModifyTime).HasColumnType("datetime");
        }
    }
}
