using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    class GizmoEditor
    {
        #region VARIABLES
        private Pen selectionPen = new Pen(Color.Red);
        private IpGizmo gizmo;
        private SelectRect selectRect;
        private IpGrid grid;
        private IpPicture pic;
        #endregion

        #region GET&GET METHODS
        public Pen SelectionPen
        {
            get { return selectionPen; }
        }
        public SelectRect SelectRect
        {
            get { return selectRect; }
        }
        #endregion

        #region PRIVATE METHODS
        private void DefineGizmoMode()
        {
            int countSelectedLines = 0;
            int countSelectedEllipses = 0;
            for (int i = 0; i < pic.CounterLines; ++i)
                if (pic.Lines[i].selected)
                    countSelectedLines++;
            for (int i = 0; i < pic.CounterEllipses; ++i)
                if (pic.Ellipses[i].selected)
                    countSelectedEllipses++;
            if ((countSelectedLines == 1 && countSelectedEllipses == 1) || countSelectedLines > 1 || countSelectedEllipses > 1) // GIZMO MIXED
            {
                gizmo = new GizmoMixed(pic, grid);
                return;
            }
            if (countSelectedLines == 1 && countSelectedEllipses == 0) // GIZMO LINE
            {
                gizmo = new GizmoLine(pic, grid);
                return;
            }
            if (countSelectedLines == 0 && countSelectedEllipses == 1)  // GIZMO ELLIPSE
            {
                return;
            }

        }
        #endregion

        #region PUBLIC METHODS
        public GizmoEditor(IpPicture pic,IpGrid grid)
        {
            Color color = Color.FromArgb(50, 0, 250, 50);
            SolidBrush brush = new SolidBrush(color);
            selectRect = new SelectRect(pic,brush);
            this.pic = pic;
            this.grid = grid;
        }

        public void DrawGizmo(Graphics graph)
        {
            if (gizmo != null)
                gizmo.DrawGizmo(graph);
        }

        public void CreateGizmo()
        {
            DefineGizmoMode();
        }

        public void ControlGizmo(int xPos, int yPos)
        {
            gizmo.Control(xPos, yPos);
        }

        public void CheckSelectedController(int xPos, int yPos)
        {
            gizmo.CheckSelectedController(xPos, yPos);
        }
        
        public void ResetGizmo()
        {
            selectRect.ResetRect();
            gizmo = null;
        }

        public void ResetCenterPoint()
        {
            gizmo.DefaultControllerPosition();
        }

        public void MirrorSelectedX()
        {
            gizmo.MirrorSelectedX();
        }
        public void MirrorSelectedY()
        {
            gizmo.MirrorSelectedY();
        }
        public void ResetControllers()
        {
            gizmo.ResetControllers();
        }
        public void DeleteSelected()
        {
            pic.DeleteSelectedLines();
            pic.DeleteSelectedCircles();
            gizmo = null;
        }

        #endregion
    }
}
