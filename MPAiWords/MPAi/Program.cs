using MPAi.Forms;
using System;
using System.Windows.Forms;
using MPAi.Modules; 
namespace MPAi
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine(VoiceType.getStringFromVoiceType(new VoiceType(GenderType.MASCULINE, LanguageType.MODERN)));
            
            Application.Run(new LoginScreen());

            Application.Exit();
        }
    } 
}