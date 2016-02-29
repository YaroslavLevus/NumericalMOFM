using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteElements
{
    class Problem
    {
        public int s { get; }

        public double a { get; }
        public double b { get; }

        public Matrix u_a { get; }
        public Matrix u_b { get; }

        public Matrix P { get; }
        public Matrix Q { get; }
        public Matrix f { get; }

        public Problem()
        {

        }
    }
}
