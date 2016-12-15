﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MPAid
{
    /// <summary>
    /// Class representing the Administrator's Console form, allowing the administrator to change all user's account settings.
    /// </summary>
    public partial class AdminConsole : Form
    {
        private MPAiUser currentUser;

        /// <summary>
        /// Constructor for the application, connecting to the UserManagement object, and initialising the values in the console.
        /// </summary>
        /// <param name="users">The UserManagement object representing all current users.</param>
        public AdminConsole()
        {
            InitializeComponent();

            currentUser = UserManagement.getCurrentUser();

            userDataView.DataSource = UserManagement.GetAllUsers();
            userDataView.DataBindings.Add(new Binding("ReadOnly", dataReadOnly, "Checked"));
        }
    }
}
