using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSLibrary.CommandLine.SSH;

namespace FW.TestPlatform.Main.DAL.EntityTypeConfigurations
{
    public class SSHEndpointConfig : IEntityTypeConfiguration<SSHEndpoint>
    {
        public void Configure(EntityTypeBuilder<SSHEndpoint> builder)
        {
            builder.ToTable("sshendpoint").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.Name).IsRequired().HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Type).IsRequired().HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Configuration).IsRequired().HasColumnType("varchar(1000)");      
            builder.Property((entity) => entity.CreateTime).HasColumnType("datetime").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property((entity) => entity.ModifyTime).HasColumnType("datetime");
            builder.Property<long>("Sequence").HasColumnName("sequence").HasColumnType("bigint").Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
        }
    }
}
