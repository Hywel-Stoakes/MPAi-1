using MPAi.Components;
using MPAi.Modules;

namespace MPAi.Forms.Popups
{
    partial class RenameFileDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenameFileDialog));
            this.labelTextBox = new System.Windows.Forms.TextBox();
            this.labelLabel = new System.Windows.Forms.Label();
            this.WordLabel = new System.Windows.Forms.Label();
            this.speakerLabel = new System.Windows.Forms.Label();
            this.filenameTextBox = new System.Windows.Forms.TextBox();
            this.filenameLabel = new System.Windows.Forms.Label();
            this.filePickerButton = new MPAi.Components.MPAiButton(this.components);
            this.renameButton = new MPAi.Components.MPAiButton(this.components);
            this.cancelButton = new MPAi.Components.MPAiButton(this.components);
            this.WordComboBox = new System.Windows.Forms.ComboBox();
            this.ExplanationLabel = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.categoryComboBox = new System.Windows.Forms.ComboBox();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.speakerComboBox = new System.Windows.Forms.ComboBox();
            this.voiceTypeConverterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.voiceTypeConverterBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTextBox
            // 
            this.labelTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelTextBox.Location = new System.Drawing.Point(81, 147);
            this.labelTextBox.Margin = new System.Windows.Forms.Padding(10);
            this.labelTextBox.Name = "labelTextBox";
            this.labelTextBox.Size = new System.Drawing.Size(322, 20);
            this.labelTextBox.TabIndex = 4;
            // 
            // labelLabel
            // 
            this.labelLabel.AutoSize = true;
            this.labelLabel.Location = new System.Drawing.Point(26, 150);
            this.labelLabel.Margin = new System.Windows.Forms.Padding(10);
            this.labelLabel.Name = "labelLabel";
            this.labelLabel.Size = new System.Drawing.Size(33, 13);
            this.labelLabel.TabIndex = 25;
            this.labelLabel.Text = "Label";
            // 
            // WordLabel
            // 
            this.WordLabel.AutoSize = true;
            this.WordLabel.Location = new System.Drawing.Point(26, 123);
            this.WordLabel.Margin = new System.Windows.Forms.Padding(10);
            this.WordLabel.Name = "WordLabel";
            this.WordLabel.Size = new System.Drawing.Size(33, 13);
            this.WordLabel.TabIndex = 21;
            this.WordLabel.Text = "Word";
            // 
            // speakerLabel
            // 
            this.speakerLabel.AutoSize = true;
            this.speakerLabel.Location = new System.Drawing.Point(26, 72);
            this.speakerLabel.Margin = new System.Windows.Forms.Padding(10);
            this.speakerLabel.Name = "speakerLabel";
            this.speakerLabel.Size = new System.Drawing.Size(47, 13);
            this.speakerLabel.TabIndex = 19;
            this.speakerLabel.Text = "Speaker";
            // 
            // filenameTextBox
            // 
            this.filenameTextBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.filenameTextBox.Location = new System.Drawing.Point(81, 45);
            this.filenameTextBox.Margin = new System.Windows.Forms.Padding(10);
            this.filenameTextBox.Name = "filenameTextBox";
            this.filenameTextBox.ReadOnly = true;
            this.filenameTextBox.Size = new System.Drawing.Size(297, 20);
            this.filenameTextBox.TabIndex = 30;
            this.filenameTextBox.TabStop = false;
            // 
            // filenameLabel
            // 
            this.filenameLabel.AutoSize = true;
            this.filenameLabel.Location = new System.Drawing.Point(26, 49);
            this.filenameLabel.Margin = new System.Windows.Forms.Padding(10);
            this.filenameLabel.Name = "filenameLabel";
            this.filenameLabel.Size = new System.Drawing.Size(49, 13);
            this.filenameLabel.TabIndex = 17;
            this.filenameLabel.Text = "Filename";
            // 
            // filePickerButton
            // 
            this.filePickerButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(220)))), ((int)(((byte)(113)))));
            this.filePickerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.filePickerButton.ForeColor = System.Drawing.Color.White;
            this.filePickerButton.Location = new System.Drawing.Point(377, 44);
            this.filePickerButton.Margin = new System.Windows.Forms.Padding(10);
            this.filePickerButton.Name = "filePickerButton";
            this.filePickerButton.Size = new System.Drawing.Size(26, 22);
            this.filePickerButton.TabIndex = 0;
            this.filePickerButton.Text = "...";
            this.filePickerButton.UseVisualStyleBackColor = true;
            this.filePickerButton.Click += new System.EventHandler(this.filePickerButton_Click);
            // 
            // renameButton
            // 
            this.renameButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(220)))), ((int)(((byte)(113)))));
            this.renameButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.renameButton.ForeColor = System.Drawing.Color.White;
            this.renameButton.Location = new System.Drawing.Point(247, 178);
            this.renameButton.Name = "renameButton";
            this.renameButton.Size = new System.Drawing.Size(75, 23);
            this.renameButton.TabIndex = 5;
            this.renameButton.Text = "Rename";
            this.renameButton.UseVisualStyleBackColor = true;
            this.renameButton.Click += new System.EventHandler(this.renameButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(220)))), ((int)(((byte)(113)))));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(328, 178);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 6;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // WordComboBox
            // 
            this.WordComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.WordComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.WordComboBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.WordComboBox.FormattingEnabled = true;
            this.WordComboBox.Location = new System.Drawing.Point(81, 120);
            this.WordComboBox.Name = "WordComboBox";
            this.WordComboBox.Size = new System.Drawing.Size(322, 21);
            this.WordComboBox.TabIndex = 2;
            this.WordComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WordComboBox_KeyPress);
            this.WordComboBox.Leave += new System.EventHandler(this.WordComboBox_Leave);
            // 
            // ExplanationLabel
            // 
            this.ExplanationLabel.AutoSize = true;
            this.ExplanationLabel.Location = new System.Drawing.Point(5, 9);
            this.ExplanationLabel.MaximumSize = new System.Drawing.Size(425, 0);
            this.ExplanationLabel.Name = "ExplanationLabel";
            this.ExplanationLabel.Size = new System.Drawing.Size(420, 26);
            this.ExplanationLabel.TabIndex = 29;
            this.ExplanationLabel.Text = "MPAi recordings must have particular names. Please enter your details below, and " +
    "MPAi will generate it automatically, and place it where it\'s meant to be.";
            this.ExplanationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "recording";
            this.openFileDialog.Filter = "Audio files (*.wav)|*.wav|All files (*.*)|*.*";
            this.openFileDialog.RestoreDirectory = true;
            // 
            // categoryComboBox
            // 
            this.categoryComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.categoryComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.categoryComboBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.categoryComboBox.FormattingEnabled = true;
            this.categoryComboBox.Location = new System.Drawing.Point(81, 94);
            this.categoryComboBox.Name = "categoryComboBox";
            this.categoryComboBox.Size = new System.Drawing.Size(322, 21);
            this.categoryComboBox.TabIndex = 31;
            this.categoryComboBox.SelectedIndexChanged += new System.EventHandler(this.categoryComboBox_SelectedIndexChanged);
            this.categoryComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.categoryComboBox_KeyPress);
            this.categoryComboBox.Leave += new System.EventHandler(this.categoryComboBox_Leave);
            // 
            // categoryLabel
            // 
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.Location = new System.Drawing.Point(26, 97);
            this.categoryLabel.Margin = new System.Windows.Forms.Padding(10);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(49, 13);
            this.categoryLabel.TabIndex = 32;
            this.categoryLabel.Text = "Category";
            // 
            // speakerComboBox
            // 
            this.speakerComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.speakerComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.speakerComboBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.speakerComboBox.FormattingEnabled = true;
            this.speakerComboBox.Location = new System.Drawing.Point(81, 69);
            this.speakerComboBox.Name = "speakerComboBox";
            this.speakerComboBox.Size = new System.Drawing.Size(322, 21);
            this.speakerComboBox.TabIndex = 33;
            // 
            // voiceTypeConverterBindingSource
            // 
            this.voiceTypeConverterBindingSource.DataSource = typeof(MPAi.Modules.VoiceType);
            // 
            // RenameFileDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(434, 212);
            this.Controls.Add(this.speakerComboBox);
            this.Controls.Add(this.categoryComboBox);
            this.Controls.Add(this.categoryLabel);
            this.Controls.Add(this.ExplanationLabel);
            this.Controls.Add(this.WordComboBox);
            this.Controls.Add(this.labelTextBox);
            this.Controls.Add(this.labelLabel);
            this.Controls.Add(this.WordLabel);
            this.Controls.Add(this.speakerLabel);
            this.Controls.Add(this.filenameTextBox);
            this.Controls.Add(this.filenameLabel);
            this.Controls.Add(this.filePickerButton);
            this.Controls.Add(this.renameButton);
            this.Controls.Add(this.cancelButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RenameFileDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rename File";
            ((System.ComponentModel.ISupportInitialize)(this.voiceTypeConverterBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox labelTextBox;
        private System.Windows.Forms.Label labelLabel;
        private System.Windows.Forms.Label WordLabel;
        private System.Windows.Forms.Label speakerLabel;
        private System.Windows.Forms.TextBox filenameTextBox;
        private System.Windows.Forms.Label filenameLabel;
        private MPAiButton filePickerButton;
        private MPAiButton renameButton;
        private MPAiButton cancelButton;
        private System.Windows.Forms.ComboBox WordComboBox;
        private System.Windows.Forms.Label ExplanationLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ComboBox categoryComboBox;
        private System.Windows.Forms.Label categoryLabel;
        private System.Windows.Forms.ComboBox speakerComboBox;
        private System.Windows.Forms.BindingSource voiceTypeConverterBindingSource;
    }
}