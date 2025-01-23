using System;
using System.Windows.Forms;

namespace MyApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.Button btnAddText;
        private System.Windows.Forms.Button btnAddImage;
        private System.Windows.Forms.Button btnStartDrawing;
        private System.Windows.Forms.Button btnStopDrawing;
        private System.Windows.Forms.Button btnClearCanvas;
        private System.Windows.Forms.Button btnSaveCanvas;
        private System.Windows.Forms.Button btnSaveCustom;
        private System.Windows.Forms.Button btnLoadCustom;

        private void InitializeComponent()
        {
            this.panelControls = new System.Windows.Forms.Panel();
            this.btnAddText = new System.Windows.Forms.Button();
            this.btnAddImage = new System.Windows.Forms.Button();
            this.btnStartDrawing = new System.Windows.Forms.Button();
            this.btnStopDrawing = new System.Windows.Forms.Button();
            this.btnClearCanvas = new System.Windows.Forms.Button();
            this.btnSaveCanvas = new System.Windows.Forms.Button();
            this.btnSaveCustom = new System.Windows.Forms.Button();
            this.btnLoadCustom = new System.Windows.Forms.Button();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControls
            // 
            this.panelControls.Controls.Add(this.btnAddText);
            this.panelControls.Controls.Add(this.btnAddImage);
            this.panelControls.Controls.Add(this.btnStartDrawing);
            this.panelControls.Controls.Add(this.btnStopDrawing);
            this.panelControls.Controls.Add(this.btnClearCanvas);
            this.panelControls.Controls.Add(this.btnSaveCanvas);
            this.panelControls.Controls.Add(this.btnSaveCustom);
            this.panelControls.Controls.Add(this.btnLoadCustom);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(1000, 116);
            this.panelControls.TabIndex = 0;
            this.panelControls.Paint += new System.Windows.Forms.PaintEventHandler(this.panelControls_Paint);
            // 
            // btnAddText
            // 
            this.btnAddText.Location = new System.Drawing.Point(10, 10);
            this.btnAddText.Name = "btnAddText";
            this.btnAddText.Size = new System.Drawing.Size(100, 50);
            this.btnAddText.TabIndex = 0;
            this.btnAddText.Text = "Добавить текст";
            this.btnAddText.Click += new System.EventHandler(this.btnAddText_Click);
            // 
            // btnAddImage
            // 
            this.btnAddImage.Location = new System.Drawing.Point(150, 10);
            this.btnAddImage.Name = "btnAddImage";
            this.btnAddImage.Size = new System.Drawing.Size(100, 50);
            this.btnAddImage.TabIndex = 1;
            this.btnAddImage.Text = "Добавить изображение";
            this.btnAddImage.Click += new System.EventHandler(this.btnAddImage_Click);
            // 
            // btnStartDrawing
            // 
            this.btnStartDrawing.Location = new System.Drawing.Point(290, 10);
            this.btnStartDrawing.Name = "btnStartDrawing";
            this.btnStartDrawing.Size = new System.Drawing.Size(100, 50);
            this.btnStartDrawing.TabIndex = 2;
            this.btnStartDrawing.Text = "Начать рисовать";
            this.btnStartDrawing.Click += new System.EventHandler(this.btnStartDrawing_Click);
            // 
            // btnStopDrawing
            // 
            this.btnStopDrawing.Location = new System.Drawing.Point(430, 10);
            this.btnStopDrawing.Name = "btnStopDrawing";
            this.btnStopDrawing.Size = new System.Drawing.Size(100, 50);
            this.btnStopDrawing.TabIndex = 3;
            this.btnStopDrawing.Text = "Закончить рисовать";
            this.btnStopDrawing.Click += new System.EventHandler(this.btnStopDrawing_Click);
            // 
            // btnClearCanvas
            // 
            this.btnClearCanvas.Location = new System.Drawing.Point(10, 63);
            this.btnClearCanvas.Name = "btnClearCanvas";
            this.btnClearCanvas.Size = new System.Drawing.Size(100, 50);
            this.btnClearCanvas.TabIndex = 4;
            this.btnClearCanvas.Text = "Очистить холст";
            this.btnClearCanvas.Click += new System.EventHandler(this.btnClearCanvas_Click);
            // 
            // btnSaveCanvas
            // 
            this.btnSaveCanvas.Location = new System.Drawing.Point(150, 63);
            this.btnSaveCanvas.Name = "btnSaveCanvas";
            this.btnSaveCanvas.Size = new System.Drawing.Size(100, 50);
            this.btnSaveCanvas.TabIndex = 5;
            this.btnSaveCanvas.Text = "Сохранить как изображение";
            this.btnSaveCanvas.Click += new System.EventHandler(this.btnSaveCanvas_Click);
            // 
            // btnSaveCustom
            // 
            this.btnSaveCustom.Location = new System.Drawing.Point(290, 63);
            this.btnSaveCustom.Name = "btnSaveCustom";
            this.btnSaveCustom.Size = new System.Drawing.Size(100, 50);
            this.btnSaveCustom.TabIndex = 6;
            this.btnSaveCustom.Text = "Сохранить как заметку";
            this.btnSaveCustom.Click += new System.EventHandler(this.btnSaveCustom_Click);
            // 
            // btnLoadCustom
            // 
            this.btnLoadCustom.Location = new System.Drawing.Point(430, 63);
            this.btnLoadCustom.Name = "btnLoadCustom";
            this.btnLoadCustom.Size = new System.Drawing.Size(100, 50);
            this.btnLoadCustom.TabIndex = 7;
            this.btnLoadCustom.Text = "Загрузить как заметку";
            this.btnLoadCustom.Click += new System.EventHandler(this.btnLoadCustom_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.panelControls);
            this.Name = "Form1";
            this.Text = "Мое приложение";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.panelControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }


    }
}
