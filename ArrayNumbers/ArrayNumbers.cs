using System;
using System.Collections.Generic;
using System.Text;

namespace ArrayNumbers
{
    public class ArrayNumbers
    {
        public int[,] DataArray { get; set; }

        public ArrayNumbers(int rows, int colums)
        {
            DataArray = new int[rows, colums];

            var random = new Random();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < colums; j++)
                {
                    DataArray[i, j] = random.Next(0, 4);
                }
            }
        }
    }
}
