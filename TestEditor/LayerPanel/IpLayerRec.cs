using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    class IpLayerRec
    {
        public int sizeX { get; private set; }
        public int sizeY { get; set; }
        public int yPos { get; set; }
        private Pen cursorPen;
        private Pen bPen;
        private SolidBrush brush;
        private SolidBrush layerSelected;
        public bool SelectedLayer;
        public bool enable;
        private Font LayerFont;
        private SolidBrush fontBrush;

        public IpCursor2 cursor { get; private set; }
        public IpLayer layer { get; private set; }

        public IpLayerRec(int yPos,int sizeX, int sizeY,SolidBrush brush)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            SelectedLayer = false;
            this.yPos = yPos;
            cursorPen = new Pen(Color.Black);
            enable = true;
            this.brush = brush;
            layerSelected = new SolidBrush(Color.Green);
            bPen = new Pen(Color.Black);
            LayerFont = new Font("Times New Roman", sizeY / 3);
            fontBrush = new SolidBrush(Color.Black);
            layer = new IpLayer();
            cursor = new IpCursor2("LayerBar\\cursor.png", sizeX - sizeY / 3 * 2, yPos + sizeY / 4, sizeY / 2, sizeY / 2);
        }

        public void DrawLayer(Graphics graph)
        {
            if (enable)
            {
                graph.DrawRectangle(bPen, 0, yPos, sizeX - bPen.Width, sizeY - bPen.Width);
                if (SelectedLayer)
                    graph.FillRectangle(layerSelected, 0, yPos, sizeX - bPen.Width, sizeY - bPen.Width);
                else
                    graph.FillRectangle(brush, 0, yPos, sizeX - bPen.Width, sizeY - bPen.Width);
                graph.DrawString(layer.name, LayerFont, fontBrush, 0, yPos + sizeY / 4);
                if (layer.active)
                    cursor.LoadImage("LayerBar\\eye.png");
                else
                    cursor.LoadImage("LayerBar\\eye_hided.png");
                cursor.x = sizeX - sizeY / 3 * 2;
                cursor.y = yPos + sizeY / 4;
                cursor.DrawCursor(graph);
            }
        }

        public bool InRectangleArea(float x, float y)
        {
            if (x >= 0 && x<= sizeX - bPen.Width && y>= yPos && y<= yPos+sizeY-bPen.Width && enable)
                return true;
            return false;
        }
    }
}
