using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TestEditor
{
    class IpHorizontalScroll:IpScroll
    {
        public event IpEventScrollHandler IpScroll;
        public event IpEventScrollHandler IpMouseUp;
        public event IpEventScrollHandler IpMouseDown;

        #region SET&GET METHODS
        public int Value
        {
            set
            {
                if (value > minValue && value < maxValue)
                {
                    this.value = value;
                    Offset = (value - minValue) * width / (maxValue - minValue);
                }
                else
                {
                    if (value <= minValue)
                    {
                        this.value = minValue;
                        Offset = 0;
                    }
                    if (value >= maxValue - lenght)
                    {
                        this.value = maxValue - lenght;
                        Offset = width - lenght * width / (maxValue - minValue);
                    }
                }

            }
            get { return value; }
        }

        #endregion

        #region PRIVATE METHODS
        protected override bool ScrollCheck(float x, float y)
        {
            if (Enable)
            {
                if (y >= this.y && y <= this.y + height
                    && x >= Offset && x <= Offset + lenght * width / (maxValue - minValue))
                {
                    offmouse = x - Offset;
                    return true;
                }
            }
            return false;
        }

        protected override void Scroll(int pos)
        {
            if (Enable)
            {
                if (pos - offmouse >= this.x && pos - offmouse + lenght * width / (maxValue - minValue) <= this.x + width)
                {
                    Offset = pos - offmouse;
                    value = Convert.ToInt16(minValue + (Offset - x) / (width) * (maxValue - minValue));
                }
                if (pos - offmouse <= this.x)
                {
                    Offset = this.x;
                    value = minValue;
                }
                if (pos - offmouse + lenght * width / (maxValue - minValue) > this.x + width)
                {
                    Offset = this.x + width - lenght * width / (maxValue - minValue);
                    value = maxValue - lenght;
                }
            }
        }

        private void Holst_MouseDown(object sender, MouseEventArgs e)
        {
            if (ScrollCheck(e.X, e.Y))
            {
                inSelect = true;
                DrawRectangle();
                if (IpMouseDown != null)
                    IpMouseDown(this, new IpScrollEventArgs(x, y, width, height, value, maxValue, maxValue));
            }
        }

        private void Holst_MouseUp(object sender, MouseEventArgs e)
        {
            inSelect = false;
            DrawRectangle();
            if (IpMouseUp != null)
                IpMouseUp(this, new IpScrollEventArgs(x, y, width, height, value, maxValue, maxValue));
        }

        private void Holst_MouseMove(object sender, MouseEventArgs e)
        {
            if (inSelect)
            {
                Scroll(e.X);
                DrawRectangle();
                if (IpScroll != null)
                    IpScroll(this, new IpScrollEventArgs(x, y, width, height, value, maxValue, maxValue));
            }
        }
        private void Holst_Paint(object sender, PaintEventArgs e)
        {
            DrawRectangle();
        }
        #endregion

        #region PUBLIC METHODS
        public IpHorizontalScroll(Control holst, int x, int y, int width, int height, int minValue, int maxValue, int lenght) : base(holst, x, y, width, height, minValue, maxValue, lenght)
        {
            Offset = this.x;

            holst.MouseDown += Holst_MouseDown;
            holst.MouseUp += Holst_MouseUp;
            holst.MouseMove += Holst_MouseMove;
            holst.Paint += Holst_Paint;

            DrawRectangle();
        }

        public override void DrawRectangle()
        {
            gbuff.FillRectangle(BackBrush, x, y, width, height);
            if (Enable)
                gbuff.FillRectangle(ScrollBrush, Offset, y, lenght * width / (maxValue - minValue), height);          // Каретка
            else
                gbuff.FillRectangle(ScrollBrushDisabled, Offset, y, width * width / (maxValue - minValue), height);  // Каретка
            graph.DrawImageUnscaled(bmp, 0, 0);
        }

        #endregion
    }
}
