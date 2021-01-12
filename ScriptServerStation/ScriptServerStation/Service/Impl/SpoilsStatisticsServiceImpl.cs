using ScriptServerStation.Database;
using ScriptServerStation.Expends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Service.Impl
{
    public class SpoilsStatisticsServiceImpl : ISpoilsStatisticsService
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public DataBaseContext DataBaseContext { get; set; }
        /// <summary>
        /// 添加一条战利品统计
        /// </summary>
        /// <param name="spoilsStatistics">战利品数据</param>
        /// <returns></returns>
        public bool Add(Spoils spoilsStatistics)
        {
            spoilsStatistics.CreateTime = DateTime.Now;
            this.DataBaseContext.Spoils.Add(spoilsStatistics);
            this.DataBaseContext.SaveChanges();
            return true;
        }
        /// <summary>
        /// 指定时间段内的指定类型的出货率
        /// </summary>
        /// <param name="from">起始时间</param>
        /// <param name="to">结束时间</param>
        /// <param name="type">类型</param>
        /// <returns>出货率</returns>
        public double ShipmentRate(DateTime fromdate, DateTime to, int type)
        {
            var list = from p in this.DataBaseContext.Spoils
                       where
                       p.CreateTime <= to && p.CreateTime >= fromdate
                       select p;
            if (list.Count() == 0)
                return 0;
            return ((double)(from p in list where p.Type == type select p).Count()) / ((double)list.Count());
        }
        /// <summary>
        /// 指定只剪短内所有类型的出货率
        /// </summary>
        /// <param name="from">起始时间</param>
        /// <param name="to">结束时间</param>
        /// <returns></returns>
        public Dictionary<int, double> ShipmentRateAll(DateTime fromdate, DateTime to)
        {
            var list = from p in this.DataBaseContext.Spoils where
                       p.CreateTime <= to && p.CreateTime >= fromdate
                       select p;
            if (list.Count() == 0)
                return null;
            Dictionary<int, double> result = new Dictionary<int, double>();
            list.Foreach<Spoils>(c => {
                if (result.ContainsKey(c.Type))
                    result[c.Type]++;
                else
                    result.Add(c.Type, 1);
            });
            double allcount = list.Count();
            result.Foreach<KeyValuePair<int, double>>(c=>{
                result[c.Key] = c.Value / allcount;
            });
            return result;
        }
        /// <summary>
        /// 指定时间段内指定类型每天的出货率
        /// </summary>
        /// <param name="from">起始时间</param>
        /// <param name="to">结束时间</param>
        /// <param name="type">类型</param>
        /// <returns>出货率</returns>
        public List<Dictionary<int, double>> ShipmentRateAllEveryDay(DateTime from, DateTime to)
        {
            List<Dictionary<int, double>> result = new List<Dictionary<int, double>>();
            for (DateTime i = from; i < to; i += TimeSpan.FromDays(1))
            {
                result.Add(ShipmentRateAll(i, i + TimeSpan.FromDays(1)));
            }
            return result;
        }
        /// <summary>
        /// 指定只剪短内所有类型的出货率
        /// </summary>
        /// <param name="from">起始时间</param>
        /// <param name="to">结束时间</param>
        /// <returns></returns>
        public List<double> ShipmentRateEveryDay(DateTime from, DateTime to, int type)
        {
            List<double> result = new List<double>();
            for (DateTime i = from; i < to; i += TimeSpan.FromDays(1))
            {
                result.Add(ShipmentRate(i, i + TimeSpan.FromDays(1), type));
            }
            return result;
        }

    }
}
