using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MIYO_Weather
{
    public static class MIYO_HTTPRequest
    {

        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <typeparam name="Type"> 返回值类型，可选项: string, Stream, byte[] </typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<Type> GetAsync<Type>(string url) 
        {

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage recvMessage = await client.GetAsync(url);
                recvMessage.EnsureSuccessStatusCode();

                if (typeof(Type) == typeof(string))
                {
                    return (Type)(object) await recvMessage.Content.ReadAsStringAsync();
                }
                else if (typeof(Type) == typeof(Stream))
                {
                    return (Type)(object) await recvMessage.Content.ReadAsStreamAsync();
                }
                else if (typeof(Type) == typeof(byte[]))
                { 
                    return (Type)(object) await recvMessage.Content?.ReadAsByteArrayAsync();
                }
                else
                {
                    throw new Exception("Get没有查找到合适的类型对于泛型类型");
                }
                
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Message:" + ex.Message + "  Source:" + ex.Source, "Error");
                return default;
            }

        }



    }
}
