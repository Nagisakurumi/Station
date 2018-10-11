using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DataBaseController;
using Microsoft.EntityFrameworkCore;
using ScriptServerStation.Service.Impl;
using ScriptServerStation.Service;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;

namespace ScriptServerStation
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddMvc();
            //DbContextOptionsBuilder dbContextOptionsBuilder = new DbContextOptionsBuilder() { };

            services.AddDbContext<DataBaseContext>(x => x.UseMySQL(Configuration["ConnectionString:DefaultConnection"]));
            
            //services.AddSmartCookies();

            services.AddMemoryCache();
            #region 使用Redis保存Session
            var redisConn = Configuration["WebConfig:Redis:Connection"];
            var redisInstanceName = Configuration["WebConfig:Redis:InstanceName"];
            //Session 过期时长分钟
            var sessionOutTime = Configuration.GetValue<int>("WebConfig:SessionTimeOut", 30);

            //var redis = StackExchange.Redis.ConnectionMultiplexer.Connect(redisConn);
            //services.AddDataProtection().PersistKeysToRedis(redis, "DataProtection-Test-Keys");
            services.AddDistributedRedisCache(option =>
            {
                //redis 连接字符串
                option.Configuration = redisConn;
                //redis 实例名
                option.InstanceName = redisInstanceName;
            }
            );

            services.AddSession();
            #endregion
            services.AddSingleton<IDistributedCache, RedisCache>();
            services.AddScoped<IUserService, UserServiceImpl>();
            services.AddScoped<IScriptService, ScriptServiceImpl>();
            //services.AddBlobStorage()
            //    .AddEntityFrameworkStorage<BlogContext>()
            //    .AddSessionUploadAuthorization();
            //services.AddTimedJob();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseDeveloperExceptionPage();
            app.UseMvc();
        }
    }
}
