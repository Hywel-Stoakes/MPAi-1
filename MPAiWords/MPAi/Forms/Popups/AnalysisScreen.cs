using MPAi.Cores.Scoreboard;
using MPAi.Modules;
using System;
using System.IO;
using System.Windows.Forms;

namespace MPAi.Forms.Popups
{
    public partial class AnalysisScreen : Form
    {
        public AnalysisScreen(float pronunciationCorrectness, string description) :
            this(pronunciationCorrectness, description, false)
        {
            
        }

        public AnalysisScreen(float pronunciationCorrectness, string description, bool alreadyAnalysed)
        {
            InitializeComponent();

            correctnessLabel.Text = string.Format(@"Pronunciation is {0:0.0%} Correct", pronunciationCorrectness);
            descriptionBox.Text = description;

            alreadyAnalysedLabel.Visible = alreadyAnalysed;
        }

        private void scoreReportButton_Click(object sender, EventArgs e)
        {
            ReportLauncher.GenerateMPAiSpeakScoreHTML(UserManagement.CurrentUser.SpeakScoreboard);

            if (File.Exists(ReportLauncher.MPAiSpeakScoreReportHTMLAddress))
            {
                ReportLauncher.ShowMPAiSpeakScoreReport();
            }
            else
            {
                scoreReportButton.Enabled = false;
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}