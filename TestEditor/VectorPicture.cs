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
        public struct LoadLines
        {
            public float x1, y1, x2, y2;
            public int layer;
        }

        public struct LoadSpline
        {
            public float x1, y1, x2, y2, x3, y3, x4, y4;
            public int layer;
        }

        public struct LoadEllipse
        {
            public float x, y, radX, radY, alpha;
            public int layer;
        }


        #region VARIABLES
        private const short addCounterPoint = 2;    // Увеличение размера массива
        private const short countWords = 10;        // Число слов в строке
        private float originX, originY;

        private string[] text;                      // Контейнер для текста
        private LoadLines[] lines;
        private LoadSpline[] splines;
        private LoadEllipse[] ellipses;
        private bool haveLayer = false;

        private int counterLines = 0;
        private int counterCircles = 0;
        private int counterSplines = 0;
        private int cL = addCounterPoint;     // Максимальное количество точек
        private int cC = addCounterPoint;
        private int cS = addCounterPoint;
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

        public LoadLines[] Lines
        {
            get { return lines; }
        }

        public LoadEllipse[] Ellipses
        {
            get { return ellipses; }
        }
    
        public LoadSpline[] Splines
        {
            get { return splines; }
        }
        #endregion

        #region PUBLIC METHODS
        public void LoadPicture(string path)
        {
            lines = new LoadLines[cL];
            ellipses = new LoadEllipse[cC];
            splines = new LoadSpline[cS];
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
                                    Array.Resize(ref lines, cL);
                                }
                                counterLines++;
                            }
                            if (str.Substring(1,6) == "circle")
                            {
                                GetCircleFromString(str);
                                if (CounterCircles >= cC - 1)
                                {
                                    cC += addCounterPoint;
                                    Array.Resize(ref ellipses, cC);
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
                                Array.Resize(ref lines, cL);
                            }
                            counterLines++;
                        }
                        if (str.Substring(1, 6) == "circle")
                        {
                            GetCircleFromString(str);
                            if (CounterCircles >= cC - 1)
                            {
                                cC += addCounterPoint;
                                Array.Resize(ref ellipses, cC);
                            }
                            counterCircles++;
                        }
                        if (str.Substring(1, 6) == "spline")
                        {
                            GetSplineFromString(str);
                            if (CounterSplines >= cS - 1)
                            {
                                cS += addCounterPoint;
                                Array.Resize(ref splines, cS);
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
            lines = null;
            ellipses = null;
            text = null;
            splines = null;
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
                        lines[CounterLines].x1 = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "y1")
                    {
                        lines[CounterLines].y1 = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "x2")
                    {
                        lines[CounterLines].x2 = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "y2")
                    {
                        lines[CounterLines].y2 = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0,1) == "l")
                    {
                        lines[counterLines].layer = Convert.ToInt16(GetNumbFromString(text[i]));
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
                        ellipses[CounterCircles].x = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "cy")
                    {
                        ellipses[CounterCircles].y = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "rx")
                    {
                        ellipses[CounterCircles].radX = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "ry")
                    {
                        ellipses[CounterCircles].radY = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 1) == "a")
                    {
                        ellipses[CounterCircles].alpha = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 1) == "l")
                    {
                        ellipses[CounterCircles].layer = Convert.ToInt16(GetNumbFromString(text[i]));
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
                        splines[CounterSplines].x1 = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "y1")
                    {
                        splines[CounterSplines].y1 = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "x2")
                    {
                        splines[CounterSplines].x2 = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "y2")
                    {
                        splines[CounterSplines].y2 = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "x3")
                    {
                        splines[CounterSplines].x3 = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "y3")
                    {
                        splines[CounterSplines].y3 = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "x4")
                    {
                        splines[CounterSplines].x4 = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "y4")
                    {
                        splines[CounterSplines].y4 = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 1) == "l")
                    {
                        splines[CounterSplines].layer = Convert.ToInt16(GetNumbFromString(text[i]));
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
