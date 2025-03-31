using UnityEngine;

namespace Assets.Scripts
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;

        public void SetSprite(Sprite sprite) => _renderer.sprite = sprite;

    }
}