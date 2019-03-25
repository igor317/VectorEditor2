using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace TestEditor
{
    class IpPicture
    {
        #region VARIABLES
        private const short countAddingLines = 2;

        private LinePic[] lines;
        private IpSpline2[] splines2;
        private Ellipse[] circles;
        private LinePic[] bufferLines;
        private Ellipse[] bufferCircles;
        private int counterLines = countAddingLines;
        private int counterCircles = countAddingLines;
        private int counterSplines2 = countAddingLines;
        private int counterC = 0;
        private int counter = 0;
        private int counterS2 = 0;
        private IpCursor selectCursor;
        private IpCursor lastCursor;
        private VectorPicture vectorPicture;
        private int sizeX, sizeY;
        private float scaleCoeff = 1;
        private float xOffset = 0;
        private float yOffset = 0;

        #endregion

        #region SET&GET METHODS
        public LinePic[] Lines
        {
            get { return lines; }
        }
        public Ellipse[] Ellipses
        {
            get { return circles; }
        }
        public IpSpline2[] Splines2
        {
            get { return splines2; }
        }

        public int CounterLines
        {
            get { return counter; }
        }
        public int CounterEllipses
        {
            get { return counterC; }
        }
        public int CounterSplines2
        {
            get { return counterS2; }
        }
        public float ScaleCoefficient
        {
            set { scaleCoeff = value; }
            get { return scaleCoeff; }
        }
        public float XOffset
        {
            set { xOffset = value; }
            get { return xOffset; }
        }
        public float YOffset
        {
            set { yOffset = value; }
            get { return yOffset; }
        }

        #endregion

        #region PUBLIC METHODS
        public IpPicture(IpCursor selectCursor, IpCursor lastCursor, int sizeX, int sizeY)
        {
            lines = new LinePic[counterLines];
            circles = new Ellipse[counterCircles];
            splines2 = new IpSpline2[counterSplines2];
            this.selectCursor = selectCursor;
            this.lastCursor = lastCursor;
            vectorPicture = new VectorPicture();
            this.sizeX = sizeX;
            this.sizeY = sizeY;
        }

        public void AddLine(Pen pen, bool DrawSelectLine)
        {
            if (selectCursor.X == lastCursor.X && selectCursor.Y == lastCursor.Y)
                return;
            if (counter >= counterLines - 1)
            {
                counterLines += countAddingLines;
                Array.Resize(ref lines, counterLines);
            }
            lines[counter].AddLine(lastCursor, selectCursor, pen);

            if (DrawSelectLine)
            {
                lastCursor.X = selectCursor.X;
                lastCursor.Y = selectCursor.Y;
                counter++;
            }
        }

        public void AddCircle(Pen pen, bool DrawSelectCircle)
        {
            if (selectCursor.X == lastCursor.X && selectCursor.Y == lastCursor.Y)
                return;
            if (counterC >= counterCircles - 1)
            {
                counterCircles += countAddingLines;
                Array.Resize(ref circles, counterCircles);
            }
            circles[counterC].AddCircle(lastCursor, selectCursor,pen);

            if (DrawSelectCircle)
            {
                lastCursor.X = selectCursor.X;
                lastCursor.Y = selectCursor.Y;
                counterC++;
            }
        }

        public void AddSpline2(Pen pen,bool DrawSelectSpline2)
        {
            if (selectCursor.X == lastCursor.X && selectCursor.Y == lastCursor.Y)
                return;
            if (counterS2 >= counterSplines2 - 1)
            {
                counterSplines2 += countAddingLines;
                Array.Resize(ref splines2, counterSplines2);
            }
            splines2[counterS2].AddLine(lastCursor, selectCursor, pen);

            if (DrawSelectSpline2)
            {
                lastCursor.X = selectCursor.X;
                lastCursor.Y = selectCursor.Y;
                selectCursor.X = splines2[counterS2].x2;
                selectCursor.Y = splines2[counterS2].y2;
                counterS2++;
            }
        }

        public void AddCurveSpline2()
        {
            splines2[counterS2 - 1].AddCurve(selectCursor);
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
            counterCircles = countAddingLines;
            Array.Clear(lines, 0, lines.Length);
            Array.Clear(circles, 0, circles.Length);
            Array.Resize(ref lines, counterLines);
            Array.Resize(ref circles, counterCircles);
            counter = 0;
            counterC = 0;
        }

        public void LoadFile(string path)
        {
            if (path == "")
                return;
            vectorPicture.LoadPicture(path);
            counter = 0;
            counterC = 0;
            float xC = sizeX / vectorPicture.OriginX;
            float yC = sizeY / vectorPicture.OriginY;
            counterLines = vectorPicture.CounterLines + 1;
            counterCircles = vectorPicture.CounterCircles + 1;
            lines = new LinePic[counterLines];
            circles = new Ellipse[counterCircles];
            for (int i = 0; i < vectorPicture.CounterLines; ++i)
            {
                lines[i].x1 = Convert.ToInt16(vectorPicture.GetLineX1(i) * xC);
                lines[i].y1 = Convert.ToInt16(vectorPicture.GetLineY1(i) * yC);
                lines[i].x2 = Convert.ToInt16(vectorPicture.GetLineX2(i) * xC);
                lines[i].y2 = Convert.ToInt16(vectorPicture.GetLineY2(i) * yC);
                lines[i].pen = new Pen(Color.Black);
                counter++;
            }
            for (int i = 0;i<vectorPicture.CounterCircles;++i)
            {
                circles[i].xR = Convert.ToInt16(vectorPicture.GetCircleXC(i) * xC);
                circles[i].yR = Convert.ToInt16(vectorPicture.GetCircleYC(i) * yC);
                circles[i].radX = Convert.ToInt16(vectorPicture.GetCircleRadius(i) * (xC+yC)/2);
                circles[i].pen = new Pen(Color.Black);
                counterC++;
            }
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

                    for (int i = 0;i<counterC;++i)
                    {
                        file.WriteLine("<circle " + "cx=" + "\"" + circles[i].xR + "\"" + " " + "cy=" + "\"" + circles[i].yR +
                            "\"" + " " + "r=" + "\"" + circles[i].radX + "\"" + "\\>");
                    }
                    file.Close();
                }
            }
        }

        public void DeleteSelectedLines()
        {
            bool f = false;
            for (int i = 0; i < CounterLines; ++i)
                if (lines[i].selected)
                {
                    f = true;
                    break;
                }
            if (!f)
                return;
            counter = 0;
            counterLines = countAddingLines;
            bufferLines = new LinePic[counterLines];

            for (int i = 0; i < lines.Length; ++i)
            {
                if (counterLines >= counterLines - 1)
                {
                    counterLines += countAddingLines;
                    Array.Resize(ref bufferLines, counterLines);
                }
                if (lines[i] != 0)
                {
                    if (!lines[i].selected)
                    {
                        bufferLines[counter] = lines[i];
                        counter++;
                    }
                }
            }

            Array.Resize(ref lines, counterLines);
            Array.Copy(bufferLines, lines, bufferLines.Length);
            Array.Clear(lines, counter, countAddingLines);
            bufferLines = null;
        }

        public void DeleteSelectedCircles()
        {
            bool f = false;
            for (int i = 0; i < CounterEllipses; ++i)
                if (Ellipses[i].selected)
                {
                    f = true;
                    break;
                }
            if (!f)
                return;
            counterC = 0;
            counterCircles = countAddingLines;
            bufferCircles = new Ellipse[counterCircles];

            for (int i = 0; i < circles.Length; ++i)
            {
                if (counterCircles >= counterCircles - 1)
                {
                    counterCircles += countAddingLines;
                    Array.Resize(ref bufferCircles, counterCircles);
                }
                if (circles[i] != 0)
                {
                    if (!circles[i].selected)
                    {
                        bufferCircles[counterC] = circles[i];
                        counterC++;
                    }
                }
            }

            Array.Resize(ref circles, counterCircles);
            Array.Copy(bufferCircles, circles, bufferCircles.Length);
            Array.Clear(circles, counterC, countAddingLines);
            bufferCircles = null;
        }
        #endregion
    }
}
