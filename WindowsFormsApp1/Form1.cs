using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        // Координаты начальной и конечной точки
        public int xn, yn, xk, yk;
        Bitmap myBitmap; // Изображение для рисования
        Color currentBorderColor; // Цвет границы
        Graphics g; // Графический объект для рисования
        Color ZalivkaColor; // Цвет заливки

        public Form1()
        {
            InitializeComponent();
            // Инициализация битмапа с размерами pictureBox
            myBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(myBitmap);
            // Заливка фона белым цветом
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = myBitmap;

            // Установка значений для NumericUpDown
            numericUpDown1.Minimum = 1;
            numericUpDown1.Maximum = 100;
            numericUpDown1.Value = 5; // Значение по умолчанию
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            // Проверка, если выбрана радиокнопка для рисования
            if (radioButton1.Checked)
            {
                int index, numberNodes;
                double xOutput, yOutput, dx, dy;
                int lineWidth = (int)numericUpDown1.Value; // Получение ширины линии
                Pen myPen = new Pen(currentBorderColor, lineWidth); // Создание пера с выбранным цветом

                // Установка конечных координат
                xk = e.X;
                yk = e.Y;
                dx = xk - xn; // Разница по X
                dy = yk - yn; // Разница по Y
                numberNodes = 200; // Количество узлов для рисования
                xOutput = xn; // Начальное значение X
                yOutput = yn; // Начальное значение Y

                // Рисование линии от начальной до конечной точки
                for (index = 1; index <= numberNodes; index++)
                {
                    g.DrawRectangle(myPen, (int)xOutput, (int)yOutput, 2, 2); // Рисуем маленький квадрат
                    xOutput = xOutput + dx / numberNodes; // Обновление X
                    yOutput = yOutput + dy / numberNodes; // Обновление Y
                }

                pictureBox1.Image = myBitmap; // Обновление изображения в pictureBox
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Очистка изображения, заполнение белым цветом
            g.FillRectangle(new SolidBrush(Color.White), 0, 0, pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = myBitmap; // Обновление изображения
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Открытие диалогового окна для выбора цвета границы
            DialogResult dialogResult = colorDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK && radioButton1.Checked)
            {
                currentBorderColor = colorDialog1.Color; // Установка выбранного цвета границы
            }
        }

        private void FloodFill(int x1, int y1)
        {
            // Получение цвета текущего пикселя
            Color oldPixelColor = myBitmap.GetPixel(x1, y1);
            // Проверка, если цвет пикселя не равен цвету границы и цвету заливки
            if ((oldPixelColor.ToArgb() != currentBorderColor.ToArgb())
           && (oldPixelColor.ToArgb() != ZalivkaColor.ToArgb()))
            {
                myBitmap.SetPixel(x1, y1, ZalivkaColor); // Установка цвета заливки
                // Рекурсивный вызов для соседних пикселей
                FloodFill(x1 + 1, y1);
                FloodFill(x1 - 1, y1);
                FloodFill(x1, y1 + 1);
                FloodFill(x1, y1 - 1);
            }
            else
            {
                return; // Выход, если цвет совпадает
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            // Блокировка кнопок во время выполнения
            button1.Enabled = false;
            button2.Enabled = false;

            // Проверка, если выбрана радиокнопка для рисования
            if (radioButton1.Checked == true)
            {
                // Рисование прямоугольников с помощью CDA
                CDA(10, 10, 10, 110);
                CDA(10, 10, 110, 10);
                CDA(10, 110, 110, 110);
                CDA(110, 10, 110, 110);

                CDA(150, 10, 150, 200);
                CDA(250, 50, 150, 200);
                CDA(150, 10, 250, 150);
            }
            else
            {
                // Если выбрана радиокнопка для заливки
                if (radioButton2.Checked == true)
                {
                    xn = 160; // Начальная точка для заливки
                    yn = 40;
                    FloodFill(xn, yn); // Вызов функции заливки
                }
            }

            // Обновление изображения в pictureBox
            pictureBox1.Image = myBitmap;
            // Включение кнопок после выполнения
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Открытие диалогового окна для выбора цвета заливки
            DialogResult dialogResult = colorDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                ZalivkaColor = colorDialog1.Color; // Установка выбранного цвета заливки
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // Проверка, если выбрана радиокнопка для рисования
            if (radioButton1.Checked == true)
            {
                // Установка начальной точки для рисования
                xn = e.X;
                yn = e.Y;
            }
            else
            {
                // Блокировка кнопок во время выполнения заливки
                button1.Enabled = false;
                button2.Enabled = false;

                // Вызов функции заливки с координатами мыши
                FloodFill(e.X, e.Y);

                // Включение кнопок после выполнения
                button1.Enabled = true;
                button2.Enabled = true;
                pictureBox1.Image = myBitmap; // Обновление изображения
            }
        }

        private void CDA(int xStart, int yStart, int xEnd, int yEnd)
        {
            // Переменные для рисования линии
            int index, numberNodes;
            double xOutput, yOutput, dx, dy;
            int lineWidth = (int)numericUpDown1.Value; // Получение ширины линии
            Pen myPen = new Pen(currentBorderColor, lineWidth); // Создание пера с выбранным цветом

            // Установка начальных и конечных координат
            xn = xStart;
            yn = yStart;
            xk = xEnd;
            yk = yEnd;
            dx = xk - xn; // Разница по X
            dy = yk - yn; // Разница по Y
            numberNodes = 200; // Количество узлов для рисования
            xOutput = xn; // Начальное значение X
            yOutput = yn; // Начальное значение Y

            // Рисование линии от начальной до конечной точки
            for (index = 1; index <= numberNodes; index++)
            {
                g.DrawRectangle(myPen, (int)xOutput, (int)yOutput, 2, 2); // Рисуем маленький квадрат
                xOutput = xOutput + dx / numberNodes; // Обновление X
                yOutput = yOutput + dy / numberNodes; // Обновление Y
            }

            pictureBox1.Image = myBitmap; // Обновление изображения
        }

    }
}

