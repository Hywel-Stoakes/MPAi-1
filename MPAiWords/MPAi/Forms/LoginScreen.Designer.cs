using MPAi.Components;

namespace MPAi.Forms
{
    partial class LoginScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginScreen));
            this.logoBox = new System.Windows.Forms.PictureBox();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.rememberCheckBox = new System.Windows.Forms.CheckBox();
            this.signupButton = new MPAi.Components.MPAiButton(this.components);
            this.speakLaunchButton = new MPAi.Components.MPAiButton(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).BeginInit();
            this.SuspendLayout();
            // 
            // logoBox
            // 
            this.logoBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logoBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("logoBox.BackgroundImage")));
            this.logoBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.logoBox.Location = new System.Drawing.Point(3, 3);
            this.logoBox.Name = "logoBox";
            this.logoBox.Size = new System.Drawing.Size(280, 118);
            this.logoBox.TabIndex = 15;
            this.logoBox.TabStop = false;
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.usernameTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.usernameTextBox.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.usernameTextBox.Location = new System.Drawing.Point(12, 127);
            this.usernameTextBox.MaxLength = 25;
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(260, 20);
            this.usernameTextBox.TabIndex = 16;
            this.usernameTextBox.Text = "Username...";
            this.usernameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.usernameTextBox.WordWrap = false;
            this.usernameTextBox.Enter += new System.EventHandler(this.usernameTextBox_Enter);
            this.usernameTextBox.Leave += new System.EventHandler(this.usernameTextBox_Leave);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.passwordTextBox.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.passwordTextBox.Location = new System.Drawing.Point(12, 162);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(260, 20);
            this.passwordTextBox.TabIndex = 17;
            this.passwordTextBox.Text = "Password...";
            this.passwordTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.passwordTextBox.WordWrap = false;
            this.passwordTextBox.Enter += new System.EventHandler(this.passwordTextBox_Enter);
            this.passwordTextBox.Leave += new System.EventHandler(this.passwordTextBox_Leave);
            // 
            // rememberCheckBox
            // 
            this.rememberCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.rememberCheckBox.AutoSize = true;
            this.rememberCheckBox.Location = new System.Drawing.Point(100, 199);
            this.rememberCheckBox.Name = "rememberCheckBox";
            this.rememberCheckBox.Size = new System.Drawing.Size(95, 17);
            this.rememberCheckBox.TabIndex = 20;
            this.rememberCheckBox.Text = "Remember Me";
            this.rememberCheckBox.UseVisualStyleBackColor = true;
            this.rememberCheckBox.CheckedChanged += new System.EventHandler(this.rememberCheckBox_CheckedChanged);
            // 
            // signupButton
            // 
            this.signupButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.signupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(220)))), ((int)(((byte)(113)))));
            this.signupButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.signupButton.ForeColor = System.Drawing.Color.White;
            this.signupButton.Location = new System.Drawing.Point(197, 227);
            this.signupButton.Name = "signupButton";
            this.signupButton.Size = new System.Drawing.Size(75, 23);
            this.signupButton.TabIndex = 19;
            this.signupButton.Text = "Sign Up";
            this.signupButton.UseVisualStyleBackColor = true;
            this.signupButton.Click += new System.EventHandler(this.signupButton_Click);
            // 
            // speakLaunchButton
            // 
            this.speakLaunchButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.speakLaunchButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(220)))), ((int)(((byte)(113)))));
            this.speakLaunchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.speakLaunchButton.ForeColor = System.Drawing.Color.White;
            this.speakLaunchButton.Location = new System.Drawing.Point(12, 227);
            this.speakLaunchButton.Name = "speakLaunchButton";
            this.speakLaunchButton.Size = new System.Drawing.Size(75, 23);
            this.speakLaunchButton.TabIndex = 18;
            this.speakLaunchButton.Text = "Log In";
            this.speakLaunchButton.UseVisualStyleBackColor = true;
            this.speakLaunchButton.Click += new System.EventHandler(this.speakLaunchButton_Click);
            // 
            // LoginScreen
            // 
            this.AcceptButton = this.speakLaunchButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.rememberCheckBox);
            this.Controls.Add(this.signupButton);
            this.Controls.Add(this.speakLaunchButton);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.logoBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(295, 284);
            this.Name = "LoginScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login to MPAi Words";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginScreen_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox logoBox;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private MPAiButton speakLaunchButton;
        private MPAiButton signupButton;
        private System.Windows.Forms.CheckBox rememberCheckBox;
    }
}