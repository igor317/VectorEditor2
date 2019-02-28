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
        #region VARIABLES
        private const short addCounterPoint = 2;    // Увеличение размера массива
        private const short countWords = 10;        // Число слов в строке

        private string[] text;                      // Контейнер для текста
        private float[] x1, y1, x2, y2;             // Массив координат
        private int counter = 0;                    // Счетчик
        private float originX, originY;             // Родные координаты
        private int CounterPoint = addCounterPoint; // Максимальное количество точек
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
        public int Counter
        {
            get { return counter; }
        }

        public float GetX1(int pos)
        {
            return x1[pos];
        }
        public float GetY1(int pos)
        {
            return y1[pos];
        }
        public float GetX2(int pos)
        {
            return x2[pos];
        }
        public float GetY2(int pos)
        {
            return y2[pos];
        }

        #endregion

        #region PUBLIC METHODS
        public void LoadPicture(string path)
        {
            x1 = new float[CounterPoint];
            y1 = new float[CounterPoint];
            x2 = new float[CounterPoint];
            y2 = new float[CounterPoint];
            string str;
            counter = 0;
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
                                GetFunctionFromString(str);
                                if (counter >= CounterPoint - 1)
                                {
                                    CounterPoint += addCounterPoint;
                                    Array.Resize(ref x1, CounterPoint);
                                    Array.Resize(ref y1, CounterPoint);
                                    Array.Resize(ref x2, CounterPoint);
                                    Array.Resize(ref y2, CounterPoint);
                                }
                                counter++;
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
                            GetFunctionFromString(str);
                            if (counter >= CounterPoint - 1)
                            {
                                CounterPoint += addCounterPoint;
                                Array.Resize(ref x1, CounterPoint);
                                Array.Resize(ref y1, CounterPoint);
                                Array.Resize(ref x2, CounterPoint);
                                Array.Resize(ref y2, CounterPoint);
                            }
                            counter++;
                        }
                    }
                }
                file.Close();
            }
        }

        public void ClearBuffer()
        {
            x1 = null;
            y1 = null;
            x2 = null;
            y2 = null;
            text = null;
        }
        #endregion

        #region PRIVATE METHODS
        private void GetFunctionFromString(string originalStr)
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
                        x1[counter] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "y1")
                    {
                        y1[counter] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "x2")
                    {
                        x2[counter] = Convert.ToSingle(GetNumbFromString(text[i]));
                    }
                    if (text[i].Substring(0, 2) == "y2")
                    {
                        y2[counter] = Convert.ToSingle(GetNumbFromString(text[i]));
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
