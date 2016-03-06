using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FiniteElements
{
    class Problem
    {
        public int s { get; private set; }

        public double a { get; private set; }
        public double b { get; private set; }

        public Vector u_a { get; private set; }
        public Vector u_b { get; private set; }

        public Matrix P { get; private set; }
        public Matrix Q { get; private set; }

        public Vector f { get; private set; }

        public Problem()
        {

        }
    }
}
