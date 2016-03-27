using System;
using System.Collections.Generic;

namespace FiniteElements
{
    class Vector
    {
        private double[] vector;
        public int N { get; private set; }

        public Vector()
        {

        }

        public Vector(int n)
        {
            N = n;

            vector = new double[N];
        }

        public Vector (double[] v, int n)
        {
            N = n;

            vector = v;
        }

        public double this[int n]
        {
            get
            {
                return vector[n];
            }
            set
            {
                vector[n] = value;
            }
        }

        public override string ToString()
        {
            string s = "";

            for (int i = 0; i < N; i++)
            {
                s += this[i] + "   ";
            }

            return s;
        }

        public bool IsZero()
        {
            bool flag = true;

            for (int i = 0; i < N; i++)
            {
                if (Math.Abs(vector[i]) < 1e-8)
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }
    }
}
