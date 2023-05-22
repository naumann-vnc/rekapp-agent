using System;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace rekapp_tray
{
    public partial class App : Form
    {
        public App()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            bool MousePointerNotOnTaskBar = Screen.GetWorkingArea(this).Contains(Cursor.Position);

            if (this.WindowState == FormWindowState.Minimized && MousePointerNotOnTaskBar)
            {
                notifyIcon1.Icon = SystemIcons.Application;
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
            }
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            if (this.WindowState == FormWindowState.Normal)
            {
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = false;
            }
        }
        private void Btn_close_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private async void btn_configure_Click(object sender, EventArgs e)
        {
            try
            {
                // Prepare the JSON data
                string ipAddress = GetIPAddress();
                string username = Environment.UserName;
                string json = $"{{\"ipAddress\":\"{ipAddress}\",\"username\":\"{username}\"}}";

                // Create the HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Set the request headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // Send the POST request
                    HttpResponseMessage response = await client.PostAsync("https://rekapp.net:8090", new StringContent(json, Encoding.UTF8, "application/json"));

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Request sent successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Request failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string GetIPAddress()
        {
            string ipAddress = string.Empty;
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    ipAddress = webClient.DownloadString("https://api.ipify.org");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to retrieve IP address: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return ipAddress;
        }
    }
}
