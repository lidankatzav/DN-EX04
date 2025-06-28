using System;
using System.Collections.Generic;
using System.Linq;

namespace Ex05.GameLogic
{
    public class GameManager
    {
        private readonly int r_NumOfChances;
        private readonly int r_Columns = 4;
        private readonly List<eColor> r_SecretCode;
        private int m_CurrentGuessIndex = 0;

        public List<List<eColor>> Guesses { get; }
        public List<GuessResult> Results { get; }

        public GameManager(int i_NumOfChances)
        {
            r_NumOfChances = i_NumOfChances;
            r_SecretCode = generateSecretCode();
            Guesses = new List<List<eColor>>();
            Results = new List<GuessResult>();
        }

        private List<eColor> generateSecretCode()
        {
            var rnd = new Random();
            var allColors = Enum.GetValues(typeof(eColor)).Cast<eColor>().ToList();
            return allColors.OrderBy(x => rnd.Next()).Take(r_Columns).ToList();
        }

        public bool IsGuessValid(List<eColor> guess)
        {
            return guess.Count == r_Columns && guess.Distinct().Count() == r_Columns;
        }

        public GuessResult SubmitGuess(List<eColor> guess)
        {
            if (!IsGuessValid(guess))
            {
                throw new ArgumentException("Invalid guess.");
            }

            int bulls = 0;
            int pgia = 0;

            bool[] secretUsed = new bool[r_Columns];
            bool[] guessUsed = new bool[r_Columns];

            for (int i = 0; i < r_Columns; i++)
            {
                if (guess[i] == r_SecretCode[i])
                {
                    bulls++;
                    secretUsed[i] = true;
                    guessUsed[i] = true;
                }
            }

            for (int i = 0; i < r_Columns; i++)
            {
                if (guessUsed[i]) continue;

                for (int j = 0; j < r_Columns; j++)
                {
                    if (!secretUsed[j] && guess[i] == r_SecretCode[j])
                    {
                        pgia++;
                        secretUsed[j] = true;
                        break;
                    }
                }
            }

            GuessResult result = new GuessResult(bulls, pgia);
            Guesses.Add(guess);
            Results.Add(result);
            m_CurrentGuessIndex++;

            return result;
        }

        public bool IsGameOver => m_CurrentGuessIndex >= r_NumOfChances || IsWin;
        public bool IsWin => Results.LastOrDefault()?.Bulls == r_Columns;
        public IReadOnlyList<eColor> SecretCode => r_SecretCode.AsReadOnly();
    }
}
