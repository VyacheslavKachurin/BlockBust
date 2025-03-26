using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _childRenderer;
    [SerializeField] private Sprite _dummy;
    [SerializeField] private Sprite _base;

    private bool _isEmpty = true;
    public bool IsEmpty => _isEmpty;


    public void Untint()
    {
        _childRenderer.enabled = false;
        _childRenderer.sprite = null;
    }

    public void Tint(Sprite sprite = null)
    {

        _childRenderer.sprite = sprite ?? _dummy;
        _childRenderer.enabled = true;
    }

    [ContextMenu("Set Tile")]
    public void PlaceTile(Sprite sprite = null)
    {
        _childRenderer.enabled = true;
        _childRenderer.sprite = sprite ?? _dummy;
        _isEmpty = false;

    }

    [ContextMenu("Reset Tile")]
    public void ClearTile()
    {
        _childRenderer.enabled = false;
        _childRenderer.sprite = _base;
        _isEmpty = true;
    }
}