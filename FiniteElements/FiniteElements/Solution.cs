
namespace FiniteElements
{
    class Solution
    {
        public Matrix U { get; private set; }
        public Vector X { get; private set; }

        public Solution(Vector x, Vector u, Vector u_a, Vector u_b)
        {
            X = x;

            FormSolution(u, u_a, u_b);
        }
        

        void FormSolution(Vector u, Vector u_a, Vector u_b)
        {
            int s = u_a.N;
            int n = u.N / s + 1;

            U = new Matrix(n + 1, s);

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < s; j++)
                {
                    U[i + 1, j] = u[i * s + j];
                }
            }

            for (int j = 0; j < s; j++)
            {
                U[0, j] = u_a[ j];
                U[n, j] = u_b[j];
            }
        }
    }
}
