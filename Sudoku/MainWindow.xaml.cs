using System;
using System.Windows;
using SudokuSolver;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Solver mySolver;
        private int[,] data;
        private DataTable dataTable;
        const int boardSize = 9;
        private bool loaded;

        public MainWindow()
        {
            InitializeComponent();

            defaultTable();
            loaded = false;
            data = new int[boardSize, boardSize];
        }

        private void solveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (loaded)
                if (!mySolver.Solve(data))
                    textBox.Text = "Unsolvable board";
        }

        /// <summary>
        /// Allows the user to browse for a sudoku board stored in a CSV file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "CSV files|*.csv|All Files|*.*";
            DialogResult result = fileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    StreamReader reader = new StreamReader(fileDialog.FileName);
                    for (int row = 0; row < boardSize; row++)
                    {

                        string[] nextLine = reader.ReadLine().Split(',');
                        for (int col = 0; col < boardSize; col++)
                        {
                            if (!String.IsNullOrEmpty(nextLine[col]))
                                data[row, col] = Int32.Parse(nextLine[col]);
                        }
                    }
                    textBox.Text = "";
                    loaded = true;
                    createTable();
                }
                catch
                {
                    loaded = false;
                    textBox.Text = "Invalid board";
                }
            }
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            createTable();
        }

        /// <summary>
        /// Creates an empty 9x9 DataTable and binds it to dataGrid.
        /// </summary>
        private void defaultTable()
        {
            dataTable = new DataTable();

            for (int col = 0; col < boardSize; col++)
            {
                dataTable.Columns.Add(col.ToString());
            }

            for (int row = 0; row < boardSize; row++)
            {
                DataRow dr = dataTable.NewRow();
                for (int col = 0; col < boardSize; col++)
                {
                    dr[col] = "";
                }
                dataTable.Rows.Add(dr);
            }

            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        /// <summary>
        /// Fills a DataTable with the loaded data and sends a reference to the Solver.
        /// Binds the DataTable to dataGrid.
        /// </summary>
        private void createTable()
        {
            if (!loaded)
                return;

            dataTable = new DataTable();
            mySolver = new Solver(dataTable);

            for (int col = 0; col < boardSize; col++)
            {
                dataTable.Columns.Add(col.ToString());
            }

            for (int row = 0; row < boardSize; row++)
            {
                DataRow dr = dataTable.NewRow();
                for (int col = 0; col < boardSize; col++)
                {
                    if (data[row, col] == 0)
                        dr[col] = "";
                    else
                        dr[col] = data[row, col];
                }
                dataTable.Rows.Add(dr);
            }

            dataGrid.ItemsSource = dataTable.DefaultView;
        }


    }
}
