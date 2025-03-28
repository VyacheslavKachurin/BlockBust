using UnityEngine;


namespace Assets.Scripts
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _childRenderer;
        [SerializeField] private Sprite _dummy;
        [SerializeField] private Sprite _base;

        private bool _isEmpty = true;
        private bool _isTinted = false;
        [SerializeField] private Color _placedColor = Color.gray;

        public bool IsEmpty => _isEmpty;


        public void Untint()
        {
            _childRenderer.enabled = false;
            _childRenderer.sprite = null;
            _isTinted = false;
        }

        public void Tint(Sprite sprite = null)
        {

            _childRenderer.sprite = sprite ?? _dummy;
            _childRenderer.enabled = true;
            _isTinted = true;
        }


        public void PlaceTile(Sprite sprite = null)
        {
            _childRenderer.enabled = true;
            _childRenderer.sprite = sprite ?? _dummy;
            _childRenderer.color = _placedColor;
            _isEmpty = false;

        }

        [ContextMenu("Tell World Space")]
        public void TellWorldSpace()
        {
            var worldSpace = transform.position;
            Debug.Log($"world space is {worldSpace}");
        }

        [ContextMenu("Set Tile")]
        public void PlaceTileCommand()
        {
            PlaceTile();
        }

        [ContextMenu("Reset Tile")]
        public void ClearTile()
        {
            _childRenderer.enabled = false;
            _childRenderer.sprite = _base;
            _isEmpty = true;
        }
    }
}