namespace MyApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.Button btnAddText;
        private System.Windows.Forms.Button btnAddImage;

        private void InitializeComponent()
        {
            this.panelControls = new System.Windows.Forms.Panel();
            this.btnAddText = new System.Windows.Forms.Button();
            this.btnAddImage = new System.Windows.Forms.Button();

            // Настройка панели
            this.panelControls.Controls.Add(this.btnAddText);
            this.panelControls.Controls.Add(this.btnAddImage);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Height = 50;

            // Настройка кнопки "Добавить текст"
            this.btnAddText.Text = "Добавить текст";
            this.btnAddText.Location = new System.Drawing.Point(10, 10);
            this.btnAddText.Click += new System.EventHandler(this.btnAddText_Click);

            // Настройка кнопки "Добавить изображение"
            this.btnAddImage.Text = "Добавить изображение";
            this.btnAddImage.Location = new System.Drawing.Point(150, 10);
            this.btnAddImage.Click += new System.EventHandler(this.btnAddImage_Click);

            // Настройка формы
            this.Controls.Add(this.panelControls);
            this.Text = "Мое приложение";
            this.Size = new System.Drawing.Size(800, 600);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
        }
    }
}
