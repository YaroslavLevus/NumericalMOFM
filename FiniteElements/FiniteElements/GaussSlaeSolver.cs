using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteElements
{
    class GaussSlaeSolver
    {
        const double eps = 1e-8;
        static Vector X;
        static int n;

        static double sum(Matrix A, int k)
        {
            double temp = 0;
            for (int j = k + 1; j < n; j++)
            {
                temp += A[k, j] * X[j];
            }
            return temp;
        }
        static bool change(Matrix A, Vector B, int k)
        {
            bool flag = true;
            double max;
            int m = 0;
            max = Math.Abs(A[k, k]);

            for (int i = k + 1; i < n; i++)
            {
                if (Math.Abs(A[i, k]) > max)
                {
                    max = Math.Abs(A[i, k]);
                    m = i;
                }
            }

            if (max <= eps)
            {
                flag = false;
            }

            else if (max != Math.Abs(A[k, k]))
            {
                for (int j = k; j < n; j++)
                {
                    max = A[k, j];
                    A[k, j] = A[m, j];
                    A[m, j] = max;
                }

                max = B[k];
                B[k] = B[m];
                B[m] = max;
            }

            return flag;
        }

        public static Vector Solve(Matrix A, Vector b)
        {
            X = new Vector(b.N);
            
            n = b.N;

            // прямий хід
            Matrix M = new Matrix(n, n);

            for (int k = 0; k < n - 1; k++)
            {
                change(A, b, k);

                for (int i = k + 1; i < n; i++)
                {
                    M[i, k] = -(A[i, k] / A[k, k]);
                    b[i] += M[i, k] * b[k];

                    for (int j = k + 1; j < n; j++)
                    {
                        A[i, j] += M[i, k] * A[k, j];
                    }
                }

                for (int z = k + 1; z < n; z++)
                {
                    A[z, k] = 0;
                }
            }

            // зворотній хід
            if (A[n - 1, n - 1] != 0)
            {
                X[n - 1] = b[n - 1] / A[n - 1, n - 1];

                for (int k = n - 2; k >= 0; k--)
                {
                    X[k] = (b[k] - sum(A, k)) / A[k, k];
                }

                return X;
            }

            return null;
        }
    }
}
