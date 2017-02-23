using MPAi.Components;
using MPAi.Modules;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MPAi.Forms.Popups
{
    partial class AdministratorConsole
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

        private System.Windows.Forms.TableLayoutPanel generateUserTable()
        {
            userButtonMap = new Dictionary<MPAiButton, MPAiUser>();
            userCheckBoxMap = new Dictionary<System.Windows.Forms.CheckBox, MPAiUser>();

            TableLayoutPanel userTable = new TableLayoutPanel();
            userTable.SuspendLayout();
            userTable.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            userTable.Location = new System.Drawing.Point(0, 0);

            List<MPAiUser> users = new List<MPAiUser>();
            if(UserManagement.GetAllUsers() != null)
            {
                users.AddRange(UserManagement.GetAllUsers());
            }

            users.Remove(UserManagement.CurrentUser);
            List<MPAiUser> usersCopy = new List<MPAiUser>();
            usersCopy.AddRange(users);

            foreach (MPAiUser user in usersCopy)
            {
                if (user.OriginalAdmin)
                {
                    users.Remove(user);
                }
            }

            if (users.Count >= 1)
            {
                userTable.ColumnCount = 4;
                userTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100));
                userTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125));
                userTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125));
                userTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84));
                userTable.Dock = System.Windows.Forms.DockStyle.Top;

                userTable.RowCount = users.Count;
                if (users.Count > 1)
                {
                    userTable.AutoSize = true;
                    userTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                }
                else
                {
                    userTable.Height = 42;
                }
                userTable.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.tableLayoutPanel1_CellPaint);
                foreach (MPAiUser user in users)
                {
                    userTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
                }

                for (int i = 0; i < users.Count; i++)
                {
                    System.Windows.Forms.Label label = new System.Windows.Forms.Label();
                    label.Text = users[i].UserID;
                    label.Anchor = System.Windows.Forms.AnchorStyles.None;
                    label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                    label.Dock = System.Windows.Forms.DockStyle.Fill;
                    label.BackColor = System.Drawing.Color.Transparent;
                    userTable.Controls.Add(label, 0, i);

                    MPAiButton resetButton = new MPAiButton();
                    resetButton.Text = "Reset";
                    resetButton.Size = new System.Drawing.Size(100, 23);
                    resetButton.Anchor = AnchorStyles.None;
                    resetButton.Click += resetButtonClick;
                    userTable.Controls.Add(resetButton, 1, i);

                    MPAiButton deleteButton = new MPAiButton();
                    deleteButton.Text = "Delete";
                    deleteButton.Size = new System.Drawing.Size(100, 23);
                    deleteButton.Anchor = AnchorStyles.None;
                    deleteButton.Click += deleteButtonClick;
                    userTable.Controls.Add(deleteButton, 2, i);

                    CheckBox checkBox = new CheckBox();
                    checkBox.Anchor = AnchorStyles.None;
                    checkBox.BackColor = System.Drawing.Color.Transparent;
                    checkBox.Checked = users[i].IsAdmin;
                    checkBox.CheckedChanged += checkBoxChanged;
                    checkBox.Size = new System.Drawing.Size(15, 15);
                    checkBox.Text = "";
                    userTable.Controls.Add(checkBox, 3, i);

                    userButtonMap.Add(deleteButton, users[i]);
                    userButtonMap.Add(resetButton, users[i]);
                    userCheckBoxMap.Add(checkBox, users[i]);
                }
            }
            else
            {
                userTable.ColumnCount = 1;
                userTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                userTable.Dock = DockStyle.Fill;
                userTable.BackColor = System.Drawing.Color.WhiteSmoke;

                Label emptyLabel = new Label();
                emptyLabel.Text = "No other users to administrate.";
                emptyLabel.Dock = DockStyle.Fill;
                emptyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

                userTable.Controls.Add(emptyLabel);
            }

            userTable.ResumeLayout();

            return userTable;
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdministratorConsole));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.generatedUserTable = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.mpAiButton1 = new MPAi.Components.MPAiButton(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(20, 20, 20, 0);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.mpAiButton1);
            this.splitContainer1.Size = new System.Drawing.Size(484, 362);
            this.splitContainer1.SplitterDistance = 309;
            this.splitContainer1.TabIndex = 1;
            this.splitContainer1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(20, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(444, 289);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(440, 285);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.generatedUserTable);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 57);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(434, 225);
            this.panel2.TabIndex = 0;
            // 
            // generatedUserTable
            // 
            this.generatedUserTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.generatedUserTable.Location = new System.Drawing.Point(0, 0);
            this.generatedUserTable.Name = "generatedUserTable";
            this.generatedUserTable.Size = new System.Drawing.Size(200, 100);
            this.generatedUserTable.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tableLayoutPanel3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(434, 54);
            this.panel3.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(220)))), ((int)(((byte)(113)))));
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label4, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label3, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(434, 54);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(105, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 52);
            this.label5.TabIndex = 3;
            this.label5.Text = "Reset Password";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(231, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 52);
            this.label4.TabIndex = 2;
            this.label4.Text = "Delete User";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(4, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 52);
            this.label2.TabIndex = 0;
            this.label2.Text = "Username";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(357, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 52);
            this.label3.TabIndex = 1;
            this.label3.Text = "Is Admin";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mpAiButton1
            // 
            this.mpAiButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(220)))), ((int)(((byte)(113)))));
            this.mpAiButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mpAiButton1.ForeColor = System.Drawing.Color.White;
            this.mpAiButton1.Location = new System.Drawing.Point(384, 14);
            this.mpAiButton1.Name = "mpAiButton1";
            this.mpAiButton1.Size = new System.Drawing.Size(75, 23);
            this.mpAiButton1.TabIndex = 0;
            this.mpAiButton1.Text = "Close";
            this.mpAiButton1.UseVisualStyleBackColor = false;
            this.mpAiButton1.Click += new System.EventHandler(this.close_Click);
            // 
            // AdministratorConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(484, 362);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AdministratorConsole";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administrator Console";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private MPAiButton mpAiButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel generatedUserTable;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label4;
        private Label label5;
    }
}