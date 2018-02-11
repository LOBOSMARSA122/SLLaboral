using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Sigesoft.Node.Sync.MainLogic;

namespace Sigesoft.Node.Sync.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Sigesoft.Node.Sync.MainLogic.frmMain());

            Application.Run(new frmInitial());
        }
    }
}
