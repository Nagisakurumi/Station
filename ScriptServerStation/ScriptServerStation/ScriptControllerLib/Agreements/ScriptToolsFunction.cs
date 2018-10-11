using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ScriptControllerLib.Agreements
{
    public class ScriptToolsFunction
    {
        [ScriptMeth("",
            "{'parameter':[" +
            "{'name':'NowTime','type':'DATETIME','defult':'','enumdatas':'','tiptext':'返回当前执行时候的时间'}" +
            "]}",
            "获取当前时间", functionName: "获取当前时间", ishasInput: false, ishasOutput: false, itemBoxEnum: ItemBoxEnum.FUNCTION)]
        public static ScriptOutput GetNowTime(ScriptInput si)
        {
            ScriptOutput so = new ScriptOutput();
            so.SetValue("NowTime", DateTime.Now);
            return so;
        }

        [ScriptMeth("{'parameter':[" +
            "{'name':'value1','type':'STRING','defult':'0','enumdatas':'','tiptext':'数1'}," +
            "{'name':'option','type':'ENUM','defult':'>','enumdatas':{'type':'value','value':'>,<,=,>=,<='},'tiptext':'进行比较的类型'}," +
            "{'name':'value2','type':'STRING','defult':'0','enumdatas':'','tiptext':'数2'}" +
            "]}",
            "{'parameter':[" +
            "{'name':'result','type':'BOOL','defult':'','enumdatas':'','tiptext':'比较的结果'}" +
            "]}",
            "比较函数", functionName: "比较")]
        public static ScriptOutput ValueEquals(ScriptInput si)
        {
            bool result = false;
            try
            {
                float int1 = Convert.ToInt32(si.GetValue("value1"));
                float int2 = Convert.ToInt32(si.GetValue("value2"));
                string option = Convert.ToString(si.GetValue("option"));
                switch (option)
                {
                    case ">":
                        result = int1 > int2;
                        break;
                    case "<":
                        result = int1 < int2;
                        break;
                    case "=":
                        result = int1 == int2;
                        break;
                    case ">=":
                        result = int1 >= int2;
                        break;
                    case "<=":
                        result = int1 <= int2;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                string int1 = Convert.ToString(si.GetValue("value1"));
                string int2 = Convert.ToString(si.GetValue("value2"));
                result = int1.Equals(int2);
            }
            ScriptOutput so = new ScriptOutput();
            so.SetValue("result", result);
            return so;
        }
        [ScriptMeth("{'parameter':[" +
            "{'name':'obj','type':'OBJECT','defult':'','enumdatas':'','tiptext':'对象'}," +
            "{'name':'message','type':'STRING','defult':'','enumdatas':'','tiptext':'文本'}" +
            "]}",
            "",
            "输出对象", functionName: "打印日志")]
        public static ScriptOutput PrintObject(ScriptInput si)
        {
            ScriptOutput scriptOutput = new ScriptOutput();
            object obj = si.GetValue("obj") as object;
            string message = si.GetValue("message").ToString();
            string content = "";
            content += obj == null ? "" : obj.ToString();
            content += message;
            scriptOutput.Write(content);
            return scriptOutput;
        }
        [ScriptMeth("{'parameter':[" +
            "{'name':'time1','type':'OBJECT','defult':1,'enumdatas':'','tiptext':'加数1'}," +
            "{'name':'time2','type':'OBJECT','defult':1,'enumdatas':'','tiptext':'加数2'}" +
            "]}",
            "{'parameter':[" +
            "{'name':'TotalDays','type':'FLOAT','defult':'','enumdatas':'','tiptext':'总天数'}," +
            "{'name':'TotalHours','type':'FLOAT','defult':'','enumdatas':'','tiptext':'总小时'}," +
            "{'name':'TotalMinutes','type':'FLOAT','defult':'','enumdatas':'','tiptext':'总分钟'}," +
            "{'name':'TotalSeconds','type':'FLOAT','defult':'','enumdatas':'','tiptext':'总秒数'}," +
            "{'name':'TotalMilliseconds','type':'FLOAT','defult':'','enumdatas':'','tiptext':'总毫秒数'}" +
            "]}",
            "time1 - time2", functionName: "时间相减")]
        public static ScriptOutput TimeDesc(ScriptInput si)
        {
            DateTime time1 = Convert.ToDateTime(si.GetValue("time1"));
            DateTime time2 = Convert.ToDateTime(si.GetValue("time2"));

            ScriptOutput so = new ScriptOutput();
            TimeSpan ts = time1 - time2;
            so.SetValue("TotalHours", ts.TotalHours);
            so.SetValue("TotalMilliseconds", ts.TotalMilliseconds);
            so.SetValue("TotalMinutes", ts.TotalMinutes);
            so.SetValue("TotalSeconds", ts.TotalSeconds);
            so.SetValue("TotalDays", ts.TotalDays);
            return so;
        }

        /// <summary>
        /// 延迟函数
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [ScriptMeth("{'parameter':[" +
            "{'name':'time','type':'INT','defult':1000,'enumdatas':'','tiptext':'超时时间'}" +
            "]}",
            "",
            "延迟函数", functionName: "延迟函数,Sleep")]
        public static ScriptOutput DelyTime(ScriptInput idx)
        {
            Thread.Sleep(Convert.ToInt32(idx.GetFirst()));
            return null;
        }
        [ScriptMeth("{'parameter':[" +
            "{'name':'value1','type':'STRING','defult':'0','enumdatas':'','tiptext':'数1'}," +
            "{'name':'option','type':'ENUM','defult':'>','enumdatas':{'type':'value','value':'+,-'},'tiptext':'进行操作的类型'}," +
            "{'name':'value2','type':'STRING','defult':'0','enumdatas':'','tiptext':'数2'}" +
            "]}",
            "{'parameter':[" +
            "{'name':'result','type':'STRING','defult':'','enumdatas':'','tiptext':'操作的结果'}" +
            "]}",
            "数值操作", functionName: "数值操作")]
        public static ScriptOutput Options(ScriptInput si)
        {
            float result = 0;
            float int1 = Convert.ToInt32(si.GetValue("value1"));
            float int2 = Convert.ToInt32(si.GetValue("value2"));
            string option = Convert.ToString(si.GetValue("option"));
            switch (option)
            {
                case "+":
                    result = int1 + int2;
                    break;
                case "-":
                    result = int1 - int2;
                    break;
                default:
                    break;
            }
            ScriptOutput so = new ScriptOutput();
            so.SetValue("result", result);
            return so;
        }
        /// <summary>
        /// 设置一个值
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [ScriptMeth("{'parameter':[" +
            "{'name':'value','type':'STRING','defult':1,'enumdatas':'','tiptext':'设置值'}" +
            "]}",
            "{'parameter':[" +
            "{'name':'result','type':'STRING','defult':'','enumdatas':'','tiptext':'设置的值'}" +
            "]}",
            "设置一个值", functionName: "设置一个值")]
        public static ScriptOutput SetValue(ScriptInput idx)
        {
            ScriptOutput so = new ScriptOutput();
            so.SetValue("result", idx.GetValue("value"));
            return so;
        }


    }
}
