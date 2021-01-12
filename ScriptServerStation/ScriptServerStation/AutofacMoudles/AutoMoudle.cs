using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using ScriptServerStation.Attribute;

namespace ScriptServerStation.AutofacMoudles
{
    /// <summary>
    /// 自动注入模块
    /// </summary>
    public class AutoMoudle : Autofac.Module
    {
        /// <summary>
        /// 加载注入
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            //读取当前命名空间下的所有的类型
            Type[] types = this.GetType().Assembly.GetTypes();

            foreach (var item in types.Where(t => t.GetCustomAttribute<ApiControllerAttribute>() != null))
            {
                builder.RegisterType(item).PropertiesAutowired();
            }

            foreach (var item in types.Where(t => t.GetCustomAttribute<InterfaceAttribute>() != null))
            {
                builder.RegisterType(item).As(item.GetInterfaces().First()).PropertiesAutowired();
            }

        }
    }
}
