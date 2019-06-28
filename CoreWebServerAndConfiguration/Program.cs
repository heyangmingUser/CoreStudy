using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CoreWebServerAndConfiguration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //kestrel服务器,是默认的服务器,对http请求实现监听功能,2.1以上的"应用"是受支持的托管配置
            //用作边缘服务器的 Kestrel 不支持在多个进程间共享相同的 IP 和端口
            .UseKestrel(option=> 
            {
                //提供其他配置的方法
            })
            .ConfigureKestrel((context, options) =>
            {
                //提供其他配置的方法
            })
            .ConfigureKestrel((context,options)=> 
            {
                //限制活动链接超时，默认为2分钟，现在设置为3分钟
                options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(3);
                //最大连接数，当为null，无限制
                options.Limits.MaxConcurrentConnections = 1000;
                //升级连接就是其中之一,已经从HTTP切换到另一个协议
                options.Limits.MaxConcurrentUpgradedConnections = 1000;
                options.Limits.
                
            })
            .UseStartup<Startup>();
    }
}
