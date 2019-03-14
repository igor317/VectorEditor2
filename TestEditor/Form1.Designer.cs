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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.ckxEnableGrid = new System.Windows.Forms.CheckBox();
            this.btnIncGrid = new System.Windows.Forms.Button();
            this.btnRedGrid = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.ckbMagnet = new System.Windows.Forms.CheckBox();
            this.chkTest = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.rbLine = new System.Windows.Forms.RadioButton();
            this.rbSelectionMode = new System.Windows.Forms.RadioButton();
            this.rbCircle = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(34, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(900, 600);
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
            // ckxEnableGrid
            // 
            this.ckxEnableGrid.AutoSize = true;
            this.ckxEnableGrid.Location = new System.Drawing.Point(975, 379);
            this.ckxEnableGrid.Name = "ckxEnableGrid";
            this.ckxEnableGrid.Size = new System.Drawing.Size(45, 17);
            this.ckxEnableGrid.TabIndex = 2;
            this.ckxEnableGrid.Text = "Grid";
            this.ckxEnableGrid.UseVisualStyleBackColor = true;
            this.ckxEnableGrid.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
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
            // ckbMagnet
            // 
            this.ckbMagnet.AutoSize = true;
            this.ckbMagnet.Location = new System.Drawing.Point(958, 465);
            this.ckbMagnet.Name = "ckbMagnet";
            this.ckbMagnet.Size = new System.Drawing.Size(62, 17);
            this.ckbMagnet.TabIndex = 10;
            this.ckbMagnet.Text = "Magnet";
            this.ckbMagnet.UseVisualStyleBackColor = true;
            this.ckbMagnet.CheckedChanged += new System.EventHandler(this.ckbMagnet_CheckedChanged);
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
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(980, 103);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(112, 23);
            this.button5.TabIndex = 13;
            this.button5.Text = "Delete Selected";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(1034, 379);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(85, 17);
            this.checkBox2.TabIndex = 14;
            this.checkBox2.Text = "RotationGrid";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // rbLine
            // 
            this.rbLine.AutoSize = true;
            this.rbLine.Checked = true;
            this.rbLine.Location = new System.Drawing.Point(975, 145);
            this.rbLine.Name = "rbLine";
            this.rbLine.Size = new System.Drawing.Size(45, 17);
            this.rbLine.TabIndex = 15;
            this.rbLine.Text = "Line";
            this.rbLine.UseVisualStyleBackColor = true;
            this.rbLine.CheckedChanged += new System.EventHandler(this.rbLine_CheckedChanged);
            // 
            // rbSelectionMode
            // 
            this.rbSelectionMode.AutoSize = true;
            this.rbSelectionMode.Location = new System.Drawing.Point(975, 191);
            this.rbSelectionMode.Name = "rbSelectionMode";
            this.rbSelectionMode.Size = new System.Drawing.Size(99, 17);
            this.rbSelectionMode.TabIndex = 16;
            this.rbSelectionMode.Text = "Selection Mode";
            this.rbSelectionMode.UseVisualStyleBackColor = true;
            this.rbSelectionMode.CheckedChanged += new System.EventHandler(this.rbSelectionMode_CheckedChanged);
            // 
            // rbCircle
            // 
            this.rbCircle.AutoSize = true;
            this.rbCircle.Location = new System.Drawing.Point(975, 168);
            this.rbCircle.Name = "rbCircle";
            this.rbCircle.Size = new System.Drawing.Size(51, 17);
            this.rbCircle.TabIndex = 17;
            this.rbCircle.Text = "Circle";
            this.rbCircle.UseVisualStyleBackColor = true;
            this.rbCircle.CheckedChanged += new System.EventHandler(this.rbCircle_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 655);
            this.Controls.Add(this.rbCircle);
            this.Controls.Add(this.rbSelectionMode);
            this.Controls.Add(this.rbLine);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.chkTest);
            this.Controls.Add(this.ckbMagnet);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnRedGrid);
            this.Controls.Add(this.btnIncGrid);
            this.Controls.Add(this.ckxEnableGrid);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox ckxEnableGrid;
        private System.Windows.Forms.Button btnIncGrid;
        private System.Windows.Forms.Button btnRedGrid;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.CheckBox ckbMagnet;
        private System.Windows.Forms.CheckBox chkTest;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.RadioButton rbLine;
        private System.Windows.Forms.RadioButton rbSelectionMode;
        private System.Windows.Forms.RadioButton rbCircle;
    }
}

