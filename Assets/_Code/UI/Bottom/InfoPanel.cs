using System;
using MdUtils;
using TMPro;
using UnityEngine;

namespace UI.Bottom
{
    public class InfoPanel : MonoSingleton<InfoPanel>
    {
        [SerializeField] private TextMeshProUGUI infoText;
        
        private bool _timerActive;
        private float _timer;
        private float _time;
        private string _previousText;
        private string _rawText;

        private void Update()
        {
            if (!_timerActive) return;
            _timer += Time.deltaTime;
            if (!(_timer >= _time)) return;
            ClearInfoText();
            if (!string.IsNullOrEmpty(_previousText))
            {
                SetInfoText(_previousText);
                _previousText = "";
            }
            _timerActive = false;
        }

        public void SetInfoText(string text)
        {
            ResetTimer();
            infoText.text = text;
            _rawText = text;
        }

        public void SetInfoText(string text, float time)
        {
            if (string.Equals(text, _rawText)) return;
            if (!string.IsNullOrEmpty(infoText.text))
            {
                _previousText = infoText.text;
            }
            SetTimer(time);
            SetInfoText(text);
        }
        
        public void SetErrorText(string text)
        {
            infoText.text = $"<color=red>{text}</color>";
            _rawText = text;
        }
        
        public void SetErrorText(string text, float time)
        {
            if (string.Equals(text, _rawText)) return;
            if (!string.IsNullOrEmpty(infoText.text))
            {
                _previousText = infoText.text;
            }
            SetTimer(time);
            SetErrorText(text);
        }
        
        public void ClearInfoText()
        {
            ResetTimer();
            infoText.text = "";
            _rawText = "";
        }
        
        private void SetTimer(float time)
        {
            _time = time;
            _timer = 0;
            _timerActive = true;
        }

        private void ResetTimer()
        {
            _timerActive = false;
        }
    }
}