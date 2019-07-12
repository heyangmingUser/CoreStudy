using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Configration
{
    /// <summary>
    /// Configuration主要是指配置文件，包含了主机配置文件和应用配置文件（主要讲应用配置文件）
    /// </summary>
    public class Program
    {

        public static readonly Dictionary<string, string> _switchMappings =
        new Dictionary<string, string>
        {
            { "-CLKey1", "CommandLineKey1" },
            { "-CLKey2", "CommandLineKey2" }
        };

        public static readonly Dictionary<string, string> _dict =
     new Dictionary<string, string>
     {
            {"MemoryCollectionKey1", "value1"},
            {"MemoryCollectionKey2", "value2"}
     };
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

            //命令行参数的交换映射
            //var config = new ConfigurationBuilder()
            //    .AddCommandLine(args, _switchMappings)
            //    .Build();
            //var host = new WebHostBuilder()
            //    .UseConfiguration(config)
            //    .UseKestrel()
            //    .UseStartup<Startup>();
            //host.Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //构建主机时调用 ConfigureAppConfiguration 以指定应用的配置提供程序以及 CreateDefaultBuilder 自动添加的配置提供程序
            .ConfigureAppConfiguration((hostContext, config) =>
            {//所有的config.出来的数据都是应用的配置提供程序

                //配置文件的根目录
                config.SetBasePath(Directory.GetCurrentDirectory());

                //以键值对的方式读取
                //ini文件
                config.AddIniFile("config.ini", true, reloadOnChange: true);
                //自定义json文件提供程序
                config.AddJsonFile(
                    "config.json", optional: true, reloadOnChange: true);

                //自定义xml程序提供程序
                config.AddXmlFile("config.xml", optional: true, reloadOnChange: true);

                //Key - per - file 配置提供程序用于 Docker 托管方案,文件的 directoryPath 必须是绝对路径。
                var path = Path.Combine(Directory.GetCurrentDirectory(), "path/to/files");
                config.AddKeyPerFile(directoryPath: path, optional: true);


                //内存配置提供程序
                config.AddInMemoryCollection(_dict);

                //自定义配置提供程序
                config.AddEFConfiguration(options => options.UseInMemoryDatabase("InMemoryDb"));

                //环境配置提供程序
                config.AddEnvironmentVariables(prefix: "PREFIX_");
                //此方法无法交换映射
                config.AddCommandLine(args);
            })
                .UseStartup<Startup>();

        //配置完了之后就是读取配置文件，

    }
}
