using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Collections.Generic;
using ZedGraph;

namespace FiniteElements
{
    public partial class Form1 : Form
    {
        Solver solver;

        List<Color> ColourList = new List<Color>
        {
            Color.LimeGreen,
            Color.DodgerBlue,
            Color.OrangeRed,
            Color.DarkViolet,
            Color.DeepPink,
            Color.DeepSkyBlue,
            Color.Orange,
            Color.Silver
        };

        #region function template
        private static string begin = @"using System;
namespace MyNamespace
{
    public delegate double Del(double x);
    public static class LambdaCreator
    {
        public static Del Create()
        {
            return (x)=>";
        private static string end = @";
        }
    }
}";
        #endregion

        public Form1()
        {
            InitializeComponent();

            int dim = (int)numericUpDown1.Value;

            GenerateGrid(dim, dim, dataGridView1, 75);
            GenerateGrid(dim, dim, dataGridView2, 75);
            GenerateGrid(1, dim, dataGridView3, 200);
            GenerateGrid(1, dim, dataGridView6, 75);
            GenerateGrid(1, dim, dataGridView7, 75);
        }

        void CreateSystem(DataGridView dataGrid, ref FuncMatrix syst)
        {  
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            parameters.ReferencedAssemblies.Add("System.dll");
            CompilerResults results;
            for (int i = 0; i < dataGrid.Rows.Count; i++)
            {
                for (int j = 0; j < dataGrid.Columns.Count; j++)
                {
                    results = provider.CompileAssemblyFromSource(parameters, begin + dataGrid[j, i].Value.ToString() + end);
                    var cls = results.CompiledAssembly.GetType("MyNamespace.LambdaCreator");
                    var method = cls.GetMethod("Create", BindingFlags.Static | BindingFlags.Public);
                    var del = (method.Invoke(null, null) as Delegate);
                    syst[i, j] = del;
                }
            }
        }

        // generate grid dimX * DimY
        void GenerateGrid(int dimX, int dimY, DataGridView dataGrid, int columnWidth, bool fillZeros = true)
        {
            dataGrid.Rows.Clear();
            dataGrid.Columns.Clear();

            for (int i = 0; i < dimX; i++)
            {
                var column = new DataGridViewTextBoxColumn();
                column.Width = columnWidth;
                dataGrid.Columns.Add(column);
            }

            for (int i = 0; i < dimY; i++)
            {
                dataGrid.Rows.Add(new DataGridViewRow());

                if (fillZeros)
                {
                    for (int j = 0; j < dimX; j++)
                    {
                        dataGrid[j, i].Value = 0;
                    }
                }
            }
        }

        void ReadFromMatrixToGrid(Matrix matrix, DataGridView dataGrid)
        {
            for (int i = 0; i < matrix.NColumns; i++)
            {
                for (int j = 0; j < matrix.NRows; j++)
                {
                    dataGrid[i, j].Value = matrix[j, i];
                }
            }
        }

        void ReadFromGridToMatrix(DataGridView dataGrid, out FuncMatrix matrix)
        {
            int cols = dataGrid.Columns.Count;
            int rows = dataGrid.Rows.Count;

            matrix = new FuncMatrix(rows, cols);

            CreateSystem(dataGrid, ref matrix);
        }

        void ReadFromGridToVector(DataGridView dataGrid, out Vector vector)
        {
            int rows = dataGrid.Rows.Count;

            vector = new Vector(rows);

            for (int j = 0; j < rows; j++)
            {
                vector[j] = double.Parse(dataGrid[0, j].Value.ToString());
            }
        }

        void ReadFromVectorToGrid(Vector vector, DataGridView dataGrid)
        {
            for (int j = 0; j < vector.N; j++)
            {
                dataGrid[0, j].Value = vector[j];
            }
        }

        void ReadInputData()
        {
            int s = (int)numericUpDown1.Value;
            int n = (int)numericUpDown2.Value;

            double a = double.Parse(textBox3.Text);
            double b = double.Parse(textBox4.Text);

            FuncMatrix P, Q;
            ReadFromGridToMatrix(dataGridView1, out P);
            ReadFromGridToMatrix(dataGridView2, out Q);

            FuncMatrix f;
            ReadFromGridToMatrix(dataGridView3, out f);

            Vector u_a, u_b;            
            ReadFromGridToVector(dataGridView6, out u_a);
            ReadFromGridToVector(dataGridView7, out u_b);

            solver = new Solver(n, new Problem(s, a, b, u_a, u_b, P, Q, f));
        }
        
