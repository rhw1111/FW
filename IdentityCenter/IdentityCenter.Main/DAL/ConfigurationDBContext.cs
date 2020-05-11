using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using IdentityCenter.Main.Entities;
using IdentityCenter.Main.IdentityServer;
using IdentityCenter.Main.DAL.EntityTypeConfigurations;
using IdentityCenter.Main.IdentityServer.ClientBindings;

namespace IdentityCenter.Main.DAL
{
    public class ConfigurationDBContext : DbContext
    {
        public ConfigurationDBContext(DbContextOptions options) : base(options)
        {
        }


        /// <summary>
        /// 认证主机
        /// </summary>
        public DbSet<IdentityHost> IdentityHosts { get; set; } = null!;
        /// <summary>
        /// 认证客户端
        /// </summary>
        public DbSet<IdentityClient> IdentityClients { get; set; } = null!;

        /// <summary>
        /// 认证提供方
        /// </summary>
        public DbSet<IdentityProvider> IdentityProviders { get; set; } = null!;
        /// <summary>
        /// 资源数据
        /// </summary>
        public DbSet<ResourceData> ResourceDatas { get; set; } = null!;
        /// <summary>
        /// 认证资源数据
        /// </summary>
        public DbSet<IdentityResourceData> IdentityResourceDatas { get; set; } = null!;
        /// <summary>
        /// Api资源数据
        /// </summary>
        public DbSet<ApiResourceData> ApiResourceDatas { get; set; } = null!;
        /// <summary>
        /// 认证客户端绑定
        /// </summary>
        public DbSet<IdentityClientBinding> IdentityClientBindings { get; set; } = null!;
        /// <summary>
        /// 认证OpenID客户端绑定
        /// </summary>
        public DbSet<IdentityClientOpenIDBinding> IdentityClientOpenIDBindings { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public DbSet<IdentityClientHost> IdentityClientHosts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilde)
        {
            modelBuilde.ApplyConfiguration<IdentityHost>(new IdentityHostConfig());
            modelBuilde.ApplyConfiguration<IdentityClient>(new IdentityClientConfig());
            modelBuilde.ApplyConfiguration<IdentityProvider>(new IdentityProviderConfig());
            modelBuilde.ApplyConfiguration<ResourceData>(new ResourceDataConfig());
            modelBuilde.ApplyConfiguration<IdentityResourceData>(new IdentityResourceDataConfig());
            modelBuilde.ApplyConfiguration<ApiResourceData>(new ApiResourceDataConfig());
            modelBuilde.ApplyConfiguration<IdentityClientBinding>(new IdentityClientBindingConfig());
            modelBuilde.ApplyConfiguration<IdentityClientOpenIDBinding>(new IdentityClientOpenIDBindingConfig());
            modelBuilde.ApplyConfiguration<IdentityClientHost>(new IdentityClientHostConfig());
        }

    }
}
