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



            //JsonModel json = new JsonModel();
            //json.InactivityThreshold = "18.569";
            //control.CreateFile(json);
            //var user_email = ConfigurationManager.AppSettings["email"];
            //Console.WriteLine($"User {user_email}");
            //string taskName = "Open Notepad++";
            //TaskDefinition td = TaskService.Instance.NewTask();
            //td.RegistrationInfo.Author = "Rekapp";
            //td.Actions.Add(new ExecAction(@"C:\Program Files\Notepad++\notepad++.exe"));
            //TaskService ts = TaskService.Instance;
            ////var folder = TaskService.Instance.RootFolder.CreateFolder("Rekapp");
            //var tasks = ts.GetFolder("Rekapp").Tasks;



            string url = "http://localhost:8080/"; // URL where the server will listen for incoming requests
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



                    // Read the request body data
                    using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        string requestBody = reader.ReadToEnd();
                        Console.WriteLine($"Received request with body: {requestBody}");
                        // Process the request data as needed
                    }



                    // Send a response back to the client
                    string responseString = "Hello from C# server!";
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
        public static void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            listener?.Stop(); // Stop the HttpListener
        }
    }
}
