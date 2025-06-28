using Ex05.GameUI.Forms;

namespace Ex05.GameUI
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ChancesForm chancesForm = new ChancesForm();
            if (chancesForm.ShowDialog() == DialogResult.OK)
            {
                int chosenChances = chancesForm.ChosenChances;
                Application.Run(new GameForm(chosenChances));
            }
        }
    }
}