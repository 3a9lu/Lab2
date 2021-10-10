using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using org.mariuszgromada.math.mxparser;
using ZedGraph;

namespace Lab1
{
    public partial class Метод : Form
    {
        public int count; 
        public static List<Point> steps = new List<Point>(); // Список для точек
        public class Point // Сохраниние самих точек
        {
            public double x, y;
            public Point(double X, double Y)
            {
                this.x = X;
                this.y = Y;
            }
        }
        public Метод()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;  // Вывод формы по центру экрана
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear(); // Очистка A
            textBox2.Clear(); // Очистка B
            textBox3.Clear(); // Очистка E
            textBox4.Clear(); // Очистка формулы
            chart1.Series[0].Points.Clear(); // Очистка графика
            chart1.Series[1].Points.Clear(); // Очистка точки минимума
            chart1.Series[2].Points.Clear(); // Очистка точек шагов
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            int length = textBox1.Text.Length;
            if (length == 0 && ch == ',' && ch == '-')
            {
                e.Handled = true;
            }
            if (!Char.IsDigit(ch) && ch != 8 && (ch != ',' || textBox1.Text.Contains(",")) && ((ch != '-' || textBox1.Text.Contains("-")))) // Если число, BACKSPACE запятая или минус, то вводим
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            int length = textBox2.Text.Length;
            if (length == 0 && ch == ',' && ch == '-')
            {
                e.Handled = true;
            }
            if (!Char.IsDigit(ch) && ch != 8 && (ch != ',' || textBox2.Text.Contains(",")) && ((ch != '-' || textBox2.Text.Contains("-")))) // Если число, BACKSPACE запятая или минус, то вводим
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            int length = textBox3.Text.Length;
            if (length == 0 && ch == ',' && ch == '-')
            {
                e.Handled = true;
            }
            if (!Char.IsDigit(ch) && ch != 8 && (ch != ',' || textBox3.Text.Contains(",")) && ((ch != '-' || textBox3.Text.Contains("-")))) // Если число, BACKSPACE запятая или минус, то вводим
            {
                e.Handled = true;
            }
        }

        

        public async Task Picture(double borderA, double borderB, double Step) // Асинхронный метод отрисовки графика
        {
            await Task.Run(() => Graph(borderA, borderB, Step));
        }

        public void Graph(double borderA, double borderB, double Step) // Отрисовка графика
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            Function f = new Function($"f(x)={textBox4.Text}");
            int n = (int)Math.Ceiling((borderB - borderA) / Step) + 1;

            double[] x = new double[n]; // Массив значений X 
            double[] y = new double[n]; // Массив значений Y

