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
        LineModeM = 1,
        LineModeD,
        ReadyToSelect,
        SelectionMode,
        CircleModeM,
        CircleModeD,

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
        public float scaleCoeff = 1;
        private int offX, offY, xS, yS;

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

        private void CalculateCoords()
        {
            offX = 0;
            offY = 0;
            xS = sizeX * (int)scaleCoeff;
            yS = sizeY * (int)scaleCoeff;
        }

        private void DrawCursor()
        {
            if (editMode == EditMode.LineModeD || editMode == EditMode.CircleModeD)
                SelectCursor.DrawXCursor(gBuff,scaleCoeff);
            if (editMode == EditMode.LineModeD || editMode == EditMode.LineModeM || editMode == EditMode.CircleModeM || editMode == EditMode.CircleModeD)
                LastCursor.DrawXCursor(gBuff, scaleCoeff);
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
            if (editMode == EditMode.LineModeD)
                count++;
            for (int i = 0; i < count; ++i)
            {
                if (pic.Lines[i] == 0)
                    return;
                pic.Lines[i].DrawLine(gBuff,scaleCoeff, gizmoEditor.SelectionPen);
            }
        }

        private void DrawCircles()
        {
            int count = pic.CounterEllipses;
            if (editMode == EditMode.CircleModeD)
                count++;
            for (int i = 0;i<count;++i)
            {
                if (pic.Ellipses[i] == 0)
                    return;
                pic.Ellipses[i].DrawEllipse(gBuff,gizmoEditor.SelectionPen);
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
            editMode = EditMode.LineModeM;
        }

        public void Draw()
        {
            CalculateCoords();
            gBuff.FillRectangle(whiteHolstBrush, 0, 0, sizeX, sizeY);
            Grid.DrawGrid(gBuff);
            DrawCursor();
            DrawSelectionRectangle();
            DrawGizmo();
            DrawLines();
            DrawCircles();
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
            if (mode != EditMode.ReadyToSelect)
                gizmoEditor.ResetGizmo();
            if (mode == EditMode.CircleModeM || mode == EditMode.LineModeM)
                SetCursorSettings(lastCursor, 5, new Pen(Color.Green));
            if (mode == EditMode.CircleModeD || mode == EditMode.LineModeD)
                SetCursorSettings(lastCursor, 5, new Pen(Color.Red));
        }

        public void IncreaseScaleCoeff()
        {
            if (scaleCoeff < 5)
            {
                scaleCoeff += 0.1f;
            }
            Grid.ScaleCoefficient = scaleCoeff;
            pic.ScaleCoefficient = scaleCoeff;
            
        }

        public void ReduceScaleCoeff()
        {
            if (scaleCoeff > 1)
            {
                scaleCoeff -= 0.1f;
            }
            Grid.ScaleCoefficient = scaleCoeff;
            pic.ScaleCoefficient = scaleCoeff;
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
