﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    class SelectRect
    {
        private float x1;
        private float y1;
        private float x2;
        private float y2;
        private float width;
        private float height;
        private SolidBrush brush;
        private IpPicture pic;

        private void ClearSelectionArray()
        {
            for (int i = 0; i < pic.CounterLines; ++i)
                pic.Lines[i].selected = false;
            for (int i = 0; i < pic.CounterEllipses; ++i)
                pic.Ellipses[i].selected = false;
            for (int i = 0; i < pic.CounterSplines; ++i)
                pic.Splines[i].selected = false;
        }

        public void ResetRect(bool resetSelection)
        {
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
            width = 0;
            height = 0;
            if (resetSelection)
                ClearSelectionArray();
        }

        public SelectRect(IpPicture pic,SolidBrush brush)
        {
            x1 = 0;
            y1 = 0;
            x2 = 0;
            y2 = 0;
            width = 0;
            height = 0;
            this.brush = brush;
            this.pic = pic;
        }

        public void DrawSelectionRectangle(Graphics graph,IpCursor selectCursor,IpCursor lastCursor)
        {
            if (selectCursor.X >= lastCursor.X && selectCursor.Y >= lastCursor.Y) // Правый нижний
            {
                x1 = lastCursor.X * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset;
                y1 = lastCursor.Y * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset;
                x2 = selectCursor.X * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset;
                y2 = selectCursor.Y * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset;
            }
            if (selectCursor.X <= lastCursor.X && selectCursor.Y >= lastCursor.Y) // Левый нижний
            {
                x1 = selectCursor.X * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset;
                y1 = lastCursor.Y * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset;
                x2 = lastCursor.X * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset;
                y2 = selectCursor.Y * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset;

            }
            if (selectCursor.X >= lastCursor.X && selectCursor.Y <= lastCursor.Y) // Правый верхний
            {
                x1 = lastCursor.X * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset;
                y1 = selectCursor.Y * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset;
                x2 = selectCursor.X * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset;
                y2 = lastCursor.Y * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset;
            }
            if (selectCursor.X <= lastCursor.X && selectCursor.Y <= lastCursor.Y) // Левый верхний
            {
                x1 = selectCursor.X * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset;
                y1 = selectCursor.Y * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset;
                x2 = lastCursor.X * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset;
                y2 = lastCursor.Y * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset;
            }
            width = x2 - x1;
            height = y2 - y1;

            graph.FillRectangle(brush, x1, y1, width, height);
        }

        public void SelectLines()
        {
            ClearSelectionArray();
            AddLinesToSelection(true);

        }
        public void AddLinesToSelection(bool add)
        {
            for (int i = 0; i < pic.CounterLines + 1; ++i)
            {
                if (pic.Lines[i].x1 * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset >= x1 && pic.Lines[i].x1 * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset <= x2
                    && pic.Lines[i].y1 * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset >= y1 && pic.Lines[i].y1 * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset <= y2
                    && pic.Lines[i].x2 * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset >= x1 && pic.Lines[i].x2 * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset <= x2
                    && pic.Lines[i].y2 * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset >= y1 && pic.Lines[i].y2 * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset <= y2)
                {
                    //if (pic.Layers[pic.Lines[i].layer].active)
                        pic.Lines[i].selected = add;
                }
            }
            for (int i = 0; i < pic.CounterEllipses + 1; ++i)
            {
                if (pic.Ellipses[i].x1 * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset >= x1 && pic.Ellipses[i].x1 * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset <= x2
                    && pic.Ellipses[i].y1 * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset >= y1 && pic.Ellipses[i].y1 * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset <= y2
                    && pic.Ellipses[i].x2 * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset >= x1 && pic.Ellipses[i].x2 * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset <= x2
                    && pic.Ellipses[i].y2 * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset >= y1 && pic.Ellipses[i].y2 * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset <= y2)
                {
                   // if (pic.Layers[pic.Ellipses[i].layer].active)
                        pic.Ellipses[i].selected = add;
                }
            }
            for (int i = 0; i < pic.CounterSplines + 1; ++i)
            {
                if (pic.Splines[i].xSmin * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset >= x1 && pic.Splines[i].xSmin * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset <= x2
                    && pic.Splines[i].ySmin * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset >= y1 && pic.Splines[i].ySmin * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset <= y2
                    && pic.Splines[i].xSmax * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset >= x1 && pic.Splines[i].xSmax * pic.ViewBox.scaleCoefficient - pic.ViewBox.xOffset <= x2
                    && pic.Splines[i].ySmax * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset >= y1 && pic.Splines[i].ySmax * pic.ViewBox.scaleCoefficient - pic.ViewBox.yOffset <= y2)
                {
                   // if (pic.Layers[pic.Splines[i].layer].active)
                        pic.Splines[i].selected = add;
                }
            }
        }
    }
}
