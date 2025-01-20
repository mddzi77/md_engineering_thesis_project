using System;
using Game;
using MainCamera;
using MdUtils;
using UI.Requirements;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScoreWindow : MonoSingleton<ScoreWindow>
    {
        [SerializeField] private TMPro.TextMeshProUGUI scoreText;
        [SerializeField] private GameObject panel;
        [SerializeField] private Button returnButton;
        [SerializeField] private Button nextButton;
        
        public static bool IsShown { get; private set; }
        
        private void Start()
        {
            panel.SetActive(false);
        }

        private void OnEnable()
        {
            returnButton.onClick.AddListener(HideScoreWindow);
            nextButton.onClick.AddListener(NextLevel);
        }
        
        private void OnDisable()
        {
            returnButton.onClick.RemoveListener(HideScoreWindow);
            nextButton.onClick.RemoveListener(NextLevel);
        }

        public void SetScoreText(string text, bool correct)
        {
            IsShown = true;
            CameraMovement.Disable();
            scoreText.text = text;
            panel.SetActive(true);

            if (correct)
            {
                if (GameManager.Instance.CurrentLevel == GameManager.Instance.Levels.Count)
                {
                    scoreText.text = "Congratulations! You have completed all levels!";
                    returnButton.gameObject.SetActive(true);
                    nextButton.gameObject.SetActive(false);
                    return;
                }
                returnButton.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(true);
            }
            else
            {
                returnButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(false);
            }
        }
        
        private void HideScoreWindow()
        {
            CameraMovement.Enable();
            panel.SetActive(false);
            IsShown = false;
        }
        
        private void NextLevel()
        {
            HideScoreWindow();
            GameManager.Instance.UnlockNextLevel();
            GameManager.Instance.NextLevel();
            RequirementsWindow.Instance.OpenWindow();
        }
    }
}