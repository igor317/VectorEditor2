using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    public struct IpPoint
    {
        public float x;
        public float y;

        public IpPoint(float x, float y) : this()
        {
            this.x = x;
            this.y = y;
        }
    }

    public class IpCursor
    {
        #region VARIBLES
        private IpPoint ipPoint;
        private float size;
        private bool enableCursor = true;
        private Pen pen;
        #endregion

        #region SET&GET METHODS
        public Pen Pen
        {
            get { return pen; }
            set { pen = value; }
        }
        public bool ShowCursor
        {
            get { return enableCursor; }
            set { enableCursor = value; }
        }
        public float Size
        {
            get { return size; }
            set { size = value; }
        }
        public float X
        {
            get { return ipPoint.x; }
            set{ ipPoint.x = value; }
        }
        public float Y
        {
            get { return ipPoint.y; }
            set { ipPoint.y = value; }
        }
        #endregion

        #region PUBLIC METHODS
        public IpCursor()
        {
            ShowCursor = false;
            Size = 5;
            Pen = new Pen(Color.Red);
            X = 0;
            Y = 0;
        }

        public IpCursor(int size,Pen pen)
        {
            ShowCursor = true;
            Size = size;
            Pen = pen;
            X = 0;
            Y = 0;
        }

        public bool InCursorArea(float x,float y,float xOffset,float yOffset,float scale)
        {
            if (!enableCursor)
                return false;
            if (x >= X * scale - size - xOffset && x <= X * scale + size - xOffset
                && y >= Y * scale - size - yOffset && y <= Y * scale + size - yOffset)
                return true;
            return false;
        }

        public void DrawXCursor(Graphics graph,float xOffset,float yOffset,float scale)
        {
            if (enableCursor)
            {
                graph.DrawLine(Pen, X * scale - Size - xOffset,
                                    Y * scale - yOffset,
                                    X * scale + Size - xOffset,
                                    Y * scale - yOffset);

                graph.DrawLine(Pen, X * scale - xOffset,
                                    Y * scale - Size - yOffset,
                                    X * scale - xOffset,
                                    Y * scale + Size - yOffset);
            }
        }
        #endregion

        public static bool operator !=(IpCursor c1, IpCursor c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null))
                return false;
            else
                if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null))
                    return true;
            if (c1.X != c2.X && c1.Y != c2.Y)
                return true;
            return false;
        }
        public static bool operator ==(IpCursor c1, IpCursor c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null))
                return true;
            if ((object)c2 == null && (object)c1 == null)
                return true;
            if (c1.X == c2.X && c1.Y != c2.Y)
                return true;
            return false;
        }
    }
}
