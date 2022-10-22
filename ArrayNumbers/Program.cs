using Newtonsoft.Json;
using System;
using System.IO;

namespace ArrayNumbers
{
    class Program
    {
        const int max_row = 9;
        const int max_column = 9;

        static bool CheckingLine(int value, int count, Element element, ArrayNumbers datas, ArrayСonditions conditions)
        {
            var nextElement = new Element(element.Row + element.BiasRow, element.Column + element.BiasColumn, element.BiasRow, element.BiasColumn);
            bool check = conditions.DataCondition[element.Row, element.Column];

            if (count > 2) check = true;

            if (nextElement.Row < max_row &&  nextElement.Column < max_column && nextElement.Row > -1 && nextElement.Column > -1 && !check)
            {
                int nextValue = datas.DataArray[nextElement.Row, nextElement.Column];

                if (nextValue == value)
                {
                    count++;
                    check = CheckingLine(value, count, nextElement, datas, conditions);                    
                }                               
            }

            conditions.DataCondition[element.Row, element.Column] = check;
            return check;
        }


        static void Main(string[] args)
        {
            int[,] array = {
                    { 3, 3, 0, 2, 0, 3, 1, 3, 1 },
                    { 3, 0, 1, 3, 2, 2, 2, 1, 2 },
                    { 3, 1, 0, 2, 2, 3, 2, 3, 3 },
                    { 0, 1, 2, 2, 1, 3, 3, 1, 0 },
                    { 0, 2, 1, 1, 0, 2, 3, 1, 3 },
                    { 0, 3, 1, 3, 1, 0, 1, 1, 0 },
                    { 0, 3, 1, 0, 0, 1, 0, 1, 2 },
                    { 3, 1, 2, 2, 3, 1, 1, 2, 2 },
                    { 1, 3, 1, 1, 2, 0, 3, 2, 2 }
                  };
            var datas = new ArrayNumbers(max_row, max_column);
            var conditions = new ArrayСonditions(max_row, max_column);
            datas.DataArray = array;

            for (int i = 0; i < max_row; i++)
            {
                for (int j = 0; j < max_column; j++)
                {                    
                    if (!conditions.DataCondition[i,j])
                    {
                        var nextElement = new Element(i, j, 0, 1);
                        var value = datas.DataArray[i, j];

                        conditions.DataCondition[i, j] = CheckingLine(value, 1, nextElement, datas, conditions);

                        if (!conditions.DataCondition[i, j])
                        {
                            nextElement = new Element(i, j, 1, 0);
                            conditions.DataCondition[i, j] = CheckingLine(value, 1, nextElement, datas, conditions);
                        }
                    }
                }
            }

            using (StreamWriter file = File.CreateText("array_numbers.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, datas);
                Console.WriteLine("Data has been saved to file");
            }

            using (StreamWriter file = File.CreateText("array_conditions.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, conditions);
                Console.WriteLine("Data has been saved to file");
            }
        }
    }
}
