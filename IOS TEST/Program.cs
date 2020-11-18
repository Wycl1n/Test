using System;
using System.IO;
using System.Text.RegularExpressions;

namespace IOS_TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            string PathToFile = @"C:\FileToRead.txt";

            GetPathOrNoPath(PathToFile);

        }

        static int[,] SearchAlgorithm(string PathToFile)
        {
            int width = GetWidth(PathToFile) + 2, height = GetHeight(PathToFile) + 2;
            int[,] Map = new int[height, width];
            int[,] t = GetMap(PathToFile);
            for(int i=0;i<height;i++)
            {
                for(int j=0;j<width;j++)
                {
                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1) 
                    {
                        Map[i, j] = 1;
                    }
                    else
                    {
                        try
                        {
                            Map[i, j] = t[i - 1, j - 1];
                        }
                        catch
                        {
                            Map[i, j] = t[i + 1, j + 1];
                        }
                    }
                }
            } 

            bool add = true;
            int[,] CopyOfMap = new int[height, width];
            int step = 0;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    if (Map[i, j] == 1)
                        CopyOfMap[i, j] = -2;
                    else
                        CopyOfMap[i, j] = -1;
                }
            CopyOfMap[GetCoordinatesOfEnd(PathToFile)[0] + 1, GetCoordinatesOfEnd(PathToFile)[1] + 1] = 0;
            while (add)
            {
                add = false;
                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                    {
                        if (CopyOfMap[j, i] == step)
                        {
                            if (i - 1 >= 0 && CopyOfMap[j - 1, i] != -2 && CopyOfMap[j - 1, i] == -1)
                                CopyOfMap[j - 1, i] = step + 1;
                            if (j - 1 >= 0 && CopyOfMap[j, i - 1] != -2 && CopyOfMap[j, i - 1] == -1)
                                CopyOfMap[j, i - 1] = step + 1;
                            if (i + 1 < width && CopyOfMap[j + 1, i] != -2 && CopyOfMap[j + 1, i] == -1)
                                CopyOfMap[j + 1, i] = step + 1;
                            if (j + 1 < height && CopyOfMap[j, i + 1] != -2 && CopyOfMap[j, i + 1] == -1)
                                CopyOfMap[j, i + 1] = step + 1;
                        }
                    }
                step++;
                add = true;
                if (CopyOfMap[GetCoordinatesOfStart(PathToFile)[0] + 1, GetCoordinatesOfStart(PathToFile)[1] + 1] != -1)
                {
                    add = false;
                }
                if (step > width * height)
                    add = false;
            }
            return CopyOfMap;
        }

        static void GetPathOrNoPath(string PathToFile)
        {
            int[,] Map = SearchAlgorithm(PathToFile);
            if (Map[GetCoordinatesOfStart(PathToFile)[0] + 1, GetCoordinatesOfStart(PathToFile)[1] + 1] != -1)
            {
                string Path = "Path is: Start->";
                char[,] ResultMap = new char[GetHeight(PathToFile) + 2, GetWidth(PathToFile) + 2];
                int x = GetCoordinatesOfStart(PathToFile)[0] + 1;
                int y = GetCoordinatesOfStart(PathToFile)[1] + 1;
                int n_of_moves = 0;
                for (int i = 0; i < GetHeight(PathToFile) + 2; i++) 
                {
                    for (int j = 0; j < GetWidth(PathToFile) + 2; j++)
                    {
                        if (Map[i, j] == -2) ResultMap[i, j] = 'O';
                        else ResultMap[i, j] = ' ';
                    }
                }
                while (Map[x,y]!=0)
                {
                    if (Map[x, y] - 1 == Map[x + 1, y])
                    {
                        ResultMap[x, y] = 'D';
                        x++;
                        n_of_moves++;
                        Path += "D->";
                        continue;
                    }
                    if (Map[x, y] - 1 == Map[x - 1, y])
                    {
                        ResultMap[x, y] = 'U';
                        x--;
                        n_of_moves++;
                        Path += "U->";
                        continue;
                    }
                    if (Map[x, y] - 1 == Map[x, y + 1])
                    {
                        ResultMap[x, y] = 'R';
                        y++;
                        n_of_moves++;
                        Path += "R->";
                        continue;
                    }
                    if (Map[x, y] - 1 == Map[x, y - 1])
                    {
                        ResultMap[x, y] = 'L';
                        y--;
                        n_of_moves++;
                        Path += "L->";
                        continue;
                    }
                }
                ResultMap[GetCoordinatesOfEnd(PathToFile)[0] + 1, GetCoordinatesOfEnd(PathToFile)[1] + 1] = 'F';
                PrintMap(ResultMap, PathToFile);
                Console.WriteLine(Path + "You are at End");
                Console.WriteLine("Number Of done moves: " + n_of_moves);
            }
            else
            {
                Console.WriteLine("there is no path");
            }
        }
        static int[,] GetMap(string PathToFile)
        {
            int a, b;
            int[,] ArrayOfBalls;
            int[] CoordinatesOfStart = new int[2];
            int[] CoordinatesOfEnd = new int[2];
            using (StreamReader sr = new StreamReader(PathToFile))
            {
                a = Convert.ToInt32(sr.ReadLine());
                b = Convert.ToInt32(sr.ReadLine());
                ArrayOfBalls = new int[a, b];
                string t = sr.ReadLine();
                t = t.Remove(0, 1).Remove(t.Length - 2, 1);
                string value = "";
                for (int i = 0; i < t.Length; i++) 
                {
                    if (t[i] == ',') 
                    {
                        CoordinatesOfStart[0] = int.Parse(Convert.ToString(value)) - 1;
                        value = "";
                        i++;
                    }
                    value += t[i];
                }
                CoordinatesOfStart[1] = int.Parse(Convert.ToString(value)) - 1;
                t = sr.ReadLine();
                t = t.Remove(0, 1).Remove(t.Length - 2, 1);
                value = "";
                for (int i = 0; i < t.Length; i++)
                {
                    if (t[i] == ',')
                    {
                        CoordinatesOfEnd[0] = int.Parse(Convert.ToString(value)) - 1;
                        value = "";
                        i++;
                    }
                    value += t[i];
                }
                CoordinatesOfEnd[1] = int.Parse(Convert.ToString(value)) - 1;
                string z = sr.ReadToEnd();
                int I, J;
                I = J = 0;
                for (int g = 0; g < z.Length; g++)
                {
                    if (z[g] == '1')
                    {
                        ArrayOfBalls[I, J++] = 1;
                        if (J >= a)
                        {
                            J = 0;
                            I++;
                        }
                    }
                    if (z[g] == '0')
                    {
                        ArrayOfBalls[I, J++] = 0;
                        if (J >= a)
                        {
                            J = 0;
                            I++;
                        }
                    }
                }
            }
            return ArrayOfBalls;
        } 
        static int GetHeight(string PathToFile) 
        {
            using (StreamReader t = new StreamReader(PathToFile))
            {
                return Convert.ToInt32(t.ReadLine());
            }
        }
        static int GetWidth(string PathToFile) 
        {
            using (StreamReader t = new StreamReader(PathToFile))
            {
                t.ReadLine();
                return Convert.ToInt32(t.ReadLine());
            }
        }
        static int[] GetCoordinatesOfStart(string PathToFile) 
        {
            using (StreamReader sr = new StreamReader(PathToFile))
            {
                int[] coordinates = new int[2];
                sr.ReadLine();
                sr.ReadLine();
                string t = sr.ReadLine();
                t = t.Remove(0, 1).Remove(t.Length - 2, 1);
                string value = "";
                for (int i = 0; i < t.Length; i++)
                {
                    if (t[i] == ',')
                    {
                        coordinates[0] = int.Parse(Convert.ToString(value)) - 1;
                        value = "";
                        i++;
                    }
                    value += t[i];
                }
                coordinates[1] = int.Parse(Convert.ToString(value)) - 1;
                return coordinates;
            }
        }
        static int[] GetCoordinatesOfEnd(string PathToFile) 
        {
            using (StreamReader sr = new StreamReader(PathToFile))
            {
                int[] coordinates = new int[2];
                sr.ReadLine();
                sr.ReadLine();
                sr.ReadLine();
                string t = sr.ReadLine();
                t = t.Remove(0, 1).Remove(t.Length - 2, 1);
                string value = "";
                for (int i = 0; i < t.Length; i++)
                {
                    if (t[i] == ',')
                    {
                        coordinates[0] = int.Parse(Convert.ToString(value)) - 1;
                        value = "";
                        i++;
                    }
                    value += t[i];
                }
                coordinates[1] = int.Parse(Convert.ToString(value)) - 1;
                return coordinates;
            }

        }
        static void PrintMap(int[,] a, string PathToFile)
        {
            for (int i = 0; i < GetHeight(PathToFile) + 2; i++) 
            {
                for (int j = 0; j < GetWidth(PathToFile) + 2; j++)
                {
                    if (a[i, j] < 10 && a[i, j] >= 0) 
                        Console.Write(" " + a[i, j] + "|");
                    else
                        Console.Write(a[i, j] + "|");
                }
                Console.WriteLine();
            }
        }
        static void PrintMap(char[,] a, string PathToFile)
        {
            for (int i = 0; i < GetHeight(PathToFile) + 2; i++)
            {
                for (int j = 0; j < GetWidth(PathToFile) + 2; j++)
                {
                    if (a[i, j] == 'O')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(a[i, j] + "|");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(a[i, j] + "|");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                }
                Console.WriteLine();
            }
        }
    }
}
