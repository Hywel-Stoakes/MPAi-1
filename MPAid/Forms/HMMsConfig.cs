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
    /// Form for adjusting settings relating to HMMs.
    /// *** Doesn't appear to be used by any other code, marked for deletion ***
    /// </summary>
    public partial class HMMsConfigForm : Form
    {
        /// <summary>
        /// Default constructor. Shows the GUI.
        /// </summary>
        public HMMsConfigForm()
        {
            InitializeComponent();
            InitUI();
        }
        /// <summary>
        /// Initialises values for UI components.
        /// </summary>
        private void InitUI()
        {
            customizedPath.Text = cPathInitial;
            customizedPath.ForeColor = Color.Gray;

            bool notCustomize = true;
            foreach (Control c in groupPresets.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rb = (RadioButton)c;
                    if (rb.Text.Equals(HMMsController.GetHMMsValue()))
                    {
                        rb.Checked = true;
                        notCustomize = false;
                    }                    
                }
            }
            if (notCustomize)
            {
                useCustomizedHMMs.Checked = true;
                customizedPath.Text = HMMsController.GetHMMsValue();
                customizedPath.ForeColor = Color.Black;
            }
            
        }
        /// <summary>
        /// Sets the directory containing the HMM files.
        /// </summary>
        /// <param name="annieDir">The path to the directory.</param>
        public void SetWorkingFolder(string annieDir)
        {
            labelWorkingDir.Text = "Working Directory - " + annieDir;
        }

        /// <summary>
        /// Gets the path to the directory based on the values in the controls.
        /// </summary>
        /// <returns>The path as a string.</returns>
        public string GetHMMsValue()
        {
            if (useCustomizedHMMs.Checked && customizedPath.Text != null)
                return customizedPath.Text;

            foreach (Control c in groupPresets.Controls)
            {
                if (c is RadioButton)
                {
                    RadioButton rb = (RadioButton)c;
                    if (rb.Checked)
                        return rb.Text;
                }
            }

            return null;
        }

        private string cPathInitial = "Specify the path here (according to the format left)";
        /// <summary>
        /// Hovering over the customised path control creates a visual effect.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void customizedPath_MouseHover(object sender, EventArgs e)
        {
            if (customizedPath.Text == cPathInitial)
            {
                customizedPath.Text = "";
                customizedPath.ForeColor = Color.Black;
                customizedPath.Focus();
            }
        }
        /// <summary>
        /// Disables the presets if the custom checkbox is checked.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void useCustomizedHMMs_CheckedChanged(object sender, EventArgs e)
        {
            groupPresets.Enabled = !useCustomizedHMMs.Checked;
        }

    }
}
