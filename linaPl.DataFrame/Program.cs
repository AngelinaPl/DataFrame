﻿using System;
using System.Collections.Generic;

namespace linaPl.DataFrame
{
    class Program
    {
        static void Main()
        {
            
            var dataT = new Dictionary<linaPl.DataFrame.DataFrame.CellKey, object>()
            {
                {new DataFrame.CellKey(){ Row = 0, Column = 0}, 12 },
                {new DataFrame.CellKey(){ Row = 1, Column = 0}, 12.4 },
                {new DataFrame.CellKey(){ Row = 2, Column = 0}, 23.424 },
                {new DataFrame.CellKey(){ Row = 3, Column = 0}, 23.453 },
                {new DataFrame.CellKey(){ Row = 4, Column = 0}, 35.453 }
            };
            var df = new linaPl.DataFrame.DataFrame(dataT);
            var typ = df.DefineColumnType(dataT, 0);
            var str = df.PrintAsTable();
            Console.WriteLine($"{str}");
            //Console.WriteLine($"{typ}");
            //dataT[new DataFrame.CellKey() { Row = 0, Column = 0 }] =  dataT[
            //    new DataFrame.CellKey() {Row = 0, Column = 0}];
            //df.ChangeType(dataT, 0, typ);
            //foreach (var element in dataT)
            //{
            //    Console.WriteLine(element.Value.GetType());
            //}
            


            //DataFrame data = new DataFrame(@"E:\Kurs_senticode\DataF\test.csv");
            //data.Show();
            //string json = JsonConvert.SerializeObject(data);
            //string path = @"E:\Kurs_senticode\DataF\jsonTest.txt";
            //File.WriteAllText(path, json);
            //var str = File.ReadAllLines(path);
            //string str1 = str[0];
            //Console.WriteLine(str1);

            //var tt = JsonConvert.DeserializeObject<DataFrame>(str1);
            //Console.WriteLine(tt);
            // tt.Show();


            //var putRows = dataFrame.Rows[0];

            //var putRow = dataFrame.Rows[0];
            //Console.WriteLine("The 0 row: ");
            //for (int i =0; i < putRow.Count; i++)
            //{
            //    Console.Write(putRow[i] + "\t");
            //} 
            //Console.WriteLine($"\nThe 0 2 row: {dataFrame.Rows[0][2]}");

            //var putColumn = dataFrame.Columns[0];
            //Console.WriteLine("The 0 column: ");
            //for (int i = 0; i < putColumn.Count; i++)
            //{
            //    Console.Write(putColumn[i] + "\n");
            //}
            //Console.WriteLine($"\nThe 0 2 column: {dataFrame.Columns[0][2]}");

            //object[][] array = new object[4][];
            //for (int i = 0; i < array.Length; i++)
            //{
            //    array[i] = new object[i + 1];
            //}

            //array[0][0] = 00;

            //array[1][0] = 10;
            //array[1][1] = "str";

            //array[2][0] = 20;
            //array[2][1] = "str";
            //array[2][2] = 22;

            //array[3][0] = 30;
            //array[3][1] = "str";
            //array[3][2] = 32.4;
            //array[3][3] = 33;

            //for (int i = 0; i < array.Length; i++)
            //{
            //    for (int j = 0; j < array[i].Length; j++)
            //    {
            //        Console.WriteLine($"{array[i][j]} {array[i][j].GetType()}");
            //    }

            //    Console.WriteLine();
            //    }
            //    DataFrame dataFrame = new DataFrame(array);
            //    var str = dataFrame.ShowAsTable();
            //    Console.Write(str);


            //var diction = new Dictionary<int, IEnumerable<int>>()
            //{
            //    {0, new HashSet<int>(){ 0, 1, 2, 3, 4} },
            //    {1, new HashSet<int>(){ 0, 1, 2, 3, 4, 5, 6} }
            //};
            //DataFrame dataf = new DataFrame(diction);
            //var str = dataf.ShowAsTable();
            //Console.Write(str);

            //DataFrame dataFrame = new DataFrame(@"E:\Kurs_senticode\DataF\test.csv");
            //var str = dataFrame.ShowAsTable();
            //Console.Write(str);


            //var putColumn = dataFrame.Columns[0];
            //Console.WriteLine("The 0 column: ");
            //for (int i = 0; i < putColumn.Count; i++)
            //{
            //    Console.Write(putColumn[i] + "\n");
            //}
            //Console.WriteLine($"\nThe 0 2 column: {dataFrame.Columns[0][2]}");

            Console.ReadLine();
            }
        }


    }

