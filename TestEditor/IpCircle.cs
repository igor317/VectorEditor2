using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    public struct Circle
    {
        public float xR;
        public float yR;
        public float radius;
        public Pen pen;
        public bool selected;

        public float x1, y1, x2, y2;

        private float angle1, angle2;
        private float radius1, radius2;
        private float hc1, wc1, hc2, wc2;
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
            hc2 = yCenter - y1;
            wc2 = x1 - xCenter;
            radius1 = Convert.ToSingle(Math.Sqrt(hc1 * hc1 + wc1 * wc1));
            radius2 = Convert.ToSingle(Math.Sqrt(hc2 * hc2 + wc2 * wc2));
            angle1 = (float)Math.Atan2(hc1, wc1);
            angle2 = (float)Math.Atan2(hc2, wc2);
        }

        public void RotateCircle(float angle)
        {
            double radAngle1 = Math.PI / 2 + angle + angle1;
            double radAngle2 = Math.PI / 2 + angle + angle2;
            x1 = xCent + radius2 * (float)Math.Sin(radAngle2);
            y1 = yCent + radius2 * (float)Math.Cos(radAngle2);
            xR = xCent + radius1 * (float)Math.Sin(radAngle1);
            yR = yCent + radius1 * (float)Math.Cos(radAngle1);
        }

        public void ReCalcPoints()
        {
            x1 = xR - radius;
            y1 = yR - radius;
            x2 = x1 + 2 * radius;
            y2 = y1 + 2 * radius;
        }

        public void AddCircle(IpCursor lastCursor, IpCursor selectCursor, Pen pen)
        {
            xR = lastCursor.X;
            yR = lastCursor.Y;
            radius = Convert.ToSingle(Math.Sqrt(Math.Pow(xR - selectCursor.X, 2) + Math.Pow(yR - selectCursor.Y, 2)));
            x1 = xR - radius;
            y1 = yR - radius;
            x2 = x1 + 2 * radius;
            y2 = y1 + 2 * radius;
            this.pen = pen;
        }

        public static bool operator !=(Circle c1, Circle c2)
        {
            if ((c1.xR != c2.xR) || (c1.yR != c2.yR) || (c1.radius != c2.radius))
                return true;
            return false;
        }
        public static bool operator ==(Circle c1, Circle c2)
        {
            if ((c1.xR == c2.xR) && (c1.yR == c2.yR) && (c1.radius == c2.radius))
                return true;
            return false;
        }

        public static bool operator !=(Circle c1, int i2)
        {
            if ((c1.xR != i2) || (c1.yR != i2) || (c1.radius != i2))
                return true;
            return false;
        }
        public static bool operator ==(Circle c1, int i2)
        {
            if ((c1.xR == i2) && (c1.yR == i2) && (c1.radius == i2))
                return true;
            return false;
        }
    }
}
