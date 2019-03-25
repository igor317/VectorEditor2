using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    abstract class IpGizmo
    {
        #region VARIABLES
        protected IpPicture pic;
        protected IpGrid grid;
        protected bool mCenterPoint;
        #endregion

        #region SET&GET METHODS
        public bool MoveCenterPointCursor
        {
         get { return mCenterPoint; }
         set { mCenterPoint = value; }
        }
        #endregion

        #region PUBLIC METHODS
        public IpGizmo(IpPicture pic, IpGrid grid)
        {
            this.pic = pic;
            this.grid = grid;
        }

        public abstract void DrawGizmo(Graphics graph);
        public abstract void Control(int xPos, int yPos);
        public abstract void CheckSelectedController(int xPos, int yPos);
        public abstract void DefaultControllerPosition();
        public abstract void MirrorSelectedX();
        public abstract void MirrorSelectedY();
        public abstract void ResetControllers();
        #endregion
    }
}
