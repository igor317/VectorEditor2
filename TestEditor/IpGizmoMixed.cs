using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    class GizmoMixed : IpGizmo
    {
        #region VARIABLES
        private float x1;
        private float y1;
        private float x2;
        private float y2;
        private float width;
        private float height;
        private Pen gizmoPen;
        private IpCursor moveCursor;
        private IpCursor xScaleR;
        private IpCursor xScaleL;
        private IpCursor yScaleU;
        private IpCursor yScaleD;
        private IpCursor xyScaleUR;
        private IpCursor rotationCursor;
        private Pen selectedControllerPen;
        private Pen controllerPen;
        private SolidBrush sectorBrush;
        private SolidBrush textBrush;
        private bool showGizmo;
        private bool showRotationTrack;
        private float radius;
        private float cursorAngle = 0;
        private float frsAngle = 0;
        private float rotationAngle;

        private bool dragSelected = false;
        private bool scaleXR = false;
        private bool scaleXL = false;
        private bool scaleYU = false;
        private bool scaleYD = false;
        private bool scaleXYUR = false;
        private bool rotatePic = false;
        #endregion

        #region SET&GET METHODS
        #endregion

        #region PRIVATE METHODS
        private void DefaultControllerPosition(bool resetCenter, bool resetRotator)
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
                radius = (float)Math.Sqrt(width / 2 * width / 2 + height / 2 * height / 2);
                CalculateNormals();
            }
        }

        private void ResetGizmo()
        {
            CalculatePoints();
            width = x2 - x1;
            height = y2 - y1;
            radius = (float)Math.Sqrt(width / 2 * width / 2 + height / 2 * height / 2);
            DefaultControllerPosition(false, false);
        }

        private void CreateGizmo()
        {
            CalculatePoints();
            showGizmo = true;
            width = x2 - x1;
            height = y2 - y1;
            radius = (float)Math.Sqrt(width / 2 * width / 2 + height / 2 * height / 2);
            DefaultControllerPosition(true, true);
            CalculateNormals();
        }

        private bool ResetControl(IpCursor cursor)
        {
            cursor.Pen = controllerPen;
            return false;
        }

        private bool ReDrawController(IpCursor cursor, int xPos, int yPos)
        {
            if (cursor.InCursorArea(xPos, yPos, pic.XOffset, pic.YOffset, pic.ScaleCoefficient))
            {
                cursor.Pen = selectedControllerPen;
                return true;
            }
            cursor.Pen = controllerPen;
            return false;
        }

        private void CalculateNormals()
        {
            for (int i = 0; i < pic.CounterLines; ++i)
                if (pic.Lines[i].selected)
                    pic.Lines[i].CalculateRotationAxes(moveCursor.X, moveCursor.Y);
            for (int i = 0; i < pic.CounterEllipses; ++i)
                if (pic.Ellipses[i].selected)
                    pic.Ellipses[i].CalculateRotationAxes(moveCursor.X, moveCursor.Y);
        }

        private void CalculatePoints()
        {
            bool fL = false;
            bool fE = false;
            float minX = 10000;
            float maxX = 0;
            float minY = 10000;
            float maxY = 0;
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
                    fE = true;
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
                    if (pic.Lines[i].y2 > maxY)
                        maxY = pic.Lines[i].y2;
                    fL = true;
                }
            }
            if (fL || fE)
            {
                x1 = minX;
                x2 = maxX;
                y1 = minY;
                y2 = maxY;
                //zeroGizmo = false;
            }
            //else
                //zeroGizmo = true;
        }

        private void MoveCenterPoint(int xPos, int yPos)
        {
            grid.MoveCursor(moveCursor, xPos, yPos, false, null);
            CalculateNormals();
        }

        private void MoveSelectedLines(float xPos, float yPos)
        {
            float k1 = xPos;
            float k2 = yPos;
            float m1 = moveCursor.X;
            float m2 = moveCursor.Y;
            IpCursor pos = new IpCursor();

            grid.MoveCursor(pos, xPos, yPos, true, null);
            k1 = pos.X;
            k2 = pos.Y;
            grid.MoveCursor(moveCursor, xPos, yPos, true, null);

            x1 += k1 - m1;
            x2 += k1 - m1;
            y1 += k2 - m2;
            y2 += k2 - m2;
            rotationCursor.X += k1 - m1;
            rotationCursor.Y += k2 - m2;
            DefaultControllerPosition(false, false);
            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                {
                    pic.Lines[i].x1 += k1 - m1;
                    pic.Lines[i].y1 += k2 - m2;
                    pic.Lines[i].x2 += k1 - m1;
                    pic.Lines[i].y2 += k2 - m2;
                    pic.Lines[i].SetCenterPoint(moveCursor.X, moveCursor.Y);
                }
            }
            for (int i = 0; i < pic.CounterEllipses; ++i)
            {
                if (pic.Ellipses[i].selected)
                {
                    pic.Ellipses[i].xR += k1 - m1;
                    pic.Ellipses[i].yR += k2 - m2;
                    pic.Ellipses[i].x1 += k1 - m1;
                    pic.Ellipses[i].y1 += k2 - m2;
                    pic.Ellipses[i].x2 += k1 - m1;
                    pic.Ellipses[i].y2 += k2 - m2;
                    pic.Ellipses[i].SetCenterPoint(moveCursor.X, moveCursor.Y);
                }
            }
        }

        private void ScaleSelectedLinesX(bool right, float xPos)
        {
            float kX = xPos;
            float mR = xScaleR.X;
            float mL = xScaleL.X;
            IpCursor pos = new IpCursor();
            grid.MoveCursor(pos, xPos, -1, true, null);
            kX = pos.X;

            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                {
                    float coeff1 = 0;
                    float coeff2 = 0;
                    if (right)
                    {
                        coeff1 = 1 - ((xScaleR.X - pic.Lines[i].x1) / width);
                        coeff2 = 1 - ((xScaleR.X - pic.Lines[i].x2) / width);
                        pic.Lines[i].x1 += (kX - mR) * coeff1;
                        pic.Lines[i].x2 += (kX - mR) * coeff2;
                    }
                    else
                    {
                        coeff1 = 1 - ((pic.Lines[i].x1 - xScaleL.X) / width);
                        coeff2 = 1 - ((pic.Lines[i].x2 - xScaleL.X) / width);
                        pic.Lines[i].x1 += (kX - mL) * coeff1;
                        pic.Lines[i].x2 += (kX - mL) * coeff2;
                    }
                    pic.Lines[i].CalculateRotationAxes(moveCursor.X, moveCursor.Y);
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
                        coeff1 = 1 - ((xScaleR.X - pic.Ellipses[i].xR) / width);
                        //coeff2 = 1 - ((gizmo.xScaleR.X - pic.Ellipses[i].radY) / gizmo.width);
                        float k = pic.Ellipses[i].radX + pic.Ellipses[i].radY;
                        float rX = pic.Ellipses[i].radX;
                        pic.Ellipses[i].radX += (kX - mR) / 2 * (float)Math.Abs(Math.Cos(pic.Ellipses[i].alpha));
                        pic.Ellipses[i].radY += (kX - mR) / 2 * (float)Math.Abs(Math.Sin(pic.Ellipses[i].alpha));
                        pic.Ellipses[i].xR += (kX - mR) * coeff1;
                        pic.Ellipses[i].CalculatePoints();
                    }
                    pic.Ellipses[i].CalculateRotationAxes(moveCursor.X, moveCursor.Y);
                }
            }

            if (right)
            {
                width += kX - mR;
                x2 = x1 + width;
                grid.MoveCursor(xScaleR, xPos, -1, true, null);
            }
            else
            {
                x1 += kX - mL;
                width = x2 - x1;
                grid.MoveCursor(xScaleL, xPos, -1, true, null);
            }
            DefaultControllerPosition(true, true);
        }

        private void ScaleSelectedLinesY(bool up, float yPos)
        {
            float kY = yPos;
            float mU = yScaleU.Y;
            float mD = yScaleD.Y;
            IpCursor pos = new IpCursor();
            grid.MoveCursor(pos, -1, yPos, true, null);
            kY = pos.Y;

            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                {
                    float coeff1 = 0;
                    float coeff2 = 0;
                    if (up)
                    {
                        coeff1 = 1 - ((pic.Lines[i].y1 - yScaleU.Y) / height);
                        coeff2 = 1 - ((pic.Lines[i].y2 - yScaleU.Y) / height);
                        pic.Lines[i].y1 += (kY - mU) * coeff1;
                        pic.Lines[i].y2 += (kY - mU) * coeff2;
                    }
                    else
                    {
                        coeff1 = 1 - ((yScaleD.Y - pic.Lines[i].y1) / height);
                        coeff2 = 1 - ((yScaleD.Y - pic.Lines[i].y2) / height);
                        pic.Lines[i].y1 += (kY - mD) * coeff1;
                        pic.Lines[i].y2 += (kY - mD) * coeff2;
                    }
                    pic.Lines[i].CalculateRotationAxes(moveCursor.X, moveCursor.Y);
                }

            }
            if (up)
            {
                y1 += kY - mU;
                height += mU - kY;
                grid.MoveCursor(yScaleU, -1, yPos, true, null);
            }
            else
            {
                height += kY - mD;
                y2 = y1 + height;
                grid.MoveCursor(yScaleD, -1, yPos, true, null);
            }
            DefaultControllerPosition(true, true);
        }

        private void CalculateAngles(float xPos, float yPos)
        {
            float w = (moveCursor.X * pic.ScaleCoefficient - pic.XOffset) - xPos;
            float h = (moveCursor.Y * pic.ScaleCoefficient - pic.YOffset) - yPos;
            cursorAngle = (float)Math.Atan2(-w, -h);
            if (grid.EnableRotationGrid)
                cursorAngle = grid.GridRotation(cursorAngle);
        }

        private void RotateCursor()
        {
            rotationCursor.X = moveCursor.X + radius * (float)Math.Sin(cursorAngle);
            rotationCursor.Y = moveCursor.Y + radius * (float)Math.Cos(cursorAngle);
            for (int i = 0; i < pic.CounterLines; ++i)
                if (pic.Lines[i].selected)
                    pic.Lines[i].RotateLine(cursorAngle - (float)Math.PI);
            for (int i = 0; i < pic.CounterEllipses; ++i)
                if (pic.Ellipses[i].selected)
                    pic.Ellipses[i].RotateCircle(cursorAngle - (float)Math.PI);
        }

        private void ShowElements(bool movecursor, bool xScalel, bool xScaler, bool yScaleu, bool yScaled, bool xyScaleur, bool rotator)
        {
            moveCursor.ShowCursor = movecursor;
            xScaleL.ShowCursor = xScalel;
            xScaleR.ShowCursor = xScaler;
            yScaleU.ShowCursor = yScaleu;
            yScaleD.ShowCursor = yScaled;
            xyScaleUR.ShowCursor = xyScaleur;
            rotationCursor.ShowCursor = rotator;
        }
        #endregion

        #region PUBLIC METHODS
        public GizmoMixed(IpPicture pic, IpGrid grid):base(pic,grid)
        {
            gizmoPen = new Pen(Color.Green);
            controllerPen = new Pen(Color.Violet);
            selectedControllerPen = new Pen(Color.Blue);
            sectorBrush = new SolidBrush(Color.FromArgb(50, 150, 0, 150));
            textBrush = new SolidBrush(Color.Black);
            moveCursor = new IpCursor(5, controllerPen);
            xScaleR = new IpCursor(5, controllerPen);
            xScaleL = new IpCursor(5, controllerPen);
            yScaleU = new IpCursor(5, controllerPen);
            yScaleD = new IpCursor(5, controllerPen);
            xyScaleUR = new IpCursor(5, controllerPen);
            rotationCursor = new IpCursor(10, controllerPen);
            CreateGizmo();
        }

        public override void DefaultControllerPosition()
        {
            moveCursor.X = x2 - width / 2;
            moveCursor.Y = y2 - height / 2;
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
            rotationCursor.X = x2 - width / 2;
            rotationCursor.Y = y2 - height / 2 - radius;
            frsAngle = (float)Math.PI;
            radius = (float)Math.Sqrt(width / 2 * width / 2 + height / 2 * height / 2);
            CalculateNormals();
        }

        public override void DrawGizmo(Graphics graph)
        {
            if (showGizmo)
                graph.DrawRectangle(gizmoPen, x1 * pic.ScaleCoefficient - pic.XOffset, y1 * pic.ScaleCoefficient - pic.YOffset, width * pic.ScaleCoefficient, height * pic.ScaleCoefficient);

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
                        f1 = k - 90;
                        rotationAngle = 360 - l;
                    }
                    graph.DrawEllipse(gizmoPen, (moveCursor.X - radius) * pic.ScaleCoefficient - pic.XOffset, (moveCursor.Y - radius) * pic.ScaleCoefficient - pic.YOffset, radius * 2 * pic.ScaleCoefficient, radius * 2 * pic.ScaleCoefficient);
                    graph.FillPie(sectorBrush, (moveCursor.X - radius) * pic.ScaleCoefficient - pic.XOffset, (moveCursor.Y - radius) * pic.ScaleCoefficient - pic.YOffset, radius * 2 * pic.ScaleCoefficient, radius * 2 * pic.ScaleCoefficient, f1, rotationAngle);
                    graph.DrawString(Convert.ToString(Math.Round(rotationAngle, 2)), new Font("Times New Roman", 10 * pic.ScaleCoefficient), textBrush, moveCursor.X * pic.ScaleCoefficient - pic.XOffset, moveCursor.Y * pic.ScaleCoefficient - pic.YOffset);
                }
                moveCursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                xScaleR.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                xScaleL.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                yScaleU.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                yScaleD.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                xyScaleUR.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                rotationCursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
        }

        public override void MirrorSelectedX()
        {
            bool f = false;
            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                {
                    if (pic.Lines[i].x1 < moveCursor.X)
                        pic.Lines[i].x1 = pic.Lines[i].x1 + Math.Abs(moveCursor.X - pic.Lines[i].x1) * 2;
                    else
                        pic.Lines[i].x1 = pic.Lines[i].x1 - Math.Abs(moveCursor.X - pic.Lines[i].x1) * 2;
                    if (pic.Lines[i].x2 > moveCursor.X)
                        pic.Lines[i].x2 = pic.Lines[i].x2 - Math.Abs(pic.Lines[i].x2 - moveCursor.X) * 2;
                    else
                        pic.Lines[i].x2 = pic.Lines[i].x2 + Math.Abs(moveCursor.X - pic.Lines[i].x2) * 2;
                    f = true;
                }
            }
            if (f)
            {
                CreateGizmo();
            }
        }

        public override void MirrorSelectedY()
        {
            bool f = false;
            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                {
                    if (pic.Lines[i].y1 < moveCursor.Y)
                        pic.Lines[i].y1 = pic.Lines[i].y1 + Math.Abs(moveCursor.Y - pic.Lines[i].y1) * 2;
                    else
                        pic.Lines[i].y1 = pic.Lines[i].y1 - Math.Abs(moveCursor.Y - pic.Lines[i].y1) * 2;
                    if (pic.Lines[i].y2 > moveCursor.Y)
                        pic.Lines[i].y2 = pic.Lines[i].y2 - Math.Abs(pic.Lines[i].y2 - moveCursor.Y) * 2;
                    else
                        pic.Lines[i].y2 = pic.Lines[i].y2 + Math.Abs(moveCursor.Y - pic.Lines[i].y2) * 2;
                    f = true;
                }
            }
            if (f)
            {
                CreateGizmo();
            }
        }

        public override void Control(int xPos, int yPos)
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
                CalculateAngles(xPos, yPos);
                RotateCursor();
                showGizmo = false;
                showRotationTrack = true;
                ShowElements(true, false, false, false, false, false, true);
                return;
            }
        }

        public override void CheckSelectedController(int xPos, int yPos)
        {
            dragSelected = ReDrawController(moveCursor, xPos, yPos);
            scaleXR = ReDrawController(xScaleR, xPos, yPos);
            scaleXL = ReDrawController(xScaleL, xPos, yPos);
            scaleYU = ReDrawController(yScaleU, xPos, yPos);
            scaleYD = ReDrawController(yScaleD, xPos, yPos);
            scaleXYUR = ReDrawController(xyScaleUR, xPos, yPos);
            rotatePic = ReDrawController(rotationCursor, xPos, yPos);
        }

        public override void ResetControllers()
        {
            if (dragSelected)
            {
                if (mCenterPoint)
                {
                    rotationCursor.X = moveCursor.X;
                    rotationCursor.Y = moveCursor.Y - radius;
                }
                dragSelected = ResetControl(moveCursor);
            }
            scaleXR = ResetControl(xScaleR);
            scaleXL = ResetControl(xScaleL);
            scaleYU = ResetControl(yScaleU);
            scaleYD = ResetControl(yScaleD);
            scaleXYUR = ResetControl(xyScaleUR);
            if (rotatePic)
            {
                ResetGizmo();
                DefaultControllerPosition(false, false);
                showGizmo = true;
                showRotationTrack = false;
                frsAngle = cursorAngle;
                rotatePic = ResetControl(rotationCursor);
                ShowElements(true, true, true, true, true, true, true);
            }
        }

        #endregion
    }
}
