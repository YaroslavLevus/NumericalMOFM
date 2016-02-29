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

        public Matrix GlobalMatrix { get; private set; }
        Matrix GlobalVector;

        public Solver()
        {

        }
                
        public Matrix GetLocalStiffnessMatrix(int k)
        {

            return null;
        }

        public Matrix GetLocalWeightMatrix(int k)
        {

            return null;
        }

        public void FillSystem()
        {

        }

        public void FillGlobalMatrix()
        {
                        
        }

        public void Solve()
        {

        }
    }
}
