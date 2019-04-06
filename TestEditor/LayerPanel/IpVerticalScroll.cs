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
        private bool enableScroll;
        private float yOffset;
        private SolidBrush brush;
        private SolidBrush scrollBrush;
        private SolidBrush scrollBrushDisabled;

        private int minValue;
        private int maxValue;
        private int value;
        private int height;

        private int minHeight = 20;
        #endregion

        #region SET&GET METHODS
        public int MaxValue
        {
            set
            {
                maxValue = value;
                yOffset = ((this.value - minValue) * (sizeY - height)) / (maxValue - minValue - height);
                enableScroll = (maxValue - minValue <= 0) ? false : true;
            }
            get { return maxValue; }
        }
        public int MinValue
        {
            set
            {
                minValue = value;
                yOffset = ((this.value - minValue) * (sizeY - height)) / (maxValue - minValue - height);
                enableScroll = (maxValue - minValue <= 0) ? false : true;
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
                if (value < minHeight)
                    height = minHeight;
                else
                    height = value;
            }
            get { return height; }
        }
        public int MinHeight
        {
            set { minHeight = value; }
            get { return minHeight; }
        }
        public bool Enable
        {
            set { enableScroll = value; }
            get { return enableScroll; }
        }
        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PUBLIC METHODS
        public bool ScrollCheck(float x, float y)
        {
            if (enableScroll)
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

        public IpVerticalScroll(int sizeX,int sizeY,int xlenght,int minValue, int maxValue, int height)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.xlenght = xlenght;
            this.minValue = minValue;
            this.maxValue = maxValue;
            if (maxValue - minValue <= 0)
            {
                enableScroll = false;
                this.height = sizeY;
            }
            else
            {
                enableScroll = true;
                this.height = height;
            }

            brush = new SolidBrush(Color.Wheat);
            scrollBrush = new SolidBrush(Color.Black);
            scrollBrushDisabled = new SolidBrush(Color.LightGray);
            xPos = sizeX - xlenght;
            yOffset = 0;
            value = minValue;

        }

        public void DrawRectangle(Graphics graph)
        {
            graph.FillRectangle(brush, xPos, 0, xlenght, sizeY);
            if (enableScroll)
                graph.FillRectangle(scrollBrush, xPos, yOffset, xlenght, height);          // Каретка
            else
                graph.FillRectangle(scrollBrushDisabled, xPos, yOffset, xlenght, height);  // Каретка
        }



        public void Scroll(int y)
        {
            if (enableScroll)
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
