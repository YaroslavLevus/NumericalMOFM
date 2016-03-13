using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteElements
{
    class GaussSlaeSolver
    {
        const double eps = 1e-8;

        static bool changeForRank(Matrix Y, int[] ind, int k, int z)
        {
            int n = Y.NRows;
            int m = Y.NColumns;
            bool flag = true;
            double max = Math.Abs(Y[k, z]);
            int l = 0;

            for (int i = k + 1; i < n; i++)
                if (Math.Abs(Y[i, z]) > max)
                {
                    max = Math.Abs(Y[i, z]);
                    l = i;
                }

            if (max <= eps) flag = false;
            else if (max != Math.Abs(Y[k, z]))
            {
                ind[k] = l;
                ind[l] = k;
                for (int j = k; j < m; j++)
                {
                    max = Y[k, j];
                    Y[k, j] = Y[l, j];
                    Y[l, j] = max;
                }
            }

            return flag;
        }
        public static Matrix ChooseMaxRankRows(Matrix Y, params int[] indexes)
        {
            // прямий хід
            int n = Y.NRows;
            int m = Y.NColumns;
            double M = 0;
            int z = 0;
            Matrix temp;
            int koef = n > m ? m : m - 1;
            Matrix clone = new Matrix(n, m);
            //Array.Copy(Y.matrix, clone.matrix, n * m);   ------------------------------!!!!!!!!!!
            int[] res = new int[n];
            for (int i = 0; i < n; i++)
                res[i] = i;

            for (int k = 0; k < n - 1; k++)
            {
                while ((z < m) && (!changeForRank(clone, res, k, z)))
                {
                    z++;
                }
                if (z < koef)
                {
                    for (int i = k + 1; i < n; i++)
                    {
                        M = -(clone[i, z] / clone[k, z]);
                        for (int j = z; j < m; j++)
                            clone[i, j] += M * clone[k, j];
                    }
                    for (int w = k + 1; w < n; w++)
                        clone[w, k] = 0;
                    z++;
                }
            }

            bool flag;
            int count = 0;

            for (int i = 0; i < n; i++)
            {
                flag = true;
                for (int j = 0; j < m; j++)
                {
                    if (Math.Abs(clone[i, j]) > eps)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    res[i] = -1;
                    count++;
                }
            }
            if (indexes.Length != 0)
                Array.Copy(res, indexes, res.Length);
            temp = new Matrix(n - count, m);
            for (int i = 0; i < n; i++)
            {
                if (res[i] != -1)
                    for (int j = 0; j < m; j++)
                    {
                        temp[i, j] = Y[res[i], j];
                    }
            }

            return temp;
        }

        static double sumForM(Matrix Y, Matrix result, int i, int j)
        {
            double temp = 0;
            for (int k = i + 1; k < Y.NRows; k++)
                temp += Y[i, k] * result[k, j];
            return temp;
        }
        static bool changeForM(Matrix Y, Matrix C, int k)
        {
            bool flag = true;
            double max;
            int m = 0;
            int n = Y.NRows;
            max = Math.Abs(Y[k, k]);

            for (int i = k + 1; i < n; i++)
                if (Math.Abs(Y[i, k]) > max)
                {
                    max = Math.Abs(Y[i, k]);
                    m = i;
                }

            if (max <= eps) flag = false;
            else if (max != Math.Abs(Y[k, k]))
            {
                for (int j = k; j < n; j++)
                {
                    max = Y[k, j];
                    Y[k, j] = Y[m, j];
                    Y[m, j] = max;
                    max = C[k, j];
                    C[k, j] = C[m, j];
                    C[m, j] = max;
                }
            }

            return flag;
        }
        public static Matrix SolveSquareMarixEquation(Matrix Y, Matrix B)
        {
            //прямий хід
            double M = 0;
            int n = Y.NRows;
            Matrix result = new Matrix(n, n);
            Matrix A = new Matrix(n, n);
            //Array.Copy(Y.matrix, A.matrix, n * n);         -----------------------!!!!!!

            for (int k = 0; k < n - 1; k++)
            {
                changeForM(A, B, k);
                for (int i = k + 1; i < n; i++)
                {
                    M = -(A[i, k] / A[k, k]);
                    for (int j = k + 1; j < n; j++)
                    {
                        A[i, j] += M * A[k, j];
                    }
                    for (int j = 0; j < n; j++)
                    {
                        B[i, j] += M * B[k, j];
                    }
                }
                for (int z = k + 1; z < n; z++) A[z, k] = 0;
            }
            Console.WriteLine(A);
            Console.WriteLine(B);

            //звортній хід
            for (int i = 0; i < n; i++)
            {
                result[n - 1, i] = B[n - 1, i] / A[n - 1, n - 1];
                for (int k = n - 2; k >= 0; k--)
                    result[k, i] = (B[k, i] - sumForM(A, result, k, i)) / A[k, k];
            }
            Console.WriteLine(result);
            Console.WriteLine(Y * result);

            return result;
        }
    }
}
