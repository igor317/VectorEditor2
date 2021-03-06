﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TestEditor
{
    class PanelLayer
    {
        #region VARIABLES
        private Graphics graph;
        private Bitmap bmp;
        private Graphics gbuff;
        private int sizeX, sizeY;
        private Pen bPen = new Pen(Color.Black);
        private SolidBrush whiteBrush = new SolidBrush(Color.White);
        private SolidBrush layerBrush = new SolidBrush(Color.FromArgb(50, 255, 0, 0));

        private int yOffset;
        private int countLayers;
        private int layerSize;
        private int scrollWidth;

        private IpLayerRec[] layerR;
        //public IpCursor2 AddLayerButton { get; private set; }
        private IpVerticalScroll Scroll;
        public IpSettingsBar settingsBar { get; private set; }

        private int maxLayersInPage;

        private int settingsBarSize;
        #endregion

        #region SET&GET METHODS

        public int MaxLayersInPage
        {
            set
            {
                if (value > 0)
                    maxLayersInPage = value;
                else
                    maxLayersInPage = 1;
            }
            get { return maxLayersInPage; }
        }
    

        #endregion

        #region PRIVATE METHODS

        private void DrawLayers()
        {
            for (int i = 0; i < countLayers; ++i)
            {
                layerR[i].enable = (layerR[i].yPos >= -layerSize && layerR[i].yPos <= layerSize * maxLayersInPage-1) ? true : false;
                layerR[i].DrawLayer(gbuff);
            }
        }

        private void AddLayer()
        {
            countLayers++;
            if (layerR == null)
            {
                countLayers = 1;
                layerR = new IpLayerRec[countLayers];
            }
            else
                Array.Resize(ref layerR, countLayers);
            layerR[countLayers - 1] = new IpLayerRec(yOffset, sizeX-scrollWidth, layerSize, new SolidBrush(Color.FromArgb(50, 255, 0, 0)));
            yOffset += layerSize;
            if (countLayers - 1 >= maxLayersInPage)
            {
                Scroll.Enable = true;
                //Scroll.Length = sizeY;
                //Scroll.Value = layerSize * (countLayers- maxLayersInPage);
                Scroll.MaxValue = layerSize * (countLayers - maxLayersInPage);
                //Scroll.Value = 
            }
        }

        private bool CheckAddingButton(int xPos, int yPos)
        {
            if (settingsBar.CheckAddingButton(xPos, yPos))
            {
                AddLayer();
                return true;
            }
            return false;
        }

        private bool SelectLayer(int xPos, int yPos)
        {
            int k = 9999;
            for (int i = 0; i < countLayers; ++i)
            {
                if (layerR[i].InRectangleArea(xPos, yPos) && yPos <= sizeY - settingsBarSize)
                    k = i;
            }
            if (k != 9999)
            {
                for (int i = 0; i < countLayers; ++i)
                {
                    if (i != k)
                        layerR[i].SelectedLayer = false;
                    else
                        layerR[i].SelectedLayer = true;
                }
                return true;
            }
            return false;
        }

        private bool ShowLayer(int xPos, int yPos)
        {
            for (int i = 0; i < countLayers; ++i)
            {
                if (layerR[i].enable && layerR[i].cursor.InCursorArea(xPos, yPos) && yPos < sizeY - settingsBarSize)
                {
                    layerR[i].layer.active = (layerR[i].layer.active) ? false : true;
                    return true;
                }
            }
            return false;
        }

        private void ScrollLayers(int y)
        {
            int k = 0;
            //Scroll.Scroll(y);
            for (int i = 0; i < countLayers; ++i)
            {
                k = i;
                layerR[i].yPos = -Scroll.Value + k * layerSize;
            }
            yOffset = -Scroll.Value + (k + 1) * layerSize;
        }

        private void ControlMouseDown(int xPos, int yPos)
        {
            if (ShowLayer(xPos, yPos))
                return;
            if (SelectLayer(xPos, yPos))
                return;
            if (CheckAddingButton(xPos, yPos))
                return;
        }

        private void LayerPanel_MouseDown(object sender, MouseEventArgs e)
        {
            ControlMouseDown(e.X, e.Y);
            Draw();
        }


        private void LayerPanel_Paint(object sender, PaintEventArgs e)
        {
            Draw();
        }


        private void Scroll_IpScroll(object sender, IpScrollEventArgs e)
        {
            ScrollLayers(e.Y);
            Draw();
        }
        #endregion

        #region PUBLIC METHODS
        public PanelLayer(Panel layerPanel,int maxLayersInPage,int scrollWitdth,int settingbarSize)
        {
            graph = layerPanel.CreateGraphics();
            sizeX = layerPanel.Width;
            sizeY = layerPanel.Height;
            bmp = new Bitmap(sizeX+scrollWidth, sizeY, graph);
            gbuff = Graphics.FromImage(bmp);
            gbuff.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            yOffset = 0;
            countLayers = 0;
            this.scrollWidth = scrollWitdth;
            this.MaxLayersInPage = maxLayersInPage;
            this.settingsBarSize = settingbarSize;

            layerSize = Convert.ToInt16(sizeY / maxLayersInPage - settingsBarSize / maxLayersInPage);
            Scroll = new IpVerticalScroll(layerPanel, sizeX- scrollWidth, 0, scrollWidth, sizeY-settingbarSize-(int)bPen.Width, 0, 100, 10);
            settingsBar = new IpSettingsBar(sizeX, sizeY, settingsBarSize);
            Scroll.Enable = false;
            AddLayer();
            layerR[0].SelectedLayer = true;

            layerPanel.MouseDown += LayerPanel_MouseDown;
            layerPanel.Paint += LayerPanel_Paint;
            Scroll.IpScroll += Scroll_IpScroll;
        }


        public void Draw()
        {
            gbuff.FillRectangle(whiteBrush, 0, 0, sizeX-scrollWidth-bPen.Width, sizeY);
            DrawLayers();
            settingsBar.DrawBar(gbuff);
            graph.DrawImageUnscaled(bmp, 0, 0);
        }
        #endregion
    }
}
