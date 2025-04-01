using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Shape : MonoBehaviour
    {
        public event Action<Shape> OnClicked;
        public event Action<Shape> OnPlaced;

        [SerializeField] private List<Tile> _tiles;
        [SerializeField] private Transform _shapeCell;
        [SerializeField] private Transform _gridCell;
        public List<Tile> Tiles => _tiles;
        private BoxCollider2D _boxCollider;

        private int _index;
        public int Index { get => _index; set => _index = value; }

        public void SetCollider(int width, int height)
        {
            _boxCollider = GetComponent<BoxCollider2D>();
            //set size;
        }


        public void RaisePlacedEvent()
        {
            OnPlaced?.Invoke(this);
        }



        private void OnMouseDown()
        {
            OnClicked?.Invoke(this);
        }
    }
}