            for (int i = 0; i < n; ++i) // Расчитываем точки для графиков функции
            {
                x[i] = borderA + Step * i; // Вычисляем значение X
                string form = $"f({x[i]})";
                string withZam = form.Replace(",", "."); // Заменяем
                Expression graph = new Expression(withZam, f);
                y[i] = graph.calculate();
            }
            chart1.ChartAreas[0].AxisX.Minimum = borderA; // Оси графика
            chart1.ChartAreas[0].AxisX.Maximum = borderB;

            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = Step; // Шаг сетки
            chart1.Series[0].Points.DataBindXY(x, y); // Добавление значений в график
        }


        async Task<Double> Res(double a, double b, double e) // Асинхронный метод золотого сечения
        {
            var result = await Task.Run(() => GoldenSection(a, b, e));
            return result;
        }

        double F(double x) // Возвращение функции
        {
            Argument x_arg = new Argument("x");
            Expression fx = new Expression(textBox4.Text, x_arg);
            x_arg.setArgumentValue(x);
            return fx.calculate();
        }

        private double GoldenSection(double a, double b, double e)  // Метод Золотого сечения
        {
            double d = (-1 + Math.Sqrt(5)) / 2;
            double x1, x2;
            while (true)
            {
                x1 = b - (b - a) * d;
                x2 = a + (b - a) * d;
                if (F(x1) >= F(x2))
                {
                    a = x1;
                    Point p = new Point(a, F(a));
                    steps.Add(p);
                }
                else
                {
                    b = x2;
                    Point p = new Point(b, F(b));
                    steps.Add(p);
                }
                if (Math.Abs(b - a) < e)
                    break;
            }

            //chart1.Series[1].Points.Clear(); // Очистка предыдущей точки
            //chart1.Series[1].Points.AddXY(c1, f1(c1)); // Добавление точки на график
            return (a + b) / 2;

        }


        private async void рассчитатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!");
                return;
            }

            /*if (textBox3.Text.Length > 12) // 10 знаков после запятой
            {
                MessageBox.Show("Уменьшите Epsilon", "Ошибка!");
                return;
            }*/

            try
            {
                string a, b;
                a = Convert.ToString(textBox1.Text);
                b = Convert.ToString(textBox2.Text);


                if (Convert.ToDouble(a) == Convert.ToDouble(b) || Convert.ToDouble(a) > Convert.ToDouble(b)) // Проверка, больше ли b хотя бы на 1
                {
                    MessageBox.Show("\"b\" должно быть больше \"a\" хотя бы на 1", "Ошибка");
                    return;
                }



                double a11 = Convert.ToDouble(a);
                double b11 = Convert.ToDouble(b);
                double eps = Convert.ToDouble(textBox3.Text);

                double result = await Res(a11, b11, eps);

                double borderA = Convert.ToDouble(textBox1.Text); // Граница A
                double borderB = Convert.ToDouble(textBox2.Text); // Граница B
                double Step = 0.5; // Шаг

                await Picture(borderA, borderB, Step);
                label8.Text = Convert.ToString(result); // Вывод значения

                count = 0;

                chart1.Series[1].Points.AddXY(result, F(result));

                Point end = new Point(result, F(result));//добавляем конечную точку
                steps.Add(end);

                label8.Text = $"Шаг {count}/{steps.Count}";


                double amin, bmax;
                PointPairList stack1 = new PointPairList();
                PointPairList stack2 = new PointPairList();
                GraphPane pane = zedGraphControl1.GraphPane;
                pane.CurveList.Clear();
                pane.GraphObjList.Clear();

                amin = Convert.ToDouble(textBox1.Text);
                bmax = Convert.ToDouble(textBox2.Text);
                
                for (double x = amin; x <= bmax; x += 0.1) // Рисование графика по границам
                {
                    stack1.Add(x, F(x));
                }

                double minX = GoldenSection(double.Parse(textBox1.Text), double.Parse(textBox2.Text), double.Parse(textBox3.Text)); // Точка минимума
                stack2.Add(minX, F(minX));

                LineItem Curve = pane.AddCurve(textBox4.Text, stack1, Color.FromArgb(184, 57, 8), SymbolType.None);
                LineItem Min = pane.AddCurve("Минимум", stack2, Color.Black, SymbolType.XCross);
                
                zedGraphControl1.AxisChange(); // Обновление графика

                Argument x_arg = new Argument("x");
                GraphPane graphfield = zedGraphControl1.GraphPane;
                graphfield.Title.Text = "График";
                zedGraphControl1.Invalidate();
            }

            catch
            {
                MessageBox.Show("Проверьте введёные значения!", "Ошибка!");
            }
        }

        private void button1_Click(object sender, EventArgs e) // Кнопка назад
        {
            if (count >= 1)
            {
                chart1.Series[2].Points.Clear(); // Очищаем предыдущую
                chart1.Series[2].Points.AddXY(steps[count - 1].x, steps[count - 1].y);
                label8.Text = $"Шаг {count}/{steps.Count}";
                --count;
            }
        }


        private void button2_Click(object sender, EventArgs e) // Кнопка вперёд
        {
            if (count < steps.Count)
            {
                chart1.Series[2].Points.Clear(); // Очищаем предыдущую
                chart1.Series[2].Points.AddXY(steps[count].x, steps[count].y);
                ++count;
                label8.Text = $"Шаг {count}/{steps.Count}";
            }
        }
    }
}
