using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMiddleWareAndHost
{
    //应用建立 UseHostedService 扩展方法，以注册在 T 中传递的托管服务
    public static class Extensions
    {
        public static IHostBuilder UseHostedService<T>(this IHostBuilder hostBuilder)where T:class,IHostedService,IDisposable
        {
            return hostBuilder.ConfigureServices(services => services.AddHostedService<T>());
        }
    }
}
