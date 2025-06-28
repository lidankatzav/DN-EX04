using System;
using System.Windows.Forms;

namespace Ex05.GameUI.Forms
{
    public partial class ChancesForm : Form
    {
        public int ChosenChances { get; private set; } = 4;

        public ChancesForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Bool Pgia";
            this.Size = new System.Drawing.Size(250, 120);

            Label label = new Label
            {
                Text = "Number of chances: " + ChosenChances,
                Location = new System.Drawing.Point(10, 10),
                Width = 200
            };

            Button startButton = new Button
            {
                Text = "Start",
                Location = new System.Drawing.Point(10, 50)
            };

            startButton.Click += (s, e) =>
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            label.Click += (s, e) =>
            {
                ChosenChances = ChosenChances == 10 ? 4 : ChosenChances + 1;
                label.Text = "Number of chances: " + ChosenChances;
            };

            this.Controls.Add(label);
            this.Controls.Add(startButton);
        }
    }
}
