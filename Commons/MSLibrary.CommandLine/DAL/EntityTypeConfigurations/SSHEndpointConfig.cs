using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSLibrary.CommandLine.SSH;

namespace MSLibrary.CommandLine.DAL.EntityTypeConfigurations
{
    public class SSHEndpointConfig : IEntityTypeConfiguration<SSHEndpoint>
    {
        public void Configure(EntityTypeBuilder<SSHEndpoint> builder)
        {
            builder.ToTable("SSHEndpoint").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.ID).IsRequired().HasColumnName("id").HasColumnType("uniqueidentifier");
            builder.Property((entity) => entity.Name).IsRequired().HasColumnName("name").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Type).IsRequired().HasColumnName("type").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Configuration).IsRequired().HasColumnName("configuration").HasColumnType("nvarchar(max)");
            builder.Property((entity) => entity.CreateTime).IsRequired().HasColumnName("createtime").HasColumnType("datetime2(7)");
            builder.Property((entity) => entity.ModifyTime).IsRequired().HasColumnName("modifytime").HasColumnType("datetime2(7)");
        }
    }
}
