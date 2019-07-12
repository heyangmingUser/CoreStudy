using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Configration
{
    /// <summary>
    /// 4.实现IConfigurationSource
    /// </summary>
    public class EFConfigurationSource : IConfigurationSource
    {
        private readonly Action<DbContextOptionsBuilder> _optionsAction;

        public EFConfigurationSource(Action<DbContextOptionsBuilder> optionsAction)
        {
            _optionsAction = optionsAction;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new EFConfigurationProvider(_optionsAction);
        }
    }

    /// <summary>
    ///3. 通过继承来创建自定义配置提供程序
    /// </summary>
    public class EFConfigurationProvider : ConfigurationProvider
    {
        Action<DbContextOptionsBuilder> OptionsAction { get; }

        public EFConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction)
        {
            OptionsAction = optionsAction;
        }

        //加载提供程序的数据
        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<EFConfigurationContext>();

            OptionsAction(builder);

            using (var dbContext = new EFConfigurationContext(builder.Options))
            {
                dbContext.Database.EnsureCreated();

                Data = !dbContext.Values.Any()
                    ? CreateAndSaveDefaultValues(dbContext)
                    : dbContext.Values.ToDictionary(c => c.Id, c => c.Value);
            }
        }

        private static IDictionary<string, string> CreateAndSaveDefaultValues(
       EFConfigurationContext dbContext)
        {
            // Quotes (c)2005 Universal Pictures: Serenity
            // https://www.uphe.com/movies/serenity
            var configValues = new Dictionary<string, string>
            {
                { "quote1", "I aim to misbehave." },
                { "quote2", "I swallowed a bug." },
                { "quote3", "You can't stop the signal, Mal." }
            };

            dbContext.Values.AddRange(configValues
                .Select(kvp => new EFConfigurationValue
                {
                    Id = kvp.Key,
                    Value = kvp.Value
                })
                .ToArray());

            dbContext.SaveChanges();

            return configValues;
        }

    }

    /// <summary>
    /// 扩展IconfigurationBuilder
    /// </summary>
    public static class EntityFrameworkExtensions
    {
        public static IConfigurationBuilder AddEFConfiguration( this IConfigurationBuilder builder,Action<DbContextOptionsBuilder> optionsAction)
        {
            return builder.Add(new EFConfigurationSource(optionsAction));
        }
    }
}
