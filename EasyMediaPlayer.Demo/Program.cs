using System;
using System.Windows.Forms;

namespace MyMediaPlayer
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if NET6_0_OR_GREATER
            ApplicationConfiguration.Initialize();
#endif
            Application.Run(new Form1());
        }
    }
}