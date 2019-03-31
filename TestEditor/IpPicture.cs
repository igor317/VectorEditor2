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
        private const short countAddingLines = 1;

        private LinePic[] lines;
        private IpSpline[] splines;
        private Ellipse[] circles;
        private IpLayer[] layers;

        private LinePic[] bufferLines;
        private Ellipse[] bufferCircles;
        private IpSpline[] bufferSplines;
        private IpLayer[] bufferLayers;
        
        int counterSBuff = 0;
        int counterLBuff = 0;
        int counterCBuff = 0;

        private int counterLines = countAddingLines;
        private int counterCircles = countAddingLines;
        private int counterSplines = countAddingLines;
        private int counterLayers = 1;
        private int counterC = 0;
        private int counter = 0;
        private int counterS = 0;
        //private int CounterLayer
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
        public IpSpline[] Splines
        {
            get { return splines; }
        }
        public IpLayer[] Layers
        {
            get { return layers; }
        }

        public int CounterLines
        {
            get { return counter; }
        }
        public int CounterEllipses
        {
            get { return counterC; }
        }
        public int CounterSplines
        {
            get { return counterS; }
        }
        public int CounterLayers
        {
            get { return counterLayers; }
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

        #region PRIVATE METHODS
        private bool CopyLines()
        {
            bufferLines = null;
            bool f = false;
            for (int i = 0; i < CounterLines; ++i)
            {
                if (lines[i].selected)
                {
                    f = true;
                    break;
                }
            }
            if (!f)
                return false;
            counterLBuff = 0;
            int counterLines1 = countAddingLines;
            bufferLines = new LinePic[counterLines1];

            for (int i = 0; i < lines.Length; ++i)
            {
                if (counterLBuff >= counterLines1 - 1)
                {
                    counterLines1 += countAddingLines;
                    Array.Resize(ref bufferLines, counterLines1);
                }
                if (lines[i].selected)
                {
                    bufferLines[counterLBuff] = lines[i];
                    counterLBuff++;
                }
            }
            return true;
        }

        private bool CopyEllises()
        {
            bufferCircles = null;
            bool f = false;
            for (int i = 0; i < CounterEllipses; ++i)
            {
                if (circles[i].selected)
                {
                    f = true;
                    break;
                }
            }
            if (!f)
                return false;
            counterCBuff = 0;
            int counterCircles1 = countAddingLines;
            bufferCircles = new Ellipse[counterCircles1];

            for (int i = 0; i < circles.Length; ++i)
            {
                if (counterCBuff >= counterCircles1 - 1)
                {
                    counterCircles1 += countAddingLines;
                    Array.Resize(ref bufferCircles, counterCircles1);
                }
                if (circles[i].selected)
                {
                    bufferCircles[counterCBuff] = circles[i];
                    counterCBuff++;
                }

            }
            return true;
        }

        private bool CopySplines()
        {
            bool f = false;
            for (int i = 0; i < CounterSplines; ++i)
                if (splines[i].selected)
                {
                    f = true;
                    break;
                }
            if (!f)
                return false;
            counterSBuff = 0;
            int counterSplines1 = countAddingLines;
            bufferSplines = new IpSpline[counterSplines1];
            for (int i = 0; i < splines.Length; ++i)
            {
                if (counterSBuff >= counterSplines1 - 1)
                {
                    counterSplines1 += countAddingLines;
                    Array.Resize(ref bufferSplines, counterSplines1);
                }
                if (splines[i].selected)
                {
                    bufferSplines[counterSBuff] = splines[i];
                    counterSBuff++;
                }
            }
            return true;
        }

        #endregion

        #region PUBLIC METHODS
        public IpPicture(IpCursor selectCursor, IpCursor lastCursor, int sizeX, int sizeY)
        {
            lines = new LinePic[counterLines];
            circles = new Ellipse[counterCircles];
            splines = new IpSpline[counterSplines];
            layers = new IpLayer[counterLayers];
            layers[0] = new IpLayer();
            this.selectCursor = selectCursor;
            this.lastCursor = lastCursor;
            vectorPicture = new VectorPicture();
            this.sizeX = sizeX;
            this.sizeY = sizeY;
        }

        public void ShowAllLayers()
        {
            for (int i = 0; i < Layers.Length; ++i)
            {
                Layers[i].active = true;
            }
        }

        public void SwitchLayer(int index)
        {
            layers[index].active = (layers[index].active) ? false : true;
        }

        public void AddSelectedToLayer(int index)
        {
            for (int i = 0;i<counter;++i)
            {
                if (lines[i].selected)
                    lines[i].layer = index;
            }
            for (int i = 0;i<counterC;++i)
            {
                if (circles[i].selected)
                    circles[i].layer = index;
            }
            for (int i = 0;i<counterS;++i)
            {
                if (splines[i].selected)
                    splines[i].layer = index;
            }
        }

        public void DeleteLayer(int index)
        {
            counterLayers--;
            int k = 0;
            bufferLayers = new IpLayer[counterLayers];
            for (int i = 0;i<layers.Length;++i)
            {
                if (i != index)
                {
                    bufferLayers[k] = layers[i];
                    k++;
                }

            }
            Array.Resize(ref layers, counterLayers);
            Array.Copy(bufferLayers, layers, counterLayers);
            bufferLayers = null;
        }

        public void AddLayer()
        {
            counterLayers++;
            Array.Resize(ref layers, counterLayers);
            layers[counterLayers - 1] = new IpLayer();
        }

        public void AddLayer(string Name)
        {
            counterLayers++;
            Array.Resize(ref layers, counterLayers);
            layers[counterLayers - 1] = new IpLayer(Name);
        }

        public void AddLine(Pen pen, bool DrawSelectLine,int layer)
        {
            if (selectCursor.X == lastCursor.X && selectCursor.Y == lastCursor.Y)
                return;
            if (counter >= counterLines - 1)
            {
                counterLines += countAddingLines;
                Array.Resize(ref lines, counterLines);
            }
            lines[counter].AddLine(lastCursor, selectCursor, pen, layer);

            if (DrawSelectLine)
            {
                lastCursor.X = selectCursor.X;
                lastCursor.Y = selectCursor.Y;
                counter++;
            }
        }

        public void AddCircle(Pen pen, bool DrawSelectCircle,int layer)
        {
            if (selectCursor.X == lastCursor.X && selectCursor.Y == lastCursor.Y)
                return;
            if (counterC >= counterCircles - 1)
            {
                counterCircles += countAddingLines;
                Array.Resize(ref circles, counterCircles);
            }
            circles[counterC].AddCircle(lastCursor, selectCursor, pen, layer);

            if (DrawSelectCircle)
            {
                lastCursor.X = selectCursor.X;
                lastCursor.Y = selectCursor.Y;
                counterC++;
            }
        }

        public void AddSpline(Pen pen, bool DrawSelectSpline2, int layer)
        {
            if (selectCursor.X == lastCursor.X && selectCursor.Y == lastCursor.Y)
                return;
            if (counterS >= counterSplines - 1)
            {
                counterSplines += countAddingLines;
                Array.Resize(ref splines, counterSplines);
            }
            splines[counterS].AddSpline(lastCursor, selectCursor, pen, layer);

            if (DrawSelectSpline2)
            {
                lastCursor.X = selectCursor.X;
                lastCursor.Y = selectCursor.Y;
                selectCursor.X = splines[counterS].x2;
                selectCursor.Y = splines[counterS].y2;
                counterS++;
            }
        }

        public void AddCurveSpline1()
        {
            splines[counterS - 1].Curve1(selectCursor);
        }

        public void AddCurveSpline2()
        {
            splines[counterS - 1].Curve2(selectCursor);
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
            counterSplines = countAddingLines;
            Array.Clear(lines, 0, lines.Length);
            Array.Clear(circles, 0, circles.Length);
            Array.Clear(splines, 0, splines.Length);
            Array.Resize(ref lines, counterLines);
            Array.Resize(ref splines, counterLines);
            Array.Resize(ref circles, counterCircles);
            counter = 0;
            counterC = 0;
            counterS = 0;
        }

        public void LoadFile(string path)
        {
            if (path == "")
                return;
            vectorPicture.LoadPicture(path);
            counter = 0;
            counterC = 0;
            counterS = 0;
            float xC = sizeX / vectorPicture.OriginX;
            float yC = sizeY / vectorPicture.OriginY;
            counterLines = vectorPicture.CounterLines + 1;
            counterCircles = vectorPicture.CounterCircles + 1;
            counterSplines = vectorPicture.CounterSplines + 1;
            lines = new LinePic[counterLines];
            circles = new Ellipse[counterCircles];
            splines = new IpSpline[counterSplines];
            for (int i = 0; i < vectorPicture.CounterLines; ++i)
            {
                lines[i].x1 = Convert.ToInt16(vectorPicture.Lines[i].x1 * xC);
                lines[i].y1 = Convert.ToInt16(vectorPicture.Lines[i].y1 * yC);
                lines[i].x2 = Convert.ToInt16(vectorPicture.Lines[i].x2 * xC);
                lines[i].y2 = Convert.ToInt16(vectorPicture.Lines[i].y2 * yC);
                lines[i].layer = vectorPicture.Lines[i].layer;
                lines[i].pen = new Pen(Color.Black);
                counter++;
            }
            for (int i = 0;i<vectorPicture.CounterCircles;++i)
            {
                circles[i].xR = Convert.ToInt16(vectorPicture.Ellipses[i].x * xC);
                circles[i].yR = Convert.ToInt16(vectorPicture.Ellipses[i].y * yC);
                circles[i].radX = Convert.ToInt16(vectorPicture.Ellipses[i].radX * xC);
                circles[i].radY = Convert.ToInt16(vectorPicture.Ellipses[i].radY * yC);
                circles[i].alpha = vectorPicture.Ellipses[i].alpha;
                circles[i].layer = vectorPicture.Ellipses[i].layer;
                circles[i].pen = new Pen(Color.Black);
                circles[i].CalculatePoints();
                counterC++;
            }
            for (int i = 0;i<vectorPicture.CounterSplines;++i)
            {
                splines[i].x1 = Convert.ToInt16(vectorPicture.Splines[i].x1 * xC);
                splines[i].y1 = Convert.ToInt16(vectorPicture.Splines[i].y1 * xC);
                splines[i].x2 = Convert.ToInt16(vectorPicture.Splines[i].x2 * xC);
                splines[i].y2 = Convert.ToInt16(vectorPicture.Splines[i].y2 * xC);
                splines[i].x3 = Convert.ToInt16(vectorPicture.Splines[i].x3 * xC);
                splines[i].y3 = Convert.ToInt16(vectorPicture.Splines[i].y3 * xC);
                splines[i].x4 = Convert.ToInt16(vectorPicture.Splines[i].x4 * xC);
                splines[i].y4 = Convert.ToInt16(vectorPicture.Splines[i].y4 * xC);
                splines[i].layer = vectorPicture.Splines[i].layer;
                splines[i].pen = new Pen(Color.Black);
                splines[i].CalculatePoints();
                counterS++;
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
                            "\"" + " " + "rx=" + "\"" + circles[i].radX + "\"" + " " + "ry=" + "\"" + circles[i].radY + "\"" +
                            " " + "a=" + "\"" + circles[i].alpha + "\"" +  "\\>");
                    }
                    
                    for (int i = 0;i<counterS;++i)
                    {
                        file.WriteLine("<spline " + "x1=" + "\"" + splines[i].x1 + "\"" + " " + "y1=" + "\"" + splines[i].y1 +
                            "\"" + " " + "x2=" + "\"" + splines[i].x2 + "\"" + " " + "y2=" + "\"" + splines[i].y2 + "\"" + " "
                            + "x3=" + "\"" + splines[i].x3 + "\"" + " " + "y3=" + "\"" + splines[i].y3 + "\"" + " " + "x4=" + "\"" + splines[i].x4 + "\"" + " "
                            + "y4=" + "\"" + splines[i].y4 + "\"" + "\\>");
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
                if (counter >= counterLines - 1)
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
                if (counterC >= counterCircles - 1)
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

        public void DeleteSelectedSpline()
        {
            bool f = false;
            for (int i = 0; i < CounterSplines; ++i)
                if (splines[i].selected)
                {
                    f = true;
                    break;
                }
            if (!f)
                return;
            counterS = 0;
            counterSplines = countAddingLines;
            bufferSplines = new IpSpline[counterSplines];

            for (int i = 0; i < splines.Length; ++i)
            {
                if (counterS >= counterSplines - 1)
                {
                    counterSplines += countAddingLines;
                    Array.Resize(ref bufferSplines, counterSplines);
                }
                if (splines[i] != 0)
                {
                    if (!splines[i].selected)
                    {
                        bufferSplines[counterS] = splines[i];
                        counterS++;
                    }
                }
            }

            Array.Resize(ref splines, counterSplines);
            Array.Copy(bufferSplines, splines, bufferSplines.Length);
            Array.Clear(splines, counterS, countAddingLines);
            bufferSplines = null;
        }

        public bool CopyPicture()
        {
            if (!CopyLines() && !CopyEllises() && !CopySplines())
                return false;
            return true;
        }

        public void PastePicture()
        {
            for (int i = 0; i < counter; ++i)
                lines[i].selected = false;
            for (int i = 0; i < counterC; ++i)
                circles[i].selected = false;
            for (int i = 0; i < counterS; ++i)
                splines[i].selected = false;

            if (bufferCircles != null)
            {
                counterC += counterCBuff-1;
                counterCircles = counterC;
                Array.Resize(ref circles, counterC+1);
                Array.Copy(bufferCircles, 0, circles, counterC - counterCBuff+1, counterCBuff);
            }
            if (bufferLines != null)
            {
                counter += counterLBuff;
                counterLines = counter + 1;
                Array.Resize(ref lines, counter + 1);
                Array.Copy(bufferLines, 0, lines, counter - counterLBuff + 1, counterLBuff);
            }
            if (bufferSplines != null)
            {
                counterS += counterSBuff-1;
                counterSplines = counterS;
                Array.Resize(ref splines, counterS + 1);
                Array.Copy(bufferSplines, 0, splines, counterS - counterSBuff + 1, counterSBuff);
            }
        }

        public int GetCountSelectedLines()
        {
            int cL = 0;
            for (int i = 0; i < counter; ++i)
                if (lines[i].selected)
                    cL++;
            return cL;
        }

        public int GetCountSelectedEllipse()
        {
            int cS = 0;
            for (int i = 0; i < counterC; ++i)
                if (circles[i].selected)
                    cS++;
            return cS;
        }

        public int GetCountSelectedSplines()
        {
            int cSp = 0;
            for (int i = 0; i < counterS; ++i)
                if (splines[i].selected)
                    cSp++;
            return cSp;
        }

        public int GetCountSelected()
        {
            return GetCountSelectedLines() + GetCountSelectedEllipse() + GetCountSelectedSplines();
        }
        #endregion
    }
}
