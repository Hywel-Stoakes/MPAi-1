using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MPAi.Components;

namespace MPAi.Forms.Popups
{
    public partial class ConfirmRandomisedPassword : Form
    {
        private Random random = new Random();
        private bool accepted = false;

        public ConfirmRandomisedPassword()
        {
            InitializeComponent();

            passwordBox.Text = generateRandomPassword();
        }

        public string ConfirmPassword(string username, string password)
        {
            userLabel.Text = username + "'s new password:";

            this.ShowDialog();

            if (this.accepted)
            {
                return passwordBox.Text;
            }
            else
            {
                return password;
            }
        }

        public string generateRandomPassword()
        {
            string[] characterArray = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "l", "m", "n", "o",
                "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
                "U", "V", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            string newPassword = "";

            for (int i = 0; i < 8; i++)
            {
                newPassword += characterArray[random.Next() % characterArray.Count()];
            }

            return newPassword;
        }

        private void cancel_ButtonClick(object sender, EventArgs e)
        {
            Close();
        }

        private void accept_ButtonClick(object sender, EventArgs e)
        {
            accepted = true;
            Close();
        }
    }
}
