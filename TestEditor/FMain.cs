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
        PanelLayer panelLayer;
        OpenFileDialog openFileDialog; // Тест
        SaveFileDialog saveFileDialog; // Тест2


        public MainWindow()
        {
            InitializeComponent();
            pictureEditor = new PictureEditor(panel1);
            pictureEditor.SetEditMode(EditMode.ReadyToSelect);
            panelLayer = new PanelLayer(pLayer,5,5,20);
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

        private void Form1_Shown(object sender, EventArgs e)
        {
            pictureEditor.Draw();
            panelLayer.Draw();

        }

        private void Clear()
        {
            pictureEditor.ClearPicture();
            pictureEditor.Draw();
            this.Text = "Vector Editor";
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
            CleanImages(pMirrorX);
            CleanImages(pMirrorY);
            CleanImages(pSpline2);
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

            pMirrorX.Image = ImageList.Images[15];
            pMirrorY.Image = ImageList.Images[16];
            pStepBack.Image = ImageList.Images[14];
            pLine.Image =       (pictureEditor.EditMode == EditMode.LineModeM || pictureEditor.EditMode == EditMode.LineModeD) ? ImageList.Images[3] : ImageList.Images[2];
            pSpline2.Image = (pictureEditor.EditMode == EditMode.SplineM || pictureEditor.EditMode == EditMode.SplineD ||
                pictureEditor.EditMode == EditMode.Spline1 || pictureEditor.EditMode == EditMode.Spline2) ? ImageList.Images[18] : ImageList.Images[17];
            pCircle.Image = (pictureEditor.EditMode == EditMode.CircleModeM || pictureEditor.EditMode == EditMode.CircleModeD) ? ImageList.Images[5] : ImageList.Images[4];
            pRotationMagnet.Image = (pictureEditor.Grid.EnableRotationGrid) ? ImageList.Images[9] : ImageList.Images[8];
            pMagnet.Image = (pictureEditor.Grid.EnableMagnet) ? ImageList.Images[7] : ImageList.Images[6];
            pGrid.Image = (pictureEditor.Grid.EnableGrid) ? ImageList.Images[11] : ImageList.Images[10];
            pMoveCenterPoint.Image = (pictureEditor.GizmoEditor.MoveCenterPointCursor) ? ImageList.Images[13] : ImageList.Images[12];
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
                        pictureEditor.shift = true;
                        DrawMode();
                    }
                    break;
                case Keys.ControlKey:
                    pictureEditor.ctrl = true;
                    break;
                case Keys.Z:
                    if (pictureEditor.ctrl)
                    {
                        pictureEditor.SetEditMode(EditMode.LineModeD);
                        pictureEditor.Picture.StepBack();
                        DrawMode();
                    }
                    break;
                case Keys.C:
                    if (pictureEditor.ctrl)
                        pictureEditor.GizmoEditor.CopySelected();
                    break;
                case Keys.V:
                    if (pictureEditor.ctrl)
                    {
                        pictureEditor.GizmoEditor.PasteSelected();
                        pictureEditor.Draw();
                    }
                    break;
            }     
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    if (pictureEditor.Grid.EnableRotationGrid)
                    {
                        pictureEditor.Grid.EnableRotationGrid = false;
                        pictureEditor.shift = false;
                        DrawMode();
                    }
                    break;
                case Keys.Delete:
                    pictureEditor.GizmoEditor.DeleteSelected();
                    pictureEditor.Draw();
                    break;
                case Keys.ControlKey:
                    pictureEditor.ctrl = false;
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
                this.Text = "Vector Editor | " + openFileDialog.SafeFileName;
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
            pictureEditor.GizmoEditor.MoveCenterPointCursor = (!pictureEditor.GizmoEditor.MoveCenterPointCursor) ? pictureEditor.GizmoEditor.MoveCenterPointCursor = true
                : pictureEditor.GizmoEditor.MoveCenterPointCursor = false;
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
            int res = 40;
            if (e.Delta > 0 && !pictureEditor.shift && !pictureEditor.ctrl)       // Скролл вверх
            {
                if (yOffsetB.Value <= yOffsetB.Maximum && yOffsetB.Value > 0)
                {
                    yOffsetB.Value = (yOffsetB.Value >= res) ? yOffsetB.Value- res : 0;
                    pictureEditor.SetOffsets(xOffsetB.Value, yOffsetB.Value);
                }
            }
            if (e.Delta < 0 && !pictureEditor.shift && !pictureEditor.ctrl)       // Скролл вниз
            {
                if (yOffsetB.Value < yOffsetB.Maximum && yOffsetB.Value >= 0 && pictureEditor.ScaleCoeff != 1)
                {
                    yOffsetB.Value = (yOffsetB.Value <= yOffsetB.Maximum- res) ? yOffsetB.Value + res : yOffsetB.Maximum;
                    pictureEditor.SetOffsets(xOffsetB.Value, yOffsetB.Value);
                }
            }
            if (e.Delta > 0 && !pictureEditor.shift && pictureEditor.ctrl)       // Скролл вправо
            {
                if (xOffsetB.Value <= xOffsetB.Maximum && xOffsetB.Value > 0)
                {
                    xOffsetB.Value = (xOffsetB.Value >= res) ? xOffsetB.Value - res : 0;
                    pictureEditor.SetOffsets(xOffsetB.Value, yOffsetB.Value);
                }
            }
            if (e.Delta < 0 && !pictureEditor.shift && pictureEditor.ctrl)       // Скролл влево
            {
                if (xOffsetB.Value < xOffsetB.Maximum && xOffsetB.Value >= 0 && pictureEditor.ScaleCoeff != 1)
                {
                    xOffsetB.Value = (xOffsetB.Value <= xOffsetB.Maximum - res) ? xOffsetB.Value + res : xOffsetB.Maximum;
                    pictureEditor.SetOffsets(xOffsetB.Value, yOffsetB.Value);
                }
            }
            if (e.Delta > 0 && pictureEditor.shift)        // Увеличить масштаб
            {
                label1.Text = Convert.ToString(pictureEditor.IncreaseScaleCoeff(e.X,e.Y));
            }
            if (e.Delta < 0 && pictureEditor.shift)
            {
                pictureEditor.ReduceScaleCoeff(e.X,e.Y);
            }
            label3.Text = Convert.ToString(e.X + " " + e.Y);
            xOffsetB.Enabled = (pictureEditor.ScaleCoeff == 1) ? false : true;
            yOffsetB.Enabled = (pictureEditor.ScaleCoeff == 1) ? false : true;

            if (xOffsetB.Enabled)
            {
                xOffsetB.Maximum = (int)(panel1.Width * (pictureEditor.ScaleCoeff - 1));
                yOffsetB.Maximum = (int)(panel1.Height * (pictureEditor.ScaleCoeff - 1));
                if ((int)pictureEditor.XOffset >= 0 && (int)pictureEditor.XOffset < xOffsetB.Maximum)
                    xOffsetB.Value = (int)pictureEditor.XOffset;
                if ((int)pictureEditor.YOffset >= 0 && (int)pictureEditor.YOffset < yOffsetB.Maximum)
                    yOffsetB.Value = (int)pictureEditor.YOffset;

            }
            else
            {
                xOffsetB.Value = 0;
                yOffsetB.Value = 0;
                pictureEditor.SetOffsets(0, 0);
            }
            lblCoeff.Text = Convert.ToString(pictureEditor.ScaleCoeff);
            pictureEditor.Draw();
        }

        private void btnMirror_Click(object sender, EventArgs e)
        {
            pictureEditor.GizmoEditor.MirrorSelectedY();
            pictureEditor.Draw();
        }

        private void pMirrorX_MouseClick(object sender, MouseEventArgs e)
        {
            pictureEditor.GizmoEditor.MirrorSelectedX();
            pictureEditor.Draw();
        }

        private void pMirrorY_MouseClick(object sender, MouseEventArgs e)
        {
            pictureEditor.GizmoEditor.MirrorSelectedY();
            pictureEditor.Draw();
        }

        private void pSpline2_Click(object sender, EventArgs e)
        {
            pictureEditor.SetEditMode(EditMode.SplineM);
            DrawMode();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureEditor.GizmoEditor.CopySelected();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureEditor.GizmoEditor.PasteSelected();
            pictureEditor.Draw();
        }
    }
}
