using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FiniteElements
{
    public partial class Form1 : Form
    {
        Solver solver;

        public Form1()
        {
            InitializeComponent();
        }

        // generate grid dimX*DimY
        void GenerateGrid(int dimX, int dimY, DataGrid dataGrid)
        {

        }

        void ReadFromMatrixToGrid(Matrix matrix, DataGrid dataGrid)
        {

        }

        void ReadFromGridToMatrix(DataGrid dataGrid, Matrix matrix)
        {

        }

        void ReadInputData(Solver s)
        {

        }

        // on text changed generate empty grids
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        // clear all results computed, read data and create the problem. if input data is not correct show the message what's wrong
        private void button1_Click(object sender, EventArgs e)
        {

        }

        // compute and show appropriate local matrices
        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
