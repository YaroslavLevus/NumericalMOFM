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

        Matrix GlobalMatrix;
        Vector GlobalVector;
        
        public Solver()
        {

        }
        public Solver(int n, Problem p)
        {
            this.n = n;
            this.p = p;

            CreateDivision();
        }

        void CreateDivision()
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
        double baseFunctionVec(double x, double x1)
        {
            return (x - x1);
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
                    m[i + p.s, j] = m[i, j + p.s] = -temp;
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
                    m[i + p.s, j] = m[i, j + p.s] = -temp;
                }
            }

            return m;
        }

        public Vector GetLocalVector(int k)
        {
            Vector v = new Vector(2 * p.s);
            double temp;

            for (int i = 0; i < p.s; i++)
            {
                temp = GaussIntegrator.Integrate(X[k - 1], X[k], baseFunctionVec, p.f[i, 0], X[k - 1]);
                v[i] = -temp / h;
                v[i + p.s] = temp / h;
            }

            return v;
        }

        public Matrix GetGlobalMatrix()
        {
            if (GlobalMatrix == null)
            {
                FillSystem();
            }

            return GlobalMatrix;
        }
        public Vector GetGlobalVector()
        {
            if (GlobalVector == null)
            {
                FillSystem();
            }

            return GlobalVector;
        }

        void IncludeLeftBoundaryValue(Matrix s,Matrix w, Vector b)
        {

        }
        void IncludeRightBoundaryValue(Matrix s, Matrix w, Vector b)
        {

        }

        void FillSystem()
        {
            GlobalMatrix = new Matrix(p.s * (n - 1), p.s * (n - 1));
            GlobalVector = new Vector(p.s * (n - 1));

            int shift;
            Matrix stiffness, weight;
            Vector b;

            stiffness = GetLocalStiffnessMatrix(1);
            weight = GetLocalWeightMatrix(1);
            b = GetLocalVector(1);

            for (int i = 0; i < p.s; i++)
            {
                for (int j = 0; j < p.s; j++)
                {
                    GlobalMatrix[i, j] += stiffness[i + p.s, j + p.s] + weight[i + p.s, j + p.s];
                }

                GlobalVector[i] += b[i + p.s];
            }

            if(!p.u_a.IsZero())
            {
                IncludeLeftBoundaryValue(stiffness,weight,b);
            }

            for (int k = 2; k < n; k++)
            {
                shift = (k - 2) * p.s;

                stiffness = GetLocalStiffnessMatrix(k);
                weight = GetLocalWeightMatrix(k);
                b = GetLocalVector(k);

                for (int i = 0; i < 2 * p.s; i++)
                {
                    for (int j = 0; j < 2 * p.s; j++)
                    {
                        GlobalMatrix[i + shift, j + shift] += stiffness[i, j] + weight[i, j];
                    }

                    GlobalVector[i + shift] += b[i];
                }
            }
        }

        public void Solve()
        {
            FillSystem();

            var res = GaussSlaeSolver.Solve(GlobalMatrix, GlobalVector);

            s = new Solution(X, res, p.u_a, p.u_b);
        }

        internal void Clear()
        {
            CreateDivision();
            GlobalMatrix = null;
            GlobalVector = null;
            s = null;
        }
    }
}
