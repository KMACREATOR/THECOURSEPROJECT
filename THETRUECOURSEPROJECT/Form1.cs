using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyApp
{
    public partial class Form1 : Form
    {
        // Переменные для текста и изображения
        private string textToDisplay = "";  // Текст, который будет отображен
        private Point textLocation = new Point(100, 100);  // Местоположение текста
        private Image currentImage = null;  // Текущее изображение
        private Point imageLocation = new Point(200, 200);  // Местоположение изображения

        // Флаги для перетаскивания
        private bool isDraggingText = false;
        private bool isDraggingImage = false;

        // Для отслеживания последней позиции при перетаскивании
        private Point lastTextPosition;
        private Point lastImagePosition;

        public Form1()
        {
            InitializeComponent();  // Вызов метода инициализации компонентов
        }

        // Отображение текста и изображения на форме
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!string.IsNullOrEmpty(textToDisplay))
            {
                e.Graphics.DrawString(textToDisplay, this.Font, Brushes.Black, textLocation);
            }

            if (currentImage != null)
            {
                e.Graphics.DrawImage(currentImage, imageLocation);
            }
        }

        // Обработчик для кнопки "Добавить текст"
        private void btnAddText_Click(object sender, EventArgs e)
        {
            using (Form inputForm = new Form())
            {
                inputForm.Text = "Введите текст";
                inputForm.Size = new Size(400, 200);

                TextBox textBox = new TextBox()
                {
                    Multiline = true,
                    Dock = DockStyle.Fill,
                    Font = new Font("Arial", 14)
                };

                Button btnOK = new Button()
                {
                    Text = "OK",
                    Dock = DockStyle.Bottom
                };

                inputForm.Controls.Add(textBox);
                inputForm.Controls.Add(btnOK);

                btnOK.Click += (s, args) =>
                {
                    textToDisplay = textBox.Text;  // Сохраняем введенный текст
                    inputForm.Close();  // Закрываем окно
                    Invalidate();  // Перерисовываем форму
                };

                inputForm.ShowDialog();
            }
        }

        // Обработчик для кнопки "Добавить изображение"
        private void btnAddImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    currentImage = Image.FromFile(openFileDialog.FileName);  // Загружаем изображение
                    Invalidate();  // Перерисовываем форму
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке изображения: " + ex.Message);
                }
            }
        }

        // Обработчики для перетаскивания текста
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // Проверка, находим ли мы на тексте для его перетаскивания
            if (textToDisplay != "" && e.Button == MouseButtons.Left &&
                new Rectangle(textLocation, new Size(300, 30)).Contains(e.Location))  // Устанавливаем область для перетаскивания текста
            {
                isDraggingText = true;
                lastTextPosition = e.Location;
            }

            // Проверка, находим ли мы на изображении для его перетаскивания
            if (currentImage != null && e.Button == MouseButtons.Left &&
                new Rectangle(imageLocation, new Size(currentImage.Width, currentImage.Height)).Contains(e.Location))  // Проверка на область изображения
            {
                isDraggingImage = true;
                lastImagePosition = e.Location;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            // Перетаскиваем текст
            if (isDraggingText)
            {
                textLocation.X += e.X - lastTextPosition.X;
                textLocation.Y += e.Y - lastTextPosition.Y;
                lastTextPosition = e.Location;
                Invalidate();  // Перерисовываем форму
            }

            // Перетаскиваем изображение
            if (isDraggingImage)
            {
                imageLocation.X += e.X - lastImagePosition.X;
                imageLocation.Y += e.Y - lastImagePosition.Y;
                lastImagePosition = e.Location;
                Invalidate();  // Перерисовываем форму
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            // Останавливаем перетаскивание
            isDraggingText = false;
            isDraggingImage = false;
        }
    }
}
