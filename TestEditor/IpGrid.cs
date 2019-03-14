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
        private bool enableMagnet = false;
        private bool rotationGrid = false;
        private int sizeX, sizeY;
        private IpPicture pic;

        private Pen gridPenLG = new Pen(Color.LightGray);
        private Pen gridPenDG = new Pen(Color.DarkGray);
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
        #endregion

        #region PRIVATE METHODS
        private void GridXPosition(IpCursor cursor,float xPos)
        {
            int LenghtLinesX = sizeX / countGridLines;
            float xR = xPos / LenghtLinesX - Convert.ToInt16(xPos / LenghtLinesX);
            int xT = LenghtLinesX * Convert.ToInt16(xPos / LenghtLinesX);
            cursor.X = (xR >= 0.5f) ? xT : xT + 1;
        }

        private void GridYPosition(IpCursor cursor,float yPos)
        {
            int LenghtLinesY = sizeY / countGridLines;
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

        public void DrawGrid(Graphics graph)
        {
            if (EnableGrid)
            {
                for (int i = 0; i < countGridLines; ++i)
                {

                    int curx = sizeX / countGridLines * i;
                    int cury = sizeY / countGridLines * i;
                    if (i == countGridLines / 2)
                    {
                        graph.DrawLine(gridPenDG, curx, 0, curx, sizeY);
                        graph.DrawLine(gridPenDG, 0, cury, sizeX, cury);
                    }
                    else
                    {
                        graph.DrawLine(gridPenLG, curx, 0, curx, sizeY);
                        graph.DrawLine(gridPenLG, 0, cury, sizeX, cury);
                    }
                }
            }
        }

        public void MoveCursor(IpCursor cursor, float xPos, float yPos, bool ignoreSelected)
        {
            if (EnableMagnet)
                if (ignoreSelected)
                    MagnetCursorPosition(cursor, xPos, yPos,true, 5);
                else
                    MagnetCursorPosition(cursor, xPos, yPos,false, 5);
            if (EnableGrid)
            {
                if (xPos != -1)
                    GridXPosition(cursor, xPos);
                if (yPos != -1)
                    GridYPosition(cursor, yPos);
            }
            if (!EnableMagnet && !EnableGrid)
            {
                if (xPos != -1)
                {
                    if (xPos < sizeX)
                        cursor.X = xPos;
                    if (xPos >= sizeX)
                        cursor.X = sizeX;
                    if (xPos < 0)
                        cursor.X = 0;
                }
                if (yPos != -1)
                {
                    if (yPos < sizeY)
                        cursor.Y = yPos;
                    if (yPos >= sizeY)
                        cursor.Y = sizeY;
                    if (yPos < 0)
                        cursor.Y = 0;
                }
            }
        }

        public float GridRotation(float alpha, int res)
        {
            float angle = 0;
            float temp = 0;
            angle = alpha.Rad2Deg();
            temp = angle % res;
            if (angle > 0)
            {
                if (Math.Abs(temp) >= res / 2)
                    angle = Convert.ToInt16(angle - temp) + res;
                else
                    angle = Convert.ToInt16(angle - temp);
            }
            else
            {
                if (Math.Abs(temp) >= res / 2)
                    angle = Convert.ToInt16(angle - temp) - res;
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
