using System;

namespace WM
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (WMGame game = new WMGame())
            {
                game.Run();
            }
        }
    }
}

