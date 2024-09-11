using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class WordFinder
    {
        private readonly char[][] matrix;
        private readonly int rows;
        private readonly int cols;
        private readonly HashSet<string> horizontalWords;
        private readonly HashSet<string> verticalWords;

        public WordFinder(IEnumerable<string> matrix)
        {
            //initial data setup
            this.matrix = matrix.Select(row => row.ToCharArray()).ToArray();
            this.rows = this.matrix.Length;
            this.cols = this.matrix[0].Length;

            //Check if conditions are met
            if (rows > 64 || cols > 64)
            {
                throw new ArgumentException("Matrix size should not exceed 64 x 64");
            }

            if (this.matrix.Any(row => row.Length != cols))
            {
                throw new ArgumentException("All matrix strings must have the same length");
            }

            horizontalWords = new HashSet<string>();
            verticalWords = new HashSet<string>();
            PreprocessMatrix();
        }

        private void PreprocessMatrix()
        {

            //Preprocess horizontal words
            //Complexity: O(n^2 * m) where n is the number of rows and m is the number of columns
            for (int i = 0; i < rows; i++)
            {
                string row = new string(matrix[i]);
                for (int len = 1; len <= cols; len++)
                {
                    for (int j = 0; j <= cols - len; j++)
                    {
                        horizontalWords.Add(row.Substring(j, len));
                    }
                }
            }

            //Preprocess vertical words
            //Complexity: O(n * m^2) where n is the number of rows and m is the number of columns
            for (int j = 0; j < cols; j++)
            {
                char[] column = new char[rows];
                for (int i = 0; i < rows; i++)
                {
                    column[i] = matrix[i][j];
                }
                string col = new string(column);
                for (int len = 1; len <= rows; len++)
                {
                    for (int i = 0; i <= rows - len; i++)
                    {
                        verticalWords.Add(col.Substring(i, len));
                    }
                }
            }
            //So total complexity: O(n^2 * m + n * m^2) where n is the number of rows and m is the number of columns
        }

        public IEnumerable<string> Find(IEnumerable<string> wordStream)
        {
            var wordCounts = new Dictionary<string, int>();

            foreach (var word in wordStream.Distinct())
            {
                if (horizontalWords.Contains(word) || verticalWords.Contains(word))
                {
                    wordCounts[word] = wordCounts.TryGetValue(word, out int count) ? count + 1 : 1;
                }
            }

            //return the top 10 most frequent words
            return wordCounts.OrderByDescending(k => k.Value)
                             .Take(10)
                             .Select(k => k.Key);
        }
    }
}
