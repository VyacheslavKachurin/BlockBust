using System.Linq;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public static class Extensions
    {

        public static void PopulateGrid(this int[,] grid, int value)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    grid[x, y] = value;
                }
            }
        }

        public static void Bind<T>(this Label lbl, T dataSource, PropertyPath propertyPath)
        {


            var binding = new DataBinding
            {
                dataSource = dataSource,
                dataSourcePath = propertyPath,
                bindingMode = BindingMode.ToTarget
            };

            lbl.SetBinding(nameof(Label.text), binding);
        }

        public static class CustomArray<T>
        {
            public static T[] GetColumn(T[,] matrix, int columnNumber)
            {
                return Enumerable.Range(0, matrix.GetLength(0))
                        .Select(x => matrix[x, columnNumber])
                        .ToArray();
            }

            public static T[] GetRow(T[,] matrix, int rowNumber)
            {
                return Enumerable.Range(0, matrix.GetLength(1))
                        .Select(x => matrix[rowNumber, x])
                        .ToArray();
            }
        }
    }
}