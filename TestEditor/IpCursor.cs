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
            if (x >= (X - size)*scale-xOffset && x <= (X + size)*scale-xOffset 
                && y >= (Y - size)*scale-yOffset && y <= (Y + size)*scale-yOffset)
                return true;
            return false;
        }

        public void DrawXCursor(Graphics graph,float xOffset,float yOffset,float scale)
        {
            if (enableCursor)
            {
                graph.DrawLine(Pen, (X - Size)*scale - xOffset, Y*scale - yOffset, (X + Size)*scale - xOffset, Y*scale - yOffset);
                graph.DrawLine(Pen, X*scale - xOffset, (Y - Size)*scale - yOffset, X*scale - xOffset, (Y + Size)*scale - yOffset);
                //graph.DrawLine(Pen, X - Size, Y, X + Size, Y);
                //graph.DrawLine(Pen, X, Y - Size, X, Y + Size);
            }
        }
        #endregion

        public static bool operator !=(IpCursor c1, IpCursor c2)
        {
            if (c1.X != c2.X && c1.Y != c2.Y)
                return true;
            return false;
        }
        public static bool operator ==(IpCursor c1, IpCursor c2)
        {
            if (c1.X == c2.X && c1.Y != c2.Y)
                return true;
            return false;
        }
    }
}
