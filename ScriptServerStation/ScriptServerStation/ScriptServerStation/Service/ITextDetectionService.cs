using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptServerStation.Service
{
    public interface ITextDetectionService
    {
        /// <summary>
        /// 图片中识别文字
        /// </summary>
        /// <param name="imagedatas">图片数据</param>
        /// <returns></returns>
        string TextDetection(byte[] imagedatas);
        /// <summary>
        /// 图片中识别文字
        /// </summary>
        /// <param name="imagedatas">图片数据</param>
        /// <returns></returns>
        string[] TextDetections(byte[] imagedatas);
        /// <summary>
        /// 图片分类
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        int ImageDetection(Stream stream);
    }
}
