using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteElements
{
    class Solver
    {
        public int n { get; }

        public Problem p { get; private set; }
        public Solution s { get; private set; }

        public Vector X { get; private set; }

        public Matrix GlobalMatrix { get; private set; }
        Matrix GlobalVector;
        
        public Solver()
        {

        }

        double baseFunction(double x, double x1, double x2)
        {
            return (x - x1) * (x - x2);
        }
        double baseFunctionFake(double x, double x1, double x2)
        {
            return 1;
        }

        double CompositeSimpsonIntegral(Func<double, double, double> baseFunc, double koef)
        {

            return 0;
        }
                
        public Matrix GetLocalStiffnessMatrix(int k)
        {

            return null;
        }

        public Matrix GetLocalWeightMatrix(int k)
        {

            return null;
        }        

        public void FillGlobalMatrix()
        {
                        
        }
        public void FillSystem()
        {

        }

        public void Solve()
        {

        }
    }
}
