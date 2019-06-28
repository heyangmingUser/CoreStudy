using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core
{
    //依赖关系注入之使用接口抽象化依赖关系实现
    public interface IMyDepedency
    {
        Task WriteMessage(string message);
    }


    //实现接口
    public class MyDependency : IMyDepedency
    {
        private readonly ILogger<MyDependency> _logger;
        public MyDependency(ILogger<MyDependency> logger,IConfiguration config)
        {
            _logger = logger;
            var myStringValue = config["myStringKey"];
        }

        public Task WriteMessage(string message)
        {
            _logger.LogInformation($"MyDependency.WriteMessage called. Message:{message}");
            return Task.FromResult(0);
        }
    }
}
