using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuGame
{
    internal class SudokuUtility
    {
        public static bool HasUniqueSolution(int[,] grid)
        {
            int[,] puzzleCopy = new int[9, 9];
            Array.Copy(grid, puzzleCopy, grid.Length);

            return SolvePuzzle(puzzleCopy);
        }

        public static void ShuffleArray(int[] array)
        {
            Random rand = new Random();
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        public static void FillUpperLeft3x3(int[,] grid)
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            ShuffleArray(numbers);

            int rowStart = 0;
            int colStart = 0;

            for (int i = 0; i < 9; i++)
            {
                int row = rowStart + i / 3;
                int col = colStart + i % 3;
                grid[row, col] = numbers[i];
            }
        }

        public static void RemoveNumbers(int[,] grid, int count)
        {
            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                int row = rand.Next(0, 9);
                int col = rand.Next(0, 9);

                while (grid[row, col] == 0)
                {
                    row = rand.Next(0, 9);
                    col = rand.Next(0, 9);
                }

                int backup = grid[row, col];
                grid[row, col] = 0;

                int[,] puzzleCopy = new int[9, 9];
                Array.Copy(grid, puzzleCopy, grid.Length);

                if (!HasUniqueSolution(puzzleCopy))
                {
                    grid[row, col] = backup;
                }
            }
        }

        public static bool IsSafe(int[,] grid, int row, int col, int num)
        {
            for (int i = 0; i < 9; i++)
            {
                if (grid[row, i] == num || grid[i, col] == num ||
                    grid[row - row % 3 + i / 3, col - col % 3 + i % 3] == num)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool SolvePuzzle(int[,] grid)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (grid[row, col] == 0)
                    {
                        for (int num = 1; num <= 9; num++)
                        {
                            if (IsSafe(grid, row, col, num))
                            {
                                grid[row, col] = num;

                                if (SolvePuzzle(grid))
                                {
                                    return true;
                                }

                                grid[row, col] = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsInvalidConfiguration(int[,] grid)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    int num = grid[row, col];

                    if (num > 0)
                    {
                        for (int c = 0; c < 9; c++)
                        {
                            if (c != col && grid[row, c] == num)
                            {
                                return true;
                            }
                        }

                        for (int r = 0; r < 9; r++)
                        {
                            if (r != row && grid[r, col] == num)
                            {
                                return true;
                            }
                        }

                        int subgridStartRow = 3 * (row / 3);
                        int subgridStartCol = 3 * (col / 3);
                        for (int r = subgridStartRow; r < subgridStartRow + 3; r++)
                        {
                            for (int c = subgridStartCol; c < subgridStartCol + 3; c++)
                            {
                                if ((r != row || c != col) && grid[r, c] == num)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
