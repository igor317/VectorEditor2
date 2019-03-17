namespace TestEditor
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnIncGrid = new System.Windows.Forms.Button();
            this.btnRedGrid = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.chkTest = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.pLine = new System.Windows.Forms.PictureBox();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.pCircle = new System.Windows.Forms.PictureBox();
            this.pSelectMode = new System.Windows.Forms.PictureBox();
            this.pMagnet = new System.Windows.Forms.PictureBox();
            this.pRotationMagnet = new System.Windows.Forms.PictureBox();
            this.pGrid = new System.Windows.Forms.PictureBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pCircle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pSelectMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMagnet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pRotationMagnet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(134, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 600);
            this.panel1.TabIndex = 0;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(1044, 413);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnIncGrid
            // 
            this.btnIncGrid.Location = new System.Drawing.Point(1044, 350);
            this.btnIncGrid.Name = "btnIncGrid";
            this.btnIncGrid.Size = new System.Drawing.Size(75, 23);
            this.btnIncGrid.TabIndex = 3;
            this.btnIncGrid.Text = "More";
            this.btnIncGrid.UseVisualStyleBackColor = true;
            this.btnIncGrid.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnRedGrid
            // 
            this.btnRedGrid.Location = new System.Drawing.Point(963, 350);
            this.btnRedGrid.Name = "btnRedGrid";
            this.btnRedGrid.Size = new System.Drawing.Size(75, 23);
            this.btnRedGrid.TabIndex = 4;
            this.btnRedGrid.Text = "Less";
            this.btnRedGrid.UseVisualStyleBackColor = true;
            this.btnRedGrid.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(962, 295);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Back";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(959, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "State";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(952, 620);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Load";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1034, 620);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Save";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // chkTest
            // 
            this.chkTest.AutoSize = true;
            this.chkTest.Location = new System.Drawing.Point(958, 522);
            this.chkTest.Name = "chkTest";
            this.chkTest.Size = new System.Drawing.Size(114, 17);
            this.chkTest.TabIndex = 11;
            this.chkTest.Text = "Move Center Point";
            this.chkTest.UseVisualStyleBackColor = true;
            this.chkTest.CheckedChanged += new System.EventHandler(this.chkTest_CheckedChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1044, 545);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 12;
            this.button4.Text = "reset";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // pLine
            // 
            this.pLine.Location = new System.Drawing.Point(12, 109);
            this.pLine.Name = "pLine";
            this.pLine.Size = new System.Drawing.Size(25, 25);
            this.pLine.TabIndex = 18;
            this.pLine.TabStop = false;
            this.pLine.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pLine_MouseClick);
            // 
            // ImageList
            // 
            this.ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ImageList.ImageStream")));
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ImageList.Images.SetKeyName(0, "Cursor.bmp");
            this.ImageList.Images.SetKeyName(1, "CursorSelected.bmp");
            this.ImageList.Images.SetKeyName(2, "Line.bmp");
            this.ImageList.Images.SetKeyName(3, "LineSelected.bmp");
            this.ImageList.Images.SetKeyName(4, "Ellipse.bmp");
            this.ImageList.Images.SetKeyName(5, "EllipseSelected.bmp");
            this.ImageList.Images.SetKeyName(6, "Magnet.bmp");
            this.ImageList.Images.SetKeyName(7, "MagnetSelected.bmp");
            this.ImageList.Images.SetKeyName(8, "RotationMagnet.bmp");
            this.ImageList.Images.SetKeyName(9, "RotationMagnetSelected.bmp");
            this.ImageList.Images.SetKeyName(10, "Grid.bmp");
            this.ImageList.Images.SetKeyName(11, "GridSelected.bmp");
            // 
            // pCircle
            // 
            this.pCircle.Location = new System.Drawing.Point(12, 140);
            this.pCircle.Name = "pCircle";
            this.pCircle.Size = new System.Drawing.Size(25, 25);
            this.pCircle.TabIndex = 19;
            this.pCircle.TabStop = false;
            this.pCircle.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pCircle_MouseClick);
            // 
            // pSelectMode
            // 
            this.pSelectMode.Location = new System.Drawing.Point(12, 78);
            this.pSelectMode.Name = "pSelectMode";
            this.pSelectMode.Size = new System.Drawing.Size(25, 25);
            this.pSelectMode.TabIndex = 20;
            this.pSelectMode.TabStop = false;
            this.pSelectMode.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pSelectMode_MouseClick);
            // 
            // pMagnet
            // 
            this.pMagnet.Location = new System.Drawing.Point(103, 39);
            this.pMagnet.Name = "pMagnet";
            this.pMagnet.Size = new System.Drawing.Size(25, 25);
            this.pMagnet.TabIndex = 21;
            this.pMagnet.TabStop = false;
            this.pMagnet.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pMagnet_MouseClick);
            // 
            // pRotationMagnet
            // 
            this.pRotationMagnet.Location = new System.Drawing.Point(72, 39);
            this.pRotationMagnet.Name = "pRotationMagnet";
            this.pRotationMagnet.Size = new System.Drawing.Size(25, 25);
            this.pRotationMagnet.TabIndex = 22;
            this.pRotationMagnet.TabStop = false;
            this.pRotationMagnet.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pRotationMagnet_MouseClick);
            // 
            // pGrid
            // 
            this.pGrid.Location = new System.Drawing.Point(43, 39);
            this.pGrid.Name = "pGrid";
            this.pGrid.Size = new System.Drawing.Size(25, 25);
            this.pGrid.TabIndex = 23;
            this.pGrid.TabStop = false;
            this.pGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pGrid_MouseClick);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 655);
            this.splitter1.TabIndex = 24;
            this.splitter1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 655);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pGrid);
            this.Controls.Add(this.pRotationMagnet);
            this.Controls.Add(this.pMagnet);
            this.Controls.Add(this.pSelectMode);
            this.Controls.Add(this.pCircle);
            this.Controls.Add(this.pLine);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.chkTest);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnRedGrid);
            this.Controls.Add(this.btnIncGrid);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Vector Editor";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pCircle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pSelectMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMagnet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pRotationMagnet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnIncGrid;
        private System.Windows.Forms.Button btnRedGrid;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox chkTest;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.PictureBox pLine;
        private System.Windows.Forms.ImageList ImageList;
        private System.Windows.Forms.PictureBox pCircle;
        private System.Windows.Forms.PictureBox pSelectMode;
        private System.Windows.Forms.PictureBox pMagnet;
        private System.Windows.Forms.PictureBox pRotationMagnet;
        private System.Windows.Forms.PictureBox pGrid;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

