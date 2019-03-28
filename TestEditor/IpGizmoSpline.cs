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
        private IpCursor rotationCursor;
        private Pen controllerPen;
        private Pen selectedControllerPen;
        private SolidBrush sectorBrush;
        private SolidBrush textBrush;

        private bool dragSelected = false;
        private bool p1CursorSelected = false;
        private bool p2CursorSelected = false;
        private bool p3CursorSelected = false;
        private bool p4CursorSelected = false;
        private bool rotateCursorSelected = false;

        private bool showRotationTrack;
        private float radius;
        private float cursorAngle = 0;
        private float frsAngle = 0;
        private float rotationAngle;

        #endregion

        #region PRIVATE METHODS
        private void CreateGizmo()
        {
            FindPoints();
            showGizmo = true;
            frsAngle = (float)Math.PI;
            cursorAngle = (float)Math.PI;
            radius = (float)Math.Sqrt((pic.Splines[index].xSmax - pic.Splines[index].xSmin) / 2 * (pic.Splines[index].xSmax - pic.Splines[index].xSmin) / 2
                + (pic.Splines[index].ySmax - pic.Splines[index].ySmin) / 2 * (pic.Splines[index].ySmax - pic.Splines[index].ySmin) / 2);
            DefaultControllerPosition(true, true);
            CalculateNormals();
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

        private void DefaultControllerPosition(bool resetCenter, bool resetRotator)
        {
            if (resetCenter)
            {
                moveCursor.X = Math.Abs(pic.Splines[index].xSmin + pic.Splines[index].xSmax) / 2;
                moveCursor.Y = Math.Abs(pic.Splines[index].ySmin + pic.Splines[index].ySmax) / 2;
            }

            p1Cursor.X = pic.Splines[index].x1;
            p1Cursor.Y = pic.Splines[index].y1;
            p2Cursor.X = pic.Splines[index].x2;
            p2Cursor.Y = pic.Splines[index].y2;
            p3Cursor.X = pic.Splines[index].x3;
            p3Cursor.Y = pic.Splines[index].y3;
            p4Cursor.X = pic.Splines[index].x4;
            p4Cursor.Y = pic.Splines[index].y4;
            rotationCursor.X = moveCursor.X + radius * (float)Math.Sin(cursorAngle);
            rotationCursor.Y = moveCursor.Y + radius * (float)Math.Cos(cursorAngle);
            if (resetRotator)
            {
                frsAngle = (float)Math.PI;
                cursorAngle = (float)Math.PI;
                rotationCursor.X = moveCursor.X + radius * (float)Math.Sin(cursorAngle);
                rotationCursor.Y = moveCursor.Y + radius * (float)Math.Cos(cursorAngle);
                radius = (float)Math.Sqrt((pic.Splines[index].xSmax- pic.Splines[index].xSmin) / 2 * (pic.Splines[index].xSmax - pic.Splines[index].xSmin) / 2
                    + (pic.Splines[index].ySmax - pic.Splines[index].ySmin) / 2 * (pic.Splines[index].ySmax - pic.Splines[index].ySmin) / 2);
                CalculateNormals();
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

        private void CalculateNormals()
        {
            pic.Splines[index].CalculateRotationAxes(moveCursor.X, moveCursor.Y);
        }

        private void CalculateAngles(float xPos, float yPos)
        {
            float w = (moveCursor.X * pic.ScaleCoefficient - pic.XOffset) - xPos;
            float h = (moveCursor.Y * pic.ScaleCoefficient - pic.YOffset) - yPos;
            cursorAngle = (float)Math.Atan2(-w, -h);
            if (grid.EnableRotationGrid)
                cursorAngle = grid.GridRotation(cursorAngle);
        }

        private void RotateCursor()
        {
            rotationCursor.X = moveCursor.X + radius * (float)Math.Sin(cursorAngle);
            rotationCursor.Y = moveCursor.Y + radius * (float)Math.Cos(cursorAngle);
            pic.Splines[index].RotateSpline(cursorAngle - (float)Math.PI);
        }

        private void MoveCenterPoint(int xPos, int yPos)
        {
            grid.MoveCursor(moveCursor, xPos, yPos, false, null);
            DefaultControllerPosition(false, true);
            CalculateNormals();
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
            pic.Splines[index].SetCenterPoint(moveCursor.X, moveCursor.Y);
            DefaultControllerPosition(false,false);
        }

        private void MovePoint1(float xPos, float yPos)
        {
            float k1 = xPos;
            float k2 = yPos;
            float m1 = p1Cursor.X;
            float m2 = p1Cursor.Y;

            grid.MoveCursor(p1Cursor, xPos, yPos, true, p2Cursor);
            k1 = p1Cursor.X;
            k2 = p1Cursor.Y;

            pic.Splines[index].x1 += k1 - m1;
            pic.Splines[index].y1 += k2 - m2;
            pic.Splines[index].CalculatePoints();
            DefaultControllerPosition(true, true);
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
            DefaultControllerPosition(true, true);
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
            DefaultControllerPosition(true, true);
        }

        private void MovePoint4(float xPos, float yPos)
        {
            float k1 = xPos;
            float k2 = yPos;
            float m1 = p4Cursor.X;
            float m2 = p4Cursor.Y;

            grid.MoveCursor(p4Cursor, xPos, yPos, true, p3Cursor);
            k1 = p4Cursor.X;
            k2 = p4Cursor.Y;

            pic.Splines[index].x4 += k1 - m1;
            pic.Splines[index].y4 += k2 - m2;
            pic.Splines[index].CalculatePoints();
            DefaultControllerPosition(true, true);
        }

        #endregion

        #region PUBLIC METHODS
        public GizmoSpline(IpPicture pic, IpGrid grid) : base(pic, grid)
        {
            sectorBrush = new SolidBrush(Color.FromArgb(50, 150, 0, 150));
            textBrush = new SolidBrush(Color.Black);
            gizmoPen = new Pen(Color.Gray);
            controllerPen = new Pen(Color.Blue);
            selectedControllerPen = new Pen(Color.Violet);
            moveCursor = new IpCursor(5, controllerPen);
            p1Cursor = new IpCursor(5, controllerPen);
            p2Cursor = new IpCursor(5, controllerPen);
            p3Cursor = new IpCursor(5, controllerPen);
            p4Cursor = new IpCursor(5, controllerPen);
            rotationCursor = new IpCursor(10, controllerPen);
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
            rotationCursor.X = moveCursor.X;
            rotationCursor.Y = pic.Splines[index].ySmin - 25;

            frsAngle = (float)Math.PI;
            cursorAngle = frsAngle;
            radius = (float)Math.Sqrt((pic.Splines[index].xSmax - pic.Splines[index].xSmin) / 2 * (pic.Splines[index].xSmax - pic.Splines[index].xSmin) / 2
                + (pic.Splines[index].ySmax - pic.Splines[index].ySmin) / 2 * (pic.Splines[index].ySmax - pic.Splines[index].ySmin) / 2);
            CalculateNormals();
        }

        public override void DrawGizmo(Graphics graph)
        {
            if (showRotationTrack)
            {

                float f = 180 - frsAngle.Rad2Deg();
                float k = 180 - cursorAngle.Rad2Deg();
                float l = ((k - f) > 0) ? k - f : 360 - Math.Abs(k - f);
                float f1 = 0;
                if (l <= 180)
                {
                    f1 = f - 90;
                    rotationAngle = l;
                }
                else
                {
                    f1 = k - 90;
                    rotationAngle = 360 - l;
                }
                graph.DrawEllipse(gizmoPen, (moveCursor.X - radius) * pic.ScaleCoefficient - pic.XOffset, (moveCursor.Y - radius) * pic.ScaleCoefficient - pic.YOffset, radius * 2 * pic.ScaleCoefficient, radius * 2 * pic.ScaleCoefficient);
                graph.FillPie(sectorBrush, (moveCursor.X - radius) * pic.ScaleCoefficient - pic.XOffset, (moveCursor.Y - radius) * pic.ScaleCoefficient - pic.YOffset, radius * 2 * pic.ScaleCoefficient, radius * 2 * pic.ScaleCoefficient, f1, rotationAngle);
                graph.DrawString(Convert.ToString(Math.Round(rotationAngle, 2)), new Font("Times New Roman", 10 * pic.ScaleCoefficient), textBrush, moveCursor.X * pic.ScaleCoefficient - pic.XOffset, moveCursor.Y * pic.ScaleCoefficient - pic.YOffset);
            }
            moveCursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
            rotationCursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
            if (showGizmo)
            {

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
            {
                if (mCenterPoint)
                    MoveCenterPoint(xPos, yPos);
                else
                    MoveSelected(xPos, yPos);
                return;
            }
            if (p1CursorSelected)
            {
                MovePoint1(xPos, yPos);
                return;
            }
            if (p2CursorSelected)
            {
                MovePoint2(xPos, yPos);
                return;
            }
            if (p3CursorSelected)
            {
                MovePoint3(xPos, yPos);
                return;
            }
            if (p4CursorSelected)
            {
                MovePoint4(xPos, yPos);
                return;
            }
            if (rotateCursorSelected)
            {
                CalculateAngles(xPos, yPos);
                RotateCursor();
                showGizmo = false;
                showRotationTrack = true;
                //ShowElements(true, false, false, false, false, false, true);
                return;
            }
        }

        public override void CheckSelectedController(int xPos, int yPos)
        {
            dragSelected = ReDrawController(moveCursor, xPos, yPos);
            p1CursorSelected = ReDrawController(p1Cursor, xPos, yPos);
            p2CursorSelected = ReDrawController(p2Cursor, xPos, yPos);
            p3CursorSelected = ReDrawController(p3Cursor, xPos, yPos);
            p4CursorSelected = ReDrawController(p4Cursor, xPos, yPos);
            rotateCursorSelected = ReDrawController(rotationCursor, xPos, yPos);
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
            if (rotateCursorSelected)
            {
                DefaultControllerPosition(false, false);
                showGizmo = true;
                showRotationTrack = false;
                frsAngle = cursorAngle;
                rotateCursorSelected = ResetControl(rotationCursor);
                //ShowElements(true, true, true, true, true, true, true);
            }
        }

        public override void MirrorSelectedX()
        {
            pic.Splines[index].MirrorX(moveCursor);
            CreateGizmo();
        }
        public override void MirrorSelectedY()
        {
            pic.Splines[index].MirrorY(moveCursor);
            CreateGizmo();
        }

        #endregion

    }
}
