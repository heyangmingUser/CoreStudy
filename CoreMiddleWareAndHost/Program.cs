using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace CoreMiddleWareAndHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //web主机
            CreateWebHostBuilder(args).Build().Run();


        }

        public static async Task Main1(string[] args)
        {
            //泛型主机，用于无法处理http请求的应用，用途是将http管道从web主机Api中分离出来
            var host = new HostBuilder().Build();
            //初始化，生成和运行主机的主要主键,这一段是基本初始化
            await host.RunAsync();



            //泛型主机添加选项配置
            var host1 = new HostBuilder().ConfigureServices((hostcontext, services) =>
            {
                services.Configure<HostOptions>(options =>
                {
                    options.ShutdownTimeout = System.TimeSpan.FromSeconds(20);
                });
                hostcontext.HostingEnvironment.ApplicationName = HostDefaults.ApplicationKey;
            }).Build();

            //在泛型主机上设置根内容和环境
            var host2 = new HostBuilder().UseContentRoot("C\\<根目录>")
                .UseEnvironment(Microsoft.Extensions.Hosting.EnvironmentName.Development)
                .UseConsoleLifetime();




            //读取设置主机配置文件
            var host3 = new HostBuilder().ConfigureHostConfiguration(hostconfig =>
            {
                hostconfig.SetBasePath(Directory.GetCurrentDirectory());
                hostconfig.AddJsonFile("hostsettings.json", optional: true);
                hostconfig.AddEnvironmentVariables(prefix: "PREFIX_");
                hostconfig.AddCommandLine(args);

            })
            .ConfigureHostConfiguration(hostconfig =>
            {

            });

            //读取设置"应用"配置文件
            var host4 = new HostBuilder().ConfigureAppConfiguration((hostContext, appconfig) =>
            {
                appconfig.SetBasePath(Directory.GetCurrentDirectory());
                appconfig.AddJsonFile("appsettings.json", optional: true);
                appconfig.AddJsonFile(
                    $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                    optional: true);
                appconfig.AddEnvironmentVariables(prefix: "PREFIX_");
                appconfig.AddCommandLine(args);

            });

            //将服务添加到应用的依赖关系注入容器
            var host5 = new HostBuilder().ConfigureServices((hostContext, services) =>
            {
                if (hostContext.HostingEnvironment.IsDevelopment())
                {
                    // Development service configuration
                }
                else
                {
                    // Non-development service configuration
                }

                services.AddHostedService<LifetimeEventsHostedService>();
                services.AddHostedService<TimedHostedService>();
            });

            ///添加了一个委托来配置提供的 ILoggingBuilder
            var host6 = new HostBuilder().ConfigureLogging((hostContext, configlogger) =>
            {
                configlogger.AddConsole();
                configlogger.AddDebug();
            });

            var host8 = new HostBuilder().UseHostedService<TimeHostedService>().Build();
            //使用服务容器工厂并为"应用"配置自定义服务容器
            var host7 = new HostBuilder().UseServiceProviderFactory(new ServiceContainerFactory())
                .ConfigureContainer<ServiceContainer>((hostContext, container) => { });

            var host10 = new HostBuilder().Build();
        }

        //web主机生成器
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //设置应用程序名称
            .UseSetting(WebHostDefaults.ApplicationKey, "CustomApplicationName")
            //承载启动程序集
            .UseSetting(WebHostDefaults.HostingStartupAssembliesKey, "assembly1;assembly2")
            //设置https重定向端口
            .UseSetting("https_port", "8080")
            //排除承载启动程序集
            .UseSetting(WebHostDefaults.HostingStartupExcludeAssembliesKey, "assembly1;assembly2")

            //作用域验证
            .UseDefaultServiceProvider((context,options)=> { options.ValidateScopes = true; })
                .UseStartup<Startup>();
        

        //重写web主机生成器
        public static IWebHostBuilder CreateWebHostBuilderTwo(string[] arg)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hostsetting.json", optional: true)
                .AddCommandLine(arg)
            .Build();
            return WebHost.CreateDefaultBuilder(arg)
                .UseUrls("http://*:5000")
                .UseConfiguration(config)
                .Configure(app=>
                {
                    app.Run(context=>context.Response.WriteAsync("Hello, World!"));
                });
        }
    }
}
