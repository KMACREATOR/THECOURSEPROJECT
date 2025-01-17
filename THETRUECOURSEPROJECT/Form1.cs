using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MyApp
{
    public partial class Form1 : Form
    {
        private string textToDisplay = "";
        private Point textLocation = new Point(100, 100);
        private Image currentImage = null;
        private Point imageLocation = new Point(200, 200);

        private bool isDraggingText = false;
        private bool isDraggingImage = false;
        private Point lastTextPosition;
        private Point lastImagePosition;

        private bool isDrawing = false;
        private Point lastDrawPoint;

        private float textFontSize = 12f;
        private float imageScale = 1.0f;

        private Bitmap canvasBitmap;
        private Graphics canvasGraphics;

        public Form1()
        {
            InitializeComponent();
            this.MouseWheel += Form1_MouseWheel;

            canvasBitmap = new Bitmap(ClientSize.Width, ClientSize.Height);
            canvasGraphics = Graphics.FromImage(canvasBitmap);
            canvasGraphics.Clear(Color.White);

            // Включаем поддержку Drag and Drop
            this.AllowDrop = true;
            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            // Проверяем, поддерживает ли объект формат изображения
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && IsImageFile(files[0]))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(canvasBitmap, Point.Empty);

            if (!string.IsNullOrEmpty(textToDisplay))
            {
                using (Font font = new Font(this.Font.FontFamily, textFontSize))
                {
                    e.Graphics.DrawString(textToDisplay, font, Brushes.Black, textLocation);
                }
            }

            if (currentImage != null)
            {
                var scaledWidth = (int)(currentImage.Width * imageScale);
                var scaledHeight = (int)(currentImage.Height * imageScale);
                e.Graphics.DrawImage(currentImage, new Rectangle(imageLocation, new Size(scaledWidth, scaledHeight)));
            }
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(textToDisplay) &&
                new Rectangle(textLocation, new Size(300, (int)(textFontSize * 2))).Contains(e.Location))
            {
                textFontSize += e.Delta > 0 ? 1f : -1f;
                textFontSize = Math.Max(1f, textFontSize);
                Invalidate();
            }

            if (currentImage != null &&
                new Rectangle(imageLocation, new Size(
                    (int)(currentImage.Width * imageScale),
                    (int)(currentImage.Height * imageScale))).Contains(e.Location))
            {
                imageScale += e.Delta > 0 ? 0.1f : -0.1f;
                imageScale = Math.Max(0.1f, imageScale);
                Invalidate();
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (textToDisplay != "" && e.Button == MouseButtons.Left &&
                new Rectangle(textLocation, new Size(300, (int)(textFontSize * 2))).Contains(e.Location))
            {
                isDraggingText = true;
                lastTextPosition = e.Location;
            }

            if (currentImage != null && e.Button == MouseButtons.Left &&
                new Rectangle(imageLocation, new Size(
                    (int)(currentImage.Width * imageScale),
                    (int)(currentImage.Height * imageScale))).Contains(e.Location))
            {
                isDraggingImage = true;
                lastImagePosition = e.Location;
            }

            if (isDrawing && e.Button == MouseButtons.Left)
            {
                lastDrawPoint = e.Location;
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingText)
            {
                textLocation.X += e.X - lastTextPosition.X;
                textLocation.Y += e.Y - lastTextPosition.Y;
                lastTextPosition = e.Location;
                Invalidate();
            }

            if (isDraggingImage)
            {
                imageLocation.X += e.X - lastImagePosition.X;
                imageLocation.Y += e.Y - lastImagePosition.Y;
                lastImagePosition = e.Location;
                Invalidate();
            }

            if (isDrawing && e.Button == MouseButtons.Left)
            {
                using (Graphics g = Graphics.FromImage(canvasBitmap))
                {
                    g.DrawLine(new Pen(Color.Red, 2), lastDrawPoint, e.Location);
                }
                lastDrawPoint = e.Location;
                Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isDraggingText = false;
            isDraggingImage = false;
        }

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
                    textToDisplay = textBox.Text;
                    inputForm.Close();
                    Invalidate();
                };

                inputForm.ShowDialog();
            }
        }

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
                    currentImage = Image.FromFile(openFileDialog.FileName);
                    imageScale = 1.0f;
                    Invalidate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке изображения: " + ex.Message);
                }
            }
        }

        private void btnStartDrawing_Click(object sender, EventArgs e)
        {
            isDrawing = true;
        }

        private void btnStopDrawing_Click(object sender, EventArgs e)
        {
            isDrawing = false;
        }

        private void btnSaveCanvas_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp",
                Title = "Сохранить как изображение"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                canvasBitmap.Save(saveFileDialog.FileName);
                MessageBox.Show("Изображение сохранено успешно!");
            }
        }

        private void btnClearCanvas_Click(object sender, EventArgs e)
        {
            // Очистить холст
            canvasGraphics.Clear(Color.White);

            // Сбросить текстовые настройки
            textToDisplay = "";
            textLocation = new Point(100, 100);
            textFontSize = 12f;

            // Сбросить настройки изображения
            currentImage = null;
            imageLocation = new Point(200, 200);
            imageScale = 1.0f;

            // Перерисовать форму
            Invalidate();
        }



        private void btnSaveWindow_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp",
                    Title = "Сохранить содержимое холста"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    canvasBitmap.Save(saveFileDialog.FileName);
                    MessageBox.Show($"Содержимое холста сохранено в файл:\n{saveFileDialog.FileName}", "Сохранение завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
