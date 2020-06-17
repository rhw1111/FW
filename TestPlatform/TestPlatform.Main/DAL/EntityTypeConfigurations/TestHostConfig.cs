using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.DAL.EntityTypeConfigurations
{
    public class TestHostConfig : IEntityTypeConfiguration<TestHost>
    {
        public void Configure(EntityTypeBuilder<TestHost> builder)
        {
            builder.ToTable("testhost").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.Address).IsRequired().HasColumnType("varchar(100)");
            builder.Property((entity) => entity.SSHEndpointID).IsRequired().HasColumnType("char(36)");
            builder.HasOne((entity) => entity.SSHEndpoint).WithMany().HasForeignKey(entity => entity.SSHEndpointID);
            builder.Property((entity) => entity.CreateTime).HasColumnType("datetime").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property((entity) => entity.ModifyTime).HasColumnType("datetime");
        }
    }
}
