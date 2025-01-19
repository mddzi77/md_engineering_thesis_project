using System;
using System.Collections.Generic;
using Game.Checker;
using MdUtils;
using TriInspector;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private CheckManager checkManager;
        [SerializeField] private List<LevelData> levels;
        
        [ShowInInspector]
        public int CurrentLevel=> _currentLevelIndex + 1;
        
        private int _currentLevelIndex;
        
        private void Start()
        {
            _currentLevelIndex = PlayerPrefs.GetInt("CurrentLevel", 0);
            checkManager.SetLevel(levels[_currentLevelIndex]);
        }

        private void OnApplicationQuit()
        {
            PlayerPrefs.Save();
        }

        public void StartCheck()
        {
        }
        
        [ContextMenu("Next Level")]
        public void NextLevel()
        {
            _currentLevelIndex++;
            if (_currentLevelIndex >= levels.Count)
            {
                Debug.Log("All levels completed!");
                return;
            }
            checkManager.SetLevel(levels[_currentLevelIndex]);
            PlayerPrefs.SetInt("CurrentLevel", _currentLevelIndex);
        }
        
        public void Restart()
        {
            _currentLevelIndex = 0;
            PlayerPrefs.SetInt("CurrentLevel", _currentLevelIndex);
            checkManager.SetLevel(levels[_currentLevelIndex]);
        }
    }
}