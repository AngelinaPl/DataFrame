﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace DataF
{
    class Program
    {
        static void Main(string[] args)
        {
            //DataFrame dataFrame = new DataFrame(4, 5);
            //dataFrame[0, 0] = 13;
            //dataFrame[1, 0] = 14.432;
            //dataFrame[2, 0] = 15;
            //dataFrame[3, 0] = "dfg";

            //dataFrame[0, 1] = "str1";
            //dataFrame[1, 1] = 432;
            //dataFrame[2, 1] = "str3";
            //dataFrame[3, 1] = 3.543;

            //dataFrame[0, 2] = 12.1;
            //dataFrame[1, 2] = 56;
            //dataFrame[2, 2] = "hjg";
            //dataFrame[3, 2] = 13.7;

            //dataFrame[0, 3] = 12.1;
            //dataFrame[1, 3] = 12.3;
            //dataFrame[2, 3] = 13.7;
            //dataFrame[3, 3] = 13.7;

            //dataFrame[0, 4] = 12.1;
            //dataFrame[1, 4] = 12.3;
            //dataFrame[2, 4] = 13.7;
            //dataFrame[3, 4] = "fgh";
            //dataFrame.Show();

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
            //array[0][ 0] = 00;

            //array[1][ 0] = 10;
            //array[1][ 1] = "str";

            //array[2][ 0] = 20;
            //array[2][ 1] = "str";
            //array[2][ 2] = 22;

            //array[3][ 0] = 30;
            //array[3][ 1] = "str";
            //array[3][ 2] = 32;
            //array[3][ 3] = 33;

            //for (int i = 0; i < array.Length; i++)
            //{
            //    for (int j = 0; j < array[i].Length; j++)
            //    {
            //        Console.WriteLine($"{array[i][j]} {array[i][ j].GetType()}");
            //    }
            //    Console.WriteLine();
            ////}
            //DataFrame dataFrame = new DataFrame(array);
            //dataFrame.Show();


            //var diction = new Dictionary<int, IEnumerable<int>>()
            //{
            //    {0, new HashSet<int>(){ 0, 1, 2, 3, 4} },
            //    {1, new HashSet<int>(){ 0, 1, 2, 3, 4, 5, 6} }
            //};
            //DataFrame dataf = new DataFrame(diction);
            ////dataf.Show();

            DataFrame dataFrame = new DataFrame(@"E:\Kurs_senticode\DataF\test.csv");
            dataFrame.Show();


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
