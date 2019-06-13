using System;

using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SudokuSolverWPF
{
    public partial class MainWindow : Window
    {
        SudokuCell[,] cellGrid = new SudokuCell[9, 9];
        public MainWindow()
        {
            InitializeComponent();
            SetUpField();
        }
        

        //Sets up 9x9 grid for numbers
        private void SetUpField()
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    SudokuCell cell = new SudokuCell(i,j);
                    cell.TextChanged += CellChanged;
                    cellGrid[i, j] = cell;
                    Grid.SetRow(cellGrid[i,j], i);
                    Grid.SetColumn(cellGrid[i, j], j);
                    SudokuField.Children.Add(cellGrid[i, j]);
                }
            }
            solveButton.Click += SolveButtonEvent;          
        }

        //On text change in cell.
        //Checks for illegal input
        public void CellChanged(Object sender, TextChangedEventArgs e)
        {
            SudokuCell cell = sender as SudokuCell;
            String content = cell.Text;
            int row = cell.PosX;
            int col = cell.PosY;

            //If user deletes cell content, cell value is zero
            if (content == "")
            {
                cell.Value = 0;
                return;
            }

            //Checks if input is only one digit, from 1 to 9
            if (!Regex.IsMatch(content, "^[1-9]$"))
            {
                cell.Clear();
                return;
            }

            int val = Int16.Parse(content);

            //Checks if it is legal sudoku puzzle move
            if (!checkValue(row, col, val))
            {
                cell.Clear();
                return;
            }

            cell.Value = val;
            
        }


        //Removes cell changed event, so the solution won't be checked again while displaying 
        public void RemoveCellChangedEvent()
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    cellGrid[i, j].TextChanged -= CellChanged;
                }
            }
        }

        public bool CheckRow(int row, int x)
        {
            for(int i = 0; i < 9; i++)
            {
                if (cellGrid[row, i].Value == x) return false;
            }
            return true;
        }

        public bool CheckColumn(int col, int x)
        {
            for(int i=0; i<9 ;i++)
            {
                if (cellGrid[i, col].Value == x) return false;
            }
            return true;
        }

        public bool CheckSquare(int row, int col, int x)
        {
            int begginingRow = row - row % 3;
            int begginingCol = col - col % 3;

            for(int i = begginingRow; i < begginingRow + 3; i++)
            {
                for(int j = begginingCol; j < begginingCol+ 3; j++)
                {
                    if (cellGrid[i, j].Value == x) return false;
                }
            }

            return true;
        }

        public bool checkValue(int row, int col, int x)
        {
            return (CheckRow(row, x) && CheckColumn(col,x) && CheckSquare(row,col,x));
        }


        //Backtracking recursive algorithm for solving sudoku puzzles
        public bool Solve()
        {
            int row = -1;
            int col = -1;
            bool solved = true;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (cellGrid[i, j].Value == 0)
                    {
                        row = i;
                        col = j;
                        solved = false;
                        break;
                    }
                }
                if (!solved)
                {
                    break;
                }
            }

            if (solved)
            {
                return true;
            }

            for (int val = 1; val <= 9; val++)
            {
                if (checkValue(row, col, val))
                {
                    cellGrid[row, col].Value = val;
                    if (Solve())
                    {
                        return true;
                    }
                    else
                    {
                        cellGrid[row, col].Value = 0;
                    }
                }
            }
            return false;
        }

        //Paints user input cells red
        public void MarkNumbers()
        {
            for (int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    if(cellGrid[i,j].Value != 0)  cellGrid[i, j].Foreground = Brushes.Red;
                }
            }
        }


        //Displaying solution 
        public void PrintSolution()
        {
            for(int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    cellGrid[i, j].Text = cellGrid[i, j].Value.ToString();
                }
            }
        }

        private void SolveButtonEvent(Object sender, EventArgs e)
        {
            RemoveCellChangedEvent();
            MarkNumbers();
            SudokuField.IsEnabled = false;
            messages.Content = "SOLVING... THIS COULD TAKE A WHILE";
            if (Solve())
            {
                messages.Content = "PUZZLE SOLVED!";
                PrintSolution();
            }
            else messages.Content = "PUZZLE UNSOLVABLE";
            solveButton.Click += NewGame;
            solveButton.Content = "New puzzle";
        }

        private void NewGame(Object sen, EventArgs e)
        {
            messages.Content = "Enter numbers";
            cellGrid = new SudokuCell[9, 9];
            SetUpField();
            SudokuField.IsEnabled = true;
        }
    }
}
