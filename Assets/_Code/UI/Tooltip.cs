using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace UI
{
    public class Tooltip: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform background;
        [SerializeField] private TMP_Text tooltipText;
        [SerializeField] private float timeToDisplay = 1f;
        [SerializeField] private float fadeSpeed = 0.6f;
        
        private bool _isMouseOver;
        private bool _isShown;
        private float _timer;
        private Vector2 _position;
        
        private void Awake()
        {
            canvasGroup.alpha = 0;
        }
        
        private void Update()
        {
            if (_isMouseOver && !_isShown)
            {
                _timer += Time.deltaTime;
                if (_timer >= timeToDisplay )
                {
                    _isShown = true;
                    canvasGroup.DOFade(1, fadeSpeed);
                }
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isMouseOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isMouseOver = false;
            _timer = 0;
            if (_isShown)
            {
                _isShown = false;
                canvasGroup.DOFade(0, fadeSpeed);
            }
        }

        public void SetText(string text)
        {
            tooltipText.text = text;
            Resize();
        }

        private void Resize()
        {
            tooltipText.ForceMeshUpdate();
            var textSize = tooltipText.textBounds.size;
            background.sizeDelta = new Vector2(textSize.x+6, textSize.y+6);
        }
    }
}