        // clear all results computed, read data and create the problem.
        private void button1_Click(object sender, EventArgs e)
        {
            ReadInputData();
            
            GenerateGrid(2 * solver.p.s, 2 * solver.p.s, dataGridView4, 75, false);
            GenerateGrid(2 * solver.p.s, 2 * solver.p.s, dataGridView5, 75, false);

            GenerateGrid((solver.n - 1) * solver.p.s, (solver.n - 1) * solver.p.s, dataGridView8, 75, false);
            GenerateGrid(1, (solver.n - 1) * solver.p.s, dataGridView9, 75, false);

            numericUpDown3.Maximum = solver.n;
        }

        // compute and show appropriate local matrices
        private void button2_Click(object sender, EventArgs e)
        {
            ReadFromMatrixToGrid(solver.GetLocalStiffnessMatrix((int)numericUpDown3.Value), dataGridView4);
            ReadFromMatrixToGrid(solver.GetLocalWeightMatrix((int)numericUpDown3.Value), dataGridView5);
        }
        // compute and show global matrix and vector
        private void button4_Click(object sender, EventArgs e)
        {
            ReadFromMatrixToGrid(solver.GetGlobalMatrix(), dataGridView8);
            ReadFromVectorToGrid(solver.GetGlobalVector(), dataGridView9);
        }

        // on text changed generate empty grids
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int dim = (int)numericUpDown1.Value;

            GenerateGrid(dim, dim, dataGridView1, 75);
            GenerateGrid(dim, dim, dataGridView2, 75);
            GenerateGrid(1, dim, dataGridView3, 200);
            GenerateGrid(1, dim, dataGridView6, 75);
            GenerateGrid(1, dim, dataGridView7, 75);

            ClearFormForDimension();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (solver != null)
            {
                solver.n = (int)numericUpDown2.Value;

                solver.Clear();

                ClearFormForDivision();
            }
        }

        void ClearFormForDimension()
        {
            dataGridView4.Rows.Clear();
            dataGridView4.Columns.Clear();
            dataGridView5.Rows.Clear();
            dataGridView5.Columns.Clear();

            // in future: clear tabs "global matrix" and "solution"
            dataGridView8.Rows.Clear();
            dataGridView8.Columns.Clear();
            dataGridView9.Rows.Clear();
            dataGridView9.Columns.Clear();

        }
        void ClearFormForDivision()
        {
            GenerateGrid(2 * solver.p.s, 2 * solver.p.s, dataGridView4, 75, false);
            GenerateGrid(2 * solver.p.s, 2 * solver.p.s, dataGridView5, 75, false);

            numericUpDown3.Maximum = solver.n;

            // in future: clear tabs "global matrix" and "solution"

            GenerateGrid((solver.n - 1) * solver.p.s, (solver.n - 1) * solver.p.s, dataGridView8, 75, false);
            GenerateGrid(1, (solver.n - 1) * solver.p.s, dataGridView9, 75, false);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            solver.Solve();
            DrawGraph();
        }
        void DrawGraph()
        {
            GraphPane pane = new GraphPane();
            pane = zedGraphControl1.GraphPane; 
            
            pane.CurveList.Clear();

            List<PointPairList> list = new List<PointPairList>();
            for(int i=0;i<solver.p.s;i++)
            {
                list.Add(new PointPairList());
            }

            //for (double i = solver.p.a; i <= solver.p.b; i += 0.05)
            //{
            //    list.Add(i, 0);//interpolate
            //}
            for(int i=0;i<solver.p.s;i++)
            {
                for(int j=0;j<=solver.n;j++)
                {
                    list[i].Add(solver.s.X[j], solver.s.U[j, i]);
                }

            }

            List<LineItem> myCurves = new List<LineItem>();
            for (int i = 0; i < solver.p.s; i++)
            {
                myCurves.Add(pane.AddCurve("", list[i], ColourList[i], SymbolType.None));
            }
            pane.Title.Text = "Graph";
            pane.XAxis.Title.Text = "x";
            pane.YAxis.Title.Text = "u(x)";

            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();             
        }
    }
}
