using System;
using System.IO;

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
    }
}
