using System;
using System.Collections.Generic;
using System.Text;

namespace ArrayNumbers
{
    public class ArrayСonditions
    {
        public bool[,] DataCondition { get; set; }

        public ArrayСonditions(int rows, int colums)
        {
            DataCondition = new bool[rows, colums];
        }
    }
}
