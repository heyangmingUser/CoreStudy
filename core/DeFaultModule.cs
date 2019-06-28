using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Module = Autofac.Module;

namespace core
{
    public class DeFaultModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<CharacterResponsitory>().As<ICharacterRepository>();
        }
    }
}
