﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    public struct Ellipse
    {
        private float[] x, y;
        public static int res = 30;
        public float xR;
        public float yR;
        public float radX;
        public float radY;
        public Pen pen;
        public bool selected;
        public float alpha;
        public float pAlpha;

        public float x1, y1, x2, y2;

        private float angle1;
        private float radius1;
        private float hc1, wc1;
        private float xCent, yCent;

        public void SetCenterPoint(float xCenter, float yCenter)
        {
            xCent = xCenter;
            yCent = yCenter;
        }

        public void CalculateRotationAxes(float xCenter, float yCenter)
        {
            SetCenterPoint(xCenter, yCenter);
            hc1 = yCenter - yR;
            wc1 = xR - xCenter;
            radius1 = Convert.ToSingle(Math.Sqrt(hc1 * hc1 + wc1 * wc1));
            angle1 = (float)Math.Atan2(hc1, wc1);
            pAlpha = alpha;
        }

        public void RotateCircle(float angle)
        {
            double radAngle1 =Math.PI/2 + angle + angle1;
            xR = xCent + radius1 * (float)Math.Sin(radAngle1);
            yR = yCent + radius1 * (float)Math.Cos(radAngle1);
            alpha = pAlpha + angle;
            CalculatePoints();
        }

        public void AddCircle(IpCursor lastCursor, IpCursor selectCursor, Pen pen)
        {
            xR = lastCursor.X;
            yR = lastCursor.Y;
            radX = Math.Abs(lastCursor.X - selectCursor.X);
            radY = Math.Abs(lastCursor.Y - selectCursor.Y);
            CalculatePoints();
            this.pen = pen;
        }
        public void CalculatePoints()
        {
            x = new float[res];
            y = new float[res];
            //ReCalculatePoints();
            float delta = (float)Math.PI * 2 / res;
            float t = 0;
            float xMin = 9999;
            float xMax = 0;
            float yMin = 9999;
            float yMax = 0;
            for (int i = 0; i < res; ++i)
            {
                t += delta;
                x[i] = Convert.ToSingle(radX * Math.Cos(t) * Math.Cos(alpha) + radY * Math.Sin(t) * Math.Sin(alpha));
                y[i] = Convert.ToSingle(-radX * Math.Cos(t) * Math.Sin(alpha) + radY * Math.Sin(t) * Math.Cos(alpha));

                if (x[i] < xMin)
                    xMin = x[i];
                if (x[i] > xMax)
                    xMax = x[i];
                if (y[i] < yMin)
                    yMin = y[i];
                if (y[i] > yMax)
                    yMax = y[i];
            }
            x1 = xR - xMin;
            y1 = yR - yMin;
            x2 = xR - xMax;
            y2 = yR - yMax;
        }

        public void DrawEllipse(Graphics buff, Pen selectedPen)
        {
            Pen sPen;
            if (selected)
                sPen = selectedPen;
            else
                sPen = pen;
            for (int i = 1; i < res; ++i)
            {
                buff.DrawLine(sPen, xR + x[i], yR + y[i], xR + x[i - 1], yR + y[i - 1]);
            }
            buff.DrawLine(sPen, xR + x[0], yR + y[0], xR + x[res-1], yR + y[res-1]);
        }

        public static bool operator !=(Ellipse c1, Ellipse c2)
        {
            if ((c1.xR != c2.xR) || (c1.yR != c2.yR) || (c1.radX != c2.radX) || (c1.radY != c2.radY))
                return true;
            return false;
        }
        public static bool operator ==(Ellipse c1, Ellipse c2)
        {
            if ((c1.xR == c2.xR) && (c1.yR == c2.yR) && (c1.radX == c2.radX) && (c1.radY == c2.radY))
                return true;
            return false;
        }

        public static bool operator !=(Ellipse c1, int i2)
        {
            if ((c1.xR != i2) || (c1.yR != i2) || (c1.radX != i2) || (c1.radY != i2))
                return true;
            return false;
        }
        public static bool operator ==(Ellipse c1, int i2)
        {
            if ((c1.xR == i2) && (c1.yR == i2) && (c1.radX == i2) && (c1.radY == i2))
                return true;
            return false;
        }
    }
}