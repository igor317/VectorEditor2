namespace TestEditor
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.panel1 = new System.Windows.Forms.Panel();
            this.yOffsetB = new System.Windows.Forms.VScrollBar();
            this.xOffsetB = new System.Windows.Forms.HScrollBar();
            this.btnIncGrid = new System.Windows.Forms.Button();
            this.btnRedGrid = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pMoveCenterPoint = new System.Windows.Forms.PictureBox();
            this.pStepBack = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCoeff = new System.Windows.Forms.Label();
            this.pMirrorY = new System.Windows.Forms.PictureBox();
            this.pMirrorX = new System.Windows.Forms.PictureBox();
            this.pSpline2 = new System.Windows.Forms.PictureBox();
            this.pLayer = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pCircle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pSelectMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMagnet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pRotationMagnet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pGrid)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pMoveCenterPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pStepBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMirrorY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMirrorX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pSpline2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(153, 39);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(900, 600);
            this.panel1.TabIndex = 0;
            // 
            // yOffsetB
            // 
            this.yOffsetB.Enabled = false;
            this.yOffsetB.LargeChange = 1;
            this.yOffsetB.Location = new System.Drawing.Point(1053, 39);
            this.yOffsetB.Maximum = 0;
            this.yOffsetB.Name = "yOffsetB";
            this.yOffsetB.Size = new System.Drawing.Size(20, 620);
            this.yOffsetB.TabIndex = 1;
            this.yOffsetB.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // xOffsetB
            // 
            this.xOffsetB.Enabled = false;
            this.xOffsetB.LargeChange = 1;
            this.xOffsetB.Location = new System.Drawing.Point(153, 639);
            this.xOffsetB.Maximum = 0;
            this.xOffsetB.Name = "xOffsetB";
            this.xOffsetB.Size = new System.Drawing.Size(900, 20);
            this.xOffsetB.TabIndex = 0;
            this.xOffsetB.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // btnIncGrid
            // 
            this.btnIncGrid.Location = new System.Drawing.Point(12, 279);
            this.btnIncGrid.Name = "btnIncGrid";
            this.btnIncGrid.Size = new System.Drawing.Size(75, 23);
            this.btnIncGrid.TabIndex = 3;
            this.btnIncGrid.Text = "More";
            this.btnIncGrid.UseVisualStyleBackColor = true;
            this.btnIncGrid.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnRedGrid
            // 
            this.btnRedGrid.Location = new System.Drawing.Point(12, 250);
            this.btnRedGrid.Name = "btnRedGrid";
            this.btnRedGrid.Size = new System.Drawing.Size(75, 23);
            this.btnRedGrid.TabIndex = 4;
            this.btnRedGrid.Text = "Less";
            this.btnRedGrid.UseVisualStyleBackColor = true;
            this.btnRedGrid.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 589);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "State";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(53, 82);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 12;
            this.button4.Text = "reset";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // pLine
            // 
            this.pLine.Location = new System.Drawing.Point(12, 151);
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
            this.ImageList.Images.SetKeyName(12, "MoveCenterPoint.bmp");
            this.ImageList.Images.SetKeyName(13, "MoveCenterPointSelected.bmp");
            this.ImageList.Images.SetKeyName(14, "Back.bmp");
            this.ImageList.Images.SetKeyName(15, "MirrorX.bmp");
            this.ImageList.Images.SetKeyName(16, "MirrorY.bmp");
            this.ImageList.Images.SetKeyName(17, "Spline2.bmp");
            this.ImageList.Images.SetKeyName(18, "Spline2Selected.bmp");
            // 
            // pCircle
            // 
            this.pCircle.Location = new System.Drawing.Point(12, 182);
            this.pCircle.Name = "pCircle";
            this.pCircle.Size = new System.Drawing.Size(25, 25);
            this.pCircle.TabIndex = 19;
            this.pCircle.TabStop = false;
            this.pCircle.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pCircle_MouseClick);
            // 
            // pSelectMode
            // 
            this.pSelectMode.Location = new System.Drawing.Point(12, 120);
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
            this.splitter1.Location = new System.Drawing.Point(0, 24);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 647);
            this.splitter1.TabIndex = 24;
            this.splitter1.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.editToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1326, 24);
            this.menuStrip1.TabIndex = 25;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.OpenToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.OpenToolStripMenuItem.Text = "Open";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // pMoveCenterPoint
            // 
            this.pMoveCenterPoint.Location = new System.Drawing.Point(12, 39);
            this.pMoveCenterPoint.Name = "pMoveCenterPoint";
            this.pMoveCenterPoint.Size = new System.Drawing.Size(25, 25);
            this.pMoveCenterPoint.TabIndex = 26;
            this.pMoveCenterPoint.TabStop = false;
            this.pMoveCenterPoint.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pMoveCenterPoint_MouseClick);
            // 
            // pStepBack
            // 
            this.pStepBack.Location = new System.Drawing.Point(88, 614);
            this.pStepBack.Name = "pStepBack";
            this.pStepBack.Size = new System.Drawing.Size(25, 25);
            this.pStepBack.TabIndex = 27;
            this.pStepBack.TabStop = false;
            this.pStepBack.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pStepBack_MouseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 556);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "State";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 626);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "State";
            // 
            // lblCoeff
            // 
            this.lblCoeff.AutoSize = true;
            this.lblCoeff.Location = new System.Drawing.Point(100, 589);
            this.lblCoeff.Name = "lblCoeff";
            this.lblCoeff.Size = new System.Drawing.Size(32, 13);
            this.lblCoeff.TabIndex = 30;
            this.lblCoeff.Text = "State";
            // 
            // pMirrorY
            // 
            this.pMirrorY.Location = new System.Drawing.Point(107, 120);
            this.pMirrorY.Name = "pMirrorY";
            this.pMirrorY.Size = new System.Drawing.Size(25, 25);
            this.pMirrorY.TabIndex = 33;
            this.pMirrorY.TabStop = false;
            this.pMirrorY.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pMirrorY_MouseClick);
            // 
            // pMirrorX
            // 
            this.pMirrorX.Location = new System.Drawing.Point(72, 120);
            this.pMirrorX.Name = "pMirrorX";
            this.pMirrorX.Size = new System.Drawing.Size(25, 25);
            this.pMirrorX.TabIndex = 34;
            this.pMirrorX.TabStop = false;
            this.pMirrorX.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pMirrorX_MouseClick);
            // 
            // pSpline2
            // 
            this.pSpline2.Location = new System.Drawing.Point(12, 213);
            this.pSpline2.Name = "pSpline2";
            this.pSpline2.Size = new System.Drawing.Size(25, 25);
            this.pSpline2.TabIndex = 35;
            this.pSpline2.TabStop = false;
            this.pSpline2.Click += new System.EventHandler(this.pSpline2_Click);
            // 
            // pLayer
            // 
            this.pLayer.Location = new System.Drawing.Point(1085, 308);
            this.pLayer.Name = "pLayer";
            this.pLayer.Size = new System.Drawing.Size(229, 319);
            this.pLayer.TabIndex = 42;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ClientSize = new System.Drawing.Size(1326, 671);
            this.Controls.Add(this.pLayer);
            this.Controls.Add(this.pSpline2);
            this.Controls.Add(this.pMirrorX);
            this.Controls.Add(this.pMirrorY);
            this.Controls.Add(this.lblCoeff);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.xOffsetB);
            this.Controls.Add(this.yOffsetB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pStepBack);
            this.Controls.Add(this.pMoveCenterPoint);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pGrid);
            this.Controls.Add(this.pRotationMagnet);
            this.Controls.Add(this.pMagnet);
            this.Controls.Add(this.pSelectMode);
            this.Controls.Add(this.pCircle);
            this.Controls.Add(this.pLine);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRedGrid);
            this.Controls.Add(this.btnIncGrid);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
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
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pMoveCenterPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pStepBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMirrorY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pMirrorX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pSpline2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnIncGrid;
        private System.Windows.Forms.Button btnRedGrid;
        private System.Windows.Forms.Label label1;
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.PictureBox pMoveCenterPoint;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.PictureBox pStepBack;
        private System.Windows.Forms.HScrollBar xOffsetB;
        private System.Windows.Forms.VScrollBar yOffsetB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCoeff;
        private System.Windows.Forms.PictureBox pMirrorY;
        private System.Windows.Forms.PictureBox pMirrorX;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.PictureBox pSpline2;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.Panel pLayer;
    }
}

