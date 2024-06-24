using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MIYO_MCL.Class
{
    public class MIYO_CustomIMG
    {
        public static string CustomImagePath = ".\\MIYOMCL\\Image";

        public static void VerifyPath()
        {
            if (Directory.Exists(CustomImagePath))
            {
               return;
            }
            Directory.CreateDirectory(CustomImagePath);
        }

        public static List<string> SacnImageFile(string path)
        {
            return Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".jpg",StringComparison.OrdinalIgnoreCase) || s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) || s.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase)).ToList();
        }
       

    }
}
