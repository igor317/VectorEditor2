using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestEditor
{
    class IpGrid
    {
        #region VARIABLES
        private int countGridLines = 2;                                       // Количесво линий в сетке
        private int minCountGridLines = 2;                                    // Минимальное количество линий в сетке
        private int maxCountGridLines = 50;                                   // Максимальное количество линий в сетке
        private bool enableGrid = false;                                      // Включить\выключить сетку
        private bool enableMagnet = false;                                    // Включить\выключить магнит
        private bool rotationGrid = false;                                    // Включить\выключить магнит поворота
        private bool magnetCenter = true;                                     // Включить\выключить магнит к центру линии
        private bool magnetCross = true;                                      // Включить\выключить магнит к пересечениям линий
        private bool magnetPoints = true;                                     // Включить\выключить магнит к точкам линий
        private int sizeX, sizeY;
        private IpPicture pic;
        private int gridDegree = 15;
        private Pen gridPenLG = new Pen(Color.LightGray);
        private Pen gridPenDG = new Pen(Color.DarkGray);

        private float scale = 1;
        private float xOff = 0, yoff = 0;
        #endregion

        #region SET&GET METHODS
        public int CountLines
        {
            get { return countGridLines; }
        }
        public int MinGridLines
        {
            set { minCountGridLines = value; }
            get { return minCountGridLines; }
        }
        public int MaxGridLines
        {
            set { maxCountGridLines = value; }
            get { return maxCountGridLines; }
        }
        public int GridDegree
        {
            set { gridDegree = value; }
            get { return gridDegree; }
        }
        public bool EnableGrid
        {
            set { enableGrid = value; }
            get { return enableGrid; }
        }
        public bool EnableMagnet
        {
            set { enableMagnet = value; }
            get { return enableMagnet; }
        }
        public bool EnableRotationGrid
        {
            set { rotationGrid = value; }
            get { return rotationGrid; }
        }

        public float ScaleCoeff
        {
            set { scale = value; }
            get { return scale; }
        }
        public float xOffset
        {
            set { xOff = value; }
            get { return xOff; }
        }
        public float yOffset
        {
            set { yoff = value; }
            get { return yoff; }
        }
        #endregion

        #region PRIVATE METHODS
        private void GridXPosition(IpCursor cursor,float xPos)
        {
            int LenghtLinesX = Convert.ToInt32(sizeX / countGridLines);
            float xR = xPos/ LenghtLinesX - Convert.ToInt16(xPos / LenghtLinesX);
            int xT = LenghtLinesX * Convert.ToInt16(xPos / LenghtLinesX);
            cursor.X = (xR >= 0.5f) ? xT : xT + 1;
        }

        private void GridYPosition(IpCursor cursor,float yPos)
        {
            int LenghtLinesY = Convert.ToInt32(sizeY / countGridLines);
            float yR = yPos / LenghtLinesY - Convert.ToInt16(yPos / LenghtLinesY);
            int yT = LenghtLinesY * Convert.ToInt16(yPos / LenghtLinesY);
            cursor.Y = (yR >= 0.5f) ? yT : yT + 1;
        }

        private void MagnetCursorPosition(IpCursor cursor, float xPos, float yPos,bool ignoreSelected, float res)
        {
            cursor.X = xPos;
            cursor.Y = yPos;
            for (int i = 0; i < pic.CounterLines; ++i)
            {
                if (pic.Lines[i] != 0)
                {
                    if (ignoreSelected)
                    {
                        if (pic.Lines[i].selected)
                            continue;
                    }
                    if (magnetPoints)
                    {
                        if (Math.Abs(pic.Lines[i].x1 - xPos) <= res && Math.Abs(pic.Lines[i].y1 - yPos) <= res)
                        {
                            cursor.X = pic.Lines[i].x1;
                            cursor.Y = pic.Lines[i].y1;
                            break;
                        }
                        if (Math.Abs(pic.Lines[i].x2 - xPos) <= res && Math.Abs(pic.Lines[i].y2 - yPos) <= res)
                        {
                            cursor.X = pic.Lines[i].x2;
                            cursor.Y = pic.Lines[i].y2;
                            break;
                        }
                    }
                    if (magnetCenter)
                        if (Math.Abs((pic.Lines[i].x1 + pic.Lines[i].x2)/2 - xPos) <= res && Math.Abs((pic.Lines[i].y1 + pic.Lines[i].y2)/2 - yPos) <= res)
                        {
                            cursor.X = (pic.Lines[i].x1 + pic.Lines[i].x2) / 2;
                            cursor.Y = (pic.Lines[i].y1 + pic.Lines[i].y2) / 2;
                            break;
                        }
                    if (magnetCross && pic.CounterLines > 1)
                    {
                        float x1 = pic.Lines[i].x1;
                        float x2 = pic.Lines[i].x2;
                        float y1 = pic.Lines[i].y1;
                        float y2 = pic.Lines[i].y2;
                        for (int k = i + 1; k < pic.CounterLines; ++k)
                        {
                            float x3 = pic.Lines[k].x1;
                            float x4 = pic.Lines[k].x2;
                            float y3 = pic.Lines[k].y1;
                            float y4 = pic.Lines[k].y2;
                            float delta = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
                            if (delta != 0)
                            {
                                float xP = ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / delta;
                                float yP = ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / delta;
                                float t1 = (x2 - x1 != 0) ? Math.Abs((xP - x1) / (x2 - x1)) : 0;
                                float t2 = (x4 - x3 != 0) ? Math.Abs((xP - x3) / (x4 - x3)) : 0;
                                float t3 = (y2 - y1 != 0) ? Math.Abs((yP - y1) / (y2 - y1)) : 0;
                                float t4 = (y4 - y3 != 0) ? Math.Abs((yP - y3) / (y4 - y3)) : 0;
                                if (t1 <= 1 && t2 <= 1 && t3 <= 1 && t4 <= 1)
                                if (Math.Abs(xP - xPos) <= res && Math.Abs(yP - yPos) <= res)
                                {
                                    cursor.X = xP;
                                    cursor.Y = yP;
                                    break;
                                }
                            }
                        }

                    }
                }
                else
                    break;
            }
        }
        #endregion

        #region PUBLIC METHODS
        public IpGrid(int sizeX, int sizeY, IpPicture pic)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.pic = pic;
        }

        /// <summary>
        /// Увеличить количество линий сетки
        /// </summary>
        public void IncreaseGridLines()
        {
            if (countGridLines >= maxCountGridLines)
                return;
            int countG = countGridLines + 1;
            while (sizeX % countG != 0 || countG % 2 != 0)
            {
                countG++;
            }
            countGridLines = countG;
        }

        /// <summary>
        /// Уменьшить количество линий сетки
        /// </summary>
        public void ReduceGridLines()
        {
            if (countGridLines <= minCountGridLines)
                return;
            int countG = countGridLines - 1;
            while (sizeX % countG != 0 || countG % 2 != 0)
            {
                countG--;
            }
            countGridLines = countG;
        }

        public void DrawGrid(Graphics graph,float xOffset,float yOffset,float coeff)
        {
            if (EnableGrid)
            {
                for (int i = 0; i < countGridLines; ++i)
                {

                    int curx = Convert.ToInt16(sizeX / countGridLines * i);
                    int cury = Convert.ToInt16(sizeY / countGridLines * i);
                    if (i == countGridLines / 2)
                    {
                        graph.DrawLine(gridPenDG, curx * coeff - xOffset, 0, curx * coeff - xOffset, sizeY * coeff - yOffset);
                        graph.DrawLine(gridPenDG, 0, cury * coeff - yOffset, sizeX * coeff - xOffset, cury * coeff - yOffset);
                    }
                    else
                    {
                        graph.DrawLine(gridPenLG, curx* coeff-xOffset, 0, curx* coeff-xOffset, sizeY* coeff-yOffset);
                        graph.DrawLine(gridPenLG, 0, cury* coeff-yOffset, sizeX* coeff-xOffset, cury * coeff-yOffset);
                    }
                }
            }
        }

        public void MoveCursor(IpCursor cursor, float xPos, float yPos, bool ignoreSelected, IpCursor pivot)
        {
            if (EnableMagnet)
            {
                if (ignoreSelected)
                    MagnetCursorPosition(cursor, (xPos + xOffset) / ScaleCoeff, (yPos + yOffset) / ScaleCoeff, true, 5);
                else
                    MagnetCursorPosition(cursor, (xPos + xOffset) / ScaleCoeff, (yPos + yOffset) / ScaleCoeff, false, 5);
                return;
            }
            if (EnableGrid)
            {
                if (xPos != -1)
                    GridXPosition(cursor, (xPos + xOffset) / ScaleCoeff);
                if (yPos != -1)
                    GridYPosition(cursor, (yPos + yOffset) / ScaleCoeff);
                return;
            }

            if (rotationGrid && pivot != null)
            {
                float xp = pivot.X * ScaleCoeff - xOff - xPos;
                float yp = pivot.Y * ScaleCoeff - yoff - yPos;
                float raduis = (float)Math.Sqrt(Math.Pow(xp, 2) + Math.Pow(yp, 2));
                float angle = (float)Math.Atan2(-xp, -yp);
                angle = GridRotation(angle);
                cursor.X = pivot.X + raduis / ScaleCoeff * (float)Math.Sin(angle);
                cursor.Y = pivot.Y + raduis / ScaleCoeff * (float)Math.Cos(angle);
                return;
            }
            if (xPos != -1)
            {
                if (xPos < sizeX)
                    cursor.X = (xPos + xOffset) / ScaleCoeff;
                if (xPos >= sizeX)
                    cursor.X = (sizeX + xOffset) / ScaleCoeff;
                if (xPos < 0)
                    cursor.X = 0;
            }
            if (yPos != -1)
            {
                if (yPos < sizeY)
                    cursor.Y = (yPos + yOffset) / ScaleCoeff;
                if (yPos >= sizeY)
                    cursor.Y = (sizeY + yOffset) / ScaleCoeff;
                if (yPos < 0)
                    cursor.Y = 0;
            }
        }

        public float GridRotation(float alpha)
        {
            float angle = 0;
            float temp = 0;
            angle = alpha.Rad2Deg();
            temp = angle % gridDegree;
            if (angle > 0)
            {
                if (Math.Abs(temp) >= gridDegree / 2)
                    angle = Convert.ToInt16(angle - temp) + gridDegree;
                else
                    angle = Convert.ToInt16(angle - temp);
            }
            else
            {
                if (Math.Abs(temp) >= gridDegree / 2)
                    angle = Convert.ToInt16(angle - temp) - gridDegree;
                else
                    angle = Convert.ToInt16(angle - temp);
            }
            return angle.Deg2Rad();
        }
        #endregion
    }

    public static class MathExtension
    {
        public static float Rad2Deg(this float rad)
        {
            return rad * 180 / (float)Math.PI;
        }

        public static float Deg2Rad(this float deg)
        {
            return deg * (float)Math.PI / 180;
        }

    }
}
