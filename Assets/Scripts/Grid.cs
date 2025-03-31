using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private int _sideSize = 8;

        [SerializeField] List<Transform> _rows;
        private Cell[,] _cells;

        private int[,] _grid;

        [SerializeField] private Shape _shape;
        [SerializeField] private ShapeRow _row;

        private List<Vector2Int> _tintedList = new List<Vector2Int>();

        private List<Vector2Int> _toTintList = new List<Vector2Int>();

        private bool _canPlaceShape = false;
        public List<Vector2Int> TintList => _toTintList;
        public List<Vector2Int> TintedList => _tintedList;

        private Camera _cam;
        [SerializeField] private float _shapeOffset;
        [SerializeField] private float _tintDistance = 0.3f;
        [SerializeField] private bool _enableTouchInput;

        void Awake()
        {
            InitCellsArray();
        }

        void Start()
        {
            _cam = Camera.main;
            _row.OnShapeClicked += SetShape;
        }

        [ContextMenu("Get Cells")]
        public void Init()
        {
            InitCellsArray();
        }

        private void InitCellsArray()
        {
            _cells = new Cell[_sideSize, _sideSize];
            _grid = new int[_sideSize, _sideSize];
            for (int y = 0; y < _sideSize; y++)
            {
                var row = _rows[y];
                var children = row.GetComponentsInChildren<Cell>();

                for (int x = 0; x < _sideSize; x++)
                {
                    var cell = children[x];
                    _cells[x, y] = cell;
                    // _grid[x, y] = (int)FillType.Empty;
                    cell.name = $"Cell {x}, {y}";
                }
            }
        }

        [ContextMenu("Log Grid")]
        private void LogGrid()
        {
            var builder = new StringBuilder();
            for (int y = 0; y < _sideSize; y++)
            {
                builder.Append("\n[");
                for (int x = 0; x < _sideSize; x++)
                {
                    builder.Append($"{_grid[x, y]}");
                    if (x < _sideSize - 1) builder.Append(","); else builder.Append("]");
                }
            }
            Debug.Log($"{builder}");
        }


        public void TintCell(int x, int y)
        {

            _cells[x, y].Tint();
            _tintedList.Add(new Vector2Int(x, y));
            _grid[x, y] = (int)FillType.Tinted;

        }


        public void UntintCell(int x, int y)
        {
            _cells[x, y].Untint();
            _grid[x, y] = (int)FillType.Empty;

        }


        [ContextMenu("Paint closest cell")]
        public void TintShape()
        {
            UntintShape();
            List<(Vector2Int, Transform)> toTintCells = new List<(Vector2Int, Transform)>();
            for (int i = 0; i < _shape.Tiles.Count; i++)
            {
                var shapeCell = _shape.Tiles[i];
                var closestRow = GetClosestRow(shapeCell.transform);
                var closestCellCoord = GetClosestCell(shapeCell.transform, closestRow);
                var (x, y) = closestCellCoord;

                toTintCells.Add((new Vector2Int(x, y), _shape.Tiles[i].transform));

            }

            foreach (var pair in toTintCells)
            {
                var cell = pair.Item1;
                var shapeCell = pair.Item2;
                var isAccessible = AddToTintList(cell.x, cell.y, shapeCell);

                if (!isAccessible)
                {
                    _canPlaceShape = false;

                    UntintShape();
                    _toTintList.Clear();
                    return;
                }
            }

            _canPlaceShape = true;

            foreach (var position in _toTintList)
                TintCell(position.x, position.y);

            _toTintList.Clear();
        }


        private bool AddToTintList(int x, int y, Transform shapeCell)
        {
            if (_toTintList.Contains(new Vector2Int(x, y)))
            {
                Debug.Log($"Element is already added to tint list:{x},{y}");
                return false;
            }

            var gridCell = _cells[x, y];
            if (!gridCell.IsEmpty)
                return false;


            // Debug.Log($"Comparing {cell.position}, worldPos:{} to {new Vector2(x, y)}");
            var distance = Vector2.Distance(gridCell.transform.position, shapeCell.position);

            if (distance > _tintDistance)
            {
                Debug.Log($"Cell is too far,distance:{distance}");
                return false;
            }


            _toTintList.Add(new Vector2Int(x, y));
            return true;

        }



        [ContextMenu("Place Shape")]
        private void PlaceShape()
        {


            foreach (var position in _tintedList)
            {
                _cells[position.x, position.y].PlaceTile();
                _grid[position.x, position.y] = (int)FillType.Placed;
            }


            _toTintList.Clear();
            _tintedList.Clear();

        }

        private void UntintShape()
        {
            if (_tintedList.Count == 0) return;
            for (int i = 0; i < _tintedList.Count; i++)
            {
                var position = _tintedList[i];
                UntintCell(position.x, position.y);

            }
            _tintedList.Clear();


        }

        private void Update()
        {
#if UNITY_EDITOR
            if (!_enableTouchInput) return;
#endif

            if (Input.touchCount == 0 || _shape == null)
                return;

            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                var newShapePos = ReadInput(touch);
                _shape.transform.position = newShapePos + new Vector2(0, _shapeOffset);
            }

            if (touch.phase == TouchPhase.Moved)
            {

                var newShapePos = ReadInput(touch);

                _shape.transform.position = newShapePos + new Vector2(0, _shapeOffset);
                TintShape();

                if (!_canPlaceShape) return;

                var (rows, columns) = CheckMatch();
                ShowMatches(rows, columns);


            }


            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                FinishDrag();

                if (!_canPlaceShape) return;

                var (rows, columns) = CheckMatch();
                HandleMatches(rows, columns);

            }

        }

        private void ShowMatches(List<int> rows, List<int> columns)
        {
           
        }

        private void HandleMatches(List<int> rows, List<int> columns)
        {
           
        }

        private (List<int>, List<int>) CheckMatch()
        {
            List<int> rows = new List<int>();
            List<int> columns = new List<int>();

            for (int y = 0; y < _sideSize; y++)
                if (CheckRowMatch(y)) rows.Add(y);

            for (int x = 0; x < _sideSize; x++)
                if (CheckColumnMatch(x)) columns.Add(x);

            if (rows.Count > 0 || columns.Count > 0)
                Debug.Log($"Found matches in rows:{string.Join(",", rows)} and columns:{string.Join(",", columns)}");

            return (rows, columns);

        }


        private bool CheckRowMatch(int y)
        {
            for (int x = 0; x < _sideSize; x++)
                if (_grid[x, y] == (int)FillType.Empty) return false;
            return true;
        }

        private bool CheckColumnMatch(int x)
        {

            for (int y = 0; y < _sideSize; y++)
                if (_grid[x, y] == (int)FillType.Empty) return false;
            return true;
        }

        private void FinishDrag()
        {
            if (_canPlaceShape)
            {
                PlaceShape();
                Destroy(_shape.gameObject);

            }
            else
            {
                _row.ReturnShape(_shape);
            }

            _shape = null;
            _toTintList.Clear();
            _tintedList.Clear();
        }

        public void SetShape(Shape shape)
        {
            _shape = shape;
            _shape.transform.parent = transform;
            _shape.transform.localScale = Vector3.one;
        }

        private Vector2 ReadInput(Touch touch)
        {

            var screenPos = new Vector3(touch.position.x, touch.position.y, _cam.transform.position.z);
            Vector2 worldPosition = _cam.ScreenToWorldPoint(screenPos);

            return worldPosition;
        }


        private (int, int) GetClosestCell(Transform targetCell, int closestRow)
        {
            var closestCell = 0;
            var closestDistance = Mathf.Infinity;

            for (int i = 0; i < _sideSize; i++)
            {
                var cell = _cells[i, closestRow];
                var distance = Vector2.Distance(new Vector2(cell.transform.position.x, 0), new Vector2(targetCell.position.x, 0));
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCell = i;
                }
            }
            return (closestCell, closestRow);
        }

        private int GetClosestRow(Transform targetCell)
        {

            var closestRow = 0;
            var closestDistance = Mathf.Infinity;
            for (int i = 0; i < _rows.Count; i++)
            {
                var row = _rows[i];

                var distance = Vector2.Distance(new Vector2(0, row.position.y), new Vector2(0, targetCell.position.y));
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestRow = i;
                }
            }
            return closestRow;
        }
    }
}

public enum FillType
{
    Empty,
    Tinted,
    Placed
}
