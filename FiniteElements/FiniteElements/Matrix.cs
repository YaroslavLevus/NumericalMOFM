using System;
using System.Collections.Generic;

namespace FiniteElements
{
    class Matrix
    {
        private double[,] matrix;
        public int NRows { get; private set; }
        public int NColumns { get; private set; }

        public Matrix()
        {

        }
        public Matrix(int r, int c)
        {
            matrix = new double[r, c];
            NRows = r;
            NColumns = c;
        }
        public Matrix(double[,] m, int r, int c)
        {
            matrix = new double[r, c];
            NRows = r;
            NColumns = c;
            Array.Copy(m, matrix, r * c);
        }

        public static Matrix Transpose(Matrix m)
        {
            Matrix temp = new Matrix(m.NColumns, m.NRows);

            for (int i = 0; i < m.NRows; i++)
            {
                for (int j = 0; j < m.NColumns; j++)
                {
                    temp[j, i] = m[i, j];
                }
            }

            return temp;
        }
        
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            int n = m1.NRows;
            int m = m2.NColumns;
            int z = m1.NColumns;

            Matrix temp = new Matrix(n, m);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    temp[i, j] = 0;
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    for (int k = 0; k < z; k++)
                    {
                        temp[i, j] += m1[i, k] * m2[k, j];
                    }
                }
            }

            return temp;
        }
        public static Matrix operator +(Matrix m1, Matrix m2)
        {
            int n = m1.NRows;
            int m = m1.NColumns;
            Matrix temp = new Matrix(n, m);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    temp[i, j] = m1[i, j] + m2[i, j];
                }
            }

            return temp;
        }
        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            int n = m1.NRows;
            int m = m1.NColumns;
            Matrix temp = new Matrix(n, m);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    temp[i, j] = m1[i, j] - m2[i, j];
                }
            }

            return temp;
        }
        public static Matrix operator *(Matrix m1, double k)
        {
            int n = m1.NRows;
            int m = m1.NColumns;
            Matrix temp = new Matrix(n, m);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    temp[i, j] = m1[i, j] * k;
                }
            }

            return temp;
        }
        public static Matrix operator *(double k, Matrix m1)
        {
            return m1 * k;
        }
        public static Matrix operator /(Matrix m1, double k)
        {
            return m1 * (1 / k);
        }

        public double this[int n, int m]
        {
            get
            {
                return this.matrix[n, m];
            }
            set
            {
                this.matrix[n, m] = value;
            }
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < NRows; i++)
            {
                for (int j = 0; j < NColumns; j++)
                {
                    s += this[i, j] + "   ";
                }
                s += "\r\n";
            }
            return s;
        }
    }
}
