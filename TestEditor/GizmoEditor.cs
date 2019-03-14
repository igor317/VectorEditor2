using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    public struct SelectRect
    {
        public float x1;
        public float y1;
        public float x2;
        public float y2;
        public float width;
        public float height;
        public SolidBrush brush;

        public SelectRect(SolidBrush brush)
        {
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
            width = 0;
            height = 0;
            this.brush = brush;
        }

        public void ResetSelectRect()
        {
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
            width = 0;
            height = 0;
        }
    }

    public struct Gizmo
    {
        public float x1;
        public float y1;
        public float x2;
        public float y2;
        public float width;
        public float height;
        private Pen gizmoPen;
        public IpCursor moveCursor;
        public IpCursor xScaleR;
        public IpCursor xScaleL;
        public IpCursor yScaleU;
        public IpCursor yScaleD;
        public IpCursor xyScaleUR;
        public IpCursor rotationCursor;
        public Pen selectedControllerPen;
        public Pen controllerPen;
        public SolidBrush SectorBrush;
        public SolidBrush TextBrush;
        public bool showGizmo;
        public bool showRotationTrack;
        public float radius;
        public float cursorAngle;
        public float frsAngle;
        private float rotationAngle;

        public Gizmo(float x1, float y1, float x2, float y2) : this()
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            showGizmo = true;
            width = x2 - x1;
            height = y2 - y1;
            radius = (float)Math.Sqrt(width / 2 * width / 2 + height / 2 * height / 2);
            gizmoPen = new Pen(Color.Green);
            controllerPen = new Pen(Color.LightSkyBlue);
            selectedControllerPen = new Pen(Color.Blue);
            SectorBrush = new SolidBrush(Color.FromArgb(50, 150, 0, 150));
            TextBrush = new SolidBrush(Color.Black);
            moveCursor = new IpCursor(5, controllerPen);
            xScaleR = new IpCursor(5, controllerPen);
            xScaleL = new IpCursor(5, controllerPen);
            yScaleU = new IpCursor(5, controllerPen);
            yScaleD = new IpCursor(5, controllerPen);
            xyScaleUR = new IpCursor(5, controllerPen);
            rotationCursor = new IpCursor(10, controllerPen);
            ResetControllers(true,true);
        }

        public void ResetControllers(bool resetCenter,bool resetRotator)
        {
            if (resetCenter)
            {
                moveCursor.X = x2 - width / 2;
                moveCursor.Y = y2 - height / 2;
            }
            xScaleR.X = x2;
            xScaleR.Y = y2 - height / 2;
            xScaleL.X = x1;
            xScaleL.Y = y2 - height / 2;

            yScaleU.X = x2 - width / 2;
            yScaleU.Y = y1;
            yScaleD.X = x2 - width / 2;
            yScaleD.Y = y2;

            xyScaleUR.X = x2;
            xyScaleUR.Y = y1;

            if (resetRotator)
            {
                rotationCursor.X = x2 - width / 2;
                rotationCursor.Y = y2 - height / 2 - radius;
                frsAngle = (float)Math.PI;
                ResetRadius();
            }
            ShowCursors();

            //frsAngle = (float)Math.PI;
        }

        private void ShowCursors()
        {
            ShowElements();
            if (width <= 25)
            {
                xScaleL.ShowCursor = false;
                xScaleR.ShowCursor = false;
                xyScaleUR.ShowCursor = false;
            }
            if (height <= 25)
            {
                yScaleU.ShowCursor = false;
                yScaleD.ShowCursor = false;
                xyScaleUR.ShowCursor = false;
            }
        }

        public void ShowElements()
        {
            moveCursor.ShowCursor = true;
            xScaleL.ShowCursor = true;
            xScaleR.ShowCursor = true;
            yScaleU.ShowCursor = true;
            yScaleD.ShowCursor = true;
            xyScaleUR.ShowCursor = true;
            rotationCursor.ShowCursor = true;
        }

        public void ShowElements(bool movecursor,bool xScalel,bool xScaler,bool yScaleu,bool yScaled,bool xyScaleur,bool rotator)
        {
            moveCursor.ShowCursor = movecursor;
            xScaleL.ShowCursor = xScalel;
            xScaleR.ShowCursor = xScaler;
            yScaleU.ShowCursor = yScaleu;
            yScaleD.ShowCursor = yScaled;
            xyScaleUR.ShowCursor = xyScaleur;
            rotationCursor.ShowCursor = rotator;
        }

        public void ResetGizmo()
        {
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
            width = 0;
            height = 0;
            radius = 0;
            ResetControllers(true,true);
        }

        public void ResetGizmo(float x1, float y1, float x2, float y2)
        {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
            width = x2 - x1;
            height = y2 - y1;
            ResetRadius();
            ResetControllers(false,false);
        }

        public void ResetRadius()
        {
            radius = (float)Math.Sqrt(width / 2 * width / 2 + height / 2 * height / 2);
        }

        public void DrawGizmo(Graphics graph)
        {
            if (showGizmo)
                graph.DrawRectangle(gizmoPen, x1, y1, width, height);
            if (showRotationTrack)
            {

                float f = 180 - frsAngle.Rad2Deg();
                float k = 180 - cursorAngle.Rad2Deg();
                float l = ((k - f) > 0) ? k - f : 360 - Math.Abs(k - f);
                float f1 = 0;
                if (l <= 180)
                {
                    f1 = f - 90;
                    rotationAngle = l;
                }
                else
                {
                    f1 = k-90;
                    rotationAngle = 360 - l;
                }
                graph.DrawEllipse(gizmoPen, moveCursor.X - radius, moveCursor.Y - radius, radius * 2, radius * 2);
                graph.FillPie(SectorBrush, moveCursor.X - radius, moveCursor.Y - radius, radius * 2, radius * 2, f1,rotationAngle);
                graph.DrawString(Convert.ToString(Math.Round(rotationAngle,2)), new Font("Times New Roman", 10), TextBrush, moveCursor.X, moveCursor.Y);
            }
            moveCursor.DrawXCursor(graph);
            xScaleR.DrawXCursor(graph);
            xScaleL.DrawXCursor(graph);
            yScaleU.DrawXCursor(graph);
            yScaleD.DrawXCursor(graph);
            xyScaleUR.DrawXCursor(graph);
            rotationCursor.DrawXCursor(graph);
        }

        public static bool operator !=(Gizmo c, int i)
        {
            if ((c.x1 != i) || (c.y1 != i) || (c.x1 != i) || (c.y2 != i))
                return true;
            return false;
        }
        public static bool operator ==(Gizmo c, int i)
        {
            if ((c.x1 == i) && (c.y1 == i) && (c.x2 == i) && (c.y2 == i))
                return true;
            return false;
        }
    }

    class GizmoEditor
    {
        #region VARIABLES
        private float minX, minY,maxX,maxY;
        private Pen selectionPen = new Pen(Color.Red);

        private Gizmo gizmo;
        private SelectRect selectRect;
        private IpGrid grid;
        private IpPicture pic;
        private IpCursor selectCursor;
        private IpCursor lastCursor;
        
        public bool mCenterPoint = false;

        private bool dragSelected = false;
        private bool scaleXR = false;
        private bool scaleXL = false;
        private bool scaleYU = false;
        private bool scaleYD = false;
        private bool scaleXYUR = false;
        private bool rotatePic = false;

        #endregion

        #region GET&GET METHODS
        public Pen SelectionPen
        {
            get { return selectionPen; }
        }
        #endregion

        #region PRIVATE METHODS
        private void CalculateLineNormal()
        {
            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                    pic.Lines[i].CalculateRotationAxes(gizmo.moveCursor.X, gizmo.moveCursor.Y);
            }
        }
    
        private void CalcGizmo()
        {
            minX = 10000;
            maxX = 0;
            minY = 10000;
            maxY = 0;
            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                {
                    if (pic.Lines[i].x1 < minX)
                        minX = pic.Lines[i].x1;
                    if (pic.Lines[i].x2 < minX)
                        minX = pic.Lines[i].x2;
                    if (pic.Lines[i].y1 < minY)
                        minY = pic.Lines[i].y1;
                    if (pic.Lines[i].y2 < minY)
                        minY = pic.Lines[i].y2;

                    if (pic.Lines[i].x1 > maxX)
                        maxX = pic.Lines[i].x1;
                    if (pic.Lines[i].x2 > maxX)
                        maxX = pic.Lines[i].x2;
                    if (pic.Lines[i].y1 > maxY)
                        maxY = pic.Lines[i].y1;
                    if (pic.Lines[i].y2 > maxY)
                        maxY = pic.Lines[i].y2;
                }
            }
        }

        private void MoveCenterPoint(int xPos, int yPos)
        {
            grid.MoveCursor(gizmo.moveCursor, xPos, yPos,false);
            CalculateLineNormal();
        }

        private void MoveSelectedLines(float xPos, float yPos)
        {
            float k1 = xPos;
            float k2 = yPos;
            float m1 = gizmo.moveCursor.X;
            float m2 = gizmo.moveCursor.Y;
            IpCursor pos = new IpCursor();

            grid.MoveCursor(pos, xPos, yPos,true);
            k1 = pos.X;
            k2 = pos.Y;
            grid.MoveCursor(gizmo.moveCursor, xPos,yPos,true);

            gizmo.x1 += k1 - m1;
            gizmo.x2 += k1 - m1;
            gizmo.y1 += k2 - m2;
            gizmo.y2 += k2 - m2;
            gizmo.rotationCursor.X += k1 - m1;
            gizmo.rotationCursor.Y += k2 - m2;
            gizmo.ResetControllers(false,false);
            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                {
                    pic.Lines[i].x1 += k1 - m1;
                    pic.Lines[i].y1 += k2 - m2;
                    pic.Lines[i].x2 += k1 - m1;
                    pic.Lines[i].y2 += k2 - m2;
                    pic.Lines[i].SetCenterPoint(gizmo.moveCursor.X, gizmo.moveCursor.Y);
                }
            }
        }

        private void ScaleSelectedLinesX(bool right, float xPos)
        {
            float kX = xPos;
            float mR = gizmo.xScaleR.X;
            float mL = gizmo.xScaleL.X;
            IpCursor pos = new IpCursor();
            grid.MoveCursor(pos, xPos, -1,true);
            kX = pos.X;

            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                {
                    float coeff1 = 0;
                    float coeff2 = 0;
                    if (right)
                    {
                        coeff1 = 1 - ((gizmo.xScaleR.X - pic.Lines[i].x1) / gizmo.width);
                        coeff2 = 1 - ((gizmo.xScaleR.X - pic.Lines[i].x2) / gizmo.width);
                        pic.Lines[i].x1 += (kX - mR) * coeff1;
                        pic.Lines[i].x2 += (kX - mR) * coeff2;
                    }
                    else
                    {
                        coeff1 = 1 - ((pic.Lines[i].x1 - gizmo.xScaleL.X) / gizmo.width);
                        coeff2 = 1 - ((pic.Lines[i].x2 - gizmo.xScaleL.X) / gizmo.width);
                        pic.Lines[i].x1 += (kX - mL) * coeff1;
                        pic.Lines[i].x2 += (kX - mL) * coeff2;
                    }
                    pic.Lines[i].CalculateRotationAxes(gizmo.moveCursor.X, gizmo.moveCursor.Y);
                }
            }
            if (right)
            {
                gizmo.width += kX - mR;
                gizmo.x2 = gizmo.x1 + gizmo.width;
                grid.MoveCursor(gizmo.xScaleR, xPos, -1,true);
            }
            else
            {
                gizmo.x1 += kX - mL;
                gizmo.width = gizmo.x2 - gizmo.x1;
                grid.MoveCursor(gizmo.xScaleL, xPos, -1, true);
            }
            gizmo.ResetControllers(true, true);
        }

        private void ScaleSelectedLinesY(bool up, float yPos)
        {
            float kY = yPos;
            float mU = gizmo.yScaleU.Y;
            float mD = gizmo.yScaleD.Y;
            IpCursor pos = new IpCursor();
            grid.MoveCursor(pos, -1, yPos, true);
            kY = pos.Y;

            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                {
                    float coeff1 = 0;
                    float coeff2 = 0;
                    if (up)
                    {
                        coeff1 = 1 - ((pic.Lines[i].y1 - gizmo.yScaleU.Y) / gizmo.height);
                        coeff2 = 1 - ((pic.Lines[i].y2 - gizmo.yScaleU.Y) / gizmo.height);
                        pic.Lines[i].y1 += (kY - mU) * coeff1;
                        pic.Lines[i].y2 += (kY - mU) * coeff2;
                    }
                    else
                    {
                        coeff1 = 1 - ((gizmo.yScaleD.Y - pic.Lines[i].y1) / gizmo.height);
                        coeff2 = 1 - ((gizmo.yScaleD.Y - pic.Lines[i].y2) / gizmo.height);
                        pic.Lines[i].y1 += (kY - mD) * coeff1;
                        pic.Lines[i].y2 += (kY - mD) * coeff2;
                    }
                    pic.Lines[i].CalculateRotationAxes(gizmo.moveCursor.X, gizmo.moveCursor.Y);
                }

            }
            if (up)
            {
                gizmo.y1 += kY - mU;
                gizmo.height += mU - kY;
                grid.MoveCursor(gizmo.yScaleU, -1, yPos, true);
            }
            else
            {
                gizmo.height += kY - mD;
                gizmo.y2 = gizmo.y1 + gizmo.height;
                grid.MoveCursor(gizmo.yScaleD, -1, yPos, true);
            }
            gizmo.ResetControllers(true,true);
        }

        private void ClearSelectionArray()
        {
            for (int i = 0;i<pic.CounterLines;++i)
                pic.Lines[i].selected = false;
            for (int i = 0; i < pic.CounterCircles; ++i)
                pic.Circles[i].selected = false;
        }

        private void CalculateAngles(float xPos, float yPos, int res)
        {
            float h = gizmo.moveCursor.X - xPos;
            float w = gizmo.moveCursor.Y - yPos;
            gizmo.cursorAngle = (float)Math.Atan2(-h, -w);
            if (grid.EnableRotationGrid)
                gizmo.cursorAngle = grid.GridRotation(gizmo.cursorAngle, res);
        }

        private void RotateCursor()
        {
            gizmo.rotationCursor.X = gizmo.moveCursor.X + gizmo.radius * (float)Math.Sin(gizmo.cursorAngle);
            gizmo.rotationCursor.Y = gizmo.moveCursor.Y + gizmo.radius * (float)Math.Cos(gizmo.cursorAngle);
            for (int i = 0; i < pic.CounterLines; ++i)
                if (pic.Lines[i].selected)
                    pic.Lines[i].RotateLine(gizmo.cursorAngle - (float)Math.PI);
        }

        private bool ReDrawController(IpCursor cursor, int xPos, int yPos)
        {
            if (cursor.InCursorArea(xPos, yPos))
            {
                cursor.Pen = gizmo.selectedControllerPen;
                return true;
            }
            cursor.Pen = gizmo.controllerPen;
            return false;
        }

        private bool ResetControl(IpCursor cursor)
        {
            cursor.Pen = gizmo.controllerPen;
            return false;
        }

        #endregion

        #region PUBLIC METHODS
        public GizmoEditor(IpPicture pic,IpGrid grid,IpCursor selectCursor,IpCursor lastCursor)
        {
            Color color = Color.FromArgb(50, 50, 200, 200);
            SolidBrush brush = new SolidBrush(color);
            gizmo = new Gizmo(0, 0, 0, 0);
            selectRect = new SelectRect(brush);
            this.pic = pic;
            this.lastCursor = lastCursor;
            this.selectCursor = selectCursor;
            this.grid = grid;
        }

        public void DrawGizmo(Graphics graph)
        {
            if (gizmo == 0)
                return;
            gizmo.DrawGizmo(graph);
        }

        public void DrawSelectionRectangle(Graphics graph)
        {
            if (selectCursor.X >= lastCursor.X && selectCursor.Y >= lastCursor.Y) // Правый нижний
            {
                selectRect.x1 = lastCursor.X;
                selectRect.y1 = lastCursor.Y;
                selectRect.x2 = selectCursor.X;
                selectRect.y2 = selectCursor.Y;
            }
            if (selectCursor.X <= lastCursor.X && selectCursor.Y >= lastCursor.Y) // Левый нижний
            {
                selectRect.x1 = selectCursor.X;
                selectRect.y1 = lastCursor.Y;
                selectRect.x2 = lastCursor.X;
                selectRect.y2 = selectCursor.Y;

            }
            if (selectCursor.X >= lastCursor.X && selectCursor.Y <= lastCursor.Y) // Правый верхний
            {
                selectRect.x1 = lastCursor.X;
                selectRect.y1 = selectCursor.Y;
                selectRect.x2 = selectCursor.X;
                selectRect.y2 = lastCursor.Y;
            }
            if (selectCursor.X <= lastCursor.X && selectCursor.Y <= lastCursor.Y) // Левый верхний
            {
                selectRect.x1 = selectCursor.X;
                selectRect.y1 = selectCursor.Y;
                selectRect.x2 = lastCursor.X;
                selectRect.y2 = lastCursor.Y;
            }
            selectRect.width = selectRect.x2 - selectRect.x1;
            selectRect.height = selectRect.y2 - selectRect.y1;

            graph.FillRectangle(selectRect.brush, selectRect.x1, selectRect.y1, selectRect.width, selectRect.height);
        }

        public void FindSelectionLines()
        {
            ClearSelectionArray();
            for (int i = 0; i < pic.CounterLines + 1; ++i)
            {
                if (pic.Lines[i] == 0)
                    break;
                if (pic.Lines[i].x1 >= selectRect.x1 && pic.Lines[i].x1 <= selectRect.x2
                    && pic.Lines[i].y1 >= selectRect.y1 && pic.Lines[i].y1 <= selectRect.y2
                    && pic.Lines[i].x2 >= selectRect.x1 && pic.Lines[i].x2 <= selectRect.x2
                    && pic.Lines[i].y2 >= selectRect.y1 && pic.Lines[i].y2 <= selectRect.y2)
                {
                    pic.Lines[i].selected = true;
                }
            }
            for (int i = 0;i<pic.CounterCircles + 1;++i)
            {
                if (pic.Circles[i] == 0)
                    break;
                if (pic.Circles[i].x1 >= selectRect.x1 && pic.Circles[i].x1 <= selectRect.x2
                    && pic.Circles[i].y1 >= selectRect.y1 && pic.Circles[i].y1 <= selectRect.y2
                    && pic.Circles[i].x1 + 2*pic.Circles[i].radius >= selectRect.x1 && pic.Circles[i].x1 + 2 * pic.Circles[i].radius <= selectRect.x2
                    && pic.Circles[i].y1 + 2 * pic.Circles[i].radius >= selectRect.y1 && pic.Circles[i].y1 + 2 * pic.Circles[i].radius <= selectRect.y2)
                {
                    pic.Circles[i].selected = true;
                }
            }

        }

        public void CreateGizmo()
        {
            bool f = false;
            for (int i = 0;i<pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                    f = true;
            }
            if (!f)
                return;
            CalcGizmo();
            gizmo = new Gizmo(minX, minY, maxX, maxY);
            CalculateLineNormal();
        }

        public void ControlGizmo(int xPos,int yPos)
        {
            if (dragSelected)
            {
                if (!mCenterPoint)
                    MoveSelectedLines(xPos, yPos);
                else
                    MoveCenterPoint(xPos, yPos);
                return;
            }
            if (scaleXR)
            {
                ScaleSelectedLinesX(true, xPos);
                return;
            }
            if (scaleXL)
            {
                ScaleSelectedLinesX(false, xPos);
                return;
            }
            if (scaleYU)
            {
                ScaleSelectedLinesY(true, yPos);
                return;
            }
            if (scaleYD)
            {
                ScaleSelectedLinesY(false, yPos);
                return;
            }
            if (scaleXYUR)
            {
                ScaleSelectedLinesX(true, xPos);
                ScaleSelectedLinesY(true, yPos);
                return;
            }
            if (rotatePic)
            {
                CalculateAngles(xPos, yPos,15);
                RotateCursor();
                gizmo.showGizmo = false;
                gizmo.showRotationTrack = true;
                gizmo.ShowElements(true,false, false, false, false, false, true);
                return;
            }
        }

        public void ResetControllers()
        {
            if (dragSelected)
            {
                if (mCenterPoint)
                {
                    gizmo.rotationCursor.X = gizmo.moveCursor.X;
                    gizmo.rotationCursor.Y = gizmo.moveCursor.Y - gizmo.radius;
                }
                dragSelected = ResetControl(gizmo.moveCursor);
            }
            scaleXR = ResetControl(gizmo.xScaleR);
            scaleXL = ResetControl(gizmo.xScaleL);
            scaleYU = ResetControl(gizmo.yScaleU);
            scaleYD = ResetControl(gizmo.yScaleD);
            scaleXYUR = ResetControl(gizmo.xyScaleUR);
            if (rotatePic)
            {
                CalcGizmo();
                gizmo.ResetGizmo(minX, minY, maxX, maxY);
                gizmo.ResetControllers(false, false);
                gizmo.showGizmo = true;
                gizmo.showRotationTrack = false;
                gizmo.frsAngle = gizmo.cursorAngle;
                rotatePic = ResetControl(gizmo.rotationCursor);
            }
        }

        public void CheckSelectedController(int xPos, int yPos)
        {
            dragSelected = ReDrawController(gizmo.moveCursor, xPos, yPos);
            scaleXR = ReDrawController(gizmo.xScaleR, xPos, yPos);
            scaleXL = ReDrawController(gizmo.xScaleL, xPos, yPos);
            scaleYU = ReDrawController(gizmo.yScaleU, xPos, yPos);
            scaleYD = ReDrawController(gizmo.yScaleD, xPos, yPos);
            scaleXYUR = ReDrawController(gizmo.xyScaleUR, xPos, yPos);
            rotatePic = ReDrawController(gizmo.rotationCursor, xPos, yPos);
        }

        public void ResetGizmo()
        {
            selectRect.ResetSelectRect();
            gizmo.ResetGizmo();
            ClearSelectionArray();
        }

        public void ResetCenterPoint()
        {
            gizmo.ResetControllers(true, true);
            CalculateLineNormal();
        }

        public void DeleteSelectedLines()
        {
            pic.DeleteSelectedLines();
            ResetGizmo();
        }
        #endregion
    }
}
