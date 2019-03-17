using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestEditor
{
    public partial class Form1 : Form
    {
        PictureEditor pictureEditor; // Класс редактора
        OpenFileDialog openFileDialog; // Тест
        SaveFileDialog saveFileDialog; // Тест2
        bool inSelect = false;
        bool f = false;
        public Form1()
        {
            InitializeComponent();
            pictureEditor = new PictureEditor(panel1);
            openFileDialog = new OpenFileDialog();
            saveFileDialog = new SaveFileDialog();
            openFileDialog.Filter = "Векторный рисунок|*.svg;*.cpi";
            saveFileDialog.Filter = "Векторный рисунок|*.cpi";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            pictureEditor.Grid.EnableGrid = ckxEnableGrid.Checked;
            pictureEditor.Draw();
            CheckState();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureEditor.Grid.IncreaseGridLines();
            pictureEditor.Draw();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pictureEditor.Grid.ReduceGridLines();
            pictureEditor.Draw();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (inSelect)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        switch (pictureEditor.EditMode)
                        {
                            case EditMode.LineModeM:
                                pictureEditor.Grid.MoveCursor(pictureEditor.LastCursor, e.X, e.Y,false);
                                break;
                            case EditMode.LineModeD:
                                pictureEditor.Grid.MoveCursor(pictureEditor.SelectCursor, e.X, e.Y,false);
                                pictureEditor.Picture.AddLine(new Pen(Color.Black), false);
                                break;
                            case EditMode.CircleModeM:
                                pictureEditor.Grid.MoveCursor(pictureEditor.LastCursor, e.X, e.Y, false);
                                break;
                            case EditMode.CircleModeD:
                                pictureEditor.Grid.MoveCursor(pictureEditor.SelectCursor, e.X, e.Y, false);
                                pictureEditor.Picture.AddCircle(new Pen(Color.Black),false);
                                break;
                            case EditMode.SelectionMode:
                                pictureEditor.Grid.MoveCursor(pictureEditor.SelectCursor, e.X, e.Y,false);
                                pictureEditor.GizmoEditor.FindSelectionLines();
                                break;
                        }
                        pictureEditor.Draw();
                        break;
                    case MouseButtons.Right:
                        switch (pictureEditor.EditMode)
                        {
                            case EditMode.ReadyToSelect:
                                pictureEditor.GizmoEditor.ControlGizmo(e.X, e.Y);
                                pictureEditor.Draw();
                                break;
                        }
                        break;
                }
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            inSelect = true;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    switch(pictureEditor.EditMode)
                    {
                        case EditMode.ReadyToSelect:
                            pictureEditor.SetEditMode(EditMode.SelectionMode);
                            pictureEditor.Grid.MoveCursor(pictureEditor.LastCursor, e.X, e.Y,false);
                            break;
                    }
                    break;
                case MouseButtons.Right:
                    switch (pictureEditor.EditMode)
                    {
                        case EditMode.ReadyToSelect:
                            pictureEditor.GizmoEditor.CheckSelectedController(e.X, e.Y);
                            break;
                    }
                    break;
            }
            CheckState();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            inSelect = false;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    switch (pictureEditor.EditMode)
                    {
                        case EditMode.LineModeD:
                            pictureEditor.Picture.AddLine(new Pen(Color.Black), true);
                            break;
                        case EditMode.CircleModeD:
                            pictureEditor.Picture.AddCircle(new Pen(Color.Black), true);
                            break;
                        case EditMode.SelectionMode:
                            pictureEditor.GizmoEditor.CreateGizmo();
                            pictureEditor.SetEditMode(EditMode.ReadyToSelect);
                            break;
                    }
                    break;
                case MouseButtons.Right:
                    switch (pictureEditor.EditMode)
                    {
                        case EditMode.LineModeM:
                            pictureEditor.SetCursorSettings(pictureEditor.LastCursor, 5, new Pen(Color.Red));
                            pictureEditor.Grid.MoveCursor(pictureEditor.SelectCursor, pictureEditor.LastCursor.X, pictureEditor.LastCursor.Y,false);
                            pictureEditor.SetEditMode(EditMode.LineModeD);
                            break;
                        case EditMode.LineModeD:
                            pictureEditor.SetCursorSettings(pictureEditor.LastCursor, 5, new Pen(Color.Green));
                            pictureEditor.SetEditMode(EditMode.LineModeM);
                            break;
                        case EditMode.CircleModeM:
                            pictureEditor.SetCursorSettings(pictureEditor.LastCursor, 5, new Pen(Color.Red));
                            pictureEditor.Grid.MoveCursor(pictureEditor.SelectCursor, pictureEditor.LastCursor.X, pictureEditor.LastCursor.Y, false);
                            pictureEditor.SetEditMode(EditMode.CircleModeD);
                            break;
                        case EditMode.CircleModeD:
                            pictureEditor.SetCursorSettings(pictureEditor.LastCursor, 5, new Pen(Color.Green));
                            pictureEditor.SetEditMode(EditMode.CircleModeM);
                            break;
                        case EditMode.ReadyToSelect:
                            pictureEditor.GizmoEditor.ResetControllers();
                            break;
                    }
                    break;
            }
            CheckState();
            pictureEditor.Draw();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureEditor.SetCursorSettings(pictureEditor.LastCursor, 5, new Pen(Color.Red));
            pictureEditor.SetEditMode(EditMode.LineModeD);
            pictureEditor.Picture.StepBack();
            pictureEditor.Draw();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            pictureEditor.Draw();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Clear();
            string path = "";
            openFileDialog.ShowDialog();
            path = openFileDialog.FileName;
            pictureEditor.Picture.LoadFile(path);
            pictureEditor.Draw();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string path = "";
            saveFileDialog.ShowDialog();
            path = saveFileDialog.FileName;
            pictureEditor.Picture.SaveFile(path);
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void Clear()
        {
            pictureEditor.ClearPicture();
            pictureEditor.Draw();
        }

        private void CheckState()
        {
            label1.Text = pictureEditor.EditMode.ToString();
        }

        private void ckbMagnet_CheckedChanged(object sender, EventArgs e)
        {
            pictureEditor.Grid.EnableMagnet = ckbMagnet.Checked;
        }

        private void chkTest_CheckedChanged(object sender, EventArgs e)
        {
            pictureEditor.GizmoEditor.mCenterPoint = chkTest.Checked;
            CheckState();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureEditor.GizmoEditor.ResetCenterPoint();
            pictureEditor.Draw();
            CheckState();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureEditor.GizmoEditor.DeleteSelected();
            pictureEditor.Draw();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            pictureEditor.Grid.EnableRotationGrid = ckBRotationGrid.Checked;
        }

        private void rbSelectionMode_CheckedChanged(object sender, EventArgs e)
        {
            DrawMode();
        }

        private void rbLine_CheckedChanged(object sender, EventArgs e)
        {
            DrawMode();
        }

        private void rbCircle_CheckedChanged(object sender, EventArgs e)
        {
            DrawMode();
        }

        private void DrawMode()
        {
            if (rbSelectionMode.Checked)
                pictureEditor.SetEditMode(EditMode.ReadyToSelect);
            if (rbLine.Checked)
                pictureEditor.SetEditMode(EditMode.LineModeM);
            if (rbCircle.Checked)
                pictureEditor.SetEditMode(EditMode.CircleModeM);
            pictureEditor.Draw();
            CheckState();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    ckBRotationGrid.Checked = true;
                    f = true;
                    break;
            }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    ckBRotationGrid.Checked = false;
                    break;
            }
        }
    }
}
