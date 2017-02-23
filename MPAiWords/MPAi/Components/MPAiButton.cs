using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MPAi.Components
{
    public partial class MPAiButton : Button
    {
        public MPAiButton()
        {
            InitializeComponent();
        }

        public MPAiButton(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public void MPAiButton_EnabledChanged(object sender, EventArgs e)
        {
            if(this.Enabled)
            {
                this.BackColor = System.Drawing.Color.FromArgb(0xFF, 0x03, 0xDC, 0x71);
            }
            else
            {
                this.BackColor = System.Drawing.Color.FromArgb(0xFF, 0x43, 0xEC, 0x91);
            }
        }
    }
}
