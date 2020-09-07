using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using IdentityCenter.Main.Entities;
using IdentityCenter.Main.IdentityServer;
using IdentityCenter.Main.DAL.EntityTypeConfigurations;

namespace IdentityCenter.Main.DAL
{
    public class TemporaryDBContext : DbContext
    {
        public TemporaryDBContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// 认证Code
        /// </summary>
        public DbSet<IdentityAuthorizationCode> IdentityAuthorizationCodes { get; set; } = null!;
        /// <summary>
        /// 认证RefreshToken
        /// </summary>
        public DbSet<IdentityRefreshToken> IdentityRefreshTokens { get; set; } = null!;
        /// <summary>
        /// 认证确认
        /// </summary>
        public DbSet<IdentityConsent> IdentityConsents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilde)
        {
            modelBuilde.ApplyConfiguration<IdentityAuthorizationCode>(new IdentityAuthorizationCodeConfig());
            modelBuilde.ApplyConfiguration<IdentityRefreshToken>(new IdentityRefreshTokenConfig());
            modelBuilde.ApplyConfiguration<IdentityConsent>(new IdentityConsentConfig());
        }

    }
}
