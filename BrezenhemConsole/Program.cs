using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrezenhemConsole
{
    class Program
    {
        private const int size = 32;
        private static bool[,] _matrix = new bool[size, size];
        private void ResetMatrix()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _matrix[i, j] = false;
                }
            }
        }

        private static void ShowMatrix()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Console.Write((_matrix[i, j]?1:0) + " ");
                }
                Console.WriteLine();
            }
        }

        private static int Sign(int x)
        {
            return (x > 0) ? 1 : (x < 0) ? -1 : 0;
            //возвращает 0, если аргумент (x) равен нулю; -1, если x < 0 и 1, если x > 0.
        }

        public static void DrawBresenhamLine(int xstart, int ystart, int xend, int yend)
        /**
         * xstart, ystart - начало;
         * xend, yend - конец; 
         * "g.drawLine (x, y, x, y);" используем в качестве "setPixel (x, y);"
         * Можно писать что-нибудь вроде g.fillRect (x, y, 1, 1);
         */
        {
            int x, y, dx, dy, incx, incy, pdx, pdy, es, el, err;

            dx = xend - xstart;//проекция на ось икс
            dy = yend - ystart;//проекция на ось игрек

            incx = Sign(dx);
            /*
             * Определяем, в какую сторону нужно будет сдвигаться. Если dx < 0, т.е. отрезок идёт
             * справа налево по иксу, то incx будет равен -1.
             * Это будет использоваться в цикле постороения.
             */
            incy = Sign(dy);
            /*
             * Аналогично. Если рисуем отрезок снизу вверх -
             * это будет отрицательный сдвиг для y (иначе - положительный).
             */

            if (dx < 0) dx = -dx;//далее мы будем сравнивать: "if (dx < dy)"
            if (dy < 0) dy = -dy;//поэтому необходимо сделать dx = |dx|; dy = |dy|
                                 //эти две строчки можно записать и так: dx = Math.abs(dx); dy = Math.abs(dy);

            if (dx > dy)
            //определяем наклон отрезка:
            {
                /*
                 * Если dx > dy, то значит отрезок "вытянут" вдоль оси икс, т.е. он скорее длинный, чем высокий.
                 * Значит в цикле нужно будет идти по икс (строчка el = dx;), значит "протягивать" прямую по иксу
                 * надо в соответствии с тем, слева направо и справа налево она идёт (pdx = incx;), при этом
                 * по y сдвиг такой отсутствует.
                 */
                pdx = incx; pdy = 0;
                es = dy; el = dx;
            }
            else//случай, когда прямая скорее "высокая", чем длинная, т.е. вытянута по оси y
            {
                pdx = 0; pdy = incy;
                es = dx; el = dy;//тогда в цикле будем двигаться по y
            }

            x = xstart;
            y = ystart;
            err = el / 2;
            _matrix[x, y] = true;
            //g.DrawLine(new Pen(Brushes.Black), x, y, x, y);//ставим первую точку
            //все последующие точки возможно надо сдвигать, поэтому первую ставим вне цикла

            for (int t = 0; t < el; t++)//идём по всем точкам, начиная со второй и до последней
            {
                err -= es;
                if (err < 0)
                {
                    err += el;
                    x += incx;//сдвинуть прямую (сместить вверх или вниз, если цикл проходит по иксам)
                    y += incy;//или сместить влево-вправо, если цикл проходит по y
                }
                else
                {
                    x += pdx;//продолжить тянуть прямую дальше, т.е. сдвинуть влево или вправо, если
                    y += pdy;//цикл идёт по иксу; сдвинуть вверх или вниз, если по y
                }

                _matrix[x, y] = true;
                //g.DrawLine(new Pen(Brushes.Black), x, y, x, y);
            }
        }

        static void Main(string[] args)
        {
            Console.Write("Enter x0: ");
            var x0 = int.Parse(Console.ReadLine());
            Console.Write("Enter y0: ");
            var y0 = int.Parse(Console.ReadLine());
            Console.Write("Enter x1: ");
            var x1 = int.Parse(Console.ReadLine());
            Console.Write("Enter y1: ");
            var y1 = int.Parse(Console.ReadLine());
            DrawBresenhamLine(x0, y0, x1, y1);
            ShowMatrix();
        }
    }
}
