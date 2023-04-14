using System;
using System.IO;

namespace rekapp_tray
{
    public class Controller
    {
        public void CreateFile(String JsonText)
        {
            // Specify the file path and name
            string filePath = @"C:\Users\Raque\Documents\rekapp-config.txt";

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
                    writer.Write(JsonText);
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
