using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace TestEditor
{
    public enum EditMode
    {
        MoveLastPoint = 1,
        DrawMode,
        ReadyToSelect,
        SelectionMode,
    };

    class PictureEditor
    {
        #region VARIABLES
        private IpGrid ipGrid;
        private GizmoEditor gizmoEditor;
        private IpPicture pic;

        private EditMode editMode;
        private IpCursor selectCursor = new IpCursor(5, new Pen(Color.Black));
        private IpCursor lastCursor = new IpCursor(5, new Pen(Color.Red));
        private int sizeX, sizeY;                                             // Размеры холста

        private Graphics graph;                                               // Первичный буфер
        private Graphics gBuff;                                               // Вторичный буфер
        private Bitmap bmp;                                                   // Изображения для буфера

        private SolidBrush whiteHolstBrush = new SolidBrush(Color.White);
        #endregion

        #region SET&GET METHODS

        public EditMode EditMode
        {
            get { return editMode; }
        }
        public IpCursor LastCursor
        {
            get { return lastCursor; }
        }
        public IpCursor SelectCursor
        {
            get { return selectCursor; }
        }
        public IpGrid Grid
        {
            get { return ipGrid; }
        }
        public IpPicture Picture
        {
            get { return pic; }
        }
        public GizmoEditor GizmoEditor
        {
            get { return gizmoEditor; }
        }
        #endregion

        #region PRIVATE METHODS

        private void DrawCursor()
        {
            if (editMode == EditMode.DrawMode)
                SelectCursor.DrawXCursor(gBuff);
            if (editMode == EditMode.DrawMode || editMode == EditMode.MoveLastPoint)
                LastCursor.DrawXCursor(gBuff);
        }

        private void DrawGizmo()
        {
            if (editMode == EditMode.ReadyToSelect)
            {
                gizmoEditor.DrawGizmo(gBuff);
            }
        }

        private void DrawSelectionRectangle()
        {
            if (editMode == EditMode.SelectionMode)
            {
                gizmoEditor.DrawSelectionRectangle(gBuff);
            }
        }

        private void DrawLines()
        {
            int count = pic.CounterLines;
            bool selected;
            if (editMode == EditMode.DrawMode)
                count++;
            for (int i = 0; i < count; ++i)
            {
                if (pic.Lines[i] != 0)
                {
                    selected = false;
                    for (int j = 0; j < gizmoEditor.CountSelectedLines; ++j)
                    {
                        if (i == gizmoEditor.SelectLines[j])
                        {
                            selected = true;
                            break;
                        }
                    }
                    if (selected)
                        gBuff.DrawLine(gizmoEditor.SelectionPen, pic.Lines[i].x1, pic.Lines[i].y1, pic.Lines[i].x2, pic.Lines[i].y2);
                    else
                        gBuff.DrawLine(pic.Lines[i].pen, pic.Lines[i].x1, pic.Lines[i].y1, pic.Lines[i].x2, pic.Lines[i].y2);
                }
                else
                    break;
            }
        }
        #endregion

        #region PUBLIC METHODS

        public PictureEditor(Panel Holst)
        {
            this.sizeX = Holst.Width;
            this.sizeY = Holst.Height;
            graph = Holst.CreateGraphics();

            pic = new IpPicture(SelectCursor, LastCursor, sizeX,sizeY);
            ipGrid = new IpGrid(sizeX, sizeY, Picture);
            gizmoEditor = new GizmoEditor(Picture,Grid, SelectCursor, LastCursor);

            ClearPicture();
            bmp = new Bitmap(sizeX, sizeY, graph);
            gBuff = Graphics.FromImage(bmp);
            gBuff.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            editMode = EditMode.MoveLastPoint;
        }

        public void Draw()
        {
            gBuff.FillRectangle(whiteHolstBrush, 0, 0, sizeX, sizeY);

            Grid.DrawGrid(gBuff);
            DrawCursor();
            DrawSelectionRectangle();
            DrawGizmo();
            DrawLines();
            graph.DrawImageUnscaled(bmp, 0, 0);
        }

        public void SetCursorSettings(IpCursor cursor, int size, Pen pen)
        {
            cursor.Size = size;
            cursor.Pen = pen;
        }

        public void ClearPicture()
        {
            pic.ResetPicture();
            gizmoEditor.ResetGizmo();
        }

        public void FindSelectionLines()
        {
            if (editMode == EditMode.SelectionMode)
            {
                gizmoEditor.FindSelectionLines();
            }
        }

        public void SetEditMode(EditMode mode)
        {
            editMode = mode;
            if (mode == EditMode.MoveLastPoint || mode == EditMode.DrawMode || mode == EditMode.SelectionMode)
                gizmoEditor.ResetGizmo();
        }

        public void RasterizeImage(string path)
        {
            if (path == "")
                return;
            bmp.Save(path);
        }
        #endregion
    }
}
