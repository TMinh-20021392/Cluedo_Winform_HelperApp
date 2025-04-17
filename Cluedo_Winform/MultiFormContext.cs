using Cluedo_Winform_HelperApp;

public class MainAppContext : ApplicationContext
{
    public MainAppContext()
    {
        var screen = Screen.PrimaryScreen.WorkingArea;

        int width1 = screen.Width * 2 / 3;
        int width2 = screen.Width - width1;
        int height = screen.Height;

        var form1 = new Form1();
        var aiForm = new AI_Assistant();

        PositionForm(form1, screen.Left, screen.Top, width1, height);
        PositionForm(aiForm, screen.Left + width1, screen.Top, width2, height);

        form1.FormClosed += (_, __) =>
        {
            if (!aiForm.IsDisposed) aiForm.Close();
            ExitThread();
        };

        aiForm.Show();
        form1.Show();
    }

    private static void PositionForm(Form form, int x, int y, int width, int height)
    {
        form.StartPosition = FormStartPosition.Manual;
        form.Bounds = new Rectangle(x, y, width, height);
    }
}
