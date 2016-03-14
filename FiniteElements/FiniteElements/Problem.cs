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

        public FuncMatrix P { get; private set; }
        public FuncMatrix Q { get; private set; }

        public FuncMatrix f { get; private set; }

        public Problem()
        {

        }

        public Problem(int s, double a, double b, Vector u_a, Vector u_b, FuncMatrix p, FuncMatrix q, FuncMatrix f)
        {
            this.s = s;

            this.a = a;
            this.b = b;

            this.u_a = u_a;
            this.u_b = u_b;

            P = p;
            Q = q;

            this.f = f;
        }
    }
}
