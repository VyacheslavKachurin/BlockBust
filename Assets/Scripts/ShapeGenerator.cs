using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class ShapeGenerator : MonoBehaviour
    {
        [SerializeField] private Tile _tilePrefab;
        [SerializeField] private float _step = 0.25f;
        [SerializeField] private int _width = 3;
        [SerializeField] private int _height = 3;

        [SerializeField] private Shape _squarePrefab;
        [SerializeField] private bool _getSquaresOnly;

        /*
                [ContextMenu("Generate Shape Grid")]
                public int[,] GenerateShapeGrid()
                {
                    //possible shapes
                    // 3x3
                    //1x4
                    //1x5
                    //get 4x4 shape

                    int[,] shapeGrid = new int[_width, _height];

                    shapeGrid.PopulateGrid(1);

                    var randomX = Random.Range(0, shapeGrid.GetLength(0));
                    var randomY = Random.Range(0, shapeGrid.GetLength(1));

                    shapeGrid[randomX, randomY] = 0;
                    LogShape(shapeGrid);
                    return shapeGrid;
                }

                private void LogShape(int[,] shapeGrid)
                {
                    StringBuilder builder = new StringBuilder();
                    for (int y = 0; y < shapeGrid.GetLength(0); y++)
                    {
                        builder.Append("\n[");
                        for (int x = 0; x < shapeGrid.GetLength(1); x++)
                        {
                            builder.Append(shapeGrid[x, y]);
                            if (x < shapeGrid.GetLength(1) - 1)
                                builder.Append(",");
                        }
                        builder.Append("]\n");
                    }
                    Debug.Log($"{builder}");
                }

                public void CreateShape(int[,] shapeGrid)
                {
                    var parent = new GameObject("Shape");
                    parent.transform.position = Vector3.zero;

                    for (int y = 0; y < shapeGrid.GetLength(0); y++)
                        for (int x = 0; x < shapeGrid.GetLength(1); x++)
                        {
                            var tile = Instantiate(_tilePrefab, new Vector3(0, 0, 0), Quaternion.identity, parent.transform);
                            tile.transform.localPosition = new Vector3(x * _step, y * _step, 0);
                            tile.gameObject.name = $"Tile {x}, {y}";
                        }
                }
        */
        public Shape GetRandomSquare()
        {
            var square = Instantiate(_squarePrefab, new Vector3(0, 0, 0), Quaternion.identity);

            if (!_getSquaresOnly)
            {
                var tileIndex = Random.Range(0, 4);
                square.Tiles[tileIndex].gameObject.SetActive(false);
                square.Tiles.RemoveAt(tileIndex);
            }

            return square;
        }
    }
}

