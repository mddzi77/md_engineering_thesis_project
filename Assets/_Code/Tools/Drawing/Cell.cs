using UnityEngine;

namespace Tools.Drawing
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;
        public void SetCollider(Vector2 size) => boxCollider.size = size;
    }
}