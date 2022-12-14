using System;
using System.Collections.Generic;
using System.Text;

namespace ArrayNumbers
{
    public class Element
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int BiasRow { get; set; }
        public int BiasColumn { get; set; }

        public Element(int row, int column, int biasRow = 0, int biasColumn = 0)
        {
            Row = row;
            Column = column;
            BiasRow = biasRow;
            BiasColumn = biasColumn;
        }
    }
}
