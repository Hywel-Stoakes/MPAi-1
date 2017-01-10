﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MPAid
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
            Application.Run(new NewForms.VideoPlayer());
            Application.Run(new NewForms.AudioPlayer());
            Application.Run(new LoginWindow());
        }
    }
}