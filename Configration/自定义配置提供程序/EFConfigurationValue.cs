using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Configration
{

    /// <summary>
    /// 1.在数据库中存储配置值的实体
    /// </summary>
    public class EFConfigurationValue
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
}
