using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
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


        .ConfigureKestrel((context, options) =>
        {
            //补充kestrel中终结点的设置的两种方式
            //什么是终结点:将
            options.ConfigurationLoader.Endpoint("https", opt => { opt.HttpsOptions.SslProtocols = SslProtocols.Ssl2; });

            options.Configure(context.Configuration.GetSection("Kestrel")).Endpoint("https", opt => { opt.HttpsOptions.SslProtocols = SslProtocols.Ssl2; })
        })
            .UseKestrel(option =>
            {
                //提供其他配置的方法
            })

            .ConfigureKestrel((context, options) =>
            {
                //提供其他配置的方法
            })
            .ConfigureKestrel((context, options) =>
            {
                //保持活动状态超时，默认为2分钟，现在设置为3分钟
                options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(3);
                //客户端最大连接数，当为null，无限制
                options.Limits.MaxConcurrentConnections = 1000; // options.Limits.MaxConcurrentConnections = null;
                //协议升级限制，协议升级后不会计入客户端的最大连接数
                options.Limits.MaxConcurrentUpgradedConnections = 1000;
                //请求正文最大大小
                options.Limits.MaxRequestBodySize = 10 * 1024;
                //请求正文的最小数据速率
                options.Limits.MinRequestBodyDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                //响应的最小速率
                options.Limits.MinResponseDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
                //服务器接收请求标头所花费的最大时间量
                options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);


                //每个连接的最大流
                options.Limits.Http2.MaxStreamsPerConnection = 100;
                //限制HPACK 解码器解压缩 HTTP/2 连接的 HTTP 标头的大小
                options.Limits.Http2.HeaderTableSize = 4096;
                //请求标头的大小
                options.Limits.Http2.MaxRequestHeaderFieldSize = 8192;
                //连接帧有效负载的最大大小
                options.Limits.Http2.MaxFrameSize = 16384;
                //服务器一次性缓存的最大请求主体数据大小中汇总
                options.Limits.Http2.InitialConnectionWindowSize = 131072;//最小值为65535
                //服务器针对每个请求（流）的一次性缓存的最大请求主体数据大小
                options.Limits.Http2.InitialConnectionWindowSize = 98304;//最小值为65535

                //绑定IP地址和端口
                options.Listen(IPAddress.Loopback, 5000);
                options.Listen(IPAddress.Loopback, 5001, listenOptions =>
                {
                    listenOptions.UseHttps("testCert.pfx", "testPassword");
                });
                //终结点配置

                //指定一个为每个指定的终结点运行配置的Action
                options.ConfigureEndpointDefaults(configureOptions =>
                {
                    configureOptions.NoDelay = true;
                });
                //为每个Https终结点运行配置Action
                options.ConfigureHttpsDefaults(httpsoptions =>
                {
                    //httpsoptions.ServerCertificate = certificate;
                });
                options.Listen(IPAddress.Loopback, 5001, listenOptions =>
                {
                    //为使用https配置kestrel,至少提供默认证书
                    listenOptions.UseHttps();
                });
            })

            //加载配置文件
            //serverOptions.Configure(context.Configuration.GetSection("Kestrel")
            //终结点的Url只能是单个值


            .UseStartup<Startup>();


    }
}
