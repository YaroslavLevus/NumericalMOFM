using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteElements
{
    class Solver
    {
        public int n { get; set; }
        public double h { get; private set; }
        
        public Problem p { get; private set; }
        public Solution s { get; private set; }

        public Vector X { get; private set; }

        public Matrix GlobalMatrix { get; private set; }
        Matrix GlobalVector;
        
        public Solver()
        {

        }
        public Solver(int n, Problem p)
        {
            this.n = n;
            this.p = p;

            CreateDivision();
        }

        public void CreateDivision()
        {
            double[] temp = new double[n + 1];

            h = (p.b - p.a) / n;

            temp[0] = p.a;
            for (int i = 1; i < n; i++)
            {
                temp[i] = p.a + i * h;
            }
            temp[temp.Length - 1] = p.b;

            X = new Vector(temp, n + 1);           
        }

        double baseFunction(double x, double x1)
        {
            return (x - x1) * (x - x1);
        }
        double baseFunctionFake(double x, double x1)
        {
            return 1;
        }
                        
        public Matrix GetLocalStiffnessMatrix(int k)
        {
            Matrix m = new Matrix(2 * p.s, 2 * p.s);
            double temp;

            for (int i = 0; i < p.s; i++)
            {
                for (int j = 0; j < p.s; j++)
                {
                    temp = GaussIntegrator.Integrate(X[k - 1], X[k], baseFunctionFake, p.P[i, j], X[k - 1]) / (h * h);
                    m[i, j] = m[i + p.s, j + p.s] = temp;
                    m[i, j + p.s] = m[i + p.s, j] = -temp;
                }
            }

            return m;
        }

        public Matrix GetLocalWeightMatrix(int k)
        {
            Matrix m = new Matrix(2 * p.s, 2 * p.s);
            double temp;

            for (int i = 0; i < p.s; i++)
            {
                for (int j = 0; j < p.s; j++)
                {
                    temp = GaussIntegrator.Integrate(X[k - 1], X[k], baseFunction, p.Q[i, j], X[k - 1]) / (h * h);
                    m[i, j] = m[i + p.s, j + p.s] = temp;
                    m[i, j + p.s] = m[i + p.s, j] = -temp;
                }
            }

            return m;
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
