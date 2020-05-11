using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MSLibrary.Serializer;
using IdentityCenter.Main.IdentityServer;

namespace IdentityCenter.Main.DAL.EntityTypeConfigurations
{
    public class IdentityClientHostConfig : IEntityTypeConfiguration<IdentityClientHost>
    {
        public void Configure(EntityTypeBuilder<IdentityClientHost> builder)
        {
            builder.ToTable("IdentityClientHost").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.ID).HasColumnName("id").IsRequired().HasColumnType("uniqueidentifier");
            builder.Property((entity) => entity.Name).IsRequired().HasColumnName("name").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.ClaimContextGeneratorName).IsRequired().HasColumnName("claimcontextgeneratorname").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.EnvironmentClaimGeneratorName).IsRequired().HasColumnName("environmentclaimgeneratorname").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.CreateTime).IsRequired().HasColumnName("createtime").HasColumnType("datetime2(7)");
            builder.Property((entity) => entity.ModifyTime).IsRequired().HasColumnName("modifytime").HasColumnType("datetime2(7)");
        }
    }
}
