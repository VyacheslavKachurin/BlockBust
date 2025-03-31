using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class ShapeRow : MonoBehaviour
    {
        public event Action<Shape> OnShapeClicked;

        private Shape[] _shapes = new Shape[3];

        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private float _scale = 0.5f;
        public int SpawnPointsAmount => _spawnPoints.Count;

        public int FreeSlots => _spawnPoints.Count - _shapes.Where(x => x != null).Count(); //recount too much, maybe do another way
        [SerializeField] private Transform _shapesParent;

        public void AddShape(Shape shape)
        {
            var emptyIndex = shape.Index == -1 ? Array.FindIndex(_shapes, x => x == null) : shape.Index;
            _shapes[emptyIndex] = shape;
            shape.OnClicked += HandleClick;
            shape.transform.position = _spawnPoints[emptyIndex].position;
            shape.transform.localScale = new Vector3(_scale, _scale, _scale);
            shape.transform.parent = _shapesParent;
            shape.Index = emptyIndex;
        }



        private void HandleClick(Shape shape)
        {
            OnShapeClicked?.Invoke(shape);//does it fire two times when returned back?
            shape.OnClicked -= HandleClick;
        }

        internal void ReturnShape(Shape shape)
        {
            AddShape(shape);
        }
    }

}