using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ComparativeTapeTest
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
            var main = new Main();
            main.TopLevel = true;
            main.TopMost = true;
            Application.Run(main);
        }
    }
}
