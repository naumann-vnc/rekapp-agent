using Microsoft.Win32.TaskScheduler;
using System;
using System.Configuration;
using System.Windows.Forms;


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

            //JsonModel json = new JsonModel();
            //json.InactivityThreshold = "18.569";
            //control.CreateFile(json);

            var user_email = ConfigurationManager.AppSettings["email"];
            Console.WriteLine($"User {user_email}");

            string taskName = "Open Notepad++";
            TaskDefinition td = TaskService.Instance.NewTask();
            td.RegistrationInfo.Author = "Rekapp";
            td.Actions.Add(new ExecAction(@"C:\Program Files\Notepad++\notepad++.exe"));

            //TaskService.Instance.RootFolder.RegisterTaskDefinition(taskName, td).Run();
            //folder.RegisterTaskDefinition(taskName, td).Run();

            TaskService ts = TaskService.Instance;
            //var folder = TaskService.Instance.RootFolder.CreateFolder("Rekapp");
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

            // Create an instance of the Controller class
            Controller controller = new Controller();

            // Call the ListenRequest method to start listening for incoming requests
            controller.ListenRequest();


        }
    }
}
