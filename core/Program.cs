using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace core 
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //创建应用主机
            //CreateWebHostBuilder(args).Build().Run();

            //有作用域的服务来解析服务获取服务
            var host = CreateWebHostBuilder(args).Build();
            using (var serviceScope=host.Services.CreateScope())
            {
                var service = serviceScope.ServiceProvider;
                try
                {
                    var serviceContext = service.GetRequiredService<MyDependency>();
                } catch (Exception e)
                {
                    var logger = service.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e,"An error occured");
                }
            }

            host.Run();

        }

        //主机生成器
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
