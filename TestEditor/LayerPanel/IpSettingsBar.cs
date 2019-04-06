using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    class IpSettingsBar
    {
        private int sizeY;
        private int sizeX;
        private int height;
        private Pen bPen;
        private SolidBrush wBrush;

        public IpCursor2 addLayerButton { get; private set; }

        public IpSettingsBar(int sizeX,int sizeY,int height)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.height = height;
            bPen = new Pen(Color.Black);
            wBrush = new SolidBrush(Color.White);

            addLayerButton = new IpCursor2("LayerBar\\plus.png", sizeX - height-bPen.Width , sizeY - height, height, height);
        }

        public void DrawBar(Graphics graph)
        {
            graph.DrawRectangle(bPen, 0, sizeY - height - bPen.Width, sizeX - bPen.Width, height);
            graph.FillRectangle(wBrush, 0, sizeY - height - bPen.Width, sizeX - bPen.Width, height);
            addLayerButton.DrawCursor(graph);
        }

        public bool CheckAddingButton(int xPos, int yPos)
        {
            if (addLayerButton.InCursorArea(xPos, yPos))
            {
                return true;
            }
            return false;
            //AddLayer();
        }
    }
}
