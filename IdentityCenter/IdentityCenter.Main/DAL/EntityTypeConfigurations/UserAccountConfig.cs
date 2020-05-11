using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityCenter.Main.Entities;

namespace IdentityCenter.Main.DAL.EntityTypeConfigurations
{
    public class UserAccountConfig : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.ToTable("UserAccount").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.ID).IsRequired().HasColumnName("id").HasColumnType("uniqueidentifier");
            builder.Property((entity) => entity.Name).IsRequired().HasColumnName("name").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.Password).IsRequired().HasColumnName("password").HasColumnType("varchar(150)");
            builder.Property((entity) => entity.CreateTime).IsRequired().HasColumnName("createtime").HasColumnType("datetime2(7)");
            builder.Property((entity) => entity.ModifyTime).IsRequired().HasColumnName("modifytime").HasColumnType("datetime2(7)");
            builder.Property((entity) => entity.Active).IsRequired().HasColumnName("active").HasColumnType("bit");
            builder.Property((entity) => entity.ExtensionInfo).IsRequired().HasColumnName("extensioninfo").HasColumnType("nvarchar(max)");
        }
    }
}
