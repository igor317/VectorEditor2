using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
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
        private float scaleCoeff = 1;
        private float xOff = 0,yOff = 0;
        #endregion

        #region GET&GET METHODS
        public Pen SelectionPen
        {
            get { return selectionPen; }
        }
        public float ScaleCoefficient
        {
            set { scaleCoeff = value; }
            get { return scaleCoeff; }
        }
        public float xOffset
        {
            set { xOff = value; }
            get { return xOff; }
        }
        public float yOffset
        {
            set { yOff = value; }
            get { return yOff; }
        }
        #endregion

        #region PRIVATE METHODS
        private void CalculateNormals()
        {
            for (int i = 0; i < pic.CounterLines; ++i)
                if (pic.Lines[i].selected)
                    pic.Lines[i].CalculateRotationAxes(gizmo.moveCursor.X, gizmo.moveCursor.Y);
            for (int i = 0; i < pic.CounterEllipses; ++i)
                if (pic.Ellipses[i].selected)
                    pic.Ellipses[i].CalculateRotationAxes(gizmo.moveCursor.X, gizmo.moveCursor.Y);
        }
    
        private void CalcGizmo()
        {
            minX = 10000;
            maxX = 0;
            minY = 10000;
            maxY = 0;
            for (int i = 0; i < pic.CounterEllipses; ++i)
            {
                if (pic.Ellipses[i].selected)
                {
                    if (pic.Ellipses[i].x1 < minX)
                        minX = pic.Ellipses[i].x1;
                    if (pic.Ellipses[i].x2 < minX)
                        minX = pic.Ellipses[i].x2;
                    if (pic.Ellipses[i].y1 < minY)
                        minY = pic.Ellipses[i].y1;
                    if (pic.Ellipses[i].y2 < minY)
                        minY = pic.Ellipses[i].y2;

                    if (pic.Ellipses[i].x1 > maxX)
                        maxX = pic.Ellipses[i].x1;
                    if (pic.Ellipses[i].x2 > maxX)
                        maxX = pic.Ellipses[i].x2;
                    if (pic.Ellipses[i].y1 > maxY)
                        maxY = pic.Ellipses[i].y1;
                    if (pic.Ellipses[i].y2 > maxY)
                        maxY = pic.Ellipses[i].y2;
                }
            }
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
                    if (pic.Lines[i].y2> maxY)
                        maxY = pic.Lines[i].y2;
                }
            }
           
        }

        private void MoveCenterPoint(int xPos, int yPos)
        {
            grid.MoveCursor(gizmo.moveCursor, xPos, yPos,false);
            CalculateNormals();
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
            for (int i = 0;i<pic.CounterEllipses;++i)
            {
                if (pic.Ellipses[i].selected)
                {
                    pic.Ellipses[i].xR += k1 - m1;
                    pic.Ellipses[i].yR += k2 - m2;
                    pic.Ellipses[i].x1 += k1 - m1;
                    pic.Ellipses[i].y1 += k2 - m2;
                    pic.Ellipses[i].x2 += k1 - m1;
                    pic.Ellipses[i].y2 += k2 - m2;
                    pic.Ellipses[i].SetCenterPoint(gizmo.moveCursor.X, gizmo.moveCursor.Y);
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
            for (int i = 0; i < pic.CounterEllipses; ++i)
            {
                if (pic.Ellipses[i].selected)
                {
                    float coeff1 = 0;
                    float coeff2 = 0;
                    if (right)
                    {
                        coeff1 = 1 - ((gizmo.xScaleR.X - pic.Ellipses[i].xR) / gizmo.width);
                        //coeff2 = 1 - ((gizmo.xScaleR.X - pic.Ellipses[i].radY) / gizmo.width);
                        float k = pic.Ellipses[i].radX + pic.Ellipses[i].radY;
                        float rX = pic.Ellipses[i].radX;
                        pic.Ellipses[i].radX += (kX - mR) / 2 * (float)Math.Abs(Math.Cos(pic.Ellipses[i].alpha));
                        pic.Ellipses[i].radY += (kX - mR) / 2 * (float)Math.Abs(Math.Sin(pic.Ellipses[i].alpha));
                        pic.Ellipses[i].xR += (kX - mR) * coeff1;
                        pic.Ellipses[i].CalculatePoints();
                    }
                    pic.Ellipses[i].CalculateRotationAxes(gizmo.moveCursor.X, gizmo.moveCursor.Y);
                }
            }
            
            if (right)
            {
                gizmo.width += kX - mR;
                gizmo.x2 = gizmo.x1 + gizmo.width;
                grid.MoveCursor(gizmo.xScaleR, xPos, -1, true);
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
            for (int i = 0; i < pic.CounterEllipses; ++i)
                pic.Ellipses[i].selected = false;
        }

        private void CalculateAngles(float xPos, float yPos, int res)
        {
            float h = (gizmo.moveCursor.X*scaleCoeff-xOff) - xPos;
            float w = (gizmo.moveCursor.Y*scaleCoeff-yOff) - yPos;
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
            for (int i = 0; i < pic.CounterEllipses; ++i)
                if (pic.Ellipses[i].selected)
                    pic.Ellipses[i].RotateCircle(gizmo.cursorAngle - (float)Math.PI);
        }

        private bool ReDrawController(IpCursor cursor, int xPos, int yPos)
        {
            if (cursor.InCursorArea(xPos, yPos,xOffset,yOffset,scaleCoeff))
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
            gizmo.DrawGizmo(graph,xOffset,yOffset, scaleCoeff);
        }

        public void DrawSelectionRectangle(Graphics graph)
        {
            if (selectCursor.X >= lastCursor.X && selectCursor.Y >= lastCursor.Y) // Правый нижний
            {
                selectRect.x1 = lastCursor.X*scaleCoeff-xOff;
                selectRect.y1 = lastCursor.Y*scaleCoeff-yOff;
                selectRect.x2 = selectCursor.X*scaleCoeff-xOff;
                selectRect.y2 = selectCursor.Y*scaleCoeff-yOff;
            }
            if (selectCursor.X <= lastCursor.X && selectCursor.Y >= lastCursor.Y) // Левый нижний
            {
                selectRect.x1 = selectCursor.X * scaleCoeff - xOff;
                selectRect.y1 = lastCursor.Y * scaleCoeff - yOff;
                selectRect.x2 = lastCursor.X * scaleCoeff - xOff;
                selectRect.y2 = selectCursor.Y * scaleCoeff - yOff;

            }
            if (selectCursor.X >= lastCursor.X && selectCursor.Y <= lastCursor.Y) // Правый верхний
            {
                selectRect.x1 = lastCursor.X * scaleCoeff - xOff;
                selectRect.y1 = selectCursor.Y * scaleCoeff - yOff;
                selectRect.x2 = selectCursor.X * scaleCoeff - xOff;
                selectRect.y2 = lastCursor.Y * scaleCoeff - yOff;
            }
            if (selectCursor.X <= lastCursor.X && selectCursor.Y <= lastCursor.Y) // Левый верхний
            {
                selectRect.x1 = selectCursor.X * scaleCoeff - xOff;
                selectRect.y1 = selectCursor.Y * scaleCoeff - yOff;
                selectRect.x2 = lastCursor.X * scaleCoeff - xOff;
                selectRect.y2 = lastCursor.Y * scaleCoeff - yOff;
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
                if (pic.Lines[i].x1 * scaleCoeff - xOff >= selectRect.x1 && pic.Lines[i].x1 * scaleCoeff - xOff <= selectRect.x2
                    && pic.Lines[i].y1 * scaleCoeff - yOff >= selectRect.y1 && pic.Lines[i].y1 * scaleCoeff - yOff <= selectRect.y2
                    && pic.Lines[i].x2 * scaleCoeff - xOff >= selectRect.x1 && pic.Lines[i].x2 * scaleCoeff - xOff <= selectRect.x2
                    && pic.Lines[i].y2 * scaleCoeff - yOff >= selectRect.y1 && pic.Lines[i].y2 * scaleCoeff - yOff <= selectRect.y2)
                {
                    pic.Lines[i].selected = true;
                }
            }
            for (int i = 0;i<pic.CounterEllipses + 1;++i)
            {
                if (pic.Ellipses[i] == 0)
                    break;
                if (pic.Ellipses[i].x1 * scaleCoeff - xOff >= selectRect.x1 && pic.Ellipses[i].x1 * scaleCoeff - xOff <= selectRect.x2
                    && pic.Ellipses[i].y1 * scaleCoeff - yOff >= selectRect.y1 && pic.Ellipses[i].y1 * scaleCoeff - yOff <= selectRect.y2
                    && pic.Ellipses[i].x2 * scaleCoeff - xOff >= selectRect.x1 && pic.Ellipses[i].x2 * scaleCoeff - xOff <= selectRect.x2
                    && pic.Ellipses[i].y2 * scaleCoeff - yOff >= selectRect.y1 && pic.Ellipses[i].y2 * scaleCoeff - yOff <= selectRect.y2)
                {
                    pic.Ellipses[i].selected = true;
                }
            }

        }

        public void CreateGizmo()
        {
            bool f = false;
            bool k = false;
            for (int i = 0;i<pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                {
                    f = true;
                    break;
                }
            }
            for (int i = 0; i < pic.CounterEllipses; ++i)
                if (pic.Ellipses[i].selected)
                {
                    k = true;
                    break;
                }
            if (!f && !k)
                return;
            CalcGizmo();
            gizmo = new Gizmo(minX, minY, maxX, maxY);
            CalculateNormals();
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
            CalculateNormals();
        }

        public void DeleteSelected()
        {
            pic.DeleteSelectedLines();
            pic.DeleteSelectedCircles();
            ResetGizmo();
        }
        #endregion
    }
}
