using Ex05.GameLogic;

namespace Ex05.GameUI.Forms
{
    public partial class GameForm : Form
    {
        private const int k_Columns = 4;
        private readonly int r_NumOfChances;
        private readonly GameManager r_GameManager;
        private readonly List<List<Button>> r_GuessButtons = new();
        private readonly List<Button> r_ArrowButtons = new();
        private readonly List<List<Button>> r_ResultButtons = new();
        private readonly List<Button> r_SecretButtons = new();
        private int m_CurrentRow = 0;

        public GameForm(int i_NumOfChances)
        {
            r_NumOfChances = i_NumOfChances;
            r_GameManager = new GameManager(r_NumOfChances);
            InitializeComponent();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            int margin = 10;
            int cellSize = 40;
            int spacing = 10;
            int resultSize = 15;

            this.Text = "Bool Pgia";
            this.BackColor = SystemColors.Control;
            this.Size = new Size(300, (r_NumOfChances + 2) * (cellSize + spacing));

            for (int i = 0; i < k_Columns; i++)
            {
                Button secretButton = new Button
                {
                    Size = new Size(cellSize, cellSize),
                    Location = new Point(margin + i * (cellSize + spacing), margin),
                    BackColor = Color.Black,
                    Enabled = false
                };
                this.Controls.Add(secretButton);
                r_SecretButtons.Add(secretButton);
            }

            for (int row = 0; row < r_NumOfChances; row++)
            {
                int yOffset = (cellSize + spacing) * (row + 1) + margin;

                List<Button> rowButtons = new();
                for (int col = 0; col < k_Columns; col++)
                {
                    Button guessButton = new Button
                    {
                        Size = new Size(cellSize, cellSize),
                        Location = new Point(margin + col * (cellSize + spacing), yOffset),
                        BackColor = Color.LightGoldenrodYellow,
                        Enabled = row == 0
                    };
                    guessButton.Click += guessButton_Click;
                    this.Controls.Add(guessButton);
                    rowButtons.Add(guessButton);
                }
                r_GuessButtons.Add(rowButtons);

                Button arrowButton = new Button
                {
                    Size = new Size(40, cellSize),
                    Location = new Point(margin + k_Columns * (cellSize + spacing), yOffset),
                    Text = "=>",
                    BackColor = Color.LightGoldenrodYellow,
                    Enabled = false
                };
                arrowButton.Click += arrowButton_Click;
                this.Controls.Add(arrowButton);
                r_ArrowButtons.Add(arrowButton);

                List<Button> resultRow = new();
                for (int i = 0; i < k_Columns; i++)
                {
                    Button resultButton = new Button
                    {
                        Size = new Size(resultSize, resultSize),
                        Location = new Point(
                            margin + k_Columns * (cellSize + spacing) + arrowButton.Width + (i % 2) * (resultSize + 2),
                            yOffset + (i / 2) * (resultSize + 2)),
                        BackColor = Color.LightGoldenrodYellow,
                        Enabled = false
                    };
                    this.Controls.Add(resultButton);
                    resultRow.Add(resultButton);
                }
                r_ResultButtons.Add(resultRow);
            }
        }

        private void guessButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null || !btn.Enabled)
                return;

            using ColorPickerForm colorPicker = new ColorPickerForm();
            if (colorPicker.ShowDialog() == DialogResult.OK && colorPicker.ChosenColor.HasValue)
            {
                eColor color = colorPicker.ChosenColor.Value;
                if (!r_GuessButtons[m_CurrentRow].Any(b => b.BackColor == convertColor(color)))
                {
                    btn.BackColor = convertColor(color);

                    if (r_GuessButtons[m_CurrentRow].All(b => b.BackColor != Color.LightGoldenrodYellow))
                    {
                        r_ArrowButtons[m_CurrentRow].Enabled = true;
                    }
                }
            }
        }

        private void arrowButton_Click(object sender, EventArgs e)
        {
            Button arrow = sender as Button;
            int rowIndex = r_ArrowButtons.IndexOf(arrow);

            List<eColor> guess = r_GuessButtons[rowIndex]
                .Select(b => reverseColor(b.BackColor))
                .ToList();

            GuessResult result = r_GameManager.SubmitGuess(guess);

            // Indicate guess correctness per position
            IReadOnlyList<eColor> secret = r_GameManager.SecretCode;
            for (int i = 0; i < k_Columns; i++)
            {
                r_ResultButtons[rowIndex][i].BackColor =
                    guess[i] == secret[i] ? Color.Yellow : Color.Black;
            }

            foreach (Button b in r_GuessButtons[rowIndex])
            {
                b.Enabled = false;
            }
            arrow.Enabled = false;

            if (r_GameManager.IsGameOver)
            {
                RevealSecretCode();
                MessageBox.Show(r_GameManager.IsWin ? "You won!" : "You lost!");
                DisableAllInput();
            }
            else
            {
                m_CurrentRow++;
                foreach (Button b in r_GuessButtons[m_CurrentRow])
                {
                    b.Enabled = true;
                }
            }
        }

        private void DisableAllInput()
        {
            foreach (var row in r_GuessButtons)
            {
                foreach (var btn in row)
                {
                    btn.Enabled = false;
                }
            }

            foreach (var arrow in r_ArrowButtons)
            {
                arrow.Enabled = false;
            }
        }

        private void RevealSecretCode()
        {
            var code = r_GameManager.SecretCode;
            for (int i = 0; i < k_Columns; i++)
            {
                r_SecretButtons[i].BackColor = convertColor(code[i]);
            }
        }

        private static Color convertColor(eColor color)
        {
            return color switch
            {
                eColor.Red => Color.Red,
                eColor.Green => Color.Green,
                eColor.Blue => Color.Blue,
                eColor.Yellow => Color.Yellow,
                eColor.Purple => Color.Purple,
                eColor.Orange => Color.Orange,
                eColor.LightBlue => Color.LightBlue,
                eColor.Brown => Color.Brown,
                _ => Color.Gray
            };
        }

        private static eColor reverseColor(Color color)
        {
            if (color == Color.Red) return eColor.Red;
            if (color == Color.Green) return eColor.Green;
            if (color == Color.Blue) return eColor.Blue;
            if (color == Color.Yellow) return eColor.Yellow;
            if (color == Color.Purple) return eColor.Purple;
            if (color == Color.Orange) return eColor.Orange;
            if (color == Color.LightBlue) return eColor.LightBlue;
            if (color == Color.Brown) return eColor.Brown;

            throw new ArgumentException("Unknown color selected");
        }
    }
}
