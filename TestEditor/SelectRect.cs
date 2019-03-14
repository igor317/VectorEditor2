using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    public struct SelectRect
    {
        public float x1;
        public float y1;
        public float x2;
        public float y2;
        public float width;
        public float height;
        public SolidBrush brush;

        public SelectRect(SolidBrush brush)
        {
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
            width = 0;
            height = 0;
            this.brush = brush;
        }

        public void ResetSelectRect()
        {
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
            width = 0;
            height = 0;
        }
    }
}
