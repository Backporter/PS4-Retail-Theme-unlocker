using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace X_unlocker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                string CUSA = Path.GetFileNameWithoutExtension(args[0]).Substring(7, 9);
                string EPID = Path.GetFileNameWithoutExtension(args[0]);
                Directory.CreateDirectory("fake_dlc_pkg");
                Directory.CreateDirectory("fake_dlc_temp");
                Directory.CreateDirectory("fake_dlc_temp\\sce_sys");
                if (File.Exists("fake_dlc_temp\\param_template.sfx") == true)
                {
                    File.Delete("fake_dlc_temp\\param_template.sfx");
                }
                File.Copy("Data\\param_template.sfx", "fake_dlc_temp\\param_template.sfx");
                string text = File.ReadAllText("fake_dlc_temp\\param_template.sfx");
                string exportinfo = File.ReadAllText("fake_dlc_temp\\param_template.sfx");
                text = text.Replace("%1", EPID);
                exportinfo = text.Replace("%2", CUSA).Replace("%3", args[1]);
                File.WriteAllText("fake_dlc_temp\\param_template.sfx", text);
                File.WriteAllText("fake_dlc_temp\\param_template.sfx", exportinfo);
                Process process1 = new Process();
                process1.StartInfo.FileName = "cmd.exe";
                process1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process1.StartInfo.Arguments = "/c data\\orbis-pub-cmd.exe sfo_create \"" + "fake_dlc_temp\\param_template.sfx" + "\" \"" + "fake_dlc_temp\\sce_sys\\param.sfo";
                process1.Start();
                process1.WaitForExit();
                if (File.Exists("fake_dlc_temp\\fake_dlc_project.gp4") == true)
                {
                    File.Delete("fake_dlc_temp\\fake_dlc_project.gp4");
                }
                File.Copy("Data\\fake_dlc_project.gp4", "fake_dlc_temp\\fake_dlc_project.gp4");
                string time = File.ReadAllText("fake_dlc_temp\\fake_dlc_project.gp4");
                time = time.Replace("TIME", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                File.WriteAllText("fake_dlc_temp\\fake_dlc_project.gp4", time);
                string id = File.ReadAllText("fake_dlc_temp\\fake_dlc_project.gp4");
                id = id.Replace("ID", EPID);
                File.WriteAllText("fake_dlc_temp\\fake_dlc_project.gp4", id);
                string dir = File.ReadAllText("fake_dlc_temp\\fake_dlc_project.gp4");
                dir = dir.Replace("DIR", AppDomain.CurrentDomain.BaseDirectory);
                File.WriteAllText("fake_dlc_temp\\fake_dlc_project.gp4", dir);
                Process process2 = new Process();
                process2.StartInfo.FileName = "cmd.exe";
                process2.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                string s = AppDomain.CurrentDomain.BaseDirectory + "fake_dlc_pkg";
                process2.StartInfo.Arguments = "/c data\\orbis-pub-cmd.exe img_create \"" + AppDomain.CurrentDomain.BaseDirectory + "fake_dlc_temp\\fake_dlc_project.gp4" + "\" \"" + AppDomain.CurrentDomain.BaseDirectory + "\\fake_dlc_pkg\\" + EPID + "-A0000-V0100.pkg";
                process2.StartInfo.UseShellExecute = false;
                process2.StartInfo.RedirectStandardOutput = true;
                process2.Start();
                process2.WaitForExit();
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "Logs\\" + Path.GetFileNameWithoutExtension(args[0]) + "log.txt", process2.StandardOutput.ReadToEnd());
                File.Delete("fake_dlc_temp\\param_template.sfx");
                File.Delete("fake_dlc_temp\\sce_sys\\param.sfo");
                File.Delete("fake_dlc_temp\\fake_dlc_project.gp4");
                Directory.Delete("fake_dlc_temp\\sce_sys");
                Directory.Delete("fake_dlc_temp");
            }

            else if (args.Length == 0)
            {
                Console.WriteLine("Invalied Args!\n<Usage> <CID> <NAME>\nEX: X-unlocker.exe EP0002-CUSA02624_00-BLACKOPS3MAPPAK1 \"Call Of Duty Black Ops III: Awakening\" ");
            }
        }
    }
}
