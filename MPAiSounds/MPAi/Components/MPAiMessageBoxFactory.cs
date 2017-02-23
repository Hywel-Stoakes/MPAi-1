using System.Windows.Forms;


namespace MPAi.Components
{
    public static class MPAiMessageBoxFactory
    {
        public static DialogResult Show(string message)
        {
            return Show(message, "Message:", MPAiMessageBoxButtons.OK);
        }

        public static DialogResult Show(string message, string caption)
        {
            return Show(message, caption, MPAiMessageBoxButtons.OK);
        }

        public static DialogResult Show(string message, string caption, MPAiMessageBoxButtons buttons)
        {
            MPAiMessageBoxSkeleton messageBox = new MPAiMessageBoxSkeleton();
            messageBox.SetMessageText(message);
            messageBox.SetCaptionText(caption);
            switch (buttons)
            {
                case MPAiMessageBoxButtons.OK:
                    messageBox.SetPanel3Button(MPAiButtonFactory.OKButton());
                    break;
                case MPAiMessageBoxButtons.OKCancel:
                    messageBox.SetPanel1Button(MPAiButtonFactory.CancelButton());
                    messageBox.SetPanel3Button(MPAiButtonFactory.OKButton());
                    break;
                case MPAiMessageBoxButtons.YesNo:
                    messageBox.SetPanel1Button(MPAiButtonFactory.YesButton());
                    messageBox.SetPanel3Button(MPAiButtonFactory.NoButton());
                    break;
                case MPAiMessageBoxButtons.YesNoCancel:
                    messageBox.SetPanel1Button(MPAiButtonFactory.YesButton());
                    messageBox.SetPanel2Button(MPAiButtonFactory.NoButton());
                    messageBox.SetPanel3Button(MPAiButtonFactory.CancelButton());
                    break;
            }
            return messageBox.ShowMessageBox();
        }

        public static DialogResult Show(string message, MPAiMessageBoxButtons buttons)
        {
            MPAiMessageBoxSkeleton messageBox = new MPAiMessageBoxSkeleton();
            messageBox.SetMessageText(message);
            messageBox.SetCaptionText("Message:");
            switch (buttons)
            {
                case MPAiMessageBoxButtons.OK:
                    messageBox.SetPanel3Button(MPAiButtonFactory.OKButton());
                    break;
                case MPAiMessageBoxButtons.OKCancel:
                    messageBox.SetPanel1Button(MPAiButtonFactory.CancelButton());
                    messageBox.SetPanel3Button(MPAiButtonFactory.OKButton());
                    break;
                case MPAiMessageBoxButtons.YesNo:
                    messageBox.SetPanel1Button(MPAiButtonFactory.YesButton());
                    messageBox.SetPanel3Button(MPAiButtonFactory.NoButton());
                    break;
                case MPAiMessageBoxButtons.YesNoCancel:
                    messageBox.SetPanel1Button(MPAiButtonFactory.YesButton());
                    messageBox.SetPanel2Button(MPAiButtonFactory.NoButton());
                    messageBox.SetPanel3Button(MPAiButtonFactory.CancelButton());
                    break;
            }
            return messageBox.ShowMessageBox();
        }
    }

    static class MPAiButtonFactory {

        public static MPAiMessageBoxButton YesButton()
        {
            MPAiMessageBoxButton button = new MPAiMessageBoxButton(DialogResult.Yes);
            button.Anchor = System.Windows.Forms.AnchorStyles.None;
            button.Name = "yesButton";
            button.Size = new System.Drawing.Size(75, 23);
            button.Text = "Yes";
            button.UseVisualStyleBackColor = true;
            button.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            button.Dock = System.Windows.Forms.DockStyle.None;
            button.MaximumSize = new System.Drawing.Size(75,23);

            return button;
        }

        public static MPAiMessageBoxButton NoButton()
        {
            MPAiMessageBoxButton button = new MPAiMessageBoxButton(DialogResult.No);
            button.Anchor = System.Windows.Forms.AnchorStyles.None;
            button.Name = "noButton";
            button.Size = new System.Drawing.Size(75, 23);
            button.Text = "No";
            button.UseVisualStyleBackColor = true;
            button.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            button.Dock = System.Windows.Forms.DockStyle.None;
            button.MaximumSize = new System.Drawing.Size(75, 23);

            return button;
        }

        public static MPAiMessageBoxButton OKButton()
        {
            MPAiMessageBoxButton button = new MPAiMessageBoxButton(DialogResult.OK);
            button.Anchor = System.Windows.Forms.AnchorStyles.None;
            button.Name = "okButton";
            button.Size = new System.Drawing.Size(75, 23);
            button.Text = "Okay";
            button.UseVisualStyleBackColor = true;
            button.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            button.Dock = System.Windows.Forms.DockStyle.None;
            button.MaximumSize = new System.Drawing.Size(75, 23);

            return button;
        }

        public static MPAiMessageBoxButton CancelButton()
        {
            MPAiMessageBoxButton button = new MPAiMessageBoxButton(DialogResult.Cancel);
            button.Anchor = System.Windows.Forms.AnchorStyles.None;
            button.Name = "cancelButton";
            button.Size = new System.Drawing.Size(75, 23);
            button.Text = "Cancel";
            button.UseVisualStyleBackColor = true;
            button.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            button.Dock = System.Windows.Forms.DockStyle.None;
            button.MaximumSize = new System.Drawing.Size(75, 23);

            return button;
        }
    }

    class MPAiMessageBoxButton : MPAiButton
    {
        public DialogResult Result;

        public MPAiMessageBoxButton()
        {
            Result = DialogResult.None;
        }

        public MPAiMessageBoxButton(DialogResult result)
        {
            this.Result = result;
        }
    }

    public enum MPAiMessageBoxButtons
    {
        OK, OKCancel, YesNo, YesNoCancel
    }
}
