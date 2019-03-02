using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace TestEditor
{
    public struct LinePic
    {
        public float x1;
        public float y1;
        public float x2;
        public float y2;
        public Pen pen;

        private float angle1, angle2;
        private float radius1, radius2;
        private float hc1, wc1, hc2, wc2;
        private float xCent, yCent;
        
        public void SetCenterPoint(float xCenter,float yCenter)
        {
            xCent = xCenter;
            yCent = yCenter;
        }

        public void CalculateRotationAxes(float xCenter,float yCenter)
        {
            SetCenterPoint(xCenter, yCenter);
            hc1 = yCenter - y1;
            wc1 = x1 - xCenter;
            hc2 = yCenter - y2;
            wc2 = x2 - xCenter;
            radius1 = Convert.ToSingle(Math.Sqrt(hc1 * hc1 + wc1 * wc1));
            radius2 = Convert.ToSingle(Math.Sqrt(hc2 * hc2 + wc2 * wc2));
            angle1 = (float)Math.Atan2(hc1, wc1);
            angle2 = (float)Math.Atan2(hc2, wc2);
        }

        public void RotateLine(float angle)
        {
            double radAngle1 = Math.PI/2 + angle + angle1;
            double radAngle2 = Math.PI / 2 + angle + angle2;
            x2 = xCent + radius2 * (float)Math.Sin(radAngle2);
            y2 = yCent + radius2 * (float)Math.Cos(radAngle2);
            x1 = xCent + radius1 * (float)Math.Sin(radAngle1);
            y1 = yCent + radius1 * (float)Math.Cos(radAngle1);
        }

        public static bool operator !=(LinePic c1, LinePic c2)
        {
            if ((c1.x1 != c2.x2) || (c1.y1 != c2.y1) || (c1.x1 != c2.x2) || (c1.y2 != c2.y2))
                return true;
            return false;
        }
        public static bool operator ==(LinePic c1, LinePic c2)
        {
            if ((c1.x1 == c2.x1) && (c1.y1 == c2.y1) && (c1.x2 == c2.x2) && (c1.y2 == c2.y2))
                return true;
            return false;
        }

        public static bool operator !=(LinePic c1, int i2)
        {
            if ((c1.x1 != i2) || (c1.y1 != i2) || (c1.x1 != i2) || (c1.y2 != i2))
                return true;
            return false;
        }
        public static bool operator ==(LinePic c1, int i2)
        {
            if ((c1.x1 == i2) && (c1.y1 == i2) && (c1.x2 == i2) && (c1.y2 == i2))
                return true;
            return false;
        }

    }

    class IpPicture
    {
        #region VARIABLES
        private const short countAddingLines = 2;

        private LinePic[] lines;
        private LinePic[] buffer;
        private int counterLines = countAddingLines;
        private int counter = 0;
        private IpCursor selectCursor;
        private IpCursor lastCursor;
        private VectorPicture vectorPicture;
        private int sizeX, sizeY;
        #endregion
        #region SET&GET METHODS
        public LinePic[] Lines
        {
            get { return lines; }
        }
        public int CounterLines
        {
            get { return counter; }
        }
        #endregion
        #region PUBLIC METHODS
        public IpPicture(IpCursor selectCursor, IpCursor lastCursor, int sizeX, int sizeY)
        {
            lines = new LinePic[counterLines];
            this.selectCursor = selectCursor;
            this.lastCursor = lastCursor;
            vectorPicture = new VectorPicture();
            this.sizeX = sizeX;
            this.sizeY = sizeY;
        }

        public void AddLine(int x, int y, Pen pen, bool DrawSelectLine)
        {
            if (selectCursor.X == lastCursor.X && selectCursor.Y == lastCursor.Y)
                return;
            if (counter >= counterLines - 1)
            {
                counterLines += countAddingLines;
                Array.Resize(ref lines, counterLines);
            }
            lines[counter].x1 = lastCursor.X;
            lines[counter].y1 = lastCursor.Y;
            lines[counter].x2 = selectCursor.X;
            lines[counter].y2 = selectCursor.Y;
            lines[counter].pen = pen;

            if (DrawSelectLine)
            {
                lastCursor.X = selectCursor.X;
                lastCursor.Y = selectCursor.Y;
                counter++;
            }
        }

        public void StepBack()
        {
            if (counter > 0)
            {
                lines[counter].x1 = 0;
                lines[counter].y1 = 0;
                lines[counter].x2 = 0;
                lines[counter].y2 = 0;
                lines[counter].pen = null;

                lastCursor.X = lines[counter - 1].x1;
                lastCursor.Y = lines[counter - 1].y1;
                lines[counter - 1].x2 = lastCursor.X;
                lines[counter - 1].y2 = lastCursor.Y;
                selectCursor.X = lastCursor.X;
                selectCursor.Y = lastCursor.Y;
                counter--;
            }
        }

        public void ResetPicture()
        {
            counterLines = countAddingLines;
            lines = new LinePic[counterLines];
            counter = 0;
        }

        public void LoadFile(string path)
        {
            if (path == "")
                return;
            vectorPicture.LoadPicture(path);
            counter = 0;
            float xC = sizeX / vectorPicture.OriginX;
            float yC = sizeY / vectorPicture.OriginY;
            counterLines = vectorPicture.Counter + 1;
            lines = new LinePic[counterLines];
            for (int i = 0; i < vectorPicture.Counter; ++i)
            {
                lines[i].x1 = Convert.ToInt16(vectorPicture.GetX1(i) * xC);
                lines[i].y1 = Convert.ToInt16(vectorPicture.GetY1(i) * yC);
                lines[i].x2 = Convert.ToInt16(vectorPicture.GetX2(i) * xC);
                lines[i].y2 = Convert.ToInt16(vectorPicture.GetY2(i) * yC);
                lines[i].pen = new Pen(Color.Black);
                counter++;
            }
            selectCursor.X = lines[vectorPicture.Counter - 1].x2;
            selectCursor.Y = lines[vectorPicture.Counter - 1].y2;
            lastCursor.X = lines[vectorPicture.Counter - 1].x2;
            lastCursor.Y = lines[vectorPicture.Counter - 1].y2;
            vectorPicture.ClearBuffer();
        }

        public void SaveFile(string path)
        {
            if (path != "")
            {
                using (StreamWriter file = new StreamWriter(path))
                {
                    file.WriteLine("viewBox " + sizeX + " " + sizeY);
                    for (int i = 0; i < counter; ++i)
                    {
                        file.WriteLine("<line " + "x1=" + "\"" + lines[i].x1 + "\"" + " " + "y1=" + "\"" + lines[i].y1 +
                            "\"" + " " + "x2=" + "\"" + lines[i].x2 + "\"" + " " + "y2=" + "\"" + lines[i].y2 + "\"" + "\\>");
                    }
                    file.Close();
                }
            }
        }

        public void DeleteLine(int[] selectedLines)
        {
            if (selectedLines == null || selectedLines[0] == GizmoEditor.unusedSelectedLines)
                return;
            int k = 0;
            bool flag = true;

            for (int i = 0; i < lines.Length; ++i)
            {
                for (int j = 0; j < selectedLines.Length; ++j)
                {
                    if (i == selectedLines[j])
                        k++;
                }
            }
            k = counter - k;

            buffer = new LinePic[k + 1];
            k = 0;

            for (int i = 0; i < counter; ++i)
            {
                flag = true;
                if (lines[i] == 0)
                    break;
                for (int j = 0; j < selectedLines.Length; ++j)
                {
                    if (i == selectedLines[j])
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    buffer[k] = lines[i];
                    k++;
                }
            }
            counter = buffer.Length - 1;
            counterLines = counter + countAddingLines;
            Array.Resize(ref lines, counterLines);
            Array.Copy(buffer, lines, buffer.Length);
            Array.Clear(lines, counter, countAddingLines);
        }
        #endregion
    }
}
