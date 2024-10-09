using UnityEngine;

namespace Drawer
{
    public class Pixel : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boxCollider;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;
        public void SetCollider(Vector2 size) => boxCollider.size = size;
    }
}