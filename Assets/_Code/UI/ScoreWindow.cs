using MainCamera;
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
            CameraMovement.Disable();
            scoreText.text = text;
            panel.SetActive(true);
            okButton.onClick.AddListener(HideScoreWindow);   
        }
        
        private void HideScoreWindow()
        {
            CameraMovement.Enable();
            okButton.onClick.RemoveAllListeners();
            panel.SetActive(false);
        }
    }
}