using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScriptServerStation.Database;
using ScriptServerStation.HelpClasses;
using ScriptServerStation.Items;
using ScriptServerStation.Service;

namespace ScriptServerStation.Controllers
{
    [ApiController]
    [Route("api/SpoilsStatistics")]
    public class SpoilsStatisticsController : ControllerBase
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
        public Result Add(string spoilsStatistics)
        {
            Result Result = new Result();
            try
            {
                Spoils spoils = Newtonsoft.Json.JsonConvert.DeserializeObject<Spoils>(spoilsStatistics);
                spoilsStatisticsService.Add(spoils);
            }
            catch (Exception)
            {
                Result.SetFail();
            }
            return Result;
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
        public Result QueryShipmentRate(DateTime from, DateTime to, int type, int model)
        {
            Result Result = new Result();
            try
            {
                if (model == 1)
                    Result.Data = spoilsStatisticsService.ShipmentRate(from,
                        to, type);
                else if (model == 2)
                    Result.Data = spoilsStatisticsService.ShipmentRateEveryDay(from,
                        to, type);
            }
            catch (Exception)
            {
                Result.SetCode(ResultCode.Fail);
            }
            return Result;
        }

        [HttpPost("queryall")]
        public Result QueryShipmentRateAll(DateTime from, DateTime to, int model)
        {
            Result Result = new Result();
            try
            {
                if(model == 1)
                    Result.Data = spoilsStatisticsService.ShipmentRateAll(from,
                        to);
                else if (model == 2)
                    Result.Data = spoilsStatisticsService.ShipmentRateAllEveryDay(from,
                        to);
            }
            catch (Exception)
            {
                Result.SetFail();
            }
            return Result;
        }
    }
}
