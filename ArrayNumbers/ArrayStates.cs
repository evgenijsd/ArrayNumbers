using System;
using System.Collections.Generic;
using System.Text;

namespace ArrayNumbers
{
    public class ArrayStates
    {
        public bool[,] DataCondition { get; set; }

        public ArrayStates(int rows, int colums)
        {
            DataCondition = new bool[rows, colums];
        }
    }
}
