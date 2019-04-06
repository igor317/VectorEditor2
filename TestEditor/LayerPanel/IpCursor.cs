using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    class IpCursor2
    {
        #region VARIBLES
        public float x { get; set; }
        public float y { get; set; }

        public float Size { get; protected set; }
        public bool ShowCursor { get; protected set; }
        private Image img;
        private bool loaded;
        private string path;
        private float width;
        private float height;
        private Pen bPen;
        #endregion

        #region PUBLIC METHODS

        public IpCursor2(string path,float x,float y, float width, float height)
        {
            ShowCursor = true;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.path = path;
            LoadImage(path);

        }

        public void LoadImage(string path)
        {
            if (img != null)
                img.Dispose();
            try
            {
                img = Image.FromFile(path);
                bPen = null;
                loaded = true;
            }
            catch(Exception)
            {
                loaded = false;
                bPen = new Pen(Color.Red);
            }

        }

        public bool InCursorArea(float x, float y)
        {
            if (!ShowCursor)
                return false;
            if (x >= this.x && x <= this.x + width
                && y >= this.y && y <= this.y + height)
                return true;
            return false;
        }

        public void DrawCursor(Graphics graph)
        {
            if (ShowCursor)
            {
                if (loaded)
                {
                    graph.DrawImage(img, x, y, width, height);
                }
                else
                {
                    graph.DrawLine(bPen, x, y, x+width, y+height);
                    graph.DrawLine(bPen, x, y + height, x + width, y);
                }
            }
        }

        #endregion
        public static bool operator !=(IpCursor2 c1, IpCursor2 c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null))
                return false;
            else
                if (ReferenceEquals(c1, null) || ReferenceEquals(c2, null))
                return true;
            if (c1.x != c2.x && c1.y != c2.y)
                return true;
            return false;
        }
        public static bool operator ==(IpCursor2 c1, IpCursor2 c2)
        {
            if (ReferenceEquals(c1, null) && ReferenceEquals(c2, null))
                return true;
            if ((object)c2 == null && (object)c1 == null)
                return true;
            if (c1.x == c2.x && c1.y != c2.y)
                return true;
            return false;
        }

    }
}
