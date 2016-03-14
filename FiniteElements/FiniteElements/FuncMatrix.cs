using System;

namespace FiniteElements
{
    class FuncMatrix
    {
        private Delegate[,] matrix;
        public int NRows { get; private set; }
        public int NColumns { get; private set; }

        public FuncMatrix()
        {

        }
        public FuncMatrix(int r, int c)
        {
            matrix = new Delegate[r, c];
            NRows = r;
            NColumns = c;
        }

        public Delegate this[int n, int m]
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
    }
}
