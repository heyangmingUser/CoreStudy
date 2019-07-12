using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Configration.Controllers
{
    /// <summary>
    /// 这个控制器用来演示如何从文件中读取值
    /// </summary>
    public class HomeController : Controller
    {
        private IConfiguration _config;
        public HomeController(IConfiguration config)
        {
            _config = config;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            //不具有具体的值，只有秘钥和路径
            var configSection = _config.GetSection("section1");
            var configSection1 = _config.GetSection("section2:subsection0");

            //获取当前配置节点下的所有子一级
            var configSection3 = _config.GetSection("section2");
            var children = configSection3.GetChildren();

            //此配置节是否存在
            var sectionExists = _config.GetSection("section2:subsection2").Exists();
            return null;
        }
    }
}
