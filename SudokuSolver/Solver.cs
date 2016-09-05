using System;
using System.Data;
using System.Diagnostics;

namespace SudokuSolver
{
    public class Solver
    {
        private int[,] solution;
        private int boardSize = 9;
        private DataTable dataTable;

        public Solver(DataTable newDT)
        {
            dataTable = newDT;
        }

        /// <summary>
        /// Uses depth-first search to determin a solution using mutual recursion.
        /// Only one solution will be found.
        /// </summary>
        /// <returns></returns>
        public bool Solve(int[,] newBoard)
        {
            if (newBoard != null)
            {
                solution = newBoard.Clone() as int[,];

                if (IsValidBoard())
                {
                    FindEmpty();
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Finds the first empty cell and calls SolveEmpty to find a value.
        /// Mutually recursive with FillEmpty
        /// </summary>
        /// <returns>True when all cells in board are filled</returns>
        private bool FindEmpty()
        {
            for (int row = 0; row < boardSize; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    if (solution[row, col] == 0)
                    {
                        return FillEmpty(row, col);
                    }
                }
            }

            
            return true;
        }

        /// <summary>
        /// Attempts to fill a cell with a valid number between 1-9 using IsValid.
        /// If successful, calls FindEmpty to find the next empty cell.
        /// Mutually recursive with FindEmpty.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>Returns true if valid number is found, false if not</returns>
        private bool FillEmpty(int row, int col)
        {
            for (int val = 1; val <= boardSize; val++)
            {
                if (IsValid(val, row, col))
                {
                    solution[row, col] = val;
                    dataTable.Rows[row][col] = solution[row, col];
                    if (FindEmpty())
                    {
                        return true;
                    }
                }
            }

            // No valid number, cell is reset and call returns to previous FindEmpty.
            solution[row, col] = 0;
            return false;
        }

        /// <summary>
        /// Checks if the Board is valid by calling IsValid on all cells.
        /// A valid board has only unique values in each row, column and square.
        /// </summary>
        /// <returns>True if board is valid</returns>
        private bool IsValidBoard()
        {
            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    if (!IsValid(solution[x, y], x, y))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks to see if the cell is valid by calling CheckRow, CheckCol and CheckSquare.
        /// A valid board has only unique values in each row, column and square.
        /// </summary>
        /// <returns>True if cell is valid</returns>
        private bool IsValid(int cellVal, int row, int col)
        {
            if (!CheckRow(cellVal, row, col) || !CheckCol(cellVal, row, col) || !CheckSquare(cellVal, row, col))
                return false;
            return true;
        }

        private bool CheckRow(int cellVal, int row, int col)
        {
            for (int i = 0; i < boardSize; i++)
            {
                if (i == col)
                    continue;
                if (cellVal != 0)
                    if (solution[row, i] == cellVal)
                    {
                        return false;
                    }
            }
            return true;
        }

        private bool CheckCol(int cellVal, int row, int col)
        {
            for (int i = 0; i < boardSize; i++)
            {
                if (i == row)
                    continue;
                if (cellVal != 0)
                    if (solution[i, col] == cellVal)
                        return false;
            }

            return true;
        }

        private bool CheckSquare(int cellVal, int row, int col)
        {
            int startRow = (row / 3) * 3;
            int startCol = (col / 3) * 3;

            for (int x = startRow; x < startRow + 3; x++)
            {
                for (int y = startCol; y < startCol + 3; y++)
                {
                    if (x == row && y == col)
                        continue;
                    if (cellVal != 0)
                        if (solution[x, y] == cellVal)
                            return false;
                }
            }

            return true;
        }
    }

}

