using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.Win32.TaskScheduler;

namespace rekapp_tray
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var user_email = ConfigurationManager.AppSettings["email"];
            var user_ip = ConfigurationManager.AppSettings["ip"];
            string taskName = "Open Notepa++";
            TaskDefinition td = TaskService.Instance.NewTask();
            td.RegistrationInfo.Author = "Rekapp";
            td.Actions.Add(new ExecAction(@"C:\Program Files\Notepad++\notepad++.exe"));

            //TaskService.Instance.RootFolder.RegisterTaskDefinition(taskName, td).Run();
            //folder.RegisterTaskDefinition(taskName, td).Run();

            TaskService ts = TaskService.Instance;
            var folder = TaskService.Instance.RootFolder.CreateFolder("Rekapp");
            var tasks = ts.GetFolder("Rekapp").Tasks;

            foreach (var task in tasks)
            {
                if (task.Name == taskName)
                {
                    task.Run();
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new App());
        }
    }
}
