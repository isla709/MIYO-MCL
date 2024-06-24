using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIYO_MCL.Class
{
    public struct AppConfigData
    {
        public string ConfigVersion;
        public string QWeatherKey;
        public string CityID;
        public string Theme;
        public string BackgroundMain;
        public string CustomImage;
        public string AccountSaveMode;
        public string LastSelectionUuid;
    }


    public class MIYO_ConfigManager
    {

        public string AssemblyAsssetFilePath = "MIYO_MCL.Config.AppConfig.json";

        public string configPath = "./MIYOMCL/Config";

        public string configFileName = "AppConfig.json";

        public MIYO_ConfigManager() { } 

        public MIYO_ConfigManager(string p_configFileName, string p_configPath = "./MIYOMCL/Config", string p_AssemblyAsssetFilePath = "/Config")
        {
            configFileName = p_configFileName;
            configPath = p_configPath;  
            AssemblyAsssetFilePath = p_AssemblyAsssetFilePath;
        }

        private string GetFullPath()
        {
            return configPath + "/" + configFileName;
        }

        public bool CheckFile()
        {
            if (File.Exists(GetFullPath()))
            {
                return true;
            }
            else 
            {
                return false;
            }
        }

        public void VerifyFile()
        {
            if (CheckFile())
            {
                return;
            }

            Trace.WriteLine("未找到" + configFileName, "Warning");
            
            if (!Directory.Exists(configPath))
            {
                Directory.CreateDirectory(configPath);
            }

            if (!File.Exists(GetFullPath()))
            {
                using Stream? AsssetStream = MIYO_EmbeddedResource.LoadAssetStream(AssemblyAsssetFilePath);
                
                if (AsssetStream != null)
                {
                    string? FileContent = MIYO_EmbeddedResource.ParseAssetStreamToString(AsssetStream);
           
                    File.WriteAllTextAsync(GetFullPath(), FileContent);
                }
                else
                {
                    Trace.WriteLine("内部资源读取失败，这会导致配置文件无法正确加载!", "Error");
                }
            }

            VerifyFile();

        }

        public string ReadConfigFile() 
        {
            if (!File.Exists(GetFullPath()))
            {
                throw new FileNotFoundException("ConfigFile Not Exists");
            }

            return File.ReadAllText(GetFullPath());
        }

        public void SerializationAndWriteConfigFile(object data)
        {
            if (!File.Exists(GetFullPath()))
            {
                throw new FileNotFoundException("ConfigFile Not Exists");
            }

            File.WriteAllText(GetFullPath(), JsonConvert.SerializeObject(data,Formatting.Indented));
        }

        public AppConfigData DeserializationAppConifgJson(string AppConfig_Json)
        { 
            return JsonConvert.DeserializeObject<AppConfigData>(AppConfig_Json);
        }



        

    }
}
