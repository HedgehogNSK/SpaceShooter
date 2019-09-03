using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SpaceShooter.Menu
{

    public class MenuManager : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField]int gameSceneIndex;
        [SerializeField]Button startBtn;
        [SerializeField]Button exitBtn;
#pragma warning restore CS0649

        private void Start()
        {
            startBtn.onClick.AddListener(StartGame);
            exitBtn.onClick.AddListener(ExitGame);
        }
        AsyncOperation loader;
        public void StartGame()
        {
            if (loader != null && !loader.isDone) return;
            loader = SceneManager.LoadSceneAsync(gameSceneIndex);
        }

        public void ExitGame()
        {
            Application.Quit();           
        }
    }
}

