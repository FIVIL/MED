using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MED
{
    public enum trak{
        //down
        deletion,
        //left
        insertion,
        //diag
        substitution
    }
    public class MED
    {
        public string Name { get => goal; }
        public int Value { get => D[0, jlenth - 1]; }
        public int[,] res { get => D; }
        public List<trak> ResPtr { get => ptr; }
        private int ilenth, jlenth;
        private string input { get; set; }
        private string goal { get; set; }
        private int Iindex(int i)
        {
            return (ilenth - 1) - i;
        }
        private int[,] D;
        private List<trak> ptr;
        public MED(string inp,string g)
        {
            input = inp;
            goal = g;
            ptr = new List<trak>();
            ilenth = input.Length + 1;
            jlenth = goal.Length + 1;
            D = new int[ilenth,jlenth];
            for (int i = 0; i < jlenth; i++)
            {
                D[ilenth - 1, i] = i;
            }
            for (int i = 0; i < ilenth; i++)
            {
                D[i, 0] = (ilenth - 1) - i;
            }
            Calculate();
        }
        private void Calculate()
        {
            for (int i = 1; i < ilenth; i++)
            {
                for (int j = 1; j < jlenth; j++)
                {
                    int a = Math.Min(D[Iindex(i - 1), j] + 1, D[Iindex(i), j - 1] + 1);
                    int value = input[i - 1] == goal[j - 1] ? 0 : 2;
                    int b = Math.Min(a, D[Iindex(i - 1), j - 1] + value);
                    D[Iindex(i), j] = b;
                }
            }
            BackTrace();
        }

        //private void BackTrace2()
        //{
        //    int i = 1, j = 1;
        //    BackTrace(i, j);
        //}
        //private void BackTrace2(int i, int j)
        //{
        //    //Console.WriteLine(D[i,j]);
        //    //int value = D[Iindex(i), j];
        //    if (i == ilenth - 1 && j == jlenth - 1) return;
        //    //if (value == 0) return;
        //    if (i == ilenth - 1) 
        //    {
        //        ptr.Add(trak.insertion);
        //        BackTrace2(i, j + 1);
        //        return;
        //    }
        //    if (j == jlenth - 1) 
        //    {
        //        ptr.Add(trak.deletion);
        //        BackTrace2(i + 1, j);
        //        return;
        //    }
        //    if (D[Iindex(i), j] == D[Iindex(i + 1), j + 1])
        //    {
        //        ptr.Add(trak.substitution);
        //        BackTrace2(i + 1, j + 1);
        //        return;
        //    }
        //    if (D[Iindex(i + 1), j] < D[Iindex(i), j + 1])
        //    {
        //        if (D[Iindex(i + 1), j + 1] < D[Iindex(i + 1), j])
        //        {
        //            ptr.Add(trak.substitution);
        //            BackTrace2(i + 1, j + 1);
        //        }
        //        else
        //        {
        //            ptr.Add(trak.deletion);
        //            BackTrace2(i + 1, j);
        //        }
        //    }
        //    else
        //    {
        //        if (D[Iindex(i + 1), j + 1] < D[Iindex(i), j + 1])
        //        {
        //            ptr.Add(trak.substitution);
        //            BackTrace2(i + 1, j + 1);
        //        }
        //        else
        //        {
        //            ptr.Add(trak.insertion);
        //            BackTrace2(i, j + 1);
        //        }
        //    }
        //}
        private void BackTrace()
        {
            int i = ilenth - 1, j = jlenth - 1;
            BackTrace(i, j);
        }
        private void BackTrace(int i, int j)
        {
            //Console.WriteLine(D[i,j]);
            //int value = D[Iindex(i), j];
            //if (value == 0) return;
            if (i == 0 && j == 0) return;
            if (i == 0)
            {
                ptr.Add(trak.insertion);
                BackTrace(i, j - 1);
                return;
            }
            if (j == 0)
            {
                ptr.Add(trak.deletion);
                BackTrace(i - 1, j);
                return;
            }
            if ((ptr.Count > 0) && ptr.Last() != trak.substitution)
            {
                if (D[Iindex(i), j] == D[Iindex(i - 1), j - 1])
                {
                    ptr.Add(trak.substitution);
                    BackTrace(i - 1, j - 1);
                    return;
                }
            }
            if (D[Iindex(i - 1), j] < D[Iindex(i), j - 1])
            {
                if (D[Iindex(i - 1), j - 1] < D[Iindex(i - 1), j])
                {
                    ptr.Add(trak.substitution);
                    BackTrace(i - 1, j - 1);
                }
                else
                {
                    ptr.Add(trak.deletion);
                    BackTrace(i - 1, j);
                }
            }
            else
            {
                if (D[Iindex(i - 1), j - 1] < D[Iindex(i), j - 1]) 
                {
                    ptr.Add(trak.substitution);
                    BackTrace(i - 1, j - 1);
                }
                else
                {
                    ptr.Add(trak.insertion);
                    BackTrace(i, j - 1);
                }
            }
        }

    }
}
