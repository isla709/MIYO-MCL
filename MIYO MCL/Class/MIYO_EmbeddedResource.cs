using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MIYO_MCL.Class
{
    /// <summary>
    /// 嵌入式资源管理类
    /// </summary>
    class MIYO_EmbeddedResource
    {
        /// <summary>
        /// 加载嵌入式资源流
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Stream? LoadAssetStream(string path)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream(path);
        }

        /// <summary>
        /// 解析流到String
        /// </summary>
        /// <param name="AssetStream"></param>
        /// <returns></returns>
        public static string? ParseAssetStreamToString(Stream AssetStream) 
        {
            if (AssetStream != null)
            {
                try 
                {
                    using (StreamReader Reader = new StreamReader(AssetStream))
                    {
                        return Reader.ReadToEnd();
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Message:" + ex.Message + "  Source:" + ex.Source, "Error");
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// 解析流到BitmapImage
        /// </summary>
        /// <param name="AssetStream"></param>
        /// <returns></returns>
        public static BitmapImage? ParseAssetStreamToBitmap(Stream AssetStream)
        {
            if (AssetStream != null)
            {
                try
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = AssetStream;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.EndInit();
                    return bitmapImage;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Message:" + ex.Message + "  Source:" + ex.Source, "Error");
                    return null;
                }
            }
            return null;
        }

    }
}
