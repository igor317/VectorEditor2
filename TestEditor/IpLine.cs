using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    public struct LinePic
    {
        public float x1;
        public float y1;
        public float x2;
        public float y2;
        public Pen pen;
        public bool selected;

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
            hc1 = yCenter - y1;
            wc1 = x1 - xCenter;
            hc2 = yCenter - y2;
            wc2 = x2 - xCenter;
            radius1 = Convert.ToSingle(Math.Sqrt(hc1 * hc1 + wc1 * wc1));
            radius2 = Convert.ToSingle(Math.Sqrt(hc2 * hc2 + wc2 * wc2));
            angle1 = (float)Math.Atan2(hc1, wc1);
            angle2 = (float)Math.Atan2(hc2, wc2);
        }

        public void RotateLine(float angle)
        {
            double radAngle1 = Math.PI / 2 + angle + angle1;
            double radAngle2 = Math.PI / 2 + angle + angle2;
            x2 = xCent + radius2 * (float)Math.Sin(radAngle2);
            y2 = yCent + radius2 * (float)Math.Cos(radAngle2);
            x1 = xCent + radius1 * (float)Math.Sin(radAngle1);
            y1 = yCent + radius1 * (float)Math.Cos(radAngle1);
        }

        public void AddLine(IpCursor lastCursor, IpCursor selectCursor, Pen pen)
        {
            x1 = lastCursor.X;
            y1 = lastCursor.Y;
            x2 = selectCursor.X;
            y2 = selectCursor.Y;
            this.pen = pen;
        }

        public void DrawLine(Graphics buff,float coeff, Pen selectedPen)
        {
            Pen sPen;
            if (selected)
                sPen = selectedPen;
            else
                sPen = pen;
            buff.DrawLine(sPen, x1*coeff, y1 * coeff, x2 * coeff, y2 * coeff);
        }

        public static bool operator !=(LinePic c1, LinePic c2)
        {
            if ((c1.x1 != c2.x2) || (c1.y1 != c2.y1) || (c1.x1 != c2.x2) || (c1.y2 != c2.y2))
                return true;
            return false;
        }
        public static bool operator ==(LinePic c1, LinePic c2)
        {
            if ((c1.x1 == c2.x1) && (c1.y1 == c2.y1) && (c1.x2 == c2.x2) && (c1.y2 == c2.y2))
                return true;
            return false;
        }

        public static bool operator !=(LinePic c1, int i2)
        {
            if ((c1.x1 != i2) || (c1.y1 != i2) || (c1.x1 != i2) || (c1.y2 != i2))
                return true;
            return false;
        }
        public static bool operator ==(LinePic c1, int i2)
        {
            if ((c1.x1 == i2) && (c1.y1 == i2) && (c1.x2 == i2) && (c1.y2 == i2))
                return true;
            return false;
        }

    }
}
