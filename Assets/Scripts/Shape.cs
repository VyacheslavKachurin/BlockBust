using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Shape : MonoBehaviour
    {
        [SerializeField] private List<Transform> _tiles;
        [SerializeField] private Transform _shapeCell;
        [SerializeField] private Transform _gridCell;
        public List<Transform> Tiles => _tiles;


        [ContextMenu("Tell Cell World Space")]
        private void TellCellWorldSpace()
        {
            var worldSpace = _shapeCell.position;
            Debug.Log($"world space is {worldSpace}");
        }

        [ContextMenu("Get Distance between shape cell and grid cell")]
        private void GetDistance()
        {
            var distance = Vector2.Distance(_shapeCell.position, _gridCell.position);
            Debug.Log($"Distance is {distance}");
        }
    }
}
