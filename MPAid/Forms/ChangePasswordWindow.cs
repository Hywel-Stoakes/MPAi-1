﻿using System.Windows.Forms;

namespace MPAid
{
    /// <summary>
    /// Class representing the window to change your password.
    /// The administrator cannot see this, and instead sees the administrator console.
    /// </summary>
    public partial class ChangePasswordWindow : Form
    {
        private MPAiUser currentUser;
        private const string title = "Change your password";

        /// <summary>
        /// Constructor for the ChangePasswordWindow. Loads the UserManagement object in and creates the UI.
        /// </summary>
        /// <param name="users">The UserManagement object containing all the current users.</param>
        public ChangePasswordWindow()
        {
            InitializeComponent();

            currentUser = UserManagement.getCurrentUser();

            InitUI();
        }

        /// <summary>
        /// Sets the title field to display the current user's name.
        /// </summary>
        private void InitUI()
        {

            Text = title + " - " + currentUser.getName();
        }

        /// <summary>
        /// When the OK button is clicked, changes the user's password.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            changePassword();
        }

        /// <summary>
        /// Verifies the password in the text box is valid, prompts the user, then closes the window if successful.
        /// </summary>
        private void changePassword()
        {
            if ((codeBox.Text.Trim() == "") || (codeBox2.Text.Trim() == ""))
            {
                MessageBox.Show("Passwords should not be empty! ",
                   "Oops", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else if (codeBox.Text != codeBox2.Text)
            {
                MessageBox.Show("Passwords do not match! ",
                   "Oops", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                UserManagement.ChangeUserCode(currentUser.getName(), codeBox.Text);
                MessageBox.Show("Password changed! ",
                      "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }

        /// <summary>
        /// When the cancel button is clicked, close the window, and don't save any changes.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
