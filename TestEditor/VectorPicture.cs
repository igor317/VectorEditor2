using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TestEditor
{
    class VectorPicture
    {
        private struct Lines
        {
            float x1, y1, x2, y2;
            int layer;
        }

        private struct Spline
        {
            float x1, y1, x2, y2, x3, y3, x4, y4;
            int layer;
        }

        private struct Ellipse
        {
            float x, y, radX, radY, aplha;
            int layer;
        }

        #region VARIABLES
        private const short addCounterPoint = 2;    // Увеличение размера массива
        private const short countWords = 10;        // Число слов в строке
        private float originX, originY;
        private int[] layers;

        private string[] text;                      // Контейнер для текста
        private float[] xL1, yL1, xL2, yL2;         // Массив координат
        private float[] xS1, yS1, xS2, yS2, xS3, yS3, xS4, yS4;

        private float[] xC, yC, radXC, radYC, alphaC;
        private int counterLines = 0;
        private int counterCircles = 0;
        private int counterSplines = 0;
        private int counterLayers = 0;
        private int cL = addCounterPoint;     // Максимальное количество точек
        private int cC = addCounterPoint;
        private int cS = addCounterPoint;
        private int cLayer = addCounterPoint;
        #endregion

        #region SET&GET METHODS
        public float OriginX
        {
            get { return originX; }
        }
        public float OriginY
        {
            get { return originY; }
        }
        public int GetLayer(int pos)
        {
            return layers[pos];
        }

        public int CounterLines
        {
            get { return counterLines; }
        }
        public int CounterCircles
        {
            get { return counterCircles; }
        }
        public int CounterSplines
        {
            get { return counterSplines; }
        }

        public float GetLineX1(int pos)
        {
            return xL1[pos];
        }
        public float GetLineY1(int pos)
        {
            return yL1[pos];
        }
        public float GetLineX2(int pos)
        {
            return xL2[pos];
        }
        public float GetLineY2(int pos)
        {
            return yL2[pos];
        }

        public float GetCircleXC(int pos)
        {
            return xC[pos];
        }
        public float GetCircleYC(int pos)
        {
            return yC[pos];
        }
        public float GetCircleRadX(int pos)
        {
            return radXC[pos];
        }
        public float GetCircleRadY(int pos)
        {
            return radYC[pos];
        }
        public float GetCircleAlpha(int pos)
        {
            return alphaC[pos];
        }

        public float GetSplineX1(int pos)
        {
            return xS1[pos];
        }
        public float GetSplineY1(int pos)
        {
            return yS1[pos];
        }
        public float GetSplineX2(int pos)
        {
            return xS2[pos];
        }
        public float GetSplineY2(int pos)
        {
            return yS2[pos];
        }
        public float GetSplineX3(int pos)
        {
            return xS3[pos];
        }
        public float GetSplineY3(int pos)
        {
            return yS3[pos];
        }
        public float GetSplineX4(int pos)
        {
            return xS4[pos];
        }
        public float GetSplineY4(int pos)
        {
            return yS4[pos];
        }
        #endregion

        #region PUBLIC METHODS
        public void LoadPicture(string path)
        {
            xL1 = new float[cL];
            yL1 = new float[cL];
            xL2 = new float[cL];
            yL2 = new float[cL];

            xC = new float[cC];
            yC = new float[cC];
            radXC = new float[cC];
            radYC = new float[cC];
            alphaC = new float[cC];

            xS1 = new float[cS];
            yS1 = new float[cS];
            xS2 = new float[cS];
            yS2 = new float[cS];
            xS3 = new float[cS];
            yS3 = new float[cS];
            xS4 = new float[cS];
            yS4 = new float[cS];
            string str;
            counterLines = 0;
            counterCircles = 0;
            counterSplines = 0;
            using (StreamReader file = new StreamReader(path))
            {
                if (path.Substring(path.Length - 3, 3) == "svg")
                {
                    while ((str = file.ReadLine()) != null)
                    {
                        if (str.Length > 10)
                        {
                            if (str.Substring(2, 7) == "viewBox")
                            {
                                GetOriginPointSVG(str);
                            }
                            if (str.Substring(1, 4) == "line")
                            {
                                GetLineFromString(str);
                                if (CounterLines >= cL - 1)
                                {
                                    cL += addCounterPoint;
                                    Array.Resize(ref xL1, cL);
                                    Array.Resize(ref yL1, cL);
                                    Array.Resize(ref xL2, cL);
                                    Array.Resize(ref yL2, cL);
                                }
                                counterLines++;
                            }
                            if (str.Substring(1,6) == "circle")
                            {
                                GetCircleFromString(str);
                                if (CounterCircles >= cC - 1)
                                {
                                    cC += addCounterPoint;
                                    Array.Resize(ref xC, cC);
                                    Array.Resize(ref yC, cC);
                                    Array.Resize(ref radXC, cC);
                                    Array.Resize(ref radYC, cC);
                                }
                                counterCircles++;
                            }
                        }
                    }
                }
                if (path.Substring(path.Length - 3, 3) == "cpi")
                {

                    while ((str = file.ReadLine()) != null)
                    {
                        if (str.Substring(0, 7) == "viewBox")
                        {
                            GetOriginPointCPI(str);
                        }
                        if (str.Substring(1, 4) == "line")
                        {
                            GetLineFromString(str);
                            if (CounterLines >= cL - 1)
                            {
                                cL += addCounterPoint;
                                Array.Resize(ref xL1, cL);
                                Array.Resize(ref yL1, cL);
                                Array.Resize(ref xL2, cL);
                                Array.Resize(ref yL2, cL);
                            }
                            counterLines++;
                        }
                        if (str.Substring(1, 6) == "circle")
                        {
                            GetCircleFromString(str);
                            if (CounterCircles >= cC - 1)
                            {
                                cC += addCounterPoint;
                                Array.Resize(ref xC, cC);
                                Array.Resize(ref yC, cC);
                                Array.Resize(ref radXC, cC);
                                Array.Resize(ref radYC, cC);
                                Array.Resize(ref alphaC, cC);
                            }
                            counterCircles++;
                        }
                        if (str.Substring(1, 6) == "spline")
                        {
                            GetSplineFromString(str);
                            if (CounterSplines >= cS - 1)
                            {
                                cS += addCounterPoint;
                                Array.Resize(ref xS1, cS);
                                Array.Resize(ref yS1, cS);
                                Array.Resize(ref xS2, cS);
                                Array.Resize(ref yS2, cS);
                                Array.Resize(ref xS3, cS);
                                Array.Resize(ref yS3, cS);
                                Array.Resize(ref xS4, cS);
                                Array.Resize(ref yS4, cS);
                            }
                            counterSplines++;
                        }
                    }
                }
                file.Close();
            }
        }

        public void ClearBuffer()
        {
            xL1 = null;
            yL1 = null;
            xL2 = null;
            yL2 = null;
            xC = null;
            yC = null;
            radXC = null;
            radYC = null;
            alphaC = null;

            text = null;

            xS1 = null;
            yS1 = null;
            xS2 = null;
            yS2 = null;
            xS3 = null;
            yS3 = null;
            xS4 = null;
            yS4 = null;
        }
        #endregion

        #region PRIVATE METHODS
        private void GetLineFromString(string originalStr)
        {
            text = new string[countWords];
            int k = 0;
            string temp = "";
            for (int i = 0; i < originalStr.Length; ++i)
            {
                if (originalStr.Substring(i, 1) != " " && originalStr.Substring(i, 1) != ">")
                {
                    temp += originalStr.Substring(i, 1);
                }
                else
                {
                    text[k] = temp;
                    k++;
                    temp = "";
                }
            }

            for (int i = 0; i < text.Length; ++i)
            {
                if (text[i] != null)
                {
                    if (text[i].Substring(0, 2) == "x1")
                    {
                        xL1[CounterLines] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "y1")
                    {
                        yL1[CounterLines] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "x2")
                    {
                        xL2[CounterLines] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "y2")
                    {
                        yL2[CounterLines] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                }
            }
        }

        private void GetCircleFromString(string originalStr)
        {
            text = new string[countWords];
            int k = 0;
            string temp = "";
            for (int i = 0; i < originalStr.Length; ++i)
            {
                if (originalStr.Substring(i, 1) != " " && originalStr.Substring(i, 1) != ">")
                {
                    temp += originalStr.Substring(i, 1);
                }
                else
                {
                    text[k] = temp;
                    k++;
                    temp = "";
                }
            }

            for (int i = 0; i < text.Length; ++i)
            {
                if (text[i] != null)
                {
                    if (text[i].Substring(0, 2) == "cx")
                    {
                        xC[counterCircles] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "cy")
                    {
                        yC[counterCircles] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "rx")
                    {
                        radXC[counterCircles] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "ry")
                    {
                        radYC[counterCircles] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 1) == "a")
                    {
                        alphaC[counterCircles] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                }
            }
        }

        private void GetSplineFromString(string originalStr)
        {
            text = new string[countWords];
            int k = 0;
            string temp = "";
            for (int i = 0; i < originalStr.Length; ++i)
            {
                if (originalStr.Substring(i, 1) != " " && originalStr.Substring(i, 1) != ">")
                {
                    temp += originalStr.Substring(i, 1);
                }
                else
                {
                    text[k] = temp;
                    k++;
                    temp = "";
                }
            }

            for (int i = 0; i < text.Length; ++i)
            {
                if (text[i] != null)
                {
                    if (text[i].Substring(0, 2) == "x1")
                    {
                        xS1[CounterSplines] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "y1")
                    {
                        yS1[CounterSplines] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "x2")
                    {
                        xS2[CounterSplines] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "y2")
                    {
                        yS2[CounterSplines] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "x3")
                    {
                        xS3[CounterSplines] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "y3")
                    {
                        yS3[CounterSplines] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "x4")
                    {
                        xS4[CounterSplines] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "y4")
                    {
                        yS4[CounterSplines] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                }
            }
        }

        private void GetOriginPointSVG(string str)
        {
            string txt = "";
            string temp = "";
            int k = str.IndexOf('"');
            txt = str.Substring(k + 1, str.Length - k - 1);
            txt = txt.Substring(0, txt.IndexOf('"')) + " ";
            string[] test = new string[4];
            int z = 0;
            for (int i = 0; i < txt.Length; ++i)
            {
                if (txt.Substring(i, 1) != " ")
                {
                    temp += txt.Substring(i, 1);
                }
                else
                {
                    test[z] = temp;
                    z++;
                    temp = "";
                }
            }

            originX = Convert.ToSingle(test[2]);
            originY = Convert.ToSingle(test[3]);
        }

        private void GetOriginPointCPI(string str)
        {
            string txt = "";
            string temp = "";
            int k = str.IndexOf(' ');
            txt = str.Substring(k+1,str.Length- k-1);
            txt += " ";
            string[] test = new string[2];
            int z = 0;
            for (int i = 0;i<txt.Length;++i)
            {
                if (txt.Substring(i,1)!= " ")
                {
                    temp += txt.Substring(i, 1);
                }
                else
                {
                    test[z] = temp;
                    z++;
                    temp = "";
                }
            }
            originX = Convert.ToSingle(test[0]);
            originY = Convert.ToSingle(test[1]);
        }

        private string GetNumbFromString(string txt)
        {
            string tmp = "";
            int z = txt.IndexOf('"') + 1;
            while (txt.Substring(z, 1) != "\"")
            {
                if (txt.Substring(z, 1) == ".")
                    tmp += ",";
                else
                    tmp += txt.Substring(z, 1);
                z++;
            }
            return tmp;
        }

        #endregion
    }
}
