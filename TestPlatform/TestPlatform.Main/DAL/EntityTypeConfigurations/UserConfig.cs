using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using FW.TestPlatform.Main.Entities;

namespace FW.TestPlatform.Main.DAL.EntityTypeConfigurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user").HasKey((entity) => entity.ID);
            builder.Property((entity) => entity.Name).IsRequired().HasColumnType("varchar(150)");
            builder.Property((entity) => entity.CreateTime).HasColumnType("datetime").Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Property((entity) => entity.ModifyTime).HasColumnType("datetime");
        }
    }
}
