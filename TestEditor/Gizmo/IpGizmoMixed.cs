﻿using System;
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
                frsAngle = (float)Math.PI;
                cursorAngle = frsAngle;
                radius = (float)Math.Sqrt(width / 2 * width / 2 + height / 2 * height / 2);
                CalculateNormals();
            }

            rotationCursor.X = moveCursor.X + radius * (float)Math.Sin(cursorAngle);
            rotationCursor.Y = moveCursor.Y + radius * (float)Math.Cos(cursorAngle);

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
            frsAngle = (float)Math.PI;
            cursorAngle = frsAngle;
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
            if (cursor.InCursorArea(xPos, yPos, pic.ViewBox.xOffset, pic.ViewBox.yOffset, pic.ViewBox.scaleCoefficient))
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
            for (int i = 0; i < pic.CounterSplines; ++i)
                if (pic.Splines[i].selected)
                    pic.Splines[i].CalculateRotationAxes(moveCursor.X, moveCursor.Y);
        }

        private void CalculatePoints()
        {
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
                }
            }
            for (int i = 0; i < pic.CounterSplines; ++i)
            {
                if (pic.Splines[i].selected)
                {
                    if (pic.Splines[i].xSmin < minX)
                        minX = pic.Splines[i].xSmin;
                    if (pic.Splines[i].ySmin < minY)
                        minY = pic.Splines[i].ySmin;

                    if (pic.Splines[i].xSmax > maxX)
                        maxX = pic.Splines[i].xSmax;
                    if (pic.Splines[i].ySmax > maxY)
                        maxY = pic.Splines[i].ySmax;
                }
            }

            x1 = minX;
            x2 = maxX;
            y1 = minY;
            y2 = maxY;
        }

        private void MoveCenterPoint(int xPos, int yPos)
        {
            grid.MoveCursor(moveCursor, xPos, yPos, false, null);
            DefaultControllerPosition(false, true);
            CalculateNormals();
        }

        private void MoveSelectedLines(float xPos, float yPos)
        {
            float m1 = moveCursor.X;
            float m2 = moveCursor.Y;
            grid.MoveCursor(moveCursor, xPos, yPos, true, null);
            float k1 = moveCursor.X;
            float k2 = moveCursor.Y;

            x1 += k1 - m1;
            x2 += k1 - m1;
            y1 += k2 - m2;
            y2 += k2 - m2;
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

            for (int i = 0;i<pic.CounterSplines;++i)
            {
                if (pic.Splines[i].selected)
                {
                    pic.Splines[i].x1 += k1 - m1;
                    pic.Splines[i].y1 += k2 - m2;
                    pic.Splines[i].x2 += k1 - m1;
                    pic.Splines[i].y2 += k2 - m2;
                    pic.Splines[i].x3 += k1 - m1;
                    pic.Splines[i].y3 += k2 - m2;
                    pic.Splines[i].x4 += k1 - m1;
                    pic.Splines[i].y4 += k2 - m2;
                    pic.Splines[i].CalculatePoints();
                    pic.Splines[i].SetCenterPoint(moveCursor.X, moveCursor.Y);
                }
            }
        }

        private void ScaleSelectedLinesX(bool right, float xPos)
        {
            float mR = 0;
            IpCursor pos = new IpCursor();
            grid.MoveCursor(pos, xPos, -1, true, null);
            float kX = pos.X;

            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                {
                    float coeff1 = 0;
                    float coeff2 = 0;
                    if (right)
                    {
                        mR = xScaleR.X;
                        coeff1 = 1 - ((xScaleR.X - pic.Lines[i].x1) / width);
                        coeff2 = 1 - ((xScaleR.X - pic.Lines[i].x2) / width);
                    }
                    else
                    {
                        mR = xScaleL.X;
                        coeff1 = 1 - ((pic.Lines[i].x1 - xScaleL.X) / width);
                        coeff2 = 1 - ((pic.Lines[i].x2 - xScaleL.X) / width);
                    }
                    pic.Lines[i].x1 += (kX - mR) * coeff1;
                    pic.Lines[i].x2 += (kX - mR) * coeff2;
                    pic.Lines[i].CalculateRotationAxes(moveCursor.X, moveCursor.Y);
                }
            }

            for (int i = 0; i < pic.CounterEllipses; ++i)
            {
                if (pic.Ellipses[i].selected)
                {
                    float coeffyR = 0;
                    float coeffxR = 0;
                    float coeff1 = 0;

                    float coeff2 = 0;
                    if (right)
                    {
                        mR = xScaleR.X;
                        coeffxR = 1 - ((xScaleR.X - pic.Ellipses[i].xR) / width);
                        coeffyR = 1 - ((xScaleR.Y - pic.Ellipses[i].yR) / height);
                        //coeff1 = 1 - pic.Ellipses[i].radX / width;
                        coeff1 = (pic.Ellipses[i].radX/width);
                        coeff2 = (pic.Ellipses[i].radY / height);
                    }
                    else
                    {
                        //mR = xScaleL.X;
                        //coeff1 = 1 - ((pic.Ellipses[i].xR - xScaleL.X) / width);
                    }
                    //pic.Ellipses[i].radX += (kX - mR)/2 * coeff1;
                    pic.Ellipses[i].xR += (kX - mR) * coeffxR*(float)Math.Cos(pic.Ellipses[i].pAlpha);
                    pic.Ellipses[i].radY += (kX - mR) * coeff2 * (float)Math.Sin(pic.Ellipses[i].pAlpha);
                    pic.Ellipses[i].radX += (kX - mR) * coeff1 * (float)Math.Cos(pic.Ellipses[i].pAlpha);
                    //pic.Ellipses[i].yR += (kX - mR) * coeffyR * (float)Math.Sin(pic.Ellipses[i].pAlpha);
                    pic.Ellipses[i].CalculatePoints();
                    pic.Ellipses[i].CalculateRotationAxes(moveCursor.X, moveCursor.Y);
                }
            }

            for (int i = 0;i<pic.CounterSplines;++i)
            {
                if (pic.Splines[i].selected)
                {
                    float coeff1 = 0;
                    float coeff2 = 0;
                    float coeff3 = 0;
                    float coeff4 = 0;
                    if (right)
                    {
                        mR = xScaleR.X;
                        coeff1 = 1 - ((xScaleR.X - pic.Splines[i].x1) / width);
                        coeff2 = 1 - ((xScaleR.X - pic.Splines[i].x2) / width);
                        coeff3 = 1 - ((xScaleR.X - pic.Splines[i].x3) / width);
                        coeff4 = 1 - ((xScaleR.X - pic.Splines[i].x4) / width);
                    }
                    else
                    {
                        mR = xScaleL.X;
                        coeff1 = 1 - ((pic.Splines[i].x1 - xScaleL.X) / width);
                        coeff2 = 1 - ((pic.Splines[i].x2 - xScaleL.X) / width);
                        coeff3 = 1 - ((pic.Splines[i].x3 - xScaleL.X) / width);
                        coeff4 = 1 - ((pic.Splines[i].x4 - xScaleL.X) / width);
                    }
                    pic.Splines[i].x1 += (kX - mR) * coeff1;
                    pic.Splines[i].x2 += (kX - mR) * coeff2;
                    pic.Splines[i].x3 += (kX - mR) * coeff3;
                    pic.Splines[i].x4 += (kX - mR) * coeff4;
                    pic.Splines[i].CalculatePoints();
                    pic.Splines[i].CalculateRotationAxes(moveCursor.X, moveCursor.Y);
                }
            }

            if (right)
            {
                width += kX - mR;
                x2 = x1 + width;
            }
            else
            {
                x1 += kX - mR;
                width = x2 - x1;
            }
            DefaultControllerPosition(true, true);
        }

        private void ScaleSelectedLinesY(bool up, float yPos)
        {
            float mR = 0;
            IpCursor pos = new IpCursor();
            grid.MoveCursor(pos, -1, yPos, true, null);
            float kY = pos.Y;

            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                {
                    float coeff1 = 0;
                    float coeff2 = 0;
                    if (up)
                    {
                        mR = yScaleU.Y;
                        coeff1 = 1 - ((pic.Lines[i].y1 - yScaleU.Y) / height);
                        coeff2 = 1 - ((pic.Lines[i].y2 - yScaleU.Y) / height);
                    }
                    else
                    {
                        mR = yScaleD.Y;
                        coeff1 = 1 - ((yScaleD.Y - pic.Lines[i].y1) / height);
                        coeff2 = 1 - ((yScaleD.Y - pic.Lines[i].y2) / height);
                    }
                    pic.Lines[i].y1 += (kY - mR) * coeff1;
                    pic.Lines[i].y2 += (kY - mR) * coeff2;
                    pic.Lines[i].CalculateRotationAxes(moveCursor.X, moveCursor.Y);
                }

            }

            for (int i = 0; i < pic.CounterSplines; ++i)
            {
                if (pic.Splines[i].selected)
                {
                    float coeff1 = 0;
                    float coeff2 = 0;
                    float coeff3 = 0;
                    float coeff4 = 0;
                    if (up)
                    {
                        mR = yScaleU.Y;
                        coeff1 = 1 - ((pic.Splines[i].y1 - yScaleU.Y) / height);
                        coeff2 = 1 - ((pic.Splines[i].y2 - yScaleU.Y) / height);
                        coeff3 = 1 - ((pic.Splines[i].y3 - yScaleU.Y) / height);
                        coeff4 = 1 - ((pic.Splines[i].y4 - yScaleU.Y) / height);
                    }
                    else
                    {
                        mR = yScaleD.Y;
                        coeff1 = 1 - ((yScaleD.Y - pic.Splines[i].y1) / height);
                        coeff2 = 1 - ((yScaleD.Y - pic.Splines[i].y2) / height);
                        coeff3 = 1 - ((yScaleD.Y - pic.Splines[i].y3) / height);
                        coeff4 = 1 - ((yScaleD.Y - pic.Splines[i].y4) / height);
                    }
                    pic.Splines[i].y1 += (kY - mR) * coeff1;
                    pic.Splines[i].y2 += (kY - mR) * coeff2;
                    pic.Splines[i].y3 += (kY - mR) * coeff3;
                    pic.Splines[i].y4 += (kY - mR) * coeff4;
                    pic.Splines[i].CalculatePoints();
                    pic.Splines[i].CalculateRotationAxes(moveCursor.X, moveCursor.Y);
                }
            }
            if (up)
            {
                y1 += kY - mR;
                height += mR - kY;
            }
            else
            {
                height += kY - mR;
                y2 = y1 + height;
            }
            DefaultControllerPosition(true, true);
        }

        private void CalculateAngles(float xPos, float yPos)
        {
            float w = (moveCursor.X * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset) - xPos;
            float h = (moveCursor.Y * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset) - yPos;
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
            for (int i = 0; i < pic.CounterSplines; ++i)
                if (pic.Splines[i].selected)
                    pic.Splines[i].RotateSpline(cursorAngle - (float)Math.PI);
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
            frsAngle = (float)Math.PI;
            cursorAngle = frsAngle;
            radius = (float)Math.Sqrt(width / 2 * width / 2 + height / 2 * height / 2);
            rotationCursor.X = moveCursor.X;
            rotationCursor.Y = moveCursor.Y - radius;
            CalculateNormals();
        }

        public override void DrawGizmo(Graphics graph)
        {
            if (showGizmo)
                graph.DrawRectangle(gizmoPen, x1 * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset,
                                              y1 * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset,
                                              width * pic.ViewBox.scaleCoefficient,
                                              height * pic.ViewBox.scaleCoefficient);

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
                    graph.DrawEllipse(gizmoPen, (moveCursor.X - radius) * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset,
                                                (moveCursor.Y - radius) * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset,
                                                radius * 2 * pic.ViewBox.scaleCoefficient,
                                                radius * 2 * pic.ViewBox.scaleCoefficient);

                    graph.FillPie(sectorBrush, (moveCursor.X - radius) * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset,
                                               (moveCursor.Y - radius) * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset,
                                               radius * 2 * pic.ViewBox.scaleCoefficient,
                                               radius * 2 * pic.ViewBox.scaleCoefficient, f1, rotationAngle);

                    graph.DrawString(Convert.ToString(Math.Round(rotationAngle, 2)), new Font("Times New Roman", 10 * pic.ViewBox.scaleCoefficient),
                        textBrush, moveCursor.X * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset, moveCursor.Y * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset);
                }
                moveCursor.DrawXCursor(graph, pic.ViewBox.xOffset, pic.ViewBox.yOffset, pic.ViewBox.scaleCoefficient);
                xScaleR.DrawXCursor(graph, pic.ViewBox.xOffset, pic.ViewBox.yOffset, pic.ViewBox.scaleCoefficient);
                xScaleL.DrawXCursor(graph, pic.ViewBox.xOffset, pic.ViewBox.yOffset, pic.ViewBox.scaleCoefficient);
                yScaleU.DrawXCursor(graph, pic.ViewBox.xOffset, pic.ViewBox.yOffset, pic.ViewBox.scaleCoefficient);
                yScaleD.DrawXCursor(graph, pic.ViewBox.xOffset, pic.ViewBox.yOffset, pic.ViewBox.scaleCoefficient);
                xyScaleUR.DrawXCursor(graph, pic.ViewBox.xOffset, pic.ViewBox.yOffset, pic.ViewBox.scaleCoefficient);
                rotationCursor.DrawXCursor(graph, pic.ViewBox.xOffset, pic.ViewBox.yOffset, pic.ViewBox.scaleCoefficient);
        }

        public override void MirrorSelectedX()
        {
            for (int i = 0; i < pic.CounterLines; ++i)
                if (pic.Lines[i].selected)
                    pic.Lines[i].MirrorX(moveCursor);
            for (int i = 0;i<pic.CounterSplines;++i)
                if (pic.Splines[i].selected)
                    pic.Splines[i].MirrorX(moveCursor);
            CreateGizmo();
        }

        public override void MirrorSelectedY()
        {
            for (int i = 0; i < pic.CounterLines; ++i)
                if (pic.Lines[i].selected)
                    pic.Lines[i].MirrorY(moveCursor);
            for (int i = 0; i < pic.CounterSplines; ++i)
                if (pic.Splines[i].selected)
                    pic.Splines[i].MirrorY(moveCursor);
            CreateGizmo();
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
