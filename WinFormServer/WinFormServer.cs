using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
namespace WinFormServer
{
    public partial class WinFormServer : Form
    {

        private IDisposable SignalR { get; set; }
        const string ServerURI = "http://localhost:1453";
        public WinFormServer()
        {
            InitializeComponent();
            StartServer();
            notifyIcon.Icon = SystemIcons.Application;
            notifyIcon.Visible = true;
        }

        public void WriteToConsole(String message)
        {
            if (richTextBox.InvokeRequired)
            {
                this.BeginInvoke((Action)(() =>
                    WriteToConsole(message)
                ));
                return;
            }
            richTextBox.AppendText(message + Environment.NewLine);
        }

        private void StartServer()
        {
            try
            {
                GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(110);
                GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30);
                GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10);
                SignalR = WebApp.Start(ServerURI);
            }
            catch (TargetInvocationException e)
            {
                new TargetInvocationException("Server başlatma hatası.", e);

            }
        }
        public class MyHub : Hub
        {
            public void Send(string message)
            {
                Program.MainForm.showForm();
                Program.MainForm.WriteToConsole(message);
                DialogResult result = MessageBox.Show(message, "Bilgilendirme Penceresi", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                Clients.All.addMessage($"{message}:{result}");
                Program.MainForm.WindowState = FormWindowState.Minimized;
            }
        }
        private void showForm()
        {

            this.BeginInvoke((Action)(() =>
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
            ));

        }
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            showForm();
        }

        private void WinFormServer_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
            }
        }
        private void kapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
