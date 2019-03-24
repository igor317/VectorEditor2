using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    class GizmoLine
    {
        private IpPicture pic;
        private IpGrid grid;

        private float x1;
        private float y1;
        private float x2;
        private float y2;
        private Pen gizmoPen;
        private bool showGizmo;
        private IpCursor moveCursor;
        private IpCursor p1Cursor;
        private IpCursor p2Cursor;
        private Pen controllerPen;


        private void FindPoints()
        {
            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                {
                    x1 = pic.Lines[i].x1;
                    y1 = pic.Lines[i].y1;
                    x2 = pic.Lines[i].x2;
                    y2 = pic.Lines[i].y2;
                    return;
                }
            }
        }

        public GizmoLine(IpPicture pic, IpGrid grid)
        {
            this.pic = pic;
            this.grid = grid;
            gizmoPen = new Pen(Color.Green);
            controllerPen = new Pen(Color.Blue);
            moveCursor = new IpCursor(5, controllerPen);
            p1Cursor = new IpCursor(5, controllerPen);
            p2Cursor = new IpCursor(5, controllerPen);

            CreateGizmo();
        }

        public void CreateGizmo()
        {
            FindPoints();
            showGizmo = true;
            DefaultControllerPosition(true);
        }

        public void DefaultControllerPosition(bool resetCenter)
        {
            if (resetCenter)
            {
                moveCursor.X = Math.Abs(x2 + x1) / 2;
                moveCursor.Y = Math.Abs(y2 + y1) / 2;
            }
            p1Cursor.X = x1;
            p1Cursor.Y = y1;
            p2Cursor.X = x2;
            p2Cursor.Y = y2;
        }

        public void DrawGizmo(Graphics graph)
        {
            if (showGizmo)
            {
                moveCursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                p1Cursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                p2Cursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
            }
        }

        public void Reset()
        {
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
            DefaultControllerPosition(true);
        }
    }
}
