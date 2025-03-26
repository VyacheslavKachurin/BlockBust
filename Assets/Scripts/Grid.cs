using System;
using System.Collections.Generic;

using UnityEngine;


public class Grid : MonoBehaviour
{
    [SerializeField] private int _sideSize = 8;

    [SerializeField] List<Transform> _rows;
    private Cell[,] _cells;

    [SerializeField] private int _cell_X = 0;
    [SerializeField] private int _cell_Y = 0;

    [SerializeField] private float _distance = 0.8f;

    [SerializeField] private Shape _target;

    private List<Vector2> _lastPositions = new List<Vector2>();

    private List<Vector2> _populateList = new List<Vector2>();

    void Awake()
    {
        InitCellsArray();
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
            }
        }
    }


    public void TintCell(int x, int y)
    {
        _cells[x, y].Tint();
        _lastPositions.Add(new Vector2(x, y));
    }


    public void UntintCell(int x, int y)
    {
        _cells[x, y].Untint();
        _lastPositions.Remove(new Vector2(x, y));
    }


    [ContextMenu("Paint closest cell")]
    public void TintClosestCell()
    {
        UntintLastCells();
        for (int i = 0; i < _target.Tiles.Count; i++)
        {
            var targetCell = _target.Tiles[i];
            var closestRow = GetClosestRow(targetCell);
            var closestCell = GetClosestCell(targetCell, closestRow);
            var x = closestCell;
            var y = closestRow;

            // PopulateCell(x, y);
            var isAccessible = AddCellToPopulateList(x, y);

            if (!isAccessible)
            {
                Debug.Log($"Cell {x}, {y} is not accessible");
                UntintLastCells();
                return;
            }

        }
        foreach (var position in _populateList)
            TintCell((int)position.x, (int)position.y);
    }

    private bool AddCellToPopulateList(int x, int y)
    {
        if (_populateList.Contains(new Vector2(x, y)))
            return false;
        if (!_cells[x, y].IsEmpty)
            return false;

        _populateList.Add(new Vector2(x, y));
        return true;

    }

    private void UntintLastCells()
    {
        for (int i = 0; i < _lastPositions.Count; i++)
        {
            var position = _lastPositions[i];
            UntintCell((int)position.x, (int)position.y);
        }
        _populateList.Clear();
    }

    private void Update()
    {
        /*
        if (Input.touchCount == 0)
            return;
            */

//        var touch = Input.GetTouch(0);

        TintClosestCell();
    }


    private int GetClosestCell(Transform targetCell, int closestRow)
    {
        var closestCell = 0;
        var closestDistance = Mathf.Infinity;
        var row = _rows[closestRow];

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
        return closestCell;
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
