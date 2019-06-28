using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace core
{
    public class Startup
    {
        //按环境配置服务
        private readonly IHostingEnvironment _env;
        //读取配置
        private readonly IConfiguration _config;
        //在记录器中创建startup.configureService
        private readonly ILoggerFactory _loggerFactory;

        public Startup(IHostingEnvironment env,IConfiguration config,ILoggerFactory loggerFactory)
        {
            _env = env;
            _config = config;
            _loggerFactory = loggerFactory;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var logger = _loggerFactory.CreateLogger<Startup>();

            if (_env.IsDevelopment())
            {
                logger.LogInformation("Development environment");
            }
            else
            {
                logger.LogInformation($"Environment:{_env.EnvironmentName}");
            }
            //利用IStartUp 有助于确保中间件在应用请求处理管道的开始或结束时由库添加的中间件之前或之后运行
            services.AddTransient<IStartupFilter, RequestSetOptionsStartupFilter>();
            //addScope注册将服务的生存周期限定为单个请求的生存期
            services.AddScoped<IMyDepedency, MyDependency>();
            //内置容器
            services.AddMvc();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DeFaultModule>();
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
