using System;
using System.Collections.Generic;

using UnityEngine;

namespace Assets.Scripts
{
    public class Grid : MonoBehaviour
    {
        [SerializeField] private int _sideSize = 8;

        [SerializeField] List<Transform> _rows;
        private Cell[,] _cells;

        [SerializeField] private Shape _shape;
        [SerializeField] private BlueprintRow _row;

        private List<Vector2Int> _tintedList = new List<Vector2Int>();

        private List<Vector2Int> _toTintList = new List<Vector2Int>();

        private bool _canPlaceShape = false;
        public List<Vector2Int> TintList => _toTintList;
        public List<Vector2Int> TintedList => _tintedList;

        private Camera _cam;
        [SerializeField] private float _shapeOffset;
        [SerializeField] private float _tintDistance = 0.3f;
        [SerializeField] private bool _isDebugDrag;

        void Awake()
        {
            InitCellsArray();
        }

        void Start()
        {
            _cam = Camera.main;
        }

        [ContextMenu("Get Cells")]
        public void Init()
        {
            InitCellsArray();
        }

        private void InitCellsArray()
        {
            _cells = new Cell[_sideSize, _sideSize];
            for (int y = 0; y < _sideSize; y++)
            {
                var row = _rows[y];
                var children = row.GetComponentsInChildren<Cell>();

                for (int x = 0; x < _sideSize; x++)
                {
                    var cell = children[x];
                    _cells[x, y] = cell;
                    cell.name = $"Cell {x}, {y}";
                }
            }
        }


        public void TintCell(int x, int y)
        {

            _cells[x, y].Tint();
            _tintedList.Add(new Vector2Int(x, y));

        }


        public void UntintCell(int x, int y)
        {
            _cells[x, y].Untint();

        }


        [ContextMenu("Paint closest cell")]
        public void TintShape()
        {
            UntintShape();
            List<(Vector2Int, Transform)> toTintCells = new List<(Vector2Int, Transform)>();
            for (int i = 0; i < _shape.Tiles.Count; i++)
            {
                var shapeCell = _shape.Tiles[i];
                var closestRow = GetClosestRow(shapeCell);
                var closestCellCoord = GetClosestCell(shapeCell, closestRow);
                var (x, y) = closestCellCoord;

                toTintCells.Add((new Vector2Int(x, y), _shape.Tiles[i]));

            }

            foreach (var pair in toTintCells)
            {
                var cell = pair.Item1;
                var shapeCell = pair.Item2;
                var isAccessible = AddToTintList(cell.x, cell.y, shapeCell);

                if (!isAccessible)
                {
                    _canPlaceShape = false;
                    Debug.Log($"Cell {cell.x}, {cell.y} is not accessible");
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
            {
                Debug.Log($"Cell is not empty: {x},{y}");
                return false;
            }


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
            if (!_canPlaceShape)
            {
                Debug.Log("Cannot place shape");
                return;
            }
            Debug.Log($"Placing shape");

            foreach (var position in _toTintList)
                _cells[position.x, position.y].PlaceTile();

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
                TintShape();

                var newShapePos = ReadInput(touch);

                _shape.transform.position = newShapePos + new Vector2(0, _shapeOffset);

            }


            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                FinishDrag();




        }

        private void FinishDrag()
        {
            foreach (var cell in _tintedList)
                _cells[cell.x, cell.y].PlaceTile();
            Destroy(_shape.gameObject);
            _shape = null;
            // UntintShape();
            _row.RestoreBlueprints();
            _toTintList.Clear();
            _tintedList.Clear();
        }

        public void SetShape(Shape shape) => _shape = shape;

        private Vector2 ReadInput(Touch touch)
        {

            var screenPos = new Vector3(touch.position.x, touch.position.y, _cam.transform.position.z);
            Vector2 worldPosition = _cam.ScreenToWorldPoint(screenPos);

            Debug.Log($"New shape position: {worldPosition}");
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
