using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScriptServerStation.Database;

namespace ScriptServerStation.Service
{
    public interface ISpoilsStatisticsService
    {
        /// <summary>
        /// 添加一个战利品统计
        /// </summary>
        /// <param name="spoilsStatistics"></param>
        /// <returns></returns>
        bool Add(Spoils spoilsStatistics);
        /// <summary>
        /// 指定时间段内的指定类型的出货率
        /// </summary>
        /// <param name="from">起始时间</param>
        /// <param name="to">结束时间</param>
        /// <param name="type">类型</param>
        /// <returns>出货率</returns>
        double ShipmentRate(DateTime from, DateTime to, int type);
        /// <summary>
        /// 指定只剪短内所有类型的出货率
        /// </summary>
        /// <param name="from">起始时间</param>
        /// <param name="to">结束时间</param>
        /// <returns></returns>
        Dictionary<int, double> ShipmentRateAll(DateTime from, DateTime to);
        /// <summary>
        /// 指定时间段内指定类型每天的出货率
        /// </summary>
        /// <param name="from">起始时间</param>
        /// <param name="to">结束时间</param>
        /// <param name="type">类型</param>
        /// <returns>出货率</returns>
        List<double> ShipmentRateEveryDay(DateTime from, DateTime to, int type);
        /// <summary>
        /// 指定只剪短内所有类型的出货率
        /// </summary>
        /// <param name="from">起始时间</param>
        /// <param name="to">结束时间</param>
        /// <returns></returns>
        List<Dictionary<int, double>> ShipmentRateAllEveryDay(DateTime from, DateTime to);
    }
}
