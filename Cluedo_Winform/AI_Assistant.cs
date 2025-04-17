namespace Cluedo_Winform_HelperApp
{
    public partial class AI_Assistant : Form
    {
        public AI_Assistant()
        {
            FormClosing += AI_Assistant_FormClosing;
        }
        /// <summary>
        /// This method is called when the form is closing. It prevents the form from being closed by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AI_Assistant_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Cancel if user tries to close it manually (Alt+F4, X button, etc.)
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Override the CreateParams property to remove the close button from the form.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                const int CP_NOCLOSE_BUTTON = 0x200;
                var cp = base.CreateParams;
                cp.ClassStyle |= CP_NOCLOSE_BUTTON;
                return cp;
            }
        }


    }
}
