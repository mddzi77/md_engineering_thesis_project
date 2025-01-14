using MdUtils;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScoreWindow : MonoSingleton<ScoreWindow>
    {
        [SerializeField] private TMPro.TextMeshProUGUI scoreText;
        [SerializeField] private GameObject panel;
        [SerializeField] private Button okButton;
        
        public void SetScoreText(string text)
        {
            scoreText.text = text;
            panel.SetActive(true);
            okButton.onClick.AddListener(HideScoreWindow);   
        }
        
        private void HideScoreWindow()
        {
            okButton.onClick.RemoveAllListeners();
            panel.SetActive(false);
        }
    }
}