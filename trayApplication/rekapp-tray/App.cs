﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

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

        private void Btn_min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
