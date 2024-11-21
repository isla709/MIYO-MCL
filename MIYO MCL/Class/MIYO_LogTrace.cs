using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MIYO_MCL.Class
{
    /// <summary>
    /// MIYOMCL 日志管理类
    /// </summary>
    internal static class MIYO_LogTrace
    {
        /// <summary>
        /// 日志文件夹路径
        /// </summary>
        private static string LogPath = "./MIYOMCL/Log";

        /// <summary>
        /// 日志文件名
        /// </summary>
        private static string LogFileName = "LatestLog.txt";

        private static string GetFullPath() 
        {
            return LogPath + "/" +LogFileName;
        }

        /// <summary>
        /// 初始化日志
        /// </summary>
        public static async void InitLogTrace()
        {

            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
            }

            if (File.Exists(GetFullPath()))
            {
                string logfile = await File.ReadAllTextAsync(GetFullPath());

                string pattern = @"Current Log Create Time:(.+)";
                Match match = Regex.Match(logfile, pattern);

                if (match.Success)
                {

                    File.Move(GetFullPath(), LogPath + "/" + match.Groups[1].Value.Trim().Replace("/", ".").Replace(":", ".") + ".txt");

                }
                else 
                {
                    File.Move(GetFullPath(), LogPath + "/" + new Random().NextInt64().ToString("x2") + ".txt");

                }

                
            }
            
            Trace.Listeners.Add( new ConsoleTraceListener());
            Trace.Listeners.Add(new TextWriterTraceListener(GetFullPath()));
            Trace.AutoFlush = true;

            Trace.WriteLine(MIYO_ASCIIArt.Type01);
            Trace.WriteLine("Current Log Create Time:" + DateTime.Now.ToString("G"),"INFO");
            
        }

        /// <summary>
        /// 结束日志
        /// </summary>
        public static void EndLogTrace()
        {
            if(File.Exists(GetFullPath()))
            {
                Trace.WriteLine("Current Log End Time:" + DateTime.Now.ToString("G"), "INFO");
            }
        }

    }
}
