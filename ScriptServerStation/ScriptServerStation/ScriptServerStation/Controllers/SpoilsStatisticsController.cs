using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataBaseController.Entitys;
using Microsoft.AspNetCore.Mvc;
using ScriptServerStation.HelpClasses;
using ScriptServerStation.Service;
using CoreHelper.Expends;

namespace ScriptServerStation.Controllers
{
    [Route("api/SpoilsStatistics")]
    public class SpoilsStatisticsController : Controller
    {
        /// <summary>
        /// 服务
        /// </summary>
        private ISpoilsStatisticsService spoilsStatisticsService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="spoilsStatistics"></param>
        public SpoilsStatisticsController(ISpoilsStatisticsService spoilsStatistics)
        {
            this.spoilsStatisticsService = spoilsStatistics;
        }

        [HttpPost("add")]
        public ReturnObj Add(string spoilsStatistics)
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                SpoilsStatistics spoils = Newtonsoft.Json.JsonConvert.DeserializeObject<SpoilsStatistics>(spoilsStatistics);
                spoilsStatisticsService.Add(spoils);
                returnObj.SetIsSuccess(true);
            }
            catch (Exception)
            {
                returnObj.SetIsSuccess(false);
            }
            return returnObj;
        }
        /// <summary>
        /// 查询单个类型的出货率
        /// </summary>
        /// <param name="from">起始时间</param>
        /// <param name="to">结束时间</param>
        /// <param name="type">类型</param>
        /// <param name="model">模式</param>
        /// <returns></returns>
        [HttpPost("querytype")]
        public ReturnObj QueryShipmentRate(string from, string to, int type, int model)
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                if (model == 1)
                    returnObj.Result = spoilsStatisticsService.ShipmentRate(from.ToDateTime(),
                        to.ToDateTime(), type);
                else if (model == 2)
                    returnObj.Result = spoilsStatisticsService.ShipmentRateEveryDay(from.ToDateTime(),
                        to.ToDateTime(), type);
                returnObj.SetIsSuccess(true);
            }
            catch (Exception)
            {
                returnObj.SetIsSuccess(false);
            }
            return returnObj;
        }

        [HttpPost("queryall")]
        public ReturnObj QueryShipmentRateAll(string from, string to, int model)
        {
            ReturnObj returnObj = new ReturnObj();
            try
            {
                if(model == 1)
                    returnObj.Result = spoilsStatisticsService.ShipmentRateAll(from.ToDateTime(),
                        to.ToDateTime());
                else if (model == 2)
                    returnObj.Result = spoilsStatisticsService.ShipmentRateAllEveryDay(from.ToDateTime(),
                        to.ToDateTime());
                returnObj.SetIsSuccess(true);
            }
            catch (Exception)
            {
                returnObj.SetIsSuccess(false);
            }
            return returnObj;
        }
    }
}
