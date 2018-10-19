using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreHelper.Model
{
    public class CompareModel
    {
        /// <summary>
        /// 实体差异比较器
        /// </summary>
        /// <param name="source">源版本实体</param>
        /// <param name="current">当前版本实体</param>
        /// <returns>true 存在变更 false 未变更</returns>
        public static bool DifferenceComparison<T1, T2>(T1 source, T2 current, List<string> exclude) 
            where T1 : class, new()
            where T2 : class, new()
        {
            Type t1 = source.GetType();
            Type t2 = current.GetType();
            PropertyInfo[] property1 = t1.GetProperties();
            //排除主键和基础字段
            //List<string> exclude = new List<string>() { "Id", "InsertTime", "UpdateTime", "DeleteTime", "Mark", "Version", "Code" };
            foreach (PropertyInfo p in property1)
            {
                string name = p.Name;
                Type type = p.PropertyType;

                if (exclude.Contains(name)) { continue; }
                if (type == typeof(decimal?))
                {

                    decimal value1 = 0;
                    decimal value2 = 0;

                    decimal.TryParse(p.GetValue(source, null)?.ToString(), out value1);
                    decimal.TryParse(t2.GetProperty(name)?.GetValue(current, null)?.ToString(), out value2);

                    if (value1 != value2)
                    {
                        return true;
                    }
                }
                else
                {
                    string value1 = p.GetValue(source, null)?.ToString();
                    string value2 = t2.GetProperty(name)?.GetValue(current, null)?.ToString();

                    if (value1 != value2)
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        /// <summary>
        /// 集合差异比较器,比较两个实体集合值是否一样
        /// </summary>
        /// <param name="source">源版本实体集合</param>
        /// <param name="current">当前版本实体集合</param>
        /// <returns>true 存在变更 false 未变更</returns>
        public static bool DifferenceComparison<T1, T2>(List<T1> source, List<T2> current) where T1 : class, new() where T2 : class, new()
        {
            List<string> exclude = new List<string>() { };

            if (source.Count != current.Count) { return true; }
            for (int i = 0; i < source.Count; i++)
            {
                bool flag = DifferenceComparison<T1, T2>(source[i], current[i], exclude);
                if (flag) { return flag; }
            }
            return false;
        }

        /// <summary>
        /// 将实体2的值动态赋值给实体1(名称一样的属性进行赋值)
        /// </summary>
        /// <param name="model1">实体1</param>
        /// <param name="model2">实体2</param>
        /// <returns>赋值后的model1</returns>
        public static T1 BindModelValue<T1, T2>(T1 model1, T2 model2) where T1 : class where T2 : class
        {
            Type t1 = model1.GetType();
            Type t2 = model2.GetType();
            PropertyInfo[] property2 = t2.GetProperties();
            //排除主键
            List<string> exclude = new List<string>() { "Id" };
            foreach (PropertyInfo p in property2)
            {
                if (exclude.Contains(p.Name)) { continue; }
                t1.GetProperty(p.Name)?.SetValue(model1, p.GetValue(model2, null));
            }
            return model1;
        }
    }
}
