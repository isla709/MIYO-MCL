using MinecraftLaunch;
using MinecraftLaunch.Classes.Models.Auth;
using MinecraftLaunch.Components.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Panuon.WPF.UI;
using System.Windows;
using System.Security.Principal;

namespace MIYO_MCL.Class
{
    public class MIYO_BSMCL
    {
        public const string ApplicationClientID = "38deaf03-814e-4bf2-87e7-935ad42aaf2a";

        public static OfflineAccount CreateOfflineUser(string _playername)
        {
            OfflineAuthenticator offlineAuthenticator = new OfflineAuthenticator(_playername);
            return offlineAuthenticator.Authenticate();
        }

        public static async Task<MicrosoftAccount> CreateMicrosoftUser(MainWindow mainWindow)
        {

            MicrosoftAuthenticator microsoftAuthenticator = new MicrosoftAuthenticator(ApplicationClientID);

            IPendingHandler? PendingHandler = PendingBox.Show("", true);

            await microsoftAuthenticator.DeviceFlowAuthAsync(devcode =>
            {
                PendingHandler.Close();

                string url = devcode.VerificationUrl;
                string code = devcode.UserCode;

                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });

                try
                {
                    Clipboard.SetText(code);
                }
                catch (Exception ex) 
                {
                    Trace.WriteLine(ex);
                }
                
                Trace.WriteLine($"你的微软验证代码为：{code} (已复制到剪切板)请在浏览器完成验证微软验证", "INFO");
                PendingHandler = PendingBox.Show($"你的微软验证代码为：{code} (已复制到剪切板)请在浏览器完成验证微软验证","微软验证",true);

            });


            var microsoftAccount = await microsoftAuthenticator.AuthenticateAsync();

            PendingHandler.Close();

            return microsoftAccount;




        }

        public static async Task<MicrosoftAccount?> ReflushMicrosoftUser(MicrosoftAccount oldaccount)
        {
            MicrosoftAuthenticator microsoftAuthenticator = new MicrosoftAuthenticator(oldaccount, ApplicationClientID, false);

            try
            {
                return await microsoftAuthenticator.AuthenticateAsync();
            }
            catch (Exception ex)
            {
               Trace.WriteLine(ex);
               return null;
            }
            
        }


    }
}
