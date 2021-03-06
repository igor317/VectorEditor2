﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    class GizmoLine : IpGizmo
    {
        #region VARIABLES
        private int index = 999999;
        private bool showGizmo;
        private IpCursor moveCursor;
        private IpCursor p1Cursor;
        private IpCursor p2Cursor;
        private Pen controllerPen;
        private Pen selectedControllerPen;
        
        private bool dragSelected = false;
        private bool p1CursorSelected = false;
        private bool p2CursorSelected = false;
        #endregion

        #region PRIVATE METHODS

        private void CreateGizmo()
        {
            FindPoints();
            showGizmo = true;
            DefaultControllerPosition();
        }

        private bool ResetControl(IpCursor cursor)
        {
            cursor.Pen = controllerPen;
            return false;
        }

        private bool ReDrawController(IpCursor cursor, int xPos, int yPos)
        {
            if (cursor.InCursorArea(xPos, yPos, pic.ViewBox.xOffset, pic.ViewBox.yOffset, pic.ViewBox.scaleCoefficient))
            {
                cursor.Pen = selectedControllerPen;
                return true;
            }
            cursor.Pen = controllerPen;
            return false;
        }

        private void MoveSelected(float xPos, float yPos)
        {
            float m1 = moveCursor.X;
            float m2 = moveCursor.Y;
            grid.MoveCursor(moveCursor, xPos, yPos, true, null);
            float k1 = moveCursor.X;
            float k2 = moveCursor.Y;

            pic.Lines[index].x1 += k1 - m1;
            pic.Lines[index].x2 += k1 - m1;
            pic.Lines[index].y1 += k2 - m2;
            pic.Lines[index].y2 += k2 - m2;
            DefaultControllerPosition();
        }

        private void MovePoint1(float xPos,float yPos)
        {
            float k1 = xPos;
            float k2 = yPos;
            float m1 = p1Cursor.X;
            float m2 = p1Cursor.Y;

            grid.MoveCursor(p1Cursor, xPos, yPos, true, p2Cursor);
            k1 = p1Cursor.X;
            k2 = p1Cursor.Y;

            pic.Lines[index].x1 += k1 - m1;
            pic.Lines[index].y1 += k2 - m2;
            DefaultControllerPosition();
        }

        private void MovePoint2(float xPos, float yPos)
        {
            float k1 = xPos;
            float k2 = yPos;
            float m1 = p2Cursor.X;
            float m2 = p2Cursor.Y;

            grid.MoveCursor(p2Cursor, xPos, yPos, true, p1Cursor);
            k1 = p2Cursor.X;
            k2 = p2Cursor.Y;

            pic.Lines[index].x2 += k1 - m1;
            pic.Lines[index].y2 += k2 - m2;
            DefaultControllerPosition();
        }

        private void FindPoints()
        {
            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i].selected)
                {
                    index = i;
                    return;
                }
            }
        }
        #endregion

        #region PUBLIC METHODS
        public GizmoLine(IpPicture pic, IpGrid grid) : base(pic,grid)
        {
            controllerPen = new Pen(Color.Blue);
            selectedControllerPen = new Pen(Color.Violet);
            moveCursor = new IpCursor(5, controllerPen);
            p1Cursor = new IpCursor(5, controllerPen);
            p2Cursor = new IpCursor(5, controllerPen);

            CreateGizmo();
        }

        public override void DefaultControllerPosition()
        {
            moveCursor.X = Math.Abs(pic.Lines[index].x2 + pic.Lines[index].x1) / 2;
            moveCursor.Y = Math.Abs(pic.Lines[index].y2 + pic.Lines[index].y1) / 2;
            p1Cursor.X = pic.Lines[index].x1;
            p1Cursor.Y = pic.Lines[index].y1;
            p2Cursor.X = pic.Lines[index].x2;
            p2Cursor.Y = pic.Lines[index].y2;
        }

        public override void DrawGizmo(Graphics graph)
        {
            if (showGizmo)
            {
                moveCursor.DrawXCursor(graph, pic.ViewBox.xOffset, pic.ViewBox.yOffset, pic.ViewBox.scaleCoefficient);
                p1Cursor.DrawXCursor(graph, pic.ViewBox.xOffset, pic.ViewBox.yOffset, pic.ViewBox.scaleCoefficient);
                p2Cursor.DrawXCursor(graph, pic.ViewBox.xOffset, pic.ViewBox.yOffset, pic.ViewBox.scaleCoefficient);
            }
        }

        public override void Control(int xPos, int yPos)
        {
            if (dragSelected)
                MoveSelected(xPos, yPos);
            if (p1CursorSelected)
                MovePoint1(xPos, yPos);
            if (p2CursorSelected)
                MovePoint2(xPos, yPos);
        }

        public override void CheckSelectedController(int xPos, int yPos)
        {
            dragSelected = ReDrawController(moveCursor, xPos, yPos);
            p1CursorSelected = ReDrawController(p1Cursor, xPos, yPos);
            p2CursorSelected = ReDrawController(p2Cursor, xPos, yPos);
        }

        public override void ResetControllers()
        {
            if (dragSelected)
            {
                dragSelected = ResetControl(moveCursor);
            }
            p1CursorSelected = ResetControl(p1Cursor);
            p2CursorSelected = ResetControl(p2Cursor);
        }

        public override void MirrorSelectedX()
        {
            pic.Lines[index].MirrorX(moveCursor);
            CreateGizmo();
        }
        public override void MirrorSelectedY()
        {
            pic.Lines[index].MirrorY(moveCursor);
            CreateGizmo();
        }

        #endregion
    }
}
