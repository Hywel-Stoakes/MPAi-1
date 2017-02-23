using MPAi.Components;

namespace MPAi.Forms.Popups
{
    partial class ConfirmRandomisedPassword
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfirmRandomisedPassword));
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.mpAiButton1 = new MPAi.Components.MPAiButton(this.components);
            this.mpAiButton2 = new MPAi.Components.MPAiButton(this.components);
            this.userLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(42, 35);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(200, 20);
            this.passwordBox.TabIndex = 0;
            // 
            // mpAiButton1
            // 
            this.mpAiButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(220)))), ((int)(((byte)(113)))));
            this.mpAiButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mpAiButton1.ForeColor = System.Drawing.Color.White;
            this.mpAiButton1.Location = new System.Drawing.Point(42, 77);
            this.mpAiButton1.Name = "mpAiButton1";
            this.mpAiButton1.Size = new System.Drawing.Size(75, 23);
            this.mpAiButton1.TabIndex = 1;
            this.mpAiButton1.Text = "Accept";
            this.mpAiButton1.UseVisualStyleBackColor = false;
            this.mpAiButton1.Click += new System.EventHandler(this.accept_ButtonClick);
            // 
            // mpAiButton2
            // 
            this.mpAiButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(220)))), ((int)(((byte)(113)))));
            this.mpAiButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mpAiButton2.ForeColor = System.Drawing.Color.White;
            this.mpAiButton2.Location = new System.Drawing.Point(167, 77);
            this.mpAiButton2.Name = "mpAiButton2";
            this.mpAiButton2.Size = new System.Drawing.Size(75, 23);
            this.mpAiButton2.TabIndex = 2;
            this.mpAiButton2.Text = "Cancel";
            this.mpAiButton2.UseVisualStyleBackColor = false;
            this.mpAiButton2.Click += new System.EventHandler(this.cancel_ButtonClick);
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(39, 19);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(108, 13);
            this.userLabel.TabIndex = 3;
            this.userLabel.Text = "user\'s new password:";
            // 
            // ConfirmRandomisedPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(284, 112);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.mpAiButton2);
            this.Controls.Add(this.mpAiButton1);
            this.Controls.Add(this.passwordBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConfirmRandomisedPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Confirm Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox passwordBox;
        private MPAiButton mpAiButton1;
        private MPAiButton mpAiButton2;
        private System.Windows.Forms.Label userLabel;
    }
}