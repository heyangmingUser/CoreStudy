using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Configration
{
    /// <summary>
    /// 2.存储和访问配置的值
    /// </summary>
    public class EFConfigurationContext:DbContext
    {
        public EFConfigurationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<EFConfigurationValue> Values { get; set; }
    }
}
