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
        private enum GizmoMode
        {
            LineGizmo = 0,
            EllipseGizmo,
            MixedGizmo,
        }

        #region VARIABLES
        private Pen selectionPen = new Pen(Color.Red);
        private GizmoMode gizmoMode;
        private Gizmo gizmoMixed;
        private GizmoLine gizmoLine;
        private SelectRect selectRect;
        private IpGrid grid;
        private IpPicture pic;

        private int countSelectedLines = 0;
        private int countSelectedEllipses = 0;
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
        public bool MoveCenterPointCursor
        {
            set { gizmoMixed.MoveCenterPointCursor = value; }
            get
            {
                switch (gizmoMode)
                {
                    case GizmoMode.MixedGizmo:
                        return gizmoMixed.MoveCenterPointCursor;
                }
                return false;
            }
        }
        #endregion

        #region PRIVATE METHODS
        private void DefineGizmoMode()
        {
            countSelectedLines = 0;
            countSelectedEllipses = 0;
            for (int i = 0; i < pic.CounterLines; ++i)
                if (pic.Lines[i].selected)
                    countSelectedLines++;
            for (int i = 0; i < pic.CounterEllipses; ++i)
                if (pic.Ellipses[i].selected)
                    countSelectedEllipses++;
            if ((countSelectedLines == 1 && countSelectedEllipses == 1) || countSelectedLines > 1 || countSelectedEllipses > 1)
            {
                gizmoMode = GizmoMode.MixedGizmo;
                return;
            }
            if (countSelectedLines == 1 && countSelectedEllipses == 0)
            {
                gizmoMode = GizmoMode.LineGizmo;
                return;
            }
            if (countSelectedLines == 0 && countSelectedEllipses == 1)
            {
                gizmoMode = GizmoMode.EllipseGizmo;
                return;
            }

        }
        #endregion

        #region PUBLIC METHODS
        public GizmoEditor(IpPicture pic,IpGrid grid)
        {
            Color color = Color.FromArgb(50, 0, 250, 50);
            SolidBrush brush = new SolidBrush(color);
            gizmoMixed = new Gizmo(pic,grid);
            gizmoLine = new GizmoLine(pic, grid);
            selectRect = new SelectRect(pic,brush);
            this.pic = pic;
            this.grid = grid;
        }

        public void DrawGizmo(Graphics graph)
        {
            switch (gizmoMode)
            {
                case GizmoMode.MixedGizmo:
                    gizmoMixed.DrawGizmo(graph);
                    break;
                case GizmoMode.LineGizmo:
                    gizmoLine.DrawGizmo(graph);
                    break;
            }
        }

        public void CreateGizmo()
        {
            gizmoMixed.Reset();
            gizmoLine.Reset();
            DefineGizmoMode();
            switch (gizmoMode)
            {
                case GizmoMode.MixedGizmo:
                    gizmoMixed.CreateGizmo();
                    break;
                case GizmoMode.LineGizmo:
                    gizmoLine.CreateGizmo();
                    break;
            }

        }

        public void ControlGizmo(int xPos, int yPos)
        {
            switch (gizmoMode)
            {
                case GizmoMode.MixedGizmo:
                    gizmoMixed.Control(xPos, yPos);
                    break;
            }
        }

        public void CheckSelectedController(int xPos, int yPos)
        {
            switch (gizmoMode)
            {
                case GizmoMode.MixedGizmo:
                    gizmoMixed.CheckSelectedController(xPos, yPos);
                    break;
            }
        }

        
        public void ResetGizmo()
        {
            selectRect.ResetRect();
            switch (gizmoMode)
            {
                case GizmoMode.MixedGizmo:
                    gizmoMixed.ResetGizmo();
                    break;
            }
        }

        public void ResetCenterPoint()
        {
            switch(gizmoMode)
            {
                case GizmoMode.MixedGizmo:
                    gizmoMixed.DefaultControllerPosition(true, true);
                    break;
            }

        }

        public void MirrorSelectedX()
        {
            switch (gizmoMode)
            {
                case GizmoMode.MixedGizmo:
                    gizmoMixed.MirrorSelectedX();
                    break;
            }
        }
        public void MirrorSelectedY()
        {
            switch (gizmoMode)
            {
                case GizmoMode.MixedGizmo:
                    gizmoMixed.MirrorSelectedY();
                    break;
            }
        }
        public void ResetControllers()
        {
            switch (gizmoMode)
            {
                case GizmoMode.MixedGizmo:
                    gizmoMixed.ResetControllers();
                    break;
            }
        }
        public void DeleteSelected()
        {
            pic.DeleteSelectedLines();
            pic.DeleteSelectedCircles();
            gizmoMixed.Reset();
        }

        #endregion
    }
}
