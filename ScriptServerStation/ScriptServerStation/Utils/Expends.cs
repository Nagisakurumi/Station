using Newtonsoft.Json;
using ScriptServerStation.Database;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScriptServerStation.Utils
{
    public static class Expends
    {

        /// <summary>
        /// 判断用户是否有特殊权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsHasSpecialPower(this User user)
        {
            return user.IsSpecial;
        }
        /// <summary>
        /// 判断用户是否有特殊权限
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsHasAdminPower(this User user)
        {
            return user.Type == 1;
        }
        /// <summary>
        /// 判断int 和指定的枚举类型是否相等
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool EqualsType<T>(this int type, T t) where T : struct
        {
            return type == t.ToBaseType<int>();
        }

        /// <summary>
        /// 获取lambda表达式表示的属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetExpressionProperty<T>(this Expression<Func<T>> expression)
        {
            var lambda = (LambdaExpression)expression;
            MemberExpression memberExpr;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpr = (UnaryExpression)lambda.Body;
                memberExpr = (MemberExpression)unaryExpr.Operand;
            }
            else
            {
                memberExpr = (MemberExpression)lambda.Body;
            }
            return memberExpr.Member.Name;
        }

        /// <summary>
        /// 获取应用程序域下的全路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetApplicationFullPath(this string path)
        {
            return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
        }
        /// <summary>
        /// 输出到控制台
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="objs"></param>
        public static void ToOutPut(this string msg, params object[] objs)
        {
            Console.WriteLine(msg, objs);
        }

        /// <summary>
        /// 追加路径
        /// </summary>
        /// <param name="url"></param>
        /// <param name="appendurl"></param>
        /// <returns></returns>
        public static string AppendPath(this string url, string appendurl)
        {
            url = System.IO.Path.Combine(url, appendurl);
            return url;
        }
        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static string ToFormat(this string msg, params object[] objs)
        {
            return string.Format(msg, objs);
        }
        /// <summary>
        /// 随机数
        /// </summary>
        /// <param name="random"></param>
        /// <param name="value"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static int ToRandom(this int value, Random random, int range = 3)
        {
            return value + random.Next(0, 3);
        }
        /// <summary>
        /// 用正则表达式提取字符串中的数字
        /// </summary>
        /// <param name="info">字符串</param>
        /// <returns>数字</returns>
        public static int RegexGetValue(this string info)
        {
            return Convert.ToInt32(Regex.Match(info, ".([0-9]{1,})").Groups[1].Value);
        }

        /// <summary>
        /// 克隆一个浅对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">要被克隆的对象</param>
        /// <returns>克隆出来的对象</returns>
        public static T CloneObject<T>(this T t)
        {
            Type type = typeof(T);
            T clone = (T)Activator.CreateInstance(type);
            foreach (var item in type.GetProperties())
            {
                if (item.SetMethod != null)
                    item.SetValue(clone, item.GetValue(t));
            }
            return clone;
        }
        /// <summary>
        /// 把from中的所有元素添加到to的集合中并且返回to
        /// </summary>
        /// <typeparam name="T">集合中的元素类型</typeparam>
        /// <param name="to">集合</param>
        /// <param name="from">集合</param>
        /// <returns>to集合</returns>
        public static ICollection<T> AddRange<T>(this ICollection<T> to, ICollection<T> from)
        {
            from.Foreach(t => to.Add(t));
            return to;
        }
        /// <summary>
        /// 通过反射设置对象的具有访问器的值
        /// </summary>
        /// <param name="obj">属性所属的对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">属性需要被赋值的值</param>
        public static void SetObjectPropertyValue(this object obj, string propertyName, object value)
        {
            obj.GetType().GetProperty(propertyName).SetValue(obj, value);
        }
        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetObjectPropertyValue<T>(this object obj, string propertyName)
        {
            return (T)obj.GetType().GetProperty(propertyName).GetValue(obj);
        }
        /// <summary>
        /// 获取属性的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T GetObjectPropertyFirstValue<T>(this object obj)
        {
            var result = obj.GetType().GetProperties().Where(p => p.PropertyType.FullName.Equals(typeof(T).FullName));
            if (result.Count() > 0)
            {
                return (T)result.FirstOrDefault().GetValue(obj);
            }
            else
            {
                throw new Exception("Not find typeof({0})".Format(typeof(T)));
            }
        }
        /// <summary>
        /// 字符串转换到Rectangle 格式必须是 x,y,width,height
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static System.Drawing.Rectangle StringToRectangle(this string rect)
        {
            try
            {
                if (rect.Contains("X"))
                {
                    string[] contents = rect.Replace("{", "").Replace("}", "").Split(',');
                    return new System.Drawing.Rectangle(contents[0].Split('=')[1].ToBaseType<int>(),
                        contents[1].Split('=')[1].ToBaseType<int>(), contents[2].Split('=')[1].ToBaseType<int>(),
                        contents[3].Split('=')[1].ToBaseType<int>());
                }


                int[] values = rect.Split(',').ChangedList<string, int>(t => t.ToBaseType<int>()).ToArray();
                return new System.Drawing.Rectangle(values[0], values[1], values[2], values[3]);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Drawing.Rectangle>(rect);
            }
        }
        /// <summary>
        /// 转换Rectangle到字符串
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static string RectangleToString(this System.Drawing.Rectangle rectangle)
        {
            return "{0},{1},{2},{3}".Format(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
        /// <summary>
        /// 清空队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queues"></param>
        /// <param name="action"></param>
        public static void ClearAll<T>(this ConcurrentQueue<T> queues, Action<T> action)
        {
            T t;
            while (queues.TryDequeue(out t)) { action?.Invoke(t); }
        }
        /// <summary>
        /// 格式化字符串 {0}{1}
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="objs">填充的对象</param>
        /// <returns></returns>
        public static string Format(this string msg, params object[] objs)
        {
            return string.Format(msg, objs);
        }
        /// <summary>
        /// 循环从start到end 闭区间
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool Range(this int start, int end, Func<int, bool> func)
        {
            if (start < end)
            {
                for (int i = start; i <= end; i++)
                {
                    if (func?.Invoke(i) == false)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i = start; i >= end; i--)
                {
                    if (func?.Invoke(i) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 集合转换到字符串
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="center">间隔符</param>
        /// <returns></returns>
        public static string JoinToString(this IEnumerable enumerable, string center)
        {
            string msg = "";
            foreach (var item in enumerable)
            {
                msg += item.ToString() + center;
            }
            return msg;
        }
        /// <summary>
        /// 集合转换到字符串
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="enumerable">集合</param>
        /// <param name="center">间隔符</param>
        /// <param name="func">转换到字符串的处理函数</param>
        /// <returns></returns>
        public static string JoinToString<T>(this IEnumerable<T> enumerable, string center, Func<T, string> func)
        {
            string msg = "";
            foreach (var item in enumerable)
            {
                msg += func?.Invoke(item) + center;
            }
            return msg;
        }
        /// <summary>
        /// 集合转换到字符串
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="enumerable">集合</param>
        /// <param name="center">间隔符</param>
        /// <param name="func">转换到字符串的处理函数</param>
        /// <returns></returns>
        public static string JoinToString<T>(this IEnumerable enumerable, string center, Func<T, string> func)
        {
            string msg = "";
            foreach (var item in enumerable)
            {
                msg += func?.Invoke((T)item) + center;
            }
            return msg;
        }
        /// <summary>
        /// 读取序号中的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="pairs"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static KeyValuePair<T, V> GetIndex<T, V>(this IDictionary<T, V> pairs, int index)
        {
            int idx = 0;
            foreach (var item in pairs)
            {
                if (idx == index)
                {
                    return item;
                }
                idx++;
            }
            throw new Exception("index out of range");
        }
        /// <summary>
        /// 匹配value与后续所有的参数，如果正确匹配到条件则回调equalevent
        /// </summary>
        /// <param name="value"></param>
        /// <param name="equalevent">返回false则终止循环</param>
        /// <param name="args"></param>
        public static bool EqualLists(this string value, Func<string, bool> equalevent, params string[] args)
        {
            return args.Foreach(t => {
                string tv = t as string;
                if (value.Equals(tv))
                {
                    return equalevent(tv);
                }
                return true;
            });
        }
        /// <summary>
        /// 枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        /// <param name="func"></param>
        /// <returns>返回enumerator可让表达式继续</returns>
        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> enumerator, Action<T> func)
        {
            foreach (var item in enumerator)
            {
                func?.Invoke(item);
            }
            return enumerator;
        }
        /// <summary>
        /// 枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        /// <param name="func"></param>
        /// <returns>返回是否全部遍历</returns>
        public static bool Foreach<T>(this IEnumerable<T> enumerator, Func<T, bool> func)
        {
            foreach (var item in enumerator)
            {
                if (!func(item))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 转换集合类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="enumerator"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<V> ChangedList<T, V>(this IEnumerable<T> enumerator, Func<T, V> func)
        {
            List<V> vs = new List<V>();

            enumerator.Foreach(t => {
                vs.Add(func(t));
            });
            return vs;
        }
        /// <summary>
        /// 列举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        public static void Foreach<T, V>(this IDictionary<T, V> enumerator, Action<T, V> func)
        {
            foreach (var item in enumerator)
            {
                func?.Invoke(item.Key, item.Value);
            }
        }
        /// <summary>
        /// 列举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        public static void Foreach(this IEnumerable enumerator, Action<object> func)
        {
            foreach (var item in enumerator)
            {
                func?.Invoke(item);
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<T> Where<T, V>(this IDictionary<T, V> enumerator,
            Func<T, V, bool> func)
        {
            List<T> ts = new List<T>();
            enumerator.Foreach((c, v) => {
                if (func?.Invoke(c, v) == true)
                {
                    ts.Add(c);
                }
            });
            return ts;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<T> Where<T>(this IEnumerable enumerator, Func<object, bool> func)
        {
            List<T> ts = new List<T>();
            enumerator.Foreach(_ => {
                if (func?.Invoke(_) == true)
                {
                    ts.Add((T)_);
                }
            });
            return ts;
        }
        /// <summary>
        /// 转到到对应的枚举类型的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetEnumType<T>(this string name)
        {
            return Enum.GetValues(typeof(T)).Where<T>(c => c.ToString().Equals(name)).First();
        }
        /// <summary>
        /// /获取美剧类型中的所有的值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string[] GetEnumTypes(this Type type)
        {
            List<string> vs = new List<string>();
            foreach (var item in Enum.GetValues(type))
            {
                vs.Add(item.ToString());
            }
            return vs.ToArray();
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        public static void ToFile(this object obj, string path, bool isSaveType = true)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            if (isSaveType)
            {
                setting.TypeNameHandling = TypeNameHandling.All;
            }
            System.IO.File.WriteAllBytes(path, Newtonsoft.Json.JsonConvert.SerializeObject(obj, setting).ToBytes());
        }
        /// <summary>
        /// 从文件读取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T ReadFromFile<T>(this string path, bool isReadType = true)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            if (isReadType)
            {
                setting.TypeNameHandling = TypeNameHandling.All;
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(
                System.Text.Encoding.UTF8.GetString(
                    System.IO.File.ReadAllBytes(path)), setting);
        }
        /// <summary>
        /// 从文件读取文本内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFromFileText(this string path)
        {
            return System.Text.Encoding.UTF8.GetString(
                    System.IO.File.ReadAllBytes(path));
        }
        /// <summary>
        /// 字符串转换到字节
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string msg)
        {
            return System.Text.Encoding.UTF8.GetBytes(msg);
        }
        /// <summary>
        /// 获取枚举类型T中的attribute属性V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public static IEnumerable<V> GetEnumModeType<T, V>()
        {
            List<V> vs = new List<V>();
            Type t = typeof(T);
            Array arrays = Enum.GetValues(t);
            for (int i = 0; i < arrays.LongLength; i++)
            {
                T test = (T)arrays.GetValue(i);
                FieldInfo fieldInfo = test.GetType().GetField(test.ToString());
                object[] attribArray = fieldInfo.GetCustomAttributes(false);
                foreach (var item in attribArray)
                {
                    if (item is V)
                    {
                        vs.Add((V)item);
                        break;
                    }
                }
            }
            return vs;
        }

        /// <summary>
        /// 获取value的枚举类型的注释名称,value必须是枚举类型，并且有Attribute的注释才能获取
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetEnumTypeModeName<T>(this object value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            object[] attribArray = fieldInfo.GetCustomAttributes(false);
            foreach (var item in attribArray)
            {
                if (item is T)
                {
                    return (T)item;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 转换到基础类型
        /// </summary>
        /// <typeparam name="T">具体的基础类型</typeparam>
        /// <param name="baseobj">待转换的对象</param>
        /// <returns></returns>
        public static T ToBaseType<T>(this object baseobj)
        {
            return (T)Convert.ChangeType(baseobj, typeof(T));
        }

        /// <summary>
        /// double类型保留小数
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="format">保留小数的形式</param>
        /// <returns></returns>
        public static double ToSavePoint(this double value, string format)
        {
            return value.ToString(format).ToBaseType<double>();
        }

        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool StringIsNumber(this string msg)
        {
            const string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(msg); //bool类型
        }
        /// <summary>
        /// 转到base64编码
        /// </summary>
        /// <param name="info"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ToBase64(this string info, Encoding encoding = null)
        {
            return Convert.ToBase64String(encoding == null ? System.Text.Encoding.UTF8.GetBytes(info) : encoding.GetBytes(info));
        }
        /// <summary>
        /// 从base64字符串转换回来
        /// </summary>
        /// <param name="info"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string FromBase64(this string info, Encoding encoding = null)
        {
            byte [] datas = Convert.FromBase64String(info);
            return encoding == null ? System.Text.Encoding.UTF8.GetString(datas) : encoding.GetString(datas);
        }
        /// <summary>
        /// 路径拼接
        /// </summary>
        /// <param name="root"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Combine(this string root, params string [] path)
        {
            List<string> ps = new List<string>();
            ps.Add(root);
            ps.AddRange(path);
            return System.IO.Path.Combine(ps.ToArray());
        }

        /// <summary>
        /// 修复版本号显示
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static string FlushVersion(this string version)
        {
            string newVersion = "";
            string[] versions = version.Split('.');
            foreach (var item in versions)
            {
                if (item.Length == 1)
                {
                    newVersion += "0" + item + ".";
                }
                else if (item.Length == 2)
                {
                    newVersion += item + ".";
                }
                else
                {
                    return null;
                }
            }

            newVersion = newVersion.Substring(0, newVersion.Length - 1);
            return newVersion;
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static long GetFileSize(this string filename)
        {
            FileInfo file = new FileInfo(filename);
            return file.Length;
        }
        /// <summary>
        /// 转换到版本号
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static Version ToVersion(this string version)
        {
            return Version.Parse(version);
        }
        /// <summary>
        /// 从项目根路径开始拼接
        /// </summary>
        /// <param name="path"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string FromProjectRootCombine(this string path, params string [] paths)
        {
            return AppContext.BaseDirectory.Combine(path).Combine(paths);
        }
    }
}
