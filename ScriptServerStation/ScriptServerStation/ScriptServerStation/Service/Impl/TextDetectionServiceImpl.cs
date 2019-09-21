using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataBaseController;
using NetBase;

namespace ScriptServerStation.Service.Impl
{
    public class TextDetectionServiceImpl : BaseService, ITextDetectionService
    {
        /// <summary>
        /// 百度文字识别api必要信息
        /// </summary>
        private static string apikey = "keUXVAzcNZCr1gkQYu1iuzV5";
        private static string clientSecret = "YljZnbSQxMauHIeCl2oGpEB9eFwz8aeQ ";
        private static string url = @"https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic";
        private Baidu.Aip.Ocr.Ocr client = new Baidu.Aip.Ocr.Ocr(apikey, clientSecret);

 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="DataBaseContext"></param>
        public TextDetectionServiceImpl(DataBaseContext DataBaseContext) : base(DataBaseContext)
        {
            client.Timeout = 60000;
        }
        /// <summary>
        /// 图片分类
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public int ImageDetection(Stream stream)
        {
            throw new Exception("此方法暂时关闭");
        }

        /// <summary>
        /// 图片中文字识别
        /// </summary>
        /// <param name="imagedatas"></param>
        /// <returns></returns>
        public string TextDetection(byte [] imagedata)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<BaiduResult>(client.GeneralBasic(imagedata).ToString()).words_result[0].words;
        }
        /// <summary>
        /// 识别图片中的所有文字
        /// </summary>
        /// <param name="imagedata"></param>
        /// <returns></returns>
        public string[] TextDetections(byte[] imagedata)
        {
            BaiduResult baiduResult = Newtonsoft.Json.JsonConvert.DeserializeObject<BaiduResult>(client.GeneralBasic(imagedata).ToString());
            List<string> msgs = new List<string>();
            baiduResult.words_result.ForEach(c => msgs.Add(c.words));
            return msgs.ToArray();
        }

        protected class BaiduResult
        {
            public string log_id { get; set; }
            public string words_result_num { get; set; }
            public List<Item> words_result { get; set; }
        }
        protected class Item
        {
            public string words { get; set; }
        }
    }
}
