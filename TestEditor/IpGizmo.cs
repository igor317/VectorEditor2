using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
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
            ResetControllers(true, true);
        }

        public void ResetControllers(bool resetCenter, bool resetRotator)
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

        public void ShowElements(bool movecursor, bool xScalel, bool xScaler, bool yScaleu, bool yScaled, bool xyScaleur, bool rotator)
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
            ResetControllers(true, true);
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
            ResetControllers(false, false);
        }

        public void ResetRadius()
        {
            radius = (float)Math.Sqrt(width / 2 * width / 2 + height / 2 * height / 2);
        }

        public void DrawGizmo(Graphics graph,float xoff,float yoff,float coeff)
        {
            if (showGizmo)
                graph.DrawRectangle(gizmoPen, x1*coeff-xoff, y1*coeff-yoff, width*coeff, height*coeff);
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
                graph.DrawEllipse(gizmoPen, (moveCursor.X - radius)*coeff-xoff, (moveCursor.Y - radius)*coeff-yoff, radius * 2*coeff, radius * 2*coeff);
                graph.FillPie(SectorBrush, (moveCursor.X - radius)*coeff-xoff, (moveCursor.Y - radius)*coeff-yoff, radius * 2*coeff, radius * 2*coeff, f1, rotationAngle);
                graph.DrawString(Convert.ToString(Math.Round(rotationAngle, 2)), new Font("Times New Roman", 10), TextBrush, moveCursor.X*coeff-xoff, moveCursor.Y*coeff-yoff);
            }
            moveCursor.DrawXCursor(graph, xoff, yoff, coeff);
            xScaleR.DrawXCursor(graph, xoff, yoff, coeff);
            xScaleL.DrawXCursor(graph, xoff, yoff, coeff);
            yScaleU.DrawXCursor(graph, xoff, yoff, coeff);
            yScaleD.DrawXCursor(graph, xoff, yoff, coeff);
            xyScaleUR.DrawXCursor(graph, xoff, yoff, coeff);
            rotationCursor.DrawXCursor(graph, xoff, yoff, coeff);
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
}
