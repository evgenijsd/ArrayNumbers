using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArrayNumbers
{
    class Program
    {
        const int max_row = 9;
        const int max_column = 9;

        static bool CheckingLine(int value, int count, Element element, ArrayNumbers data, ArrayStates states)
        {
            var nextElement = new Element(
                element.Row + element.BiasRow, 
                element.Column + element.BiasColumn, 
                element.BiasRow, 
                element.BiasColumn
            );
            bool check = false;            

            if (count > 2) check = true;

            if (nextElement.Row < max_row &&  
                nextElement.Column < max_column && 
                nextElement.Row > -1 && 
                nextElement.Column > -1)
            {
                int nextValue = data.DataArray[nextElement.Row, nextElement.Column];
                bool checkNext = states.DataCondition[nextElement.Row, nextElement.Column];

                if (nextValue == value && !checkNext)
                {
                    count++;
                    check = CheckingLine(value, count, nextElement, data, states);                    
                }                               
            }

            states.DataCondition[element.Row, element.Column] = check;
            return check;
        }

        static void MoveElement(Element element, ArrayNumbers datas, ArrayStates states)
        {
            var nextElement = new Element(
                element.Row + element.BiasRow, 
                element.Column + element.BiasColumn, 
                element.BiasRow, 
                element.BiasColumn
            );

            if (nextElement.Row < max_row && 
                nextElement.Column < max_column && 
                nextElement.Row > -1 && 
                nextElement.Column > -1)
            {
                if (!states.DataCondition[nextElement.Row, nextElement.Column])
                {
                    int value = datas.DataArray[element.Row, element.Column];
                    states.DataCondition[element.Row, element.Column] = false;
                    datas.DataArray[element.Row, element.Column] = datas.DataArray[nextElement.Row, nextElement.Column];
                    element.Row = nextElement.Row;
                    element.Column = nextElement.Column;
                    states.DataCondition[element.Row, element.Column] = true;                                        
                    datas.DataArray[element.Row, element.Column] = value;
                    MoveElement(element, datas, states);
                }
            }
        }

        static void Main(string[] args)
        {
            var data = new ArrayNumbers(max_row, max_column);
            var states = new ArrayStates(max_row, max_column);
            var random = new Random();
            var listStates = new List<Element>();

            do
            {
                listStates.Clear();

                for (int i = 0; i < max_row; i++)
                {
                    for (int j = 0; j < max_column; j++)
                    {
                        if (!states.DataCondition[i, j])
                        {
                            var nextElement = new Element(i, j, 0, 1);
                            var value = data.DataArray[i, j];

                            states.DataCondition[i, j] = CheckingLine(value, 1, nextElement, data, states);

                            if (!states.DataCondition[i, j])
                            {
                                nextElement = new Element(i, j, 1, 0);
                                states.DataCondition[i, j] = CheckingLine(value, 1, nextElement, data, states);
                            }
                        }

                        if (states.DataCondition[i, j]) listStates.Add(new Element(i, j, -1, 0));
                    }
                }

                foreach (var element in listStates)
                {
                    MoveElement(element, data, states);
                }

                foreach (var element in listStates)
                {
                    states.DataCondition[element.Row, element.Column] = false;
                    data.DataArray[element.Row, element.Column] = random.Next(0, 4);
                }
            }
            while (listStates.Count > 0);

            Console.WriteLine();

            for (int i = 0; i < max_row; i++)
            {
                for (int j = 0; j < max_column; j++)
                {
                    Console.Write($" {data.DataArray[i, j]}");
                }
                Console.WriteLine();
            }

            using (StreamWriter file = File.CreateText("array_numbers_log.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, data);
                Console.WriteLine();
                Console.WriteLine(" Data has been saved to file. Press any key...");
                Console.ReadKey();
            }
        }
    }
}
