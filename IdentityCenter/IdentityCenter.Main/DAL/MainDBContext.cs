using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using IdentityCenter.Main.Entities;
using IdentityCenter.Main.IdentityServer;
using IdentityCenter.Main.DAL.EntityTypeConfigurations;

namespace IdentityCenter.Main.DAL
{
    public class MainDBContext : DbContext
    {
        public MainDBContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// 用户账号
        /// </summary>
        public DbSet<UserAccount> UserAccounts { get; set; } = null!;

        /// <summary>
        /// 用户第三方账号
        /// </summary>
        public DbSet<UserThirdPartyAccount> UserThirdPartyAccounts { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilde)
        {
            modelBuilde.ApplyConfiguration<UserAccount>(new UserAccountConfig());
            modelBuilde.ApplyConfiguration<UserThirdPartyAccount>(new UserThirdPartyAccountConfig());
            
        }

    }

}
