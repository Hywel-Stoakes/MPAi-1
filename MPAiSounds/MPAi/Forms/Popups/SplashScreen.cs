using System.Windows.Forms;

namespace MPAi.Forms.Popups
{
    /// <summary>
    /// Class for the splash screen that shows while the program is loading.
    /// </summary>
    public partial class SplashScreen : Form
    {
        /// <summary>
        /// Default constructor, displaying the splash screen.
        /// </summary>
        public SplashScreen()
        {
            InitializeComponent();
            InitUI();
        }
        /// <summary>
        /// Sets the image to the MPAi logo, and makes it take up the entire frame.
        /// </summary>
        private void InitUI()
        {
            Width = BackgroundImage.Width;
            Height = BackgroundImage.Height;
            Icon = Properties.Resources.MPAi;
        }

    }
}
