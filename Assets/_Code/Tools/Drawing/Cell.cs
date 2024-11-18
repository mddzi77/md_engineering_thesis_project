using UnityEngine;

namespace Tools.Drawing
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public void SetSprite(Sprite sprite) => spriteRenderer.sprite = sprite;
    }
}