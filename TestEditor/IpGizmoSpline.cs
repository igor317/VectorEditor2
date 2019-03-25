using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    class GizmoSpline : IpGizmo
    {
        #region VARIABLES
        private int index;
        private Pen gizmoPen;
        private bool showGizmo;
        private IpCursor moveCursor;
        private IpCursor p1Cursor;
        private IpCursor p2Cursor;
        private IpCursor p3Cursor;
        private IpCursor p4Cursor;
        private Pen controllerPen;
        private Pen selectedControllerPen;

        private bool dragSelected = false;
        private bool p1CursorSelected = false;
        private bool p2CursorSelected = false;
        private bool p3CursorSelected = false;
        private bool p4CursorSelected = false;
        #endregion

        #region PRIVATE METHODS
        private void CreateGizmo()
        {
            FindPoints();
            showGizmo = true;
            DefaultControllerPosition();
        }

        private void FindPoints()
        {
            for (int i = 0; i < pic.CounterSplines; ++i)
            {
                if (pic.Splines[i].selected)
                {
                    index = i;
                    return;
                }
            }
        }

        private bool ReDrawController(IpCursor cursor, int xPos, int yPos)
        {
            if (cursor.InCursorArea(xPos, yPos, pic.XOffset, pic.YOffset, pic.ScaleCoefficient))
            {
                cursor.Pen = selectedControllerPen;
                return true;
            }
            cursor.Pen = controllerPen;
            return false;
        }

        private bool ResetControl(IpCursor cursor)
        {
            cursor.Pen = controllerPen;
            return false;
        }

        private void MoveSelected(float xPos, float yPos)
        {
            float k1 = xPos;
            float k2 = yPos;
            float m1 = moveCursor.X;
            float m2 = moveCursor.Y;
            IpCursor pos = new IpCursor();

            grid.MoveCursor(pos, xPos, yPos, true, null);
            k1 = pos.X;
            k2 = pos.Y;
            grid.MoveCursor(moveCursor, xPos, yPos, true, null);


            pic.Splines[index].x1 += k1 - m1;
            pic.Splines[index].y1 += k2 - m2;
            pic.Splines[index].x2 += k1 - m1;
            pic.Splines[index].y2 += k2 - m2;
            pic.Splines[index].x3 += k1 - m1;
            pic.Splines[index].y3 += k2 - m2;
            pic.Splines[index].x4 += k1 - m1;
            pic.Splines[index].y4 += k2 - m2;
            pic.Splines[index].CalculatePoints();
            DefaultControllerPosition();
        }

        private void MovePoint1(float xPos, float yPos)
        {
            float k1 = xPos;
            float k2 = yPos;
            float m1 = p1Cursor.X;
            float m2 = p1Cursor.Y;

            grid.MoveCursor(p1Cursor, xPos, yPos, true, moveCursor);
            k1 = p1Cursor.X;
            k2 = p1Cursor.Y;

            pic.Splines[index].x1 += k1 - m1;
            pic.Splines[index].y1 += k2 - m2;
            pic.Splines[index].CalculatePoints();
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

            pic.Splines[index].x2 += k1 - m1;
            pic.Splines[index].y2 += k2 - m2;
            pic.Splines[index].CalculatePoints();
            DefaultControllerPosition();
        }

        private void MovePoint3(float xPos, float yPos)
        {
            float k1 = xPos;
            float k2 = yPos;
            float m1 = p3Cursor.X;
            float m2 = p3Cursor.Y;

            grid.MoveCursor(p3Cursor, xPos, yPos, true, p4Cursor);
            k1 = p3Cursor.X;
            k2 = p3Cursor.Y;

            pic.Splines[index].x3 += k1 - m1;
            pic.Splines[index].y3 += k2 - m2;
            pic.Splines[index].CalculatePoints();
            DefaultControllerPosition();
        }

        private void MovePoint4(float xPos, float yPos)
        {
            float k1 = xPos;
            float k2 = yPos;
            float m1 = p4Cursor.X;
            float m2 = p4Cursor.Y;

            grid.MoveCursor(p4Cursor, xPos, yPos, true, moveCursor);
            k1 = p4Cursor.X;
            k2 = p4Cursor.Y;

            pic.Splines[index].x4 += k1 - m1;
            pic.Splines[index].y4 += k2 - m2;
            pic.Splines[index].CalculatePoints();
            DefaultControllerPosition();
        }

        #endregion

        #region PUBLIC METHODS
        public GizmoSpline(IpPicture pic, IpGrid grid) : base(pic, grid)
        {
            gizmoPen = new Pen(Color.Gray);
            controllerPen = new Pen(Color.Blue);
            selectedControllerPen = new Pen(Color.Violet);
            moveCursor = new IpCursor(5, controllerPen);
            p1Cursor = new IpCursor(5, controllerPen);
            p2Cursor = new IpCursor(5, controllerPen);
            p3Cursor = new IpCursor(5, controllerPen);
            p4Cursor = new IpCursor(5, controllerPen);

            CreateGizmo();
        }

        public override void DefaultControllerPosition()
        {
            moveCursor.X = Math.Abs(pic.Splines[index].xSmin + pic.Splines[index].xSmax) / 2;
            moveCursor.Y = Math.Abs(pic.Splines[index].ySmin + pic.Splines[index].ySmax) / 2;
            p1Cursor.X = pic.Splines[index].x1;
            p1Cursor.Y = pic.Splines[index].y1;
            p2Cursor.X = pic.Splines[index].x2;
            p2Cursor.Y = pic.Splines[index].y2;
            p3Cursor.X = pic.Splines[index].x3;
            p3Cursor.Y = pic.Splines[index].y3;
            p4Cursor.X = pic.Splines[index].x4;
            p4Cursor.Y = pic.Splines[index].y4;
        }

        public override void DrawGizmo(Graphics graph)
        {
            if (showGizmo)
            {
                moveCursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                p1Cursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                p2Cursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                p3Cursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                p4Cursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                graph.DrawLine(gizmoPen, pic.Splines[index].x1 * pic.ScaleCoefficient - pic.XOffset, pic.Splines[index].y1 * pic.ScaleCoefficient - pic.YOffset,
                    pic.Splines[index].x2 * pic.ScaleCoefficient - pic.XOffset, pic.Splines[index].y2 * pic.ScaleCoefficient - pic.YOffset);
                graph.DrawLine(gizmoPen, pic.Splines[index].x3 * pic.ScaleCoefficient - pic.XOffset, pic.Splines[index].y3 * pic.ScaleCoefficient - pic.YOffset,
                    pic.Splines[index].x4 * pic.ScaleCoefficient - pic.XOffset, pic.Splines[index].y4 * pic.ScaleCoefficient - pic.YOffset);
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
            if (p3CursorSelected)
                MovePoint3(xPos, yPos);
            if (p4CursorSelected)
                MovePoint4(xPos, yPos);
        }

        public override void CheckSelectedController(int xPos, int yPos)
        {
            dragSelected = ReDrawController(moveCursor, xPos, yPos);
            p1CursorSelected = ReDrawController(p1Cursor, xPos, yPos);
            p2CursorSelected = ReDrawController(p2Cursor, xPos, yPos);
            p3CursorSelected = ReDrawController(p3Cursor, xPos, yPos);
            p4CursorSelected = ReDrawController(p4Cursor, xPos, yPos);
        }

        public override void ResetControllers()
        {
            if (dragSelected)
            {
                dragSelected = ResetControl(moveCursor);
            }
            p1CursorSelected = ResetControl(p1Cursor);
            p2CursorSelected = ResetControl(p2Cursor);
            p3CursorSelected = ResetControl(p3Cursor);
            p4CursorSelected = ResetControl(p4Cursor);
        }

        public override void MirrorSelectedX()
        {

        }
        public override void MirrorSelectedY()
        {

        }

        #endregion

    }
}
