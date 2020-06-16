using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestApp.src;

namespace TestApp
{
    class Program
    {
        /*Формат таблицы: ID - X - Y*/
        static List<FirstTable> mainPoints = new List<FirstTable>();
        /*Формат таблицы: ID - Xs - Ys - Xe - Ye*/
        static List<SecondTable> segPoints = new List<SecondTable>();
        /*Путь к папке с программой*/
        static string path = AppDomain.CurrentDomain.BaseDirectory;
        /*Результат*/
        static double Result = 0;
        static void Main(string[] args)
        {
            /*Чтение данных из таблиц (формат таблиц (MS-DOS) .csv)*/
            try {
                using (var reader = new StreamReader($"{path}firstTable.csv")) {
                    while (!reader.EndOfStream) {
                        var line = reader.ReadLine().Split(';').ToList();
                        mainPoints.Add(new FirstTable(line[0], line[1], line[2]));
                    }
                    mainPoints.RemoveAt(0);//удаление шапки
                }
                using (var reader = new StreamReader($"{path}secondTable.csv")){
                    while (!reader.EndOfStream) {
                        var line = reader.ReadLine().Split(';').ToList();
                        segPoints.Add(new SecondTable(line[0], line[1], line[2], line[3], line[4]));
                    }
                    segPoints.RemoveRange(0,2);
                }
            }
            catch { Console.WriteLine($"Ошибка, не удалось прочесть одну из таблиц"); }
            foreach (var item in segPoints)
                CheckLine(item);
            Console.WriteLine($"Результат: {Result}");
            Console.ReadKey();
        }
        /*Проверка на принадлежность отрезка к фигуре и вычисления его длины*/
        static void CheckLine(SecondTable item)
        {
            int check = 0;
            /* 0 - отрезок вне фигуры
               1 - отрезок пересекает фигуру
               2 - отрезок полностью внутри фигуры */
            double _x = 0, _y = 0;
            bool startIn = false, endIn = false;
            if (pnpoly(item.Xs, item.Ys)) {
                startIn = true;
                check++;
            }
            if (pnpoly(item.Xe, item.Ye)) {
                endIn = true;
                check++;
            }
            if (check == 1) {
                var a = GetXY(item);
                _x = a[0];
                _y = a[1];
                a.Clear();
            }
            /*длина отрезка = √((X2-X1)²+(Y2-Y1)²)*/
            if (check == 2) 
                Result += Math.Sqrt(Math.Pow(Math.Abs(item.Xe - item.Xs), 2) + Math.Pow(Math.Abs(item.Ye - item.Ys), 2));
            else if (check == 1 && startIn) 
                Result += Math.Sqrt(Math.Pow(Math.Abs(_x - item.Xs), 2) + Math.Pow(Math.Abs(_y - item.Ys), 2));
            else if (check == 1 && endIn) 
                Result += Math.Sqrt(Math.Pow(Math.Abs(item.Xe - _x), 2) + Math.Pow(Math.Abs(item.Ye - _y), 2));
        }
        /*Вычисление точки пересечения*/
        static List<double> GetXY(SecondTable item)
        {
            double xi1 = item.Xs, xi2 = item.Xe, yi1 = item.Ys, yi2 = item.Ye;
            double x = 0, y = 0, tmp, k1, k2, b1, b2;
            /* Чтобы вычислить правильные угловые коэффициенты,
             * должно выполняться условие x1 ≤ x2 и x3 ≤ x4.
             * Если нет - то необходимо поменять местами пары координат отрезков.
             */
            if (xi1 > xi2) {
                tmp = xi1;
                xi1 = xi2;
                xi2 = tmp;
                tmp = yi1;
                yi1 = yi2;
                yi2 = tmp;
            }
            /*Определяем угловой коэффициент в уравнении прямой*/
            if (yi1 == yi2) k1 = 0;
            else k1 = (yi2 - yi1) / (xi2 - xi1);
            b1 = yi1 - k1 * xi1;//Вычисление свободного члена в уравнении прямой
            for (int i = 0; i < mainPoints.Count - 1; i++) {
                int j = i + 1 != mainPoints.Count ? i++ : 0;//Вычисление второй вершины грани фигуры
                double xm1 = mainPoints[i].X, xm2 = mainPoints[j].X, ym1 = mainPoints[i].Y, ym2 = mainPoints[j].Y;
                if (xm1 >= xm2) {
                    tmp = xm1;
                    xm1 = xm2;
                    xm2 = tmp;
                    tmp = ym1;
                    ym1 = ym2;
                    ym2 = tmp;
                }
                if (ym2 == ym1) k2 = 0;
                else k2 = (ym2 - ym1) / (xm2 - xm1);
                if (k1 == k2) continue;//Проверка отрезков на параллельность
                b2 = ym1 - k2 * xm1;
                x = (b2 - b1) / (k1 - k2);
                y = k1*x + b1;
                /*Проверка на принадлежность точки к отрезку*/
                if ((x < Math.Max(xi1, xm1)) || (x > Math.Min(xi2, xm2))) continue;
                else if ((xi1 <= xm2 && xm2 <= xi2)||(xi1 <= xm1 && xm1 <= xi2)){
                    break;
                }
            }
            List<double> XY = new List<double>();
            XY.Add(x);
            XY.Add(y);
            return XY;
        }
        /*Проверка точки на принадлежность*/
        static bool pnpoly(double x, double y)
        {
            var npol = mainPoints.Count();
            var xp = mainPoints.Select(f => f.X).ToArray();
            var yp = mainPoints.Select(f => f.Y).ToArray();
            bool c = false;
            for (int i = 0, j = npol - 1; i < npol; j = i++)
                if ((((yp[i] <= y) && (y < yp[j])) ||
                    ((yp[j] <= y) && (y < yp[i]))) &&
                    (((yp[j] - yp[i]) != 0) && (x > ((xp[j] - xp[i]) * (y - yp[i]) / (yp[j] - yp[i]) + xp[i]))))
                    c = !c;
            return c;
        }
    }
}