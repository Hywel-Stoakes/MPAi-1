namespace MPAi.Forms
{
    /// <summary>
    /// Used by all forms with a menu bar, to guarantee the functionality that it will need.
    /// </summary>
    interface MainFormInterface
    {
        /// <summary>
        /// Used to close the form without ending the program.
        /// </summary>
        void closeThis();

        /// <summary>
        /// Used to refresh list boxes when the user changes their settings.
        /// </summary>
        void userChanged();
    }
}
