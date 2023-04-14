using Microsoft.Win32.TaskScheduler;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;


namespace rekapp_tray
{
    static class Program
    {
        static HttpListener listener;
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {

            string url = "http://26.141.179.224:8080/"; // URL where the server will listen for incoming requests
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine($"Server listening on {url}");

            // Start a separate thread to handle incoming requests
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    HttpListenerContext context = listener.GetContext();
                    HttpListenerRequest request = context.Request;
                    string requestText;
                    // Read the request body data
                    using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        string requestBody = reader.ReadToEnd();
                        requestText = requestBody;
                        Console.WriteLine($"Received request with body: {requestBody}");
                        // Process the request data as needed


                    }
                    Controller controler = new Controller();
                    // Send a response back to the client
                    controler.CreateFile(requestText);
                    string responseString = $"Hello from C# server! {requestText} ";
                    //TaskSchedulerConf(requestText);
                    byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes(responseString);
                    context.Response.ContentLength64 = responseBytes.Length;
                    context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                    context.Response.OutputStream.Close();
                }
            });
            thread.Start();

            // Run the WinForms application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new App());
        }
        public static void TaskSchedulerConf(string requestText)
        {

            string taskName = requestText;
            TaskDefinition td = TaskService.Instance.NewTask();
            td.RegistrationInfo.Author = "Rekapp Ltda.";
            td.Actions.Add(new ExecAction(@"C:\Program Files\Notepad++\notepad++.exe"));

            TaskService.Instance.RootFolder.RegisterTaskDefinition(taskName, td).Run();
        }

        public static void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            listener?.Stop(); // Stop the HttpListener
        }
    }
}
