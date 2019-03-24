using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    public struct GizmoLine
    {
        private Pen gizmoPen;
        public bool showGizmo;
        public IpCursor moveCursor;
        public IpCursor p1Cursor;
        public IpCursor p2Cursor;
        public Pen controllerPen;

        public GizmoLine(float x1, float y1, float x2, float y2) : this()
        {
            showGizmo = true;
            controllerPen = new Pen(Color.Violet);
            moveCursor = new IpCursor(5, controllerPen);
            p1Cursor = new IpCursor(5, controllerPen);
            p2Cursor = new IpCursor(5, controllerPen);
            ResetControllers(true);
        }

        public void ResetControllers(bool resetCenter)
        {

        }

    }
}
