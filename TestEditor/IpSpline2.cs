using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    public struct IpSpline2
    {
        private float[] x, y;
        public static int res = 10;
        public float x1;
        public float y1;
        public float x2;
        public float y2;
        public float x3;
        public float y3;
        public Pen pen;
        public bool selected;

        private float angle1, angle2, angle3;
        private float radius1, radius2, radius3;
        private float hc1, wc1, hc2, wc2, hc3, wc3;
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
            radius1 = Convert.ToSingle(Math.Sqrt(hc1 * hc1 + wc1 * wc1));
            radius2 = Convert.ToSingle(Math.Sqrt(hc2 * hc2 + wc2 * wc2));
            radius3 = Convert.ToSingle(Math.Sqrt(hc3 * hc3 + wc3 * wc3));
            angle1 = (float)Math.Atan2(hc1, wc1);
            angle2 = (float)Math.Atan2(hc2, wc2);
            angle3 = (float)Math.Atan2(hc3, wc3);
        }

        public void RotateSpline2(float angle)
        {
            double radAngle1 = Math.PI / 2 + angle + angle1;
            double radAngle2 = Math.PI / 2 + angle + angle2;
            double radAngle3 = Math.PI / 2 + angle + angle3;
            x3 = xCent + radius3 * (float)Math.Sin(radAngle3);
            y3 = yCent + radius3 * (float)Math.Cos(radAngle3);
            x2 = xCent + radius2 * (float)Math.Sin(radAngle2);
            y2 = yCent + radius2 * (float)Math.Cos(radAngle2);
            x1 = xCent + radius1 * (float)Math.Sin(radAngle1);
            y1 = yCent + radius1 * (float)Math.Cos(radAngle1);
        }

        public void AddLine(IpCursor lastCursor, IpCursor selectCursor, Pen pen)
        {
            x1 = lastCursor.X;
            y1 = lastCursor.Y;
            x3 = selectCursor.X;
            y3 = selectCursor.Y;
            x2 = (x1+x3)/2;
            y2 = (y1+y3)/2;
            this.pen = pen;
            CalculatePoints();
        }
        public void AddCurve(IpCursor selectCursor)
        {
            x2 = selectCursor.X;
            y2 = selectCursor.Y;
            CalculatePoints();
        }

        public void CalculatePoints()
        {
            x = new float[res];
            y = new float[res];
            float delta = 1 / (float)res;
            float t = 0;
            x[0] = x1;
            y[0] = y1;
            for (int i = 1; i < res-1; ++i)
            {
                t += delta;
                x[i] = (1 - 2 * t + t * t) * x1 + (2 * t - 2 * t * t) * x2 + t * t * x3;
                y[i] = (1 - 2 * t + t * t) * y1 + (2 * t - 2 * t * t) * y2 + t * t * y3;
            }
            x[res - 1] = x3;
            y[res - 1] = y3;
        }


        public void DrawSpline2(Graphics buff, float xf, float yf, float coeff, Pen selectedPen)
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

        public static bool operator !=(IpSpline2 s1, IpSpline2 s2)
        {
            if ((s1.x1 != s2.x1) || (s1.y1 != s2.y1) || (s1.x2 != s2.x2) || (s1.y2 != s2.y2) || (s1.x3 != s2.x3) || (s1.y3 != s2.y3))
                return true;
            return false;
        }
        public static bool operator ==(IpSpline2 s1, IpSpline2 s2)
        {
            if ((s1.x1 == s2.x1) && (s1.y1 == s2.y1) && (s1.x2 == s2.x2) && (s1.y2 == s2.y2) && (s1.x3 == s2.x3) && (s1.y3 == s2.y3))
                return true;
            return false;
        }

        public static bool operator !=(IpSpline2 s1, int i2)
        {
            if ((s1.x1 != i2) || (s1.y1 != i2) || (s1.x2 != i2) || (s1.y2 != i2) || (s1.x3 != i2) || (s1.y3 != i2))
                return true;
            return false;
        }
        public static bool operator ==(IpSpline2 s1, int i2)
        {
            if ((s1.x1 == i2) && (s1.y1 == i2) && (s1.x2 == i2) && (s1.y2 == i2) && (s1.x3 == i2) && (s1.y3 == i2))
                return true;
            return false;
        }

    }
}
