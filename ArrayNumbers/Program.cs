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

        static bool CheckingLine(int value, int count, Element element, ArrayNumbers datas, ArrayСonditions conditions)
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
                int nextValue = datas.DataArray[nextElement.Row, nextElement.Column];
                bool checkNext = conditions.DataCondition[nextElement.Row, nextElement.Column];

                if (nextValue == value && !checkNext)
                {
                    count++;
                    check = CheckingLine(value, count, nextElement, datas, conditions);                    
                }                               
            }

            conditions.DataCondition[element.Row, element.Column] = check;
            return check;
        }

        static void MoveElement(Element element, ArrayNumbers datas, ArrayСonditions conditions)
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
                if (!conditions.DataCondition[nextElement.Row, nextElement.Column])
                {
                    int value = datas.DataArray[element.Row, element.Column];
                    conditions.DataCondition[element.Row, element.Column] = false;
                    datas.DataArray[element.Row, element.Column] = datas.DataArray[nextElement.Row, nextElement.Column];
                    element.Row = nextElement.Row;
                    element.Column = nextElement.Column;
                    conditions.DataCondition[element.Row, element.Column] = true;                                        
                    datas.DataArray[element.Row, element.Column] = value;
                    MoveElement(element, datas, conditions);
                }
            }
        }

        static void Main(string[] args)
        {
            var datas = new ArrayNumbers(max_row, max_column);
            var conditions = new ArrayСonditions(max_row, max_column);
            var random = new Random();
            var listConditions = new List<Element>();

            do
            {
                listConditions.Clear();

                for (int i = 0; i < max_row; i++)
                {
                    for (int j = 0; j < max_column; j++)
                    {
                        if (!conditions.DataCondition[i, j])
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

                        if (conditions.DataCondition[i, j]) listConditions.Add(new Element(i, j, -1, 0));
                    }
                }

                foreach (var element in listConditions)
                {
                    MoveElement(element, datas, conditions);
                }

                foreach (var element in listConditions)
                {
                    conditions.DataCondition[element.Row, element.Column] = false;
                    datas.DataArray[element.Row, element.Column] = random.Next(0, 4);
                }
            }
            while (listConditions.Count > 0);

            Console.WriteLine();

            for (int i = 0; i < max_row; i++)
            {
                for (int j = 0; j < max_column; j++)
                {
                    Console.Write($" {datas.DataArray[i, j]}");
                }
                Console.WriteLine();
            }

            using (StreamWriter file = File.CreateText("array_numbers_log.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, datas);
                Console.WriteLine();
                Console.WriteLine(" Data has been saved to file. Press any key...");
                Console.ReadKey();
            }
        }
    }
}
