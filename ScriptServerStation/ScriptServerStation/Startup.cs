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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using ScriptServerStation.HelpClasses.Cache;
using ScriptServerStation.HelpClasses.Cache.Redis;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Hosting;
using Autofac;
using ScriptServerStation.Database;
using AspectCore.Extensions.Autofac;
using System.Reflection;
using System.IO;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using ScriptServerStation.Service.Impl;
using ScriptServerStation.Service;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using ScriptServerStation.Items;
using Microsoft.AspNetCore.Http;
using ScriptServerStation.Filters;
using ScriptServerStation.HelpClasses.Configs.Configuration;
using ScriptServerStation.HelpClasses.Cache.Memory;

namespace ScriptServerStation
{
    public class Startup
    {

        /// <summary>
        /// 配置文件
        /// </summary>
        public IConfiguration Configuration { get; set; }
        /// <summary>
        /// 全局日志
        /// </summary>
        public ILog Log { get; set; } = new WxxandxyxLog("全局日志");
        /// <summary>
        /// 服务器配置
        /// </summary>
        public CacheKey CacheKey { get; set; } 
        /// <summary>
        /// 缓存
        /// </summary>
        public IMemoryCache MemoryCache { get; set; } = new MemoryCache();


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            CacheKey = new CacheKey(configuration);
        }

        /// <summary>
        /// 集成Autofac
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //添加依赖注入实例，AutofacModuleRegister就继承自Autofac.Module的类
            builder.RegisterModule(new AutofacMoudles.AutoMoudle());
            //注入日志
            builder.RegisterInstance(new WxxandxyxLog(Configuration["Log:Path"])).As<ILog>().SingleInstance().PropertiesAutowired();
            //注入配置文件读取实例
            builder.RegisterInstance(new CacheKey(Configuration)).SingleInstance().PropertiesAutowired();
            //注入数据库
            builder.Register<DataBaseContext>(c => {
                DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder(new DbContextOptions<DataBaseContext>());
                optionsBuilder.UseMySQL(Configuration["DataBase:ConnectionString"]);
                return new DataBaseContext(optionsBuilder.Options as DbContextOptions<DataBaseContext>);
            }).PropertiesAutowired();

            var redisConn = Configuration["WebConfig:Redis:Connection"];

            /////自定义redis
            //builder.Register<IMemoryCache>(c => 
            //    new RedisDataBase(new ExRedisCacheOptions()
            //    {
            //        Configuration = redisConn,
            //    }
            //    )).SingleInstance().PropertiesAutowired();
            //注入内存缓存
            builder.RegisterInstance<IMemoryCache>(MemoryCache).SingleInstance().PropertiesAutowired();
            //注入配置文件
            builder.RegisterInstance<IConfiguration>(Configuration).SingleInstance().PropertiesAutowired();
            ///添加邮件服务
            builder.Register<EmailConnect>(c =>
            {
                return new EmailConnect(Configuration["MailConfig:Account"],
                    Configuration["MailConfig:Password"],
                    Configuration["MailConfig:Address"],
                    Convert.ToInt32(Configuration["MailConfig:Port"]),
                    Configuration["MailConfig:NickName"],
                    Configuration["MailConfig:Host"]);
            }).As<IEmailConnect>().SingleInstance().PropertiesAutowired();
            //代理
            builder.RegisterDynamicProxy();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //替换容器服务
            _ = services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            services.AddControllers().AddNewtonsoftJson();
            ////配置跨域问题
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                 builder => builder.AllowAnyMethod().SetIsOriginAllowed(_ =>
                 {
                     return true;
                 })
                 .AllowAnyHeader()
                 //.AllowAnyOrigin()
                 .AllowCredentials()
                 ));

            //企用缓存
            services.AddMemoryCache();
            services.AddSession();

            services.AddMvc(o => {
                o.Filters.Add(new LoginAuthFilterAction(new string[] {
                    "user/login",
                    "user/register"
                }, Log, CacheKey, MemoryCache));
            });

            services.AddSwaggerGen(c =>
            {
                //c.OperationFilter<ExamplesOperationFilter>();
                //c.OperationFilter<DescriptionOperationFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                    Description = "My API",
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //启动跨域
            app.UseCors("CorsPolicy");
            //全局异常
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        Log.Error("gloab ------------------ ", error.Error);
                        var ex = error.Error;
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(Result.Fail(ex.Message)));
                    }
                });
            });
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API"); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
