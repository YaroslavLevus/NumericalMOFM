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
            n = N;
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
        
    }
}
