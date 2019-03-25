using System;
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
        private int index;
        private Pen gizmoPen;
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
            if (cursor.InCursorArea(xPos, yPos, pic.XOffset, pic.YOffset, pic.ScaleCoefficient))
            {
                cursor.Pen = selectedControllerPen;
                return true;
            }
            cursor.Pen = controllerPen;
            return false;
        }

        private void MoveSelectedLines(float xPos, float yPos)
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

            pic.Lines[index].x1 += k1 - m1;
            pic.Lines[index].x2 += k1 - m1;
            pic.Lines[index].y1 += k2 - m2;
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
            gizmoPen = new Pen(Color.Green);
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
                moveCursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                p1Cursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
                p2Cursor.DrawXCursor(graph, pic.XOffset, pic.YOffset, pic.ScaleCoefficient);
            }
        }

        public override void Control(int xPos, int yPos)
        {
            if (dragSelected)
                MoveSelectedLines(xPos, yPos);
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

        }
        public override void MirrorSelectedY()
        {

        }

        #endregion
    }
}
