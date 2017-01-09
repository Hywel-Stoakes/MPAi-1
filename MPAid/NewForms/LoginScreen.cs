﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MPAid.NewForms
{
    public partial class LoginScreen : Form
    {
        // Strings kept here for changability.
        private string defaultUsernameText = "Username...";
        private string defaultPasswordText = "Password...";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LoginScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the user clicks on the text box, remove the watermark, if it's present.
        /// </summary>
        /// <param name="sender">Automaitcally generated by Visual Studio.</param>
        /// <param name="e">Automaitcally generated by Visual Studio.</param>
        private void usernameTextBox_Enter(object sender, EventArgs e)
        {
            if (usernameTextBox.Text.Equals(defaultUsernameText) && usernameTextBox.ForeColor.Equals(SystemColors.InactiveCaption))
            {
                watermarkUsername(true);
            }
        }

        /// <summary>
        /// When the user loses focus on the text box, add the watermark if they've left it blank.
        /// </summary>
        /// <param name="sender">Automaitcally generated by Visual Studio.</param>
        /// <param name="e">Automaitcally generated by Visual Studio.</param>
        private void usernameTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(usernameTextBox.Text))
            {
                watermarkUsername(false);
            }
        }

        /// <summary>
        /// Handles turning on or off the watermark for the username text box.
        /// </summary>
        /// <param name="enable">True if the watermark is being turned on, false if it is being turned off.</param>
        private void watermarkUsername(bool enable)
        {
            usernameTextBox.ForeColor = enable ? SystemColors.ControlText : SystemColors.InactiveCaption;
            usernameTextBox.Text = enable ? string.Empty : defaultUsernameText;
        }

        /// <summary>
        /// When the user clicks on the text box, remove the watermark, if it's present.
        /// </summary>
        /// <param name="sender">Automaitcally generated by Visual Studio.</param>
        /// <param name="e">Automaitcally generated by Visual Studio.</param>
        private void passwordTextBox_Enter(object sender, EventArgs e)
        {
            if (passwordTextBox.Text.Equals(defaultPasswordText) && passwordTextBox.ForeColor.Equals(SystemColors.InactiveCaption))
            {
                watermarkPassword(true);
            }
        }

        /// <summary>
        /// When the user loses focus on the text box, add the watermark if they've left it blank.
        /// </summary>
        /// <param name="sender">Automaitcally generated by Visual Studio.</param>
        /// <param name="e">Automaitcally generated by Visual Studio.</param>
        private void passwordTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(passwordTextBox.Text))
            {
                watermarkPassword(false);
            }
        }

        /// <summary>
        /// Handles turning on or off the watermark for the password text box.
        /// </summary>
        /// <param name="enable">True if the watermark is being turned on, false if it is being turned off.</param>
        private void watermarkPassword(bool enable)
        {
            passwordTextBox.ForeColor = enable ? SystemColors.ControlText : SystemColors.InactiveCaption;
            passwordTextBox.Text = enable ? string.Empty : defaultPasswordText;
            passwordTextBox.UseSystemPasswordChar = enable;
        }

        /// <summary>
        /// Automatically enters a username and password into the correct text boxes.
        /// </summary>
        /// <param name="Username">The username to enter, as a string.</param>
        /// <param name="Password">The password to enter, as a string.</param>
        {
            watermarkUsername(false);

            watermarkPassword(false);
        }
    }
}
