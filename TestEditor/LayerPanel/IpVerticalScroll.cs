using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    class IpVerticalScroll
    {
        #region VARIABLES
        private int sizeX;
        private int sizeY;
        private int xPos;
        private int xlenght;

        private float yoffmouse;
        private float yOffset;

        private int minValue;
        private int maxValue;
        private int value;
        private int height;

        public SolidBrush BackBrush { get; set; }
        public SolidBrush ScrollBrush { get; set; }
        public SolidBrush ScrollBrushDisabled { set; get; }
        public bool Enable { get; set; }
        public int MinHeight { get; set; }
        #endregion

        #region SET&GET METHODS
        public int MaxValue
        {
            set
            {
                maxValue = value;
                yOffset = ((this.value - minValue) * (sizeY - height)) / (maxValue - minValue - height);
                Enable = (maxValue - minValue <= 0) ? false : true;
            }
            get { return maxValue; }
        }
        public int MinValue
        {
            set
            {
                minValue = value;
                yOffset = ((this.value - minValue) * (sizeY - height)) / (maxValue - minValue - height);
                Enable = (maxValue - minValue <= 0) ? false : true;
            }
            get { return minValue; }
        }
        public int Value
        {
            set
            {
                if (value > minValue && value < maxValue)
                {
                    this.value = value;
                    yOffset = ((value - minValue) * (sizeY - height)) / (maxValue - minValue - height);
                }
                else
                {
                    if (value <= minValue)
                    {
                        this.value = minValue;
                        yOffset = 0;
                    }
                    if (value >= maxValue - height)
                    {
                        this.value = maxValue - height;
                        yOffset = sizeY - height;
                    }
                }

            }
            get { return value; }
        }
        public int Height
        {
            set
            {
                if (value < MinHeight)
                    height = MinHeight;
                else
                    height = value;
            }
            get { return height; }
        }
        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PUBLIC METHODS
        public bool ScrollCheck(float x, float y)
        {
            if (Enable)
            {
                if (x >= xPos && x <= sizeX
                    && y >= yOffset && y <= yOffset + height)
                {
                    yoffmouse = y - yOffset;
                    return true;
                }
            }
            return false;
        }

        public IpVerticalScroll(int sizeX,int sizeY,int width,int minValue, int maxValue, int height)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.xlenght = width;
            this.minValue = minValue;
            this.maxValue = maxValue;
            MinHeight = 20;
            if (maxValue - minValue <= 0)
            {
                Enable = false;
                this.height = sizeY;
            }
            else
            {
                Enable = true;
                this.height = height;
            }

            BackBrush = new SolidBrush(Color.Wheat);
            ScrollBrush = new SolidBrush(Color.Black);
            ScrollBrushDisabled = new SolidBrush(Color.LightGray);
            xPos = sizeX - xlenght;
            yOffset = 0;
            value = minValue;

        }

        public void DrawRectangle(Graphics graph)
        {
            graph.FillRectangle(BackBrush, xPos, 0, xlenght, sizeY);
            if (Enable)
                graph.FillRectangle(ScrollBrush, xPos, yOffset, xlenght, height);          // Каретка
            else
                graph.FillRectangle(ScrollBrushDisabled, xPos, yOffset, xlenght, height);  // Каретка
        }



        public void Scroll(int y)
        {
            if (Enable)
            {
                if (y - yoffmouse >= 0 && y - yoffmouse + height <= sizeY && height < sizeY)
                {
                    yOffset = y - yoffmouse;
                    value = Convert.ToInt16(minValue + yOffset/ (sizeY-height)*(maxValue-minValue- height));
                }
                if (y - yoffmouse < 0 && height < sizeY)
                {
                    yOffset = 0;
                    value = minValue;
                }
                if (y - yoffmouse + height >= sizeY && height < sizeY)
                {
                    yOffset = sizeY - height;
                    value = maxValue- height;
                }
            }
        }


        #endregion
    }
}
