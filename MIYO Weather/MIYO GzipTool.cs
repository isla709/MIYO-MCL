using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Compression;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace MIYO_Weather
{
    public static class MIYO_GzipTool
    {

        public static async Task<string> DezipToString(Stream data, Encoding encoding)
        {
            try 
            {
                using (GZipStream gZipStream = new GZipStream(data, CompressionMode.Decompress))
                {
                    using (StreamReader streamReader = new StreamReader(gZipStream, encoding))
                    {
                        return await streamReader.ReadToEndAsync();
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Message:" + ex.Message + "  Source:" + ex.Source, "Error"); 
                return null;
            }
        }






    }
}
