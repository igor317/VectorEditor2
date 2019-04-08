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
        SplineM,
        SplineD,
        Spline1,
        Spline2,
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
        private int yScrollWidth = 20;

        private Graphics graph;                                               // Первичный буфер
        private Graphics gBuff;                                               // Вторичный буфер
        private Bitmap bmp;                                                   // Изображения для буфера
        private float scaleCoeff = 1;
        private float xOffset = 0, yOffset = 0;
        private float maxScale = 5;
        private float minScale = 1;
        private float deltaScale = 0.5f;
        private float f1 = 0, f2 = 0,f3 = 0,f4 = 0,f5 = 0,f6 = 0,f7 = 0,f8 = 0;
        private SolidBrush whiteHolstBrush = new SolidBrush(Color.White);
        private SolidBrush textBrush = new SolidBrush(Color.Black);
        private Font textFont = new Font("Times New Roman", 20);
        private bool drawScaleCoeff = true;
        private IpVerticalScroll yScroll;

        private bool inSelect = false;
        public bool shift;
        public bool ctrl;

        private bool scrollMode = false;
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

        public float ScaleCoeff
        {
            get { return scaleCoeff; }
        }
        public float XOffset
        {
            get { return xOffset; }
        }
        public float YOffset
        {
            get { return yOffset; }
        }

        #endregion

        #region PRIVATE METHODS

        private void DrawCursor()
        {
            if (editMode == EditMode.LineModeD || editMode == EditMode.CircleModeD || editMode == EditMode.SplineD || editMode == EditMode.Spline1 || editMode == EditMode.Spline2)
                SelectCursor.DrawXCursor(gBuff,xOffset,yOffset,scaleCoeff);
            if (editMode == EditMode.LineModeD || editMode == EditMode.LineModeM || editMode == EditMode.CircleModeM || editMode == EditMode.CircleModeD || editMode == EditMode.SplineM)
                LastCursor.DrawXCursor(gBuff, xOffset, yOffset, scaleCoeff);
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
                gizmoEditor.SelectRect.DrawSelectionRectangle(gBuff,selectCursor,lastCursor);
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

                float xMin = Math.Min(pic.Lines[i].x1, pic.Lines[i].x2) * scaleCoeff - XOffset;
                float xMax = Math.Max(pic.Lines[i].x1, pic.Lines[i].x2) * scaleCoeff - XOffset;
                float yMin = Math.Min(pic.Lines[i].y1, pic.Lines[i].y2) * scaleCoeff - YOffset;
                float yMax = Math.Max(pic.Lines[i].y1, pic.Lines[i].y2) * scaleCoeff - YOffset;
                float lX = xMax - xMin;
                float lY = yMax - yMin;
                if (xMin + lX >= 0 && xMax - lX <= sizeX && yMin + lY >= 0 && yMax - lY <= sizeY)
                {
                    pic.Lines[i].DrawLine(gBuff, xOffset, yOffset, scaleCoeff, gizmoEditor.SelectionPen);
                }
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

                float xMax = pic.Ellipses[i].x1 * scaleCoeff - XOffset;
                float xMin = pic.Ellipses[i].x2 * scaleCoeff - XOffset;
                float yMax = pic.Ellipses[i].y1 * scaleCoeff - YOffset;
                float yMin = pic.Ellipses[i].y2 * scaleCoeff - YOffset;
                float lX = xMax - xMin;
                float lY = yMax - yMin;
                if (xMin + lX >= 0 && xMax - lX <= sizeX && yMin + lY >= 0 && yMax - lY <= sizeY)
                {
                    pic.Ellipses[i].DrawEllipse(gBuff, xOffset, yOffset, scaleCoeff, gizmoEditor.SelectionPen);
                }
            }
        }

        private void DrawSplines()
        {
            int count = pic.CounterSplines;
            if (editMode == EditMode.SplineD)
                count++;
            for (int i = 0;i<count;++i)
            {
                if (pic.Splines[i] == 0)
                    return;

                float xMax = pic.Splines[i].xSmax * scaleCoeff - XOffset;
                float xMin = pic.Splines[i].xSmin * scaleCoeff - XOffset;
                float yMax = pic.Splines[i].ySmax * scaleCoeff - YOffset;
                float yMin = pic.Splines[i].ySmin * scaleCoeff - YOffset;
                float lX = xMax - xMin;
                float lY = yMax - yMin;
                if (xMin + lX >= 0 && xMax - lX <= sizeX && yMin + lY >= 0 && yMax - lY <= sizeY)
                {
                    pic.Splines[i].DrawSpline(gBuff, xOffset, yOffset, scaleCoeff, gizmoEditor.SelectionPen);
                }
            }
        }

        private void Holst_MouseDown(object sender, MouseEventArgs e)
        {
            inSelect = true;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (yScroll.ScrollCheck(e.X, e.Y) && yScroll.Enable)
                    {
                        scrollMode = true;
                        return;
                    }
                    switch (EditMode)
                    {
                        case EditMode.ReadyToSelect:
                            SetEditMode(EditMode.SelectionMode);
                            Grid.MoveCursor(LastCursor, e.X, e.Y, false, null);
                            if (!ctrl && !shift)
                                gizmoEditor.SelectRect.ResetRect(true);
                            break;
                    }
                    return;
                case MouseButtons.Right:
                    switch (EditMode)
                    {
                        case EditMode.ReadyToSelect:
                            GizmoEditor.CheckSelectedController(e.X, e.Y);
                            break;
                    }
                    return;
            }
        }

        private void Holst_MouseUp(object sender, MouseEventArgs e)
        {
            inSelect = false;
            scrollMode = false;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    switch (EditMode)
                    {
                        case EditMode.LineModeD:
                            Picture.AddLine(new Pen(Color.Black), true, 0);
                            break;
                        case EditMode.CircleModeD:
                            Picture.AddCircle(new Pen(Color.Black), true, 0);
                            break;
                        case EditMode.SelectionMode:
                            GizmoEditor.CreateGizmo();
                            GizmoEditor.SelectRect.ResetRect(false);
                            SetEditMode(EditMode.ReadyToSelect);
                            break;
                        case EditMode.SplineD:
                            Picture.AddSpline(new Pen(Color.Black), true, 0);
                            SetEditMode(EditMode.Spline1);
                            break;
                        case EditMode.Spline1:
                            SetEditMode(EditMode.Spline2);
                            break;
                        case EditMode.Spline2:
                            SelectCursor.X = LastCursor.X;
                            SelectCursor.Y = LastCursor.Y;
                            SetEditMode(EditMode.SplineD);
                            break;
                    }
                    break;
                case MouseButtons.Right:
                    switch (EditMode)
                    {
                        case EditMode.LineModeM:
                            SelectCursor.X = LastCursor.X;
                            SelectCursor.Y = LastCursor.Y;
                            SetEditMode(EditMode.LineModeD);
                            break;
                        case EditMode.LineModeD:
                            SetEditMode(EditMode.LineModeM);
                            break;
                        case EditMode.CircleModeM:
                            SelectCursor.X = LastCursor.X;
                            SelectCursor.Y = LastCursor.Y;
                            SetEditMode(EditMode.CircleModeD);
                            break;
                        case EditMode.CircleModeD:
                            SetEditMode(EditMode.CircleModeM);
                            break;
                        case EditMode.ReadyToSelect:
                            GizmoEditor.ResetControllers();
                            break;
                        case EditMode.SplineM:
                            SelectCursor.X = LastCursor.X;
                            SelectCursor.Y = LastCursor.Y;
                            SetEditMode(EditMode.SplineD);
                            break;
                        case EditMode.SplineD:
                            SetEditMode(EditMode.SplineM);
                            break;
                        case EditMode.Spline1:
                            SelectCursor.X = LastCursor.X;
                            SelectCursor.Y = LastCursor.Y;
                            SetEditMode(EditMode.SplineM);
                            break;
                        case EditMode.Spline2:
                            SelectCursor.X = LastCursor.X;
                            SelectCursor.Y = LastCursor.Y;
                            SetEditMode(EditMode.SplineM);
                            break;
                    }
                    break;
            }
            Draw();
        }

        private void Holst_MouseMove(object sender, MouseEventArgs e)
        {
            if (inSelect)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        if (scrollMode)
                        {
                            //yScroll.Scroll(e.Y);
                            YScroll(e.Y);
                            Draw();
                            return;
                            //ScrollLayers(e.Y);
                        }
                        switch (EditMode)
                        {
                            case EditMode.LineModeM:    // Двигаем lastPoint LINE
                                Grid.MoveCursor(LastCursor, e.X, e.Y, false, null);
                                break;
                            case EditMode.LineModeD:    // Двигаем selectPoint и рисуем LINE
                                Grid.MoveCursor(SelectCursor, e.X, e.Y, false, LastCursor);
                                Picture.AddLine(new Pen(Color.Black), false, 0);
                                break;
                            case EditMode.CircleModeM:  // Двигаем lastPoint ELLIPSE
                                Grid.MoveCursor(LastCursor, e.X, e.Y, false, null);
                                break;
                            case EditMode.CircleModeD:  // Двигаем selectPoint и рисуем ELLIPSE
                                Grid.MoveCursor(SelectCursor, e.X, e.Y, false, LastCursor);
                                Picture.AddCircle(new Pen(Color.Black), false, 0);
                                break;
                            case EditMode.SplineM:     // Двигаем p1 SPLINE
                                Grid.MoveCursor(LastCursor, e.X, e.Y, false, null);
                                break;
                            case EditMode.SplineD:     // Двигаем p4 SPLINE
                                Grid.MoveCursor(SelectCursor, e.X, e.Y, false, LastCursor);
                                Picture.AddSpline(new Pen(Color.Black), false, 0);
                                break;
                            case EditMode.Spline1:     // Двигаем p2 SPLINE
                                Grid.MoveCursor(SelectCursor, e.X, e.Y, false, null);
                                Picture.AddCurveSpline1();
                                break;
                            case EditMode.Spline2:     // Двигаем p3 SPLINE
                                Grid.MoveCursor(SelectCursor, e.X, e.Y, false, null);
                                Picture.AddCurveSpline2();
                                break;
                            case EditMode.SelectionMode: // Двигаем selectPoint SelectionMode
                                Grid.MoveCursor(SelectCursor, e.X, e.Y, false, null);
                                if (ctrl)
                                {
                                    GizmoEditor.SelectRect.AddLinesToSelection(false);
                                    break;
                                }
                                if (shift)
                                {
                                    GizmoEditor.SelectRect.AddLinesToSelection(true);
                                    break;
                                }
                                GizmoEditor.SelectRect.SelectLines();
                                break;
                        }
                        Draw();
                        return;
                    case MouseButtons.Right:
                        switch (EditMode)
                        {
                            case EditMode.ReadyToSelect:
                                GizmoEditor.ControlGizmo(e.X, e.Y);
                                Draw();
                                break;
                        }
                        return;
                }
            }
        }

        private void Holst_Paint(object sender, PaintEventArgs e)
        {
            Draw();
        }

        private void YScroll(int yPos)
        {
            yScroll.Scroll(yPos);
            SetOffsets(0, yScroll.Value);
        }

        #endregion

        #region PUBLIC METHODS

        public PictureEditor(Control Holst)
        {
            this.sizeX = Holst.Width-yScrollWidth;
            this.sizeY = Holst.Height;
            graph = Holst.CreateGraphics();

            pic = new IpPicture(SelectCursor, LastCursor, sizeX,sizeY);
            ipGrid = new IpGrid(sizeX, sizeY, Picture);
            gizmoEditor = new GizmoEditor(Picture,Grid);
            yScroll = new IpVerticalScroll(sizeX+ yScrollWidth, sizeY, yScrollWidth, 0, 100, 50);
            //yScroll.ScrollBrush = new SolidBrush(Color.Blue);
            yScroll.Enable = false;
            ClearPicture();
            bmp = new Bitmap(sizeX+yScrollWidth, sizeY, graph);
            gBuff = Graphics.FromImage(bmp);
            gBuff.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            editMode = EditMode.LineModeM;

            Holst.MouseDown += Holst_MouseDown;
            Holst.MouseUp += Holst_MouseUp;
            Holst.MouseMove += Holst_MouseMove;
            Holst.Paint += Holst_Paint;
        }

        public void Draw()
        {
            gBuff.FillRectangle(whiteHolstBrush, 0, 0, sizeX, sizeY);
            Grid.DrawGrid(gBuff,xOffset,yOffset,scaleCoeff);
            DrawCursor();
            DrawSelectionRectangle();
            DrawGizmo();
            DrawLines();
            DrawCircles();
            DrawSplines();
            if (drawScaleCoeff && scaleCoeff > minScale)
                gBuff.DrawString("x"+scaleCoeff.ToString(), textFont, textBrush, 0, 0);
            yScroll.DrawRectangle(gBuff);
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

        public void SetEditMode(EditMode mode)
        {
            editMode = mode;
            if (mode != EditMode.ReadyToSelect)
            {
                gizmoEditor.ResetGizmo();
                if (mode != EditMode.SelectionMode)
                    gizmoEditor.SelectRect.ResetRect(true);
            }



            if (mode == EditMode.CircleModeM || mode == EditMode.LineModeM)
                SetCursorSettings(lastCursor, 5, new Pen(Color.Green));
            if (mode == EditMode.CircleModeD || mode == EditMode.LineModeD)
                SetCursorSettings(lastCursor, 5, new Pen(Color.Red));
        }

        public void SetOffsets(float xOffset,float yOffset)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            Grid.xOffset = xOffset;
            Grid.yOffset = yOffset;
            pic.XOffset = xOffset;
            pic.YOffset = yOffset;
        }

        public void IncreaseScaleCoeff(int xPos,int yPos)
        {
            if (scaleCoeff < maxScale)
            {
                scaleCoeff += deltaScale;
                pic.ScaleCoefficient = scaleCoeff;
                Grid.ScaleCoeff = scaleCoeff;

                yScroll.Enable = true;
                yScroll.MaxValue = (int)(sizeY * (ScaleCoeff - 1));
            }
        }

        public void ReduceScaleCoeff(int xPos, int yPos)
        {
            if (scaleCoeff > minScale)
            {
                scaleCoeff -= deltaScale;
                pic.ScaleCoefficient = scaleCoeff;
                Grid.ScaleCoeff = scaleCoeff;

                if (scaleCoeff == minScale)
                {
                    yScroll.Enable = false;
                    return;
                }
                yScroll.MaxValue = (int)(sizeY * (ScaleCoeff - 1));
            }
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
