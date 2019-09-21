using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBaseController;
using DataBaseController.Entitys;
using CoreHelper.Expends;

namespace ScriptServerStation.Service.Impl
{
    public class SpoilsStatisticsServiceImpl : BaseService, ISpoilsStatisticsService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataBaseContext"></param>
        public SpoilsStatisticsServiceImpl(DataBaseContext DataBaseContext) : base(DataBaseContext)
        {
        }
        /// <summary>
        /// 添加一条战利品统计
        /// </summary>
        /// <param name="spoilsStatistics">战利品数据</param>
        /// <returns></returns>
        public bool Add(SpoilsStatistics spoilsStatistics)
        {
            spoilsStatistics.Id = Guid.NewGuid().ToString();
            spoilsStatistics.Date = DateTime.Now.ToString();
            this.DataBaseContext.SpoilsStatistics.Add(spoilsStatistics);
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
            var list = from p in this.DataBaseContext.SpoilsStatistics where
                       Convert.ToDateTime(p.Date) <= to && Convert.ToDateTime(p.Date) >= fromdate
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
            var list = from p in this.DataBaseContext.SpoilsStatistics where
                       Convert.ToDateTime(p.Date) <= to && Convert.ToDateTime(p.Date) >= fromdate
                       select p;
            if (list.Count() == 0)
                return null;
            Dictionary<int, double> result = new Dictionary<int, double>();
            list.ForEach<SpoilsStatistics>(c => {
                if (result.ContainsKey(c.Type))
                    result[c.Type]++;
                else
                    result.Add(c.Type, 1);
            });
            double allcount = list.Count();
            result.ForEach<KeyValuePair<int, double>>(c=>{
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
