using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace rekapp_tray
{
    public class Controller
    {
        public void CreateFile(JsonModel json)
        {
            // Specify the file path and name
            string filePath = @"C:\Users\fs2118\example.txt";

            try
            {
                // Create the file and close the FileStream
                using (FileStream fs = File.Create(filePath))
                {
                    fs.Close();
                }

                // Create a StreamWriter to write to the file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Write data to the file
                    writer.WriteLine($"ReceiverIP={json.ReceiverIP}");
                    writer.WriteLine($"ReceiverPort={json.ReceiverPort}");
                    writer.WriteLine($"PackageCaptureTime={json.PackageCaptureTime}");
                    writer.WriteLine($"PackageCaptureInterval={json.PackageCaptureInterval}");
                    writer.WriteLine($"InactivityThreshold={json.InactivityThreshold}");
                    // Close the StreamWriter
                    writer.Close();
                }

                Console.WriteLine("File contents updated successfully using regex.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating or writing to file: " + ex.Message);
            }
        }

        public async void ListenRequest()
        {
            int port = 12345; // Port number to listen on

            // Create a TcpListener bound to the specified IP address and port
            TcpListener listener = new TcpListener(IPAddress.Any, port);

            // Start listening for incoming connections
            listener.Start();
            Console.WriteLine($"Listening on port {port}...");

            try
            {
                while (true)
                {
                    // Accept an incoming connection
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    Console.WriteLine("Client connected.");

                    // Get the network stream for reading and writing data
                    NetworkStream stream = client.GetStream();

                    // Read data from the client
                    byte[] buffer = new byte[1024];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received data: {receivedData}");

                    // Parse the received data as an HTTP request
                    HttpRequest httpRequest = HttpRequest.Parse(receivedData);

                    // Check if the request is a POST request
                    if (httpRequest.Method == "POST")
                    {
                        // Extract the data from the POST request
                        string postData = httpRequest.Body;

                        // Process the POST data
                        // ... (do something with postData)

                        // Send a response back to the client
                        string responseData = "Hello from the server!";
                        byte[] responseBuffer = Encoding.UTF8.GetBytes(responseData);
                        await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
                        Console.WriteLine($"Sent data: {responseData}");
                    }

                    // Close the client connection
                    client.Close();
                    Console.WriteLine("Client disconnected.");
                }
            }
            finally
            {
                // Stop listening for connections
                listener.Stop();
            }
        }

        // Simple HTTP request class for parsing HTTP requests
        public class HttpRequest
        {
            public string Method { get; set; }
            public string Path { get; set; }
            public string Body { get; set; }

            public static HttpRequest Parse(string httpRequest)
            {
                HttpRequest request = new HttpRequest();
                string[] lines = httpRequest.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                string[] requestLine = lines[0].Split(' ');
                request.Method = requestLine[0];
                request.Path = requestLine[1];

                // Extract the body from the request, if present
                int byteIndex = Array.IndexOf(lines, "");
                if (byteIndex != -1 && byteIndex + 1 < lines.Length)
                {
                    request.Body = lines[byteIndex + 1];
                }
                return request;
            }
        }

    }
}
