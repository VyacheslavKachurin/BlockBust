using UnityEngine;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private ShapeGenerator _shapeGenerator;
        [SerializeField] private Grid _grid;
        [SerializeField] private ShapeRow _row;


        private void Start()
        {

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