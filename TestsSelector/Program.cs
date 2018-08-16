using System;
using System.Windows.Forms;

namespace TestsSelector
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TS_Main(args));
        }
    }
}