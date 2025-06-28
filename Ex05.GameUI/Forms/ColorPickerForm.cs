using Ex05.GameLogic;

namespace Ex05.GameUI.Forms
{
    public partial class ColorPickerForm : Form
    {
        private const int k_ButtonSize = 40;
        private const int k_Margin = 10;
        private const int k_Columns = 4;

        public eColor? ChosenColor { get; private set; }

        public ColorPickerForm()
        {
            InitializeComponent();
            InitializeColorButtons();
        }

        private void InitializeColorButtons()
        {
            this.Text = "Pick A Color:";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            List<(Color, eColor)> colorOptions = new()
            {
                (Color.Red, eColor.Red),
                (Color.Green, eColor.Green),
                (Color.Blue, eColor.Blue),
                (Color.Yellow, eColor.Yellow),
                (Color.Purple, eColor.Purple),
                (Color.Orange, eColor.Orange),
                (Color.LightBlue, eColor.LightBlue),
                (Color.Brown, eColor.Brown)
            };

            int rows = (int)Math.Ceiling(colorOptions.Count / (double)k_Columns);
            int totalWidth = k_Columns * (k_ButtonSize + k_Margin) + k_Margin;
            int totalHeight = rows * (k_ButtonSize + k_Margin) + k_Margin;

            this.ClientSize = new Size(totalWidth, totalHeight);

            for (int i = 0; i < colorOptions.Count; i++)
            {
                int row = i / k_Columns;
                int col = i % k_Columns;

                Button colorButton = new Button
                {
                    Size = new Size(k_ButtonSize, k_ButtonSize),
                    Location = new Point(k_Margin + col * (k_ButtonSize + k_Margin), k_Margin + row * (k_ButtonSize + k_Margin)),
                    BackColor = colorOptions[i].Item1,
                    FlatStyle = FlatStyle.Popup,
                    Tag = colorOptions[i].Item2
                };
                colorButton.Click += colorButton_Click;
                this.Controls.Add(colorButton);
            }
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            ChosenColor = (eColor)clickedButton.Tag;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
