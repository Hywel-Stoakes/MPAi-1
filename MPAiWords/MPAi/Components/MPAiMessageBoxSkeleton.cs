using System;
using System.Windows.Forms;

namespace MPAi.Components
{
    public partial class MPAiMessageBoxSkeleton : Form
    {
        private DialogResult result = DialogResult.None;

        private MPAiButton button1 = null, button2 = null, button3 = null;

        public MPAiMessageBoxSkeleton()
        {
            InitializeComponent();
        }

        public void SetPanel1Button(MPAiButton button)
        {
            tableLayoutPanel1.Controls.Add(button, 0, 0);
            button.Click += buttonClick;
            button.TabIndex = 0;
            button.TabStop = true;
            button1 = button;
        }

        public void SetPanel2Button(MPAiButton button)
        {
            tableLayoutPanel1.Controls.Add(button, 1, 0);
            button.Click += buttonClick;
            button.TabIndex = 0;
            button.TabStop = true;
            button2 = button;
        }

        public void SetPanel3Button(MPAiButton button)
        {
            tableLayoutPanel1.Controls.Add(button, 2, 0);
            button.Click += buttonClick;
            button.TabIndex = 0;
            button.TabStop = true;
            button3 = button;
        }

        public void SetMessageText(string text)
        {
            messageTextBox.Text = text.Replace("\n", "\r\n");
        }

        public void SetCaptionText(string caption)
        {
            this.captionLabel.Text = caption;
        }

        private void buttonClick(object sender, EventArgs e)
        {
            result = ((MPAiMessageBoxButton)sender).Result;
            this.Close();
        }

        public DialogResult ShowMessageBox()
        {
            if(button3 != null)
            {
                this.button3.Select();
            }
            else if (button2 != null)
            {
                this.button2.Select();
            }
            else if (button1 != null)
            {
                this.button1.Select();
            }
            else
            {
                captionLabel.Select();
            }

            this.ShowDialog();
            return result;
        }
    }
}
