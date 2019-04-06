using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    public struct IpSpline
    {
        private float[] x, y;
        public static int res = 30;
        public float x1;
        public float y1;
        public float x2;
        public float y2;
        public float x3;
        public float y3;
        public float x4;
        public float y4;
        public Pen pen;
        public bool selected;
        public float xSmin,ySmin,xSmax,ySmax;
        public int layer;

        private float angle1, angle2, angle3, angle4;
        private float radius1, radius2, radius3, radius4;
        private float hc1, wc1, hc2, wc2, hc3, wc3, hc4, wc4;
        private float xCent, yCent;

        public void SetCenterPoint(float xCenter, float yCenter)
        {
            xCent = xCenter;
            yCent = yCenter;
        }

        public void CalculateRotationAxes(float xCenter, float yCenter)
        {
            SetCenterPoint(xCenter, yCenter);
            hc1 = yCenter - y1;
            wc1 = x1 - xCenter;
            hc2 = yCenter - y2;
            wc2 = x2 - xCenter;
            hc3 = yCenter - y3;
            wc3 = x3 - xCenter;
            hc4 = yCenter - y4;
            wc4 = x4 - xCenter;
            radius1 = Convert.ToSingle(Math.Sqrt(hc1 * hc1 + wc1 * wc1));
            radius2 = Convert.ToSingle(Math.Sqrt(hc2 * hc2 + wc2 * wc2));
            radius3 = Convert.ToSingle(Math.Sqrt(hc3 * hc3 + wc3 * wc3));
            radius4 = Convert.ToSingle(Math.Sqrt(hc4 * hc4 + wc4 * wc4));
            angle1 = (float)Math.Atan2(hc1, wc1);
            angle2 = (float)Math.Atan2(hc2, wc2);
            angle3 = (float)Math.Atan2(hc3, wc3);
            angle4 = (float)Math.Atan2(hc4, wc4);
        }

        public void RotateSpline(float angle)
        {
            double radAngle1 = Math.PI / 2 + angle + angle1;
            double radAngle2 = Math.PI / 2 + angle + angle2;
            double radAngle3 = Math.PI / 2 + angle + angle3;
            double radAngle4 = Math.PI / 2 + angle + angle4;
            x4 = xCent + radius4 * (float)Math.Sin(radAngle4);
            y4 = yCent + radius4 * (float)Math.Cos(radAngle4);
            x3 = xCent + radius3 * (float)Math.Sin(radAngle3);
            y3 = yCent + radius3 * (float)Math.Cos(radAngle3);
            x2 = xCent + radius2 * (float)Math.Sin(radAngle2);
            y2 = yCent + radius2 * (float)Math.Cos(radAngle2);
            x1 = xCent + radius1 * (float)Math.Sin(radAngle1);
            y1 = yCent + radius1 * (float)Math.Cos(radAngle1);
            CalculatePoints();
        }

        public void AddSpline(IpCursor lastCursor, IpCursor selectCursor, Pen pen,int layer)
        {
            x1 = lastCursor.X;
            y1 = lastCursor.Y;
            x4 = selectCursor.X;
            y4 = selectCursor.Y;
            x2 = (x1 + x4) / 2;
            y2 = (y1 + y4) / 2;
            x3 = (x1 + x4) / 2;
            y3 = (y1 + y4) / 2;
            this.pen = pen;
            this.layer = layer;
            CalculatePoints();
            selected = false;
        }
        public void Curve1(IpCursor selectCursor)
        {
            x2 = selectCursor.X;
            y2 = selectCursor.Y;
            x3 = selectCursor.X;
            y3 = selectCursor.Y;
            CalculatePoints();
        }
        public void Curve2(IpCursor selectCursor)
        {
            x3 = selectCursor.X;
            y3 = selectCursor.Y;
            CalculatePoints();
        }

        public void MirrorX(IpCursor cursor)
        {
            if (x1 < cursor.X)
                x1 = x1 + Math.Abs(cursor.X - x1) * 2;
            else
                x1 = x1 - Math.Abs(cursor.X - x1) * 2;

            if (x2 < cursor.X)
                x2 = x2 + Math.Abs(cursor.X - x2) * 2;
            else
                x2 = x2 - Math.Abs(cursor.X - x2) * 2;

            if (x3 < cursor.X)
                x3 = x3 + Math.Abs(cursor.X - x3) * 2;
            else
                x3 = x3 - Math.Abs(cursor.X - x3) * 2;

            if (x4 < cursor.X)
                x4 = x4 + Math.Abs(cursor.X - x4) * 2;
            else
                x4 = x4 - Math.Abs(cursor.X - x4) * 2;
            CalculatePoints();
        }

        public void MirrorY(IpCursor cursor)
        {
            if (y1 < cursor.Y)
                y1 = y1 + Math.Abs(cursor.Y - y1) * 2;
            else
                y1 = y1 - Math.Abs(cursor.Y - y1) * 2;

            if (y2 < cursor.Y)
                y2 = y2 + Math.Abs(cursor.Y - y2) * 2;
            else
                y2 = y2 - Math.Abs(cursor.Y - y2) * 2;

            if (y3 < cursor.Y)
                y3 = y3 + Math.Abs(cursor.Y - y3) * 2;
            else
                y3 = y3 - Math.Abs(cursor.Y - y3) * 2;

            if (y4 < cursor.Y)
                y4 = y4 + Math.Abs(cursor.Y - y4) * 2;
            else
                y4 = y4 - Math.Abs(cursor.Y - y4) * 2;
            CalculatePoints();
        }

        public void CalculatePoints()
        {
            x = null;
            y = null;
            xSmin = 9999;
            ySmin = 9999;
            xSmax = 0;
            ySmax = 0;
            x = new float[res];
            y = new float[res];
            float delta = 1 / (float)res;
            float t = 0;
            x[0] = x1;
            y[0] = y1;

            if (x[0] > xSmax)
                xSmax = x[0];
            if (x[0] < xSmin)
                xSmin = x[0];
            if (y[0] > ySmax)
                ySmax = y[0];
            if (y[0] < ySmin)
                ySmin = y[0];
            for (int i = 1; i < res-1; ++i)
            {
                t += delta;
                x[i] = Convert.ToSingle(Math.Pow(1 - t, 3) * x1 + 3 * t * Math.Pow(1 - t, 2) * x2 + 3 * t * t * (1 - t) * x3 + t * t * t * x4);
                y[i] = Convert.ToSingle(Math.Pow(1 - t, 3) * y1 + 3 * t * Math.Pow(1 - t, 2) * y2 + 3 * t * t * (1 - t) * y3 + t * t * t * y4);

                if (x[i] > xSmax)
                    xSmax = x[i];
                if (x[i] < xSmin)
                    xSmin = x[i];
                if (y[i] > ySmax)
                    ySmax = y[i];
                if (y[i] < ySmin)
                    ySmin = y[i];
            }
            x[res - 1] = x4;
            y[res - 1] = y4;

            if (x[res - 1] > xSmax)
                xSmax = x[res - 1];
            if (x[res - 1] < xSmin)
                xSmin = x[res - 1];
            if (y[res - 1] > ySmax)
                ySmax = y[res - 1];
            if (y[res - 1] < ySmin)
                ySmin = y[res - 1];
        }


        public void DrawSpline(Graphics buff, float xf, float yf, float coeff, Pen selectedPen)
        {
            Pen sPen;
            if (selected)
                sPen = selectedPen;
            else
                sPen = pen;
            for (int i = 1; i < res; ++i)
            {
                buff.DrawLine(sPen, x[i] * coeff - xf, y[i] * coeff - yf, x[i - 1] * coeff - xf, y[i - 1] * coeff - yf);
            }
        }

        public static bool operator !=(IpSpline s1, IpSpline s2)
        {
            if ((s1.x1 != s2.x1) || (s1.y1 != s2.y1) || (s1.x2 != s2.x2) || (s1.y2 != s2.y2) || (s1.x3 != s2.x3) || (s1.y3 != s2.y3))
                return true;
            return false;
        }
        public static bool operator ==(IpSpline s1, IpSpline s2)
        {
            if ((s1.x1 == s2.x1) && (s1.y1 == s2.y1) && (s1.x2 == s2.x2) && (s1.y2 == s2.y2) && (s1.x3 == s2.x3) && (s1.y3 == s2.y3))
                return true;
            return false;
        }

        public static bool operator !=(IpSpline s1, int i2)
        {
            if ((s1.x1 != i2) || (s1.y1 != i2) || (s1.x2 != i2) || (s1.y2 != i2) || (s1.x3 != i2) || (s1.y3 != i2))
                return true;
            return false;
        }
        public static bool operator ==(IpSpline s1, int i2)
        {
            if ((s1.x1 == i2) && (s1.y1 == i2) && (s1.x2 == i2) && (s1.y2 == i2) && (s1.x3 == i2) && (s1.y3 == i2))
                return true;
            return false;
        }

    }
}
