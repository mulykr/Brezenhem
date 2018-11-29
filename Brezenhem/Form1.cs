using System;
using System.Drawing;
using System.Windows.Forms;

namespace Brezenhem
{
    public partial class Form1 : Form
    {
        private const int size = 32;
        private readonly bool[,] _matrix = new bool[size, size];
        private readonly Graphics _graphics;
        private readonly Bitmap _img; 

        public Form1()
        {
            InitializeComponent();

            _img = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = _img;
            _graphics = Graphics.FromImage(_img);
            ShowGrid(_graphics);
        }

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

        private void ShowMatrix(Graphics g)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (_matrix[i, j])
                    {
                        g.FillRectangle(Brushes.Black, i*20, j*20, 20, 20);
                    }
                    else
                    {
                        g.FillRectangle(Brushes.White, i * 20, j * 20, 20, 20);
                    }
                }
            }
        }

        private int Sign(int x)
        {
            return (x > 0) ? 1 : (x < 0) ? -1 : 0;
        }

        public void DrawBresenhamLine(int xstart, int ystart, int xend, int yend)
        {
            int pdx, pdy, es, el;

            var dx = xend - xstart;
            var dy = yend - ystart;

            var incx = Sign(dx);
            var incy = Sign(dy);

            if (dx < 0) dx = -dx;
            if (dy < 0) dy = -dy;

            if (dx > dy)
            {
                pdx = incx;
                pdy = 0;
                es = dy;
                el = dx;
            }
            else
            {
                pdx = 0;
                pdy = incy;
                es = dx;
                el = dy;
            }

            var x = xstart;
            var y = ystart;
            var err = el / 2;
            _matrix[x, y] = true;

            for (int t = 0; t < el; t++)
            {
                err -= es;
                if (err < 0)
                {
                    err += el;
                    x += incx;
                    y += incy;
                }
                else
                {
                    x += pdx;
                    y += pdy;
                }

                _matrix[x, y] = true;
            }
        }

        private void ShowGrid(Graphics g)
        {
            for (int i = 0; i < 32; i++)
            {
                g.DrawLine(new Pen(Color.DarkGray), 0, i * 20, 640, i*20);
                g.DrawLine(new Pen(Color.DarkGray), i * 20, 0, i * 20, 640);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResetMatrix();

            try
            {
                var x0 = int.Parse(textBox1.Text);
                var y0 = int.Parse(textBox2.Text);
                var x1 = int.Parse(textBox3.Text);
                var y1 = int.Parse(textBox4.Text);

                DrawBresenhamLine(x0, y0, x1, y1);

                pictureBox1.Image = _img;

                ShowMatrix(_graphics);
                //Fulfil(15, 15, 15, _graphics);
                pictureBox1.Image = _img;
                ShowGrid(_graphics);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Error: " + exception.Message);
            }
            
        }

        public void Fulfil(int radius, int x, int y, Graphics g)
        {
            // (x-x0)^2 +(y-y0)^2 = r^2
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (Math.Abs((x-i)*(x-i) + (y - j)*(y - j) - radius*radius) < 10)
                    {
                        DrawBresenhamLine(x, y, i, j);
                    }
                }
            }

            ShowMatrix(g);
        }


    }
}
