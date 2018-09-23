using System;

namespace linaPl.DataFrame
{
    class Program
    {
        static void Main()
        {

            object[][] array = new object[4][];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new object[4];
            }


            array[0][0] = 30/12;
            array[0][1] = "str";
            array[0][2] = 32.4;
            array[0][3] = 33.5;


            array[1][0] = "str";
            array[1][1] = "str";
            array[1][2] = 32.4;
            array[1][3] = 33;


            array[2][0] = 30.67;
            array[2][1] = "str";
            array[2][2] = 32.4;
            array[2][3] = 33/3;

            array[3][0] = 30.45;
            array[3][1] = "str";
            array[3][2] = 32.4;
            array[3][3] = "str";
            
            DataFrame.DataFrame dataFrame = new DataFrame.DataFrame(array);
            var str = dataFrame.PrintAsTable();
            Console.WriteLine(str);
            if (dataFrame.DeleteRow(3))
            {
                str = dataFrame.PrintAsTable();
                Console.WriteLine(str);
            }

            if (dataFrame.DeleteColumn(3))
            {
                str = dataFrame.PrintAsTable();
                Console.WriteLine(str);
            }

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


            Console.ReadLine();
            }
        }


    }

