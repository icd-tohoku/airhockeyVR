using System;
using System.Collections;
using System.Collections.Generic;
using AirHockey.Data;
using AirHockey.Systems;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AirHockey.UI
{
    [RequireComponent(typeof(Image))]
    public class GameFinishPanel : MonoBehaviour
    {
        [SerializeField] private PlayerType panelUser;
        
        [SerializeField] private Text winText = null;
        [SerializeField] private Text loseText = null;
        [SerializeField] private Button restartButton = null;
        [SerializeField] private Button quitGameButton = null;

        [SerializeField] private float restartGameInterval;

        private void Awake()
        {
            gameObject.SetActive(false);
            restartButton.gameObject.SetActive(true);
            quitGameButton.gameObject.SetActive(true);
            GetComponent<Image>().enabled = true;
            
            EventManager.GameFinishEvent += OnGameFinish;
            
            restartButton.onClick.AddListener(OnRestartButtonClicked);
            quitGameButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void OnGameFinish(PlayerType winner)
        {
            if (winner == panelUser)
            {
                winText.gameObject.SetActive(true);
            }
            else
            {
                loseText.gameObject.SetActive(true);
            }
            
            gameObject.SetActive(true);

            StartCoroutine(RestartGameCoroutine());
        }

        private IEnumerator RestartGameCoroutine()
        {
            yield return new WaitForSeconds(restartGameInterval);

            gameObject.SetActive(false);
            winText.gameObject.SetActive(false);
            loseText.gameObject.SetActive(false);
            
            GameData.ChangeScore(PlayerType.Desktop, 0);
            GameData.ChangeScore(PlayerType.VR, 0);
            
            EventManager.InvokeGameStartEvent();
        }

        private void OnRestartButtonClicked()
        {
            var loadScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(loadScene.name);
        }

        private void OnQuitButtonClicked()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            Application.Quit();
#endif
        }

        private void OnDestroy()
        {
            EventManager.GameFinishEvent -= OnGameFinish;
            restartButton.onClick.RemoveAllListeners();
            quitGameButton.onClick.RemoveAllListeners();
        }
    }
}