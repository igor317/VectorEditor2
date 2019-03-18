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
    public partial class MainWindow : Form
    {
        PictureEditor pictureEditor; // Класс редактора
        OpenFileDialog openFileDialog; // Тест
        SaveFileDialog saveFileDialog; // Тест2
        bool inSelect = false;
        bool k = false, kk = false;
        public MainWindow()
        {
            InitializeComponent();
            pictureEditor = new PictureEditor(panel1);
            pictureEditor.SetEditMode(EditMode.ReadyToSelect);
            panel1.MouseWheel += new MouseEventHandler(panel1_MouseWheel);
            DrawMode();
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
                                pictureEditor.Grid.MoveCursor(pictureEditor.LastCursor, e.X, e.Y, false);
                                break;
                            case EditMode.LineModeD:
                                pictureEditor.Grid.MoveCursor(pictureEditor.SelectCursor, e.X, e.Y, false);
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
                                pictureEditor.Grid.MoveCursor(pictureEditor.SelectCursor, e.X, e.Y, false);
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
                            pictureEditor.Grid.MoveCursor(pictureEditor.LastCursor, e.X, e.Y, false);
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
                            //pictureEditor.Grid.MoveCursor(pictureEditor.SelectCursor, pictureEditor.LastCursor.X, pictureEditor.LastCursor.Y,false);
                            pictureEditor.SelectCursor.X = pictureEditor.LastCursor.X;
                            pictureEditor.SelectCursor.Y = pictureEditor.LastCursor.Y;
                            pictureEditor.SetEditMode(EditMode.LineModeD);
                            break;
                        case EditMode.LineModeD:
                            pictureEditor.SetEditMode(EditMode.LineModeM);
                            break;
                        case EditMode.CircleModeM:
                            //pictureEditor.Grid.MoveCursor(pictureEditor.SelectCursor, pictureEditor.LastCursor.X, pictureEditor.LastCursor.Y, false);
                            pictureEditor.SelectCursor.X = pictureEditor.LastCursor.X;
                            pictureEditor.SelectCursor.Y = pictureEditor.LastCursor.Y;
                            pictureEditor.SetEditMode(EditMode.CircleModeD);
                            break;
                        case EditMode.CircleModeD:
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

        private void Form1_Shown(object sender, EventArgs e)
        {
            pictureEditor.Draw();

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

        private void button4_Click(object sender, EventArgs e)
        {
            pictureEditor.GizmoEditor.ResetCenterPoint();
            pictureEditor.Draw();
            CheckState();
        }

        private void DrawMode()
        {
            CleanImages(pSelectMode);
            CleanImages(pRotationMagnet);
            CleanImages(pLine);
            CleanImages(pCircle);
            CleanImages(pGrid);
            CleanImages(pMagnet);
            CleanImages(pMoveCenterPoint);
            CleanImages(pStepBack);
            if (!pictureEditor.Grid.EnableGrid)
                toolTip1.SetToolTip(pGrid, "Show Grid");
            else
                toolTip1.SetToolTip(pGrid, "Hide Grid");

            if (!pictureEditor.Grid.EnableMagnet)
                toolTip1.SetToolTip(pMagnet, "Activate Magnet");
            else
                toolTip1.SetToolTip(pMagnet, "Disactivate Magnet");

            if (!pictureEditor.Grid.EnableRotationGrid)
                toolTip1.SetToolTip(pRotationMagnet, "Activate Rotation Grid (Shift)");
            else
                toolTip1.SetToolTip(pRotationMagnet, "Disactivate Rotation Grid (Shift)");

            pStepBack.Image = ImageList.Images[14];
            pLine.Image =       (pictureEditor.EditMode == EditMode.LineModeM || pictureEditor.EditMode == EditMode.LineModeD) ? ImageList.Images[3] : ImageList.Images[2];
            pCircle.Image = (pictureEditor.EditMode == EditMode.CircleModeM || pictureEditor.EditMode == EditMode.CircleModeD) ? ImageList.Images[5] : ImageList.Images[4];
            pRotationMagnet.Image = (pictureEditor.Grid.EnableRotationGrid) ? ImageList.Images[9] : ImageList.Images[8];
            pMagnet.Image = (pictureEditor.Grid.EnableMagnet) ? ImageList.Images[7] : ImageList.Images[6];
            pGrid.Image = (pictureEditor.Grid.EnableGrid) ? ImageList.Images[11] : ImageList.Images[10];
            pMoveCenterPoint.Image = (pictureEditor.GizmoEditor.mCenterPoint) ? ImageList.Images[13] : ImageList.Images[12];
            pSelectMode.Image = (pictureEditor.EditMode == EditMode.ReadyToSelect) ? ImageList.Images[1] : ImageList.Images[0];

            pictureEditor.Draw();
            CheckState();
        }

        private void CleanImages(PictureBox picture)
        {
            if (picture.Image != null)
            {
                picture.Image.Dispose();
                picture.Image = null;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    if (!pictureEditor.Grid.EnableRotationGrid)
                    {
                        pictureEditor.Grid.EnableRotationGrid = true;
                        DrawMode();
                    }
                    break;
                case Keys.ControlKey:
                    k = true;
                    break;
                case Keys.Z:
                    kk = true;
                    break;
            }     
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (k && kk)
            {
                pictureEditor.SetEditMode(EditMode.LineModeD);
                pictureEditor.Picture.StepBack();
                DrawMode();
            }
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    if (pictureEditor.Grid.EnableRotationGrid)
                    {
                        pictureEditor.Grid.EnableRotationGrid = false;
                        DrawMode();
                    }
                    break;
                case Keys.Delete:
                    pictureEditor.GizmoEditor.DeleteSelected();
                    pictureEditor.Draw();
                    break;
                case Keys.ControlKey:
                    k = false;
                    break;
                case Keys.Z:
                    kk = false;
                    break;
            }

        }

        private void pSelectMode_MouseClick(object sender, MouseEventArgs e)
        {
            pictureEditor.SetEditMode(EditMode.ReadyToSelect);
            DrawMode();
        }

        private void pLine_MouseClick(object sender, MouseEventArgs e)
        {
            pictureEditor.SetEditMode(EditMode.LineModeM);
            DrawMode();
        }

        private void pCircle_MouseClick(object sender, MouseEventArgs e)
        {
            pictureEditor.SetEditMode(EditMode.CircleModeM);
            DrawMode();
        }

        private void pMagnet_MouseClick(object sender, MouseEventArgs e)
        {
            pictureEditor.Grid.EnableMagnet = (!pictureEditor.Grid.EnableMagnet) ? pictureEditor.Grid.EnableMagnet = true : pictureEditor.Grid.EnableMagnet = false;
            DrawMode();
        }

        private void pRotationMagnet_MouseClick(object sender, MouseEventArgs e)
        {
            pictureEditor.Grid.EnableRotationGrid = (!pictureEditor.Grid.EnableRotationGrid) ? pictureEditor.Grid.EnableRotationGrid = true : pictureEditor.Grid.EnableRotationGrid = false;
            DrawMode();
        }

        private void pGrid_MouseClick(object sender, MouseEventArgs e)
        {
            pictureEditor.Grid.EnableGrid = (!pictureEditor.Grid.EnableGrid) ? pictureEditor.Grid.EnableGrid = true : pictureEditor.Grid.EnableGrid = false;
            DrawMode();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = "";
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Векторный рисунок|*.svg;*.cpi";
            openFileDialog.ShowDialog();
            path = openFileDialog.FileName;
            if (path != "")
            {
                Clear();

                pictureEditor.Picture.LoadFile(path);
                pictureEditor.Draw();
            }
            openFileDialog.Dispose();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = "";
            saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Векторный рисунок|*.cpi";
            saveFileDialog.ShowDialog();
            path = saveFileDialog.FileName;
            if (path != "")
                pictureEditor.Picture.SaveFile(path);
            saveFileDialog.Dispose();
        }

        private void pStepBack_MouseClick(object sender, MouseEventArgs e)
        {
            pictureEditor.SetEditMode(EditMode.LineModeD);
            pictureEditor.Picture.StepBack();
            DrawMode();
        }

        private void pMoveCenterPoint_MouseClick(object sender, MouseEventArgs e)
        {
            pictureEditor.GizmoEditor.mCenterPoint = (!pictureEditor.GizmoEditor.mCenterPoint) ? pictureEditor.GizmoEditor.mCenterPoint = true : pictureEditor.GizmoEditor.mCenterPoint = false;
            DrawMode();
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            pictureEditor.SetOffsets(xOffsetB.Value, yOffsetB.Value);
            pictureEditor.Draw();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            pictureEditor.SetOffsets(xOffsetB.Value, yOffsetB.Value);
            pictureEditor.Draw();
        }

        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                pictureEditor.IncreaseScaleCoeff();
            }
            else
            {
                pictureEditor.ReduceScaleCoeff();
            }
            xOffsetB.Enabled = (pictureEditor.ScaleCoeff == 1) ? false : true;
            yOffsetB.Enabled = (pictureEditor.ScaleCoeff == 1) ? false : true;

            if (xOffsetB.Enabled)
            {
                xOffsetB.Maximum = (int)(panel1.Width * (pictureEditor.ScaleCoeff - 1));
                yOffsetB.Maximum = (int)(panel1.Height * (pictureEditor.ScaleCoeff - 1));
                pictureEditor.GizmoEditor.CreateGizmo();
            }
            else
            {
                xOffsetB.Value = 0;
                yOffsetB.Value = 0;
                pictureEditor.SetOffsets(0, 0);
            }
            pictureEditor.Draw();
        }
    }
}
