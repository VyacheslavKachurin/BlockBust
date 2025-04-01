using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private ShapeGenerator _shapeGenerator;
        [SerializeField] private Grid _grid;
        [SerializeField] private ShapeRow _row;

        [SerializeField] private ScoreReference _scoreReference;

        private int _currentScore = 0;
        public int CurrentScore => _currentScore;


        private void Start()
        {
            EventManager.OnShapePlaced += HandleShapePlaced;
            EventManager.OnLineCleared += HandleLineCleared;

            PopulateRow();
        }

        private void OnDestroy()
        {
            EventManager.OnShapePlaced -= HandleShapePlaced;
            EventManager.OnLineCleared -= HandleLineCleared;
        }

        private void HandleLineCleared(int tiles, int lines)
        {
            AddScore(tiles, _scoreReference.LineMultiplier);
        }

        private void HandleShapePlaced(Shape shape)
        {
            if (_row.FreeSlots == _row.SpawnPointsAmount)
                PopulateRow();

            AddScore(shape.TileCount, _scoreReference.TileMultiplier);
        }

        private void AddScore(int tileCount, int multiplier)
        {
            var scoreToAdd = tileCount * multiplier;
            _currentScore += scoreToAdd;
            Debug.Log($"Added {scoreToAdd} to score \n Current Score: {_currentScore}");
        }



        [ContextMenu("Populate Row")]
        private void PopulateRow()
        {
            var count = _row.FreeSlots;
            for (int i = 0; i < count; i++)
            {
                var shape = _shapeGenerator.GetRandomSquare();
                shape.Index = -1;
                _row.AddShape(shape);
            }

        }
    }
}