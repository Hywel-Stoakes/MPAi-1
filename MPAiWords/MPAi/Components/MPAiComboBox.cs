using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MPAi.Components
{
    public partial class MPAiComboBox : ComboBox
    {
        public MPAiComboBox()
        {
            InitializeComponent();
            this.DrawItem += this_DrawItem;
        }

        public MPAiComboBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.DrawItem += this_DrawItem;
        }

        private void this_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
            {
                return;
            }
            e.DrawBackground();
            Color colour, highlightColour;
            // Set the colour here.
            colour = Color.White;
            highlightColour = System.Drawing.Color.FromArgb(0xFF, 0x03, 0xDC, 0x71);

            // If the item is not selected, paint over it with the correct colour
            if (!((e.State & DrawItemState.Selected) == DrawItemState.Selected))
            {
                e.Graphics.FillRectangle(new SolidBrush(colour), e.Bounds);
                e.Graphics.DrawString(this.GetItemText(this.Items[e.Index]), SystemFonts.DefaultFont, new SolidBrush(Color.Black), e.Bounds);
            }
            // If it is selected, paint over it with the highlight colour.
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(highlightColour), e.Bounds);
                e.Graphics.DrawString(this.GetItemText(this.Items[e.Index]), SystemFonts.DefaultFont, new SolidBrush(Color.White), e.Bounds);
            }
            e.DrawFocusRectangle();
        }
    }
}
