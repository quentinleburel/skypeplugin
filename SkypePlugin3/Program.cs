using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using SKYPE4COMLib;

namespace SkypePlugin3
{
    partial class SysTrayApp : Form
        {
            private NotifyIcon trayIcon;
            private ContextMenu trayMenu;
        private static Skype skype;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            System.Windows.Forms.Application.Run(new SysTrayApp());
   

        }


        public SysTrayApp()
            {
                // Create a simple tray menu with only one item.
                trayMenu = new ContextMenu();
                trayMenu.MenuItems.Add("Exit", OnExit);

                // Create a tray icon. In this example we use a
                // standard system icon for simplicity, but you
                // can of course use your own custom icon too.
                trayIcon = new NotifyIcon();
                trayIcon.Text = "Skype auto-busy";
                trayIcon.Icon = new Icon(SkypePlugin3.Properties.Resources.imageres_5330, 40, 40);

                // Add menu to tray icon and show it.
                trayIcon.ContextMenu = trayMenu;
                trayIcon.Visible = true;
            }

            protected override void OnLoad(EventArgs e)
            {
                Visible = false; // Hide form window.
                ShowInTaskbar = false; // Remove from taskbar.
            skype = new Skype();
            while (!skype.Client.IsRunning)
            {
                // start minimized with no splash screen
                // skype.Client.Start(true, true);
                Console.WriteLine("skype not running");
                System.Threading.Thread.Sleep(5000);
            }


            // wait for the client to be connected and ready
            skype.Attach(6, true);


            skype.CallStatus += new _ISkypeEvents_CallStatusEventHandler(skype_CallStatus);
            base.OnLoad(e);
            }

            private void OnExit(object sender, EventArgs e)
            {
            System.Windows.Forms.Application.Exit();
            }

            protected override void Dispose(bool isDisposing)
            {
                if (isDisposing)
                {
                    // Release the icon resource.
                    trayIcon.Dispose();
                }

                base.Dispose(isDisposing);
            }


            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

        static void skype_CallStatus(Call pCall, TCallStatus Status)
        {
            if (Status == TCallStatus.clsInProgress) { skype.CurrentUserStatus = TUserStatus.cusDoNotDisturb; }
            if (Status == TCallStatus.clsFinished) { skype.CurrentUserStatus = TUserStatus.cusOnline; }


        }

    }


       
    }



