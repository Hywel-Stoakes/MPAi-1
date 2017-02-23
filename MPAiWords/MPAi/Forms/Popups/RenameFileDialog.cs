using MPAi.Components;
using MPAi.Cores;
using MPAi.DatabaseModel;
using MPAi.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MPAi.Forms.Popups
{
    public partial class RenameFileDialog : Form
    {
        // Strings kept in fields to make text easier to change.
        private string noSuchFileText = "No such file!";
        private string alreadyExistsText = "Destination file already exists!";
        private string dataLinkErrorText = "Database linking error!";
        private string wordNotFoundText = "That word is not valid, try another, or select from the list.";
        private string categoryNotFoundText = "That category is not valid, try another, or select from the list.";

        // If the user is adding a recording to the database, some validation should be removed.
        private bool newFile = false;

        private FileInfo file;

        public FileInfo RenamedFile { get; set; }

        /// <summary>
        /// Default constructor. Where possible, fills the user's details into the text boxes.
        /// </summary>
        public RenameFileDialog()
        {
            InitializeComponent();
            // Use user settings or menu data to automatically fill the fields.
            // Speaker = user's screen name
            speakerComboBox.Items.AddRange( new object[4]{ VoiceType.getDisplayNameFromVoiceType(new VoiceType(GenderType.FEMININE, LanguageType.NATIVE)),
                VoiceType.getDisplayNameFromVoiceType(new VoiceType(GenderType.MASCULINE, LanguageType.NATIVE)),
                VoiceType.getDisplayNameFromVoiceType(new VoiceType(GenderType.FEMININE, LanguageType.MODERN)),
                VoiceType.getDisplayNameFromVoiceType(new VoiceType(GenderType.MASCULINE, LanguageType.MODERN)) });
            speakerComboBox.SelectedItem = VoiceType.getDisplayNameFromVoiceType(UserManagement.CurrentUser.Voice);
            // Category has a drop down list for scalability, although in this version it's just Word and Vowel.
            populateCategoryComboBox(); 
            // Word will need a drop-down list
            populateWordComboBox();
            // Label should start with focus and be up to them.
        }

        /// <summary>
        /// Constructor overload allowing a file to be passed in from elsewhere to be renamed.
        /// </summary>
        /// <param name="filename">The file to rename.</param>
        public RenameFileDialog(string filename) : this()
        {
            file = new FileInfo(filename);
            filenameTextBox.Text = file.Name;
        }

        /// <summary>
        /// Constructor allowing the user to disable certain validation so that new words can be added to the database.
        /// </summary>
        /// <param name="newFile"></param>
        public RenameFileDialog(string filename, bool newFile) : this(filename)
        {
            this.newFile = newFile;
        }

        /// <summary>
        /// Gets the categories from the database.
        /// </summary>
        private void populateCategoryComboBox()
        {
            try
            {
                // Create new database context.
                using (MPAiModel DBModel = new MPAiModel())
                {
                    DBModel.Database.Initialize(false); // Added for safety; if the database has not been initialised, initialise it.

                    MPAiUser current = UserManagement.CurrentUser;

                    List<Category> view = DBModel.Category.ToList();
                    view.Reverse();
                    categoryComboBox.DataSource = new BindingSource() { DataSource = view };
                    categoryComboBox.DisplayMember = "Name";
                }
            }
            catch (Exception exp)
            {
                MPAiMessageBoxFactory.Show(dataLinkErrorText);
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// Due to the legacy code on the batch files, the enums for voice type don't match the text required in filenames.
        /// This method converts between the two.
        /// </summary>
        /// <param name="voice">A VoiceType enum representing the voice type to convert.</param>
        /// <returns>A string which is the old format for the input enum.</returns>
        private string ConvertDisplayVoiceType(string voice)
        {
            switch (voice)
            {
                case ("Feminine, Kuia Māori"):
                    return "oldfemale";
                case ("Masculine, Kaumatua Māori"):
                    return "oldmale";
                case ("Feminine, Modern Māori"):
                    return "youngfemale";
                case ("Masculine, Modern Māori"):
                    return "youngmale";
                default:
                    MPAiMessageBoxFactory.Show("Invalid Speaker type!");
                    return null;
            }
        }

        /// <summary>
        /// Gets the words from the database.
        /// </summary>
        private void populateWordComboBox()
        {
            try
            {
                // Create new database context.
                using (MPAiModel DBModel = new MPAiModel())
                {
                    DBModel.Database.Initialize(false); // Added for safety; if the database has not been initialised, initialise it.

                    MPAiUser current = UserManagement.CurrentUser;
                    UserManagement.CurrentUser.setSpeakerFromVoiceType();

                    List<Word> view = DBModel.Word.Where(x => (
                       x.Category.Name.Equals(((Category)categoryComboBox.SelectedItem).Name)
                       && x.Recordings.Any(y => y.Speaker.SpeakerId == UserManagement.CurrentUser.Speaker.SpeakerId) 
                       )).ToList();

                    view.Sort(new VowelComparer());
                    WordComboBox.DataSource = new BindingSource() { DataSource = view };
                    WordComboBox.DisplayMember = "Name";
                }
            }
            catch (Exception exp)
            {
                MPAiMessageBoxFactory.Show(dataLinkErrorText);
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// Handles functionality for the "..." button that opens a file picker, and puts the result in the relevant text box.
        /// </summary>
        /// <param name="sender">Automatically Generated by Visual Studio.</param>
        /// <param name="e">Automatically Generated by Visual Studio.</param>
        private void filePickerButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filenameTextBox.Text = openFileDialog.SafeFileName;
                    file = new FileInfo(openFileDialog.FileName);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        /// <summary>
        /// Handles functionality for the rename button, and closes the form if successful.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void renameButton_Click(object sender, EventArgs e)
        {
            renameCurrentFile();
        }

        /// <summary>
        /// Invokes the name parser to rename the specified file according to naming conventions, and closes the window if successful.
        /// </summary>
        private void renameCurrentFile()
        {
            try
            {
                NameParser parser = new NameParser();
                parser.Address = Path.GetDirectoryName(file.FullName);
                parser.Ext = Path.GetExtension(file.FullName);
                parser.Speaker = ConvertDisplayVoiceType((string)speakerComboBox.SelectedItem);
                parser.Category = categoryComboBox.Text;
                parser.Word = WordComboBox.Text;
                parser.Label = labelTextBox.Text;

                File.Move(file.FullName, parser.SingleFile);

                RenamedFile = new FileInfo(parser.SingleFile);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception exp)
            {
                if (exp.GetType() == typeof(FileNotFoundException))
                {
                    MPAiMessageBoxFactory.Show(exp.Message, noSuchFileText);
                }
                else if (exp.GetType() == typeof(IOException))
                {
                    MPAiMessageBoxFactory.Show(exp.Message, alreadyExistsText);
                }
            }
        }

        /// <summary>
        /// Prevents two lists appearing onscreen at once by closing the main list when the suggestion list is visible.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void WordComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            WordComboBox.DroppedDown = false;
        }

        /// <summary>
        /// Ensures only valid words are entered, by comparing the text to the names of all words when focus is lost.
        /// Validation is skipped if the user is uploading a new file, as the word may not be in the database yet.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void WordComboBox_Leave(object sender, EventArgs e)
        {
            // Separate if statements used for code clarity.
            // If the user is uploading a new file, don't enforce this rule.
            if (newFile)
            {
                return;
            }
            // Prevents the user getting stuck when there are no words. 
            if (WordComboBox.Items.Count < 1)
            {
                return;
            }
            foreach (Word w in WordComboBox.Items)
            {
                if (w.Name.Equals(WordComboBox.Text))
                {
                    return;
                }
            }
            MPAiMessageBoxFactory.Show(wordNotFoundText);
            WordComboBox.Focus();
        }

        /// <summary>
        /// Prevents two lists appearing onscreen at once by closing the main list when the suggestion list is visible.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void categoryComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            categoryComboBox.DroppedDown = false;
        }

        /// <summary>
        /// Ensures only valid categories are entered, by comparing the text to the names of all categories when focus is lost.
        /// Validation is skipped if the user is uploading a new file, as the category may not be in the database yet.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void categoryComboBox_Leave(object sender, EventArgs e)
        {
            // Separate if statements used for code clarity.
            // If the user is uploading a new file, don't enforce this rule.
            if (newFile)
            {
                return;
            }
            // Prevents the user getting stuck when there are no words. 
            if (categoryComboBox.Items.Count < 1)
            {
                return;
            }
            foreach (Category w in categoryComboBox.Items)
            {
                if (w.Name.Equals(categoryComboBox.Text))
                {
                    return;
                }
            }
            MPAiMessageBoxFactory.Show(categoryNotFoundText);
            categoryComboBox.Focus();
        }

        /// <summary>
        /// When the category changes, update the words to be the words in that category.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void categoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateWordComboBox();
        }

        /// <summary>
        /// Cancels the rename operation if the user presses cancel.
        /// </summary>
        /// <param name="sender">Automatically generated by Visual Studio.</param>
        /// <param name="e">Automatically generated by Visual Studio.</param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
