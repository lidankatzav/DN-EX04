using System;
using System.Collections.Generic;

namespace Ex05.GameLogic
{
    public class GuessResult
    {
        public int Bulls { get; }
        public int Pgia { get; }

        public GuessResult(int bulls, int pgia)
        {
            Bulls = bulls;
            Pgia = pgia;
        }
    }
}