using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TestEditor
{
    class IpScrollEventArgs 
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
        public int Value { get; }
        public int MinValue { get; }
        public int MaxValue { get; }

        public IpScrollEventArgs(int x,int y,int width,int height,int value, int minValue, int maxValue)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Value = value;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }


    abstract class IpScroll
    {
        public delegate void IpEventScrollHandler(object sender, IpScrollEventArgs e);

        public int x { get; }
        public int y { get; }
        public int height { get; }
        public int width { get; }
        protected Graphics graph;
        protected Graphics gbuff;
        protected Bitmap bmp;

        protected float offmouse;
        protected float Offset;

        protected int minValue;
        protected int maxValue;
        protected int value;
        protected int lenght;
        protected bool inSelect = false;

        public SolidBrush BackBrush { get; set; }
        public SolidBrush ScrollBrush { get; set; }
        public SolidBrush ScrollBrushDisabled { set; get; }
        public bool Enable { get; set; }
        public int MinLength { get; set; }

        public int MaxValue
        {
            set
            {
                this.value += maxValue - value;
                maxValue = value;

            }
            get { return maxValue; }
        }
        public int MinValue
        {
            set { minValue = value; }
            get { return minValue; }
        }
        public int Length
        {
            set
            {
                /*if (value <= MinLength)
                    lenght = MinLength;
                else
                {
                    if (value >= (maxValue - minValue) * height)
                        lenght = (maxValue - minValue)*height;
                    else
                    {
                        lenght = value;
                        if (value < 0)
                            lenght = 0;
                    }
                }*/
                lenght = value;
            }
            get { return lenght; }
        }

        protected abstract void Scroll(int pos);
        protected abstract bool ScrollCheck(float x, float y);
        public abstract void DrawRectangle();

        public IpScroll(Control holst, int x, int y, int width, int height, int minValue, int maxValue, int lenght)
        {
            graph = holst.CreateGraphics();
            bmp = new Bitmap(width+x, height+y, graph);
            gbuff = Graphics.FromImage(bmp);

            this.x = x;
            this.y = y;
            this.height = height;
            this.width = width;
            this.minValue = minValue;
            this.maxValue = maxValue;
            MinLength = 20;
            Length = lenght;

            Enable = true;
            BackBrush = new SolidBrush(Color.Wheat);
            ScrollBrush = new SolidBrush(Color.Black);
            ScrollBrushDisabled = new SolidBrush(Color.LightGray);
            //value = minValue;
        }
    }
}
