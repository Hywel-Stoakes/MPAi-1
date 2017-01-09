﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPAid.Models;

namespace MPAid.NewForms
{
    public partial class UserCreationScreen : Form
    {
        private string username = null;
        private string usercode = null;
        private VoiceType? voiceType = null;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public UserCreationScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the values of the user from the text boxes, and uses them to create an MPAi user object.
        /// </summary>
        /// <returns>The created user object.</returns>
        public MPAiUser getCandidate()
        {
            username = userNameBox.Text;
            usercode = passwordBox.Text;
            if(masculineRadioButton.Checked && heritageRadioButton.Checked)
            {
                voiceType = VoiceType.MASCULINE_HERITAGE;
            }
            else if (masculineRadioButton.Checked && modernRadioButton.Checked)
            {
                voiceType = VoiceType.MASCULINE_MODERN;
            }
            else if (feminineRadioButton.Checked && heritageRadioButton.Checked)
            {
                voiceType = VoiceType.FEMININE_HERITAGE;
            }
            else if (feminineRadioButton.Checked && modernRadioButton.Checked)
            {
                voiceType = VoiceType.FEMININE_MODERN;
            }

            return (new MPAiUser(username, usercode, voiceType));
        }

        private void UserCreationScreen_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Ensures that the new user is valid, and closes the window.
        /// </summary>
        private void createUser()
        {
            if (userNameBox.Text.Trim() == "")
            {
                MessageBox.Show("Username should not be empty! ",
                  "Oops", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if ((passwordBox.Text.Trim() == "") || (confirmPasswordBox.Text.Trim() == ""))
            {
                MessageBox.Show("Passwords should not be empty! ",
                   "Oops", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (passwordBox.Text != confirmPasswordBox.Text)
            {
                MessageBox.Show("Passwords do not match! ",
                    "Oops", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            MPAiUser candidate = getCandidate();

            if (!UserManagement.CreateNewUser(candidate))
            {
                MessageBox.Show("User already exists, please use a different name! ",
                    "Oops", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Registration successful! ",
                        "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UserManagement.WriteSettings();
                LoginScreen loginWindow = (LoginScreen)this.Owner;      // Only LoginWindow can open this form.
                loginWindow.VisualizeUser(candidate);
                Close();
            }
        }

        /// <summary>
        /// Closes the window if the operation is cancelled.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// When the ok button is pressed, calls the method to verify the new user.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void okayButton_Click(object sender, EventArgs e)
        {
            createUser();
        }
    }
}
