using MinecraftLaunch.Classes.Models.Auth;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MinecraftLaunch.Classes.Enums;

namespace MIYO_MCL.Class
{

    public struct OfflineAccountData
    {
        public AccountType Type { get; set; }

        public string Name { get; set; }

        public Guid Uuid { get; set; }

        public string AccessToken { get; set; }



    }

    public struct AccountData
    {
        public List<MicrosoftAccount> MicrosoftAccounts;
        public List<OfflineAccount> OfflineAccounts;
        public List<YggdrasilAccount> YggdrasilAccounts;
    }



    public class MIYO_BSMCLUserManager
    {
       

        public string UnsafeAccountPath = ".\\MIYOMCL\\Account";

        public string safeAccountPath;

        public string AccountFileName = "Account.json";

        public bool SafeModeStatus = true;

        public MIYO_BSMCLUserManager() 
        {
            try 
            {
                safeAccountPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\MIYOMCL";
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex);
            }
            
        }

        public string GetFullPath(bool UseSafeMode)
        {
            if (UseSafeMode) 
            {
                return safeAccountPath + "\\" + AccountFileName;
            }
            return UnsafeAccountPath + "\\" + AccountFileName;
        }

        public bool CheckFile(bool UseSafeMode)
        {
            if (File.Exists(GetFullPath(UseSafeMode)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void VerifyFile(bool UseSafeMode)
        {
            if (CheckFile(UseSafeMode))
            {
                return;
            }

            Trace.WriteLine("未找到" + AccountFileName, "Warning");

            if (UseSafeMode)
            {
                if (!Directory.Exists(safeAccountPath))
                {
                    Directory.CreateDirectory(safeAccountPath);
                }
            }
            else 
            {
                if (!Directory.Exists(UnsafeAccountPath))
                {
                    Directory.CreateDirectory(UnsafeAccountPath);
                }
            }

            if (!File.Exists(GetFullPath(UseSafeMode)))
            {
                File.WriteAllText(GetFullPath(UseSafeMode), "{\r\n  \"MicrosoftAccounts\": [],\r\n  \"OfflineAccounts\": [],\r\n  \"YggdrasilAccounts\": []\r\n}");
            }

        }

        public void SerializationUserToFile(AccountData accountlist,bool UseSafeMode)
        {
            File.WriteAllTextAsync(GetFullPath(UseSafeMode), SerializationUser(accountlist));
        }

        public static string SerializationUser(AccountData accountlist)
        {
            return JsonConvert.SerializeObject(accountlist,Formatting.Indented);
        }

        public async Task<AccountData> DeSerializationUser(bool UseSafeMode)
        {
            string jsondata = await File.ReadAllTextAsync(GetFullPath(UseSafeMode));
            return JsonConvert.DeserializeObject<AccountData>(jsondata);
        }




    }
}
