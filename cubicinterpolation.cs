using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace methods
{
    class Program
    {

        public static double Splines(double[,] f_x, int n, double x)
        {
            double[] a = new double[n + 1];
            double[] b = new double[n];
            double[] d = new double[n];
            double[] h = new double[n];
            double[] alpha = new double[n];
            double[] c = new double[n + 1];
            double[] l = new double[n + 1];
            double[] mu = new double[n + 1];
            double[] z = new double[n + 1];
            // Нахождение коэффициентов
            for (int i = 0; i < n; i++)
            {
                a[i] = f_x[i, 1];
            }
            for (int i = 1; i < n; i++)
            {
                h[i - 1] = f_x[i, 0] - f_x[i - 1, 0];
            }
            for (int i = 1; i < n; i++)
            {
                alpha[i] = (3 * (a[i + 1] - a[i]) / h[i]) - (3 * (a[i] - a[i - 1]) / h[i - 1]);
            }
            l[0] = 1;
            mu[0] = z[0] = 0;
            for (int i = 1; i < (n - 1); i++)
            {
                l[i] = 2 * (f_x[i + 1, 0] - f_x[i - 1, 0]) - (h[i - 1] * mu[i - 1]);
                mu[i] = h[i] / l[i];
                z[i] = (alpha[i] - h[i - 1] * z[i - 1]) / l[i];
            }
            l[n] = 1;
            z[n] = c[n] = 0;
            for (int j = n - 1; j >= 0; j--)
            {
                c[j] = z[j] - mu[j] * c[j + 1];
                b[j] = ((a[j + 1] - a[j]) / h[j]) - ((h[j] * (c[j + 1] + 2 * c[j])) / 3);
                d[j] = (c[j + 1] - c[j]) / (3 * h[j]);
            }
            double[] output_set = new double[n];
            double sum = 0;
            // Заполнение набора сплайнов
            for (int i = 1; i < (n - 1); i++)
            {
                output_set[i] = a[i] + b[i] * (x - f_x[i - 1, 0]) + c[i] * Math.Pow(x - f_x[i - 1, 0], 2.0) + d[i]
                * Math.Pow(x - f_x[i - 1, 0], 3.0);
                sum += output_set[i];
            }
            return sum;
        }
        public static double[] Func(double[,] Xs, int n)
        {
            Console.WriteLine("Разделенные разности многочлена Ньютона:");
            List<double> res = new List<double>();
            List<double> bufArr = new List<double>();
            res.Add(Xs[0, 1]);
            for (int i = 1; i < n; i++)
                bufArr.Add((Xs[i, 1] - Xs[i - 1, 1]) / (Xs[i, 0] - Xs[i - 1, 0]));
            for (int step = 1; step < n; step++)
            {
                res.Add(bufArr[0]);
                int count = bufArr.Count;
                for (int i = 1; i < count; i++)
                {
                    bufArr.Add((bufArr[i] - bufArr[i - 1]) / (Xs[i + step, 0] - Xs[i - 1, 0]));
                }
                for (int i = 0; i < count; i++)
                    Console.Write(bufArr[i] + " ");
                Console.WriteLine();
                for (int i = 0; i < count; i++)
                    bufArr.RemoveAt(0);
            }
            return res.ToArray();
        }
        public static void FShow(double[,] arr, int n)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < n * 2 - 1; j++)
                {
                    Console.Write(arr[j, i] + " ");
                }
                Console.WriteLine();
            }
        }
        public static void Lx(ref double[,] Xs, int n, double[] elems)
        {
            for (int elem = 0; elem < elems.Length; elem++)
            {
                double chislitel;
                double znamenatel;
                double sum = 0;
                for (int kIter = 0; kIter < n * 2; kIter += 2)
                {
                    chislitel = 1;
                    znamenatel = 1;
                    for (int iter = 0; iter < n * 2; iter += 2)
                    {
                        if (Xs[iter, 0] != Xs[kIter, 0])
                            chislitel *= (elems[elem] - Xs[iter, 0]);
                        else chislitel *= 1;
                        if (Xs[iter, 0] != Xs[kIter, 0])
                            znamenatel *= (Xs[kIter, 0] - Xs[iter, 0]);
                        else znamenatel *= 1;
                    }
                    sum += Xs[kIter, 1] * (chislitel / znamenatel);
                }
                Xs[elem * 2 + 1, 1] = sum;
            }
        }
        public static void MatrixResult(double[,] Xs, int n)
        {
            Console.WriteLine("Матрица для Гаусса:");
            double[,] Matrix = new double[n, n + 1];
            for (int i = 0; i < n * 2; i += 2)
            {
                for (int j = n; j > -1; j--)
                {
                    Matrix[i / 2, n - j] = Math.Pow(Xs[i, 0], j);
                }
            }
            for (int i = 0; i < n; i++)
                Matrix[i, n] = Xs[2 * i, 1];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n + 1; j++)
                {
                    Console.Write(Matrix[i, j] + " ");
                }
                Console.WriteLine();
            }

        }


        static void Main(string[] args)
        {
            Console.WriteLine("Введите кол-во узлов: ");
            int n = int.Parse(Console.ReadLine());
            double[,] arr = new double[n * 2, 2];
            double[,] array = new double[n, 2];
            for (int i = 0; i < n * 2; i += 2)
            {
                Console.WriteLine("Введите узел и зн-е функции в узле");
                string[] s = Console.ReadLine().Split();
                arr[i, 0] = double.Parse(s[0]);
                arr[i, 1] = double.Parse(s[1]);
                array[i / 2, 0] = arr[i, 0];

                array[i / 2, 1] = arr[i, 1];
            }
            double[] elems = new double[n - 1];//(x0+x1)/2
            for (int i = 1; i < n * 2 - 1; i += 2)
            {
                double k = (arr[i - 1, 0] +
                arr[i + 1, 0]) / 2;
                arr[i, 0] = k;
                elems[i / 2] = k;
            }
            MatrixResult(arr, n);
            Lx(ref arr, n, elems);
            double[,] cubeInterp = new double[n * 2, 2];
            for (int i = 0; i < n * 2; i += 2)
            {
                cubeInterp[i, 0] = arr[i, 0];
                cubeInterp[i, 1] = arr[i, 1];
            }
            for (int i = 1; i < n * 2 - 1; i += 2)
            {
                cubeInterp[i, 0] = (cubeInterp[i - 1, 0] + cubeInterp[i + 1, 0]) / 2;
                cubeInterp[i, 1] = Splines(array, n, cubeInterp[i, 0]);
            }
            Console.WriteLine("Кубическая интерполяция:");
            for (int i = 0; i < n * 2; i++)
            {
                Console.Write(cubeInterp[i, 0] + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < n * 2; i++)
            {
                Console.Write(cubeInterp[i, 1] + " ");
            }
            Console.ReadKey();
        }
    }
}