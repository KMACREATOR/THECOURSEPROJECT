using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MyApp
{
    public partial class Form1 : Form
    {

        private Image currentImage = null;
        private string textToDisplay = "";
        private Point textLocation = new Point(100, 100);
        private Point imageLocation = new Point(200, 200);
        private Point lastTextPosition;
        private Point lastImagePosition;
        private Point lastDrawPoint = Point.Empty;
        private Point lastMousePosition;


        private bool isDraggingText = false;
        private bool isDraggingImage = false;
        private bool isDrawing = false;
        private bool isDragging = false;

        private float textFontSize = 12f;
        private float imageScale = 1.0f;


        private List<CanvasObject> canvasObjects = new List<CanvasObject>();
        private Bitmap canvasBitmap;
        private Graphics canvasGraphics;


        private CanvasObject draggedObject = null;
        private PictureBox drawingCanvas;

        private class CanvasObject
        {
            public enum ObjectType { Text, Image }
            public ObjectType Type { get; set; }
            public string Text { get; set; }
            public Point Location { get; set; }
            public float FontSize { get; set; }
            public Image Image { get; set; }
            public float ImageScale { get; set; }
        }


        public Form1()
        {
            InitializeComponent();
            //InitializeDrawingCanvas(); 
            //WHAT THE FUCK DOES THIS METHOD EVEN DO THAT IT KILLS ALMOST
            //ALL OTHER FUNCTIONS?! FIX A S A P!!!
            InitializeCanvas();
            this.DoubleBuffered = true;

            canvasBitmap = new Bitmap(ClientSize.Width, ClientSize.Height);
            canvasGraphics = Graphics.FromImage(canvasBitmap);
            canvasGraphics.Clear(Color.White);

            this.MouseWheel += Form1_MouseWheel;
            this.DragEnter += Form1_DragEnter;
            this.DragDrop += Form1_DragDrop;
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
            this.MouseDoubleClick += Form1_MouseDoubleClick;
            this.AllowDrop = true; // Включение поддержки Drag-and-Drop
        }



        private void InitializeCanvas()
        {

            canvasBitmap = new Bitmap(ClientSize.Width, ClientSize.Height);
            canvasGraphics = Graphics.FromImage(canvasBitmap);
            canvasGraphics.Clear(Color.White);

            this.Paint += Form1_Paint;
        }


        private void InitializeDrawingCanvas()
        {
            // Инициализация PictureBox для рисования
            drawingCanvas = new PictureBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Parent = this,
            };

            drawingCanvas.Image = canvasBitmap;
            Controls.Add(drawingCanvas);


        }

        private void DrawingCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (isDrawing && e.Button == MouseButtons.Left)
            {
                lastDrawPoint = e.Location;
            }
        }

        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && e.Button == MouseButtons.Left && lastDrawPoint != Point.Empty)
            {
                using (Graphics g = Graphics.FromImage(canvasBitmap))
                {
                    g.DrawLine(Pens.Black, lastDrawPoint, e.Location);
                }
                lastDrawPoint = e.Location;
                drawingCanvas.Invalidate();
            }
        }

        private void DrawingCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing && e.Button == MouseButtons.Left)
            {
                lastDrawPoint = Point.Empty;
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // Поиск объекта для перетаскивания
                draggedObject = canvasObjects
                    .AsEnumerable()
                    .Reverse()
                    .FirstOrDefault(obj => IsObjectHit(obj, e.Location));

                if (draggedObject != null)
                {
                    isDragging = true;
                    lastMousePosition = e.Location;
                    // Перемещаем объект наверх
                    canvasObjects.Remove(draggedObject);
                    canvasObjects.Add(draggedObject);
                }
                else if (isDrawing)
                {
                    // Начинаем рисование
                    lastDrawPoint = e.Location;
                }
                Invalidate();
            }
        }

        // Проверка попадания курсора в объект
        private bool IsObjectHit(CanvasObject obj, Point location)
        {
            if (obj.Type == CanvasObject.ObjectType.Text)
            {
                using (var font = new Font(Font.FontFamily, obj.FontSize))
                {
                    Size textSize = TextRenderer.MeasureText(obj.Text, font);
                    return new Rectangle(obj.Location, textSize).Contains(location);
                }
            }
            else if (obj.Type == CanvasObject.ObjectType.Image && obj.Image != null)
            {
                Size scaledSize = new Size(
                    (int)(obj.Image.Width * obj.ImageScale),
                    (int)(obj.Image.Height * obj.ImageScale));
                return new Rectangle(obj.Location, scaledSize).Contains(location);
            }
            return false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && lastDrawPoint != Point.Empty)
            {
                // Рисуем линию на основном Bitmap
                using (var pen = new Pen(Color.Black, 2))
                {
                    canvasGraphics.DrawLine(pen, lastDrawPoint, e.Location);
                }
                lastDrawPoint = e.Location;
                Invalidate();
            }
            else if (isDragging && draggedObject != null)
            {
                // Перетаскивание объекта
                draggedObject.Location = new Point(
                    draggedObject.Location.X + e.X - lastMousePosition.X,
                    draggedObject.Location.Y + e.Y - lastMousePosition.Y);
                lastMousePosition = e.Location;
                Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                draggedObject = null;
            }
            else if (isDrawing)
            {
                lastDrawPoint = Point.Empty;
            }
        }


        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        public void AddImage(Image image, Point location)
        {
            canvasGraphics.DrawImage(image, location);
            drawingCanvas.Refresh();
        }

        public void DrawText(string text, Point location, Font font, Color color)
        {
            using (Brush brush = new SolidBrush(color))
            {
                canvasGraphics.DrawString(text, font, brush, location);
            }
            drawingCanvas.Refresh();
        }


        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    try
                    {
                        Image image = Image.FromFile(file);
                        canvasObjects.Add(new CanvasObject
                        {
                            Type = CanvasObject.ObjectType.Image,
                            Image = image,
                            Location = this.PointToClient(new Point(e.X, e.Y)),
                            ImageScale = 1.0f
                        });
                        Invalidate();
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка при загрузке файла.");
                    }
                }
            }
        }


        private bool IsImageFile(string filePath)
        {
            string[] extensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
            return extensions.Contains(Path.GetExtension(filePath).ToLower());
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(canvasBitmap, Point.Empty);

            foreach (var obj in canvasObjects)
            {
                if (obj.Type == CanvasObject.ObjectType.Text && !string.IsNullOrEmpty(obj.Text))
                {
                    using (Font font = new Font(Font.FontFamily, obj.FontSize))
                    {
                        e.Graphics.DrawString(obj.Text, font, Brushes.Black, obj.Location);
                    }
                }
                else if (obj.Type == CanvasObject.ObjectType.Image && obj.Image != null)
                {
                    var scaledSize = new Size(
                        (int)(obj.Image.Width * obj.ImageScale),
                        (int)(obj.Image.Height * obj.ImageScale));
                    e.Graphics.DrawImage(obj.Image, new Rectangle(obj.Location, scaledSize));
                }
            }
        }





        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            var obj = canvasObjects.LastOrDefault(o =>
                (o.Type == CanvasObject.ObjectType.Text &&
                new Rectangle(o.Location, new Size(300, (int)(o.FontSize * 2))).Contains(e.Location)) ||
                (o.Type == CanvasObject.ObjectType.Image &&
                new Rectangle(o.Location, new Size(
                    (int)(o.Image.Width * o.ImageScale),
                    (int)(o.Image.Height * o.ImageScale))).Contains(e.Location)));

            if (obj != null)
            {
                if (obj.Type == CanvasObject.ObjectType.Text)
                {
                    obj.FontSize += e.Delta > 0 ? 1f : -1f;
                    obj.FontSize = Math.Max(1f, obj.FontSize);
                }
                else if (obj.Type == CanvasObject.ObjectType.Image)
                {
                    obj.ImageScale += e.Delta > 0 ? 0.1f : -0.1f;
                    obj.ImageScale = Math.Max(0.1f, obj.ImageScale);
                }
                Invalidate();
            }
        }









        private void btnAddText_Click(object sender, EventArgs e)
        {
            isDrawing = false;

            using (Form inputForm = new Form())
            {
                inputForm.Text = "Введите текст";
                inputForm.Size = new Size(400, 200);

                TextBox textBox = new TextBox() { Multiline = true, Dock = DockStyle.Fill, Font = new Font("Arial", 14) };
                Button btnOK = new Button() { Text = "OK", Dock = DockStyle.Bottom };

                inputForm.Controls.Add(textBox);
                inputForm.Controls.Add(btnOK);

                btnOK.Click += (s, args) =>
                {
                    if (!string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        canvasObjects.Add(new CanvasObject
                        {
                            Type = CanvasObject.ObjectType.Text,
                            Text = textBox.Text,
                            Location = textLocation,
                            FontSize = textFontSize
                        });
                        Invalidate();
                        inputForm.Close();
                    }
                };

                inputForm.ShowDialog();
            }
        }


        private void btnAddImage_Click(object sender, EventArgs e)
        {
            isDrawing = false;

            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Изображения (*.jpg; *.jpeg; *.png)|*.jpg;*.jpeg;*.png" };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var image = Image.FromFile(openFileDialog.FileName);
                    canvasObjects.Add(new CanvasObject
                    {
                        Type = CanvasObject.ObjectType.Image,
                        Image = image,
                        Location = imageLocation,
                        ImageScale = 1.0f
                    });
                    Invalidate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке изображения: " + ex.Message);
                }
            }
        }



        public void StartDrawing()
        {
            isDrawing = true;
        }

        public void StopDrawing()
        {
            isDrawing = false;
        }
        public void AddText(string text)
        {
            canvasObjects.Add(new CanvasObject
            {
                Type = CanvasObject.ObjectType.Text,
                Text = text,
                Location = new Point(100, 100),
                FontSize = 12f
            });
            Invalidate();
        }

        private void btnStartDrawing_Click(object sender, EventArgs e)
        {
            isDrawing = true;
            lastDrawPoint = Point.Empty;
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
                SaveCanvas(saveFileDialog.FileName);
            }
        }

        private void SaveCanvas(string filePath)
        {
            using (Bitmap finalBitmap = new Bitmap(ClientSize.Width, ClientSize.Height))
            using (Graphics g = Graphics.FromImage(finalBitmap))
            {
                g.Clear(Color.White);
                // Рисуем нарисованные линии
                g.DrawImage(canvasBitmap, Point.Empty);

                // Рисуем все объекты поверх
                foreach (var obj in canvasObjects)
                {
                    if (obj.Type == CanvasObject.ObjectType.Text)
                    {
                        using (Font font = new Font(Font.FontFamily, obj.FontSize))
                        {
                            g.DrawString(obj.Text, font, Brushes.Black, obj.Location);
                        }
                    }
                    else if (obj.Type == CanvasObject.ObjectType.Image)
                    {
                        Size scaledSize = new Size(
                            (int)(obj.Image.Width * obj.ImageScale),
                            (int)(obj.Image.Height * obj.ImageScale));
                        g.DrawImage(obj.Image, new Rectangle(obj.Location, scaledSize));
                    }
                }
                finalBitmap.Save(filePath);
            }
            MessageBox.Show("Изображение сохранено успешно!");
        }


        private void btnClearCanvas_Click(object sender, EventArgs e)
        {
            canvasGraphics.Clear(Color.White);
            canvasObjects.Clear();
            Invalidate();
        }







        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var clickedObject = canvasObjects.LastOrDefault(o =>
                    (o.Type == CanvasObject.ObjectType.Text &&
                     new Rectangle(o.Location, new Size(300, (int)(o.FontSize * 2))).Contains(e.Location)) ||
                    (o.Type == CanvasObject.ObjectType.Image &&
                     new Rectangle(o.Location, new Size(
                         (int)(o.Image.Width * o.ImageScale),
                         (int)(o.Image.Height * o.ImageScale))).Contains(e.Location)));

                if (clickedObject != null)
                {
                    canvasObjects.Remove(clickedObject);
                    MessageBox.Show("Элемент удален", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Invalidate();
                }
            }
        }



        private void AddCanvasObject(CanvasObject obj)
        {
            canvasObjects.Add(obj);
            //Invalidate();
        }


        private void RemoveCanvasObject(CanvasObject obj)
        {
            canvasObjects.Remove(obj);
            //Invalidate();
        }

        private void Invalidate()
        {
            base.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AllowDrop = true;
        }
    }
}
