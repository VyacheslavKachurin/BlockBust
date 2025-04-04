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
        [SerializeField] private Color _tintColor;

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


        [ContextMenu("Set Tile")]
        public void PlaceTileCommand()
        {
            PlaceTile();
        }

        [ContextMenu("Reset Tile")]
        public void ClearTile()
        {
            Debug.Log($"Clearing tile");
            _childRenderer.enabled = false;
            _childRenderer.sprite = _base;
            _childRenderer.color = _tintColor;
            _isEmpty = true;
        }
    }
}