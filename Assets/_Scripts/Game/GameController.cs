using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Hedge.UI;
using System;
using System.Linq;

namespace SpaceShooter
{
    public class GameController : MonoBehaviour
    {
        

#pragma warning disable CS0649
        [SerializeField] LevelSettings lvlSettings;
        [SerializeField] Vector3 spawnValues;
        [Tooltip("Spawn delay between hazards in seconds inside current wave")]
        [SerializeField] float spawnWait;
        [Tooltip("Start delay in seconds before waves of hazards will be spawned")]
        [SerializeField] float startWait;
        [Tooltip("Delay in second between waves")]
        [SerializeField] float waveWait;

        [SerializeField] Text restartText;
        [SerializeField] Text gameOverText;

        [SerializeField] GameObject GameOverMenu;
        [SerializeField] Button go2menuBtn;
        [SerializeField] Button restartBtn;
        [SerializeField] int menuSceneIndex = 0;
#pragma warning restore CS0649

        private bool gameOver;
        private bool restart;

        private int score;
        public int Score
        {
            get { return score; }
            protected set
            {
                if (value > 0)
                    score = value;
                DataSpreader.OnUpdate?.Invoke(ParameterType.Points, value);
            }
        }

        PlayerController player;
        List<Hazard> spawnedEnemies;

        void Start()
        {
            spawnedEnemies = new List<Hazard>();
            ActivateGameOverMenu(false);
            gameOver = false;
            restart = false;
            restartText.text = "";
            gameOverText.text = "";
            Score = 0;
            StartCoroutine(SpawnWaves());
            player = FindObjectOfType<PlayerController>();
            if (player)
            {
                player.OnDie += GameOverWrapper;
                InputMoveController.OnMove += player.SetMovement;
                InputFireController.OnFire += player.Fire;
            }
            else
            {
                Debug.LogError("There is no player on the scene");
            }

        }

        private void GameOverWrapper(HitArgs obj)
        {            
            GameOver();
        }

#if KEYBOARD
        void Update()
        {
            if (restart)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    RestartGame();
                }
            }
        }
#endif

        IEnumerator SpawnWaves()
        {
            yield return new WaitForSeconds(startWait);
            while (true)
            {
                for (int i = 0; i < lvlSettings.HazardsForWaveCount; i++)
                {
                    TrySpawnEnemy();

                    SpawnHazard(lvlSettings.GetRandomAsteroid);
                    
                    
                    yield return new WaitForSeconds(spawnWait);
                }
                yield return new WaitForSeconds(waveWait);

                if (gameOver)
                {
                    break;
                }
            }
        }

        //private void SpawnEnemy()
        //{
        //    Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
        //    Quaternion spawnRotation = Quaternion.identity;
        //    IHitable hitable = Instantiate(lvlSettings.GetRandomEnemy, spawnPosition, spawnRotation).GetComponent<IHitable>();
        //    if (hitable != null) hitable.OnDie += AddScore;
        //}

        private bool TrySpawnEnemy()
        {
            
            if (spawnedEnemies.Sum(x => x.Health) < lvlSettings.EnemyShips.Max(ship => ship.Health))
            {
                float rand = UnityEngine.Random.Range(0.0f, 1.0f);
                if (rand > 0.8f)
                {
                    spawnedEnemies.Add(SpawnHazard(lvlSettings.GetRandomEnemy));
                }
                return true;
            }
            Debug.Log(spawnedEnemies.Sum(x => x.Health) +":"+ lvlSettings.EnemyShips.Max(ship => ship.Health));
            return false;
            
        }

        private Hazard SpawnHazard(Hazard hazardPrefab)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
            Quaternion spawnRotation = Quaternion.identity;
            Hazard hazard = Instantiate(hazardPrefab, spawnPosition, spawnRotation);
            IHitable hitable = hazard.GetComponent<IHitable>();
            if (hitable != null) hitable.OnDie += AddScore;
            return hazard;
        }

        private void AddScore(HitArgs hitArgs)
        {
            if (hitArgs.Attacker && hitArgs.Attacker.GetComponent<PlayerController>())
            {
                Hazard hazard = hitArgs.Victim.GetComponent<Hazard>();
                if (hazard)
                {
                    spawnedEnemies.Remove(hazard);
                    Score += hazard.Reward;
                }
            }

        }

        public void GameOver()
        {
            if(player)
            {
                InputMoveController.OnMove -= player.SetMovement;
                InputFireController.OnFire -= player.Fire;
            }                      
            
            gameOver = true;
            ActivateGameOverMenu(gameOver);
        }

        private void ActivateGameOverMenu(bool active)
        {
            GameOverMenu.SetActive(active);
            if (active)
            {
                gameOverText.text = "Game Over!";
                go2menuBtn.onClick.AddListener(LoadMenu);
                restartBtn.onClick.AddListener(RestartGame);
            }
            else
            {
                
                go2menuBtn.onClick.RemoveListener(LoadMenu);
                restartBtn.onClick.RemoveListener(RestartGame);
            }

#if KEYBOARD
            if (active)
            {
                restartText.text = "Press 'R' for Restart";
                restart = true;
            }
            else
            {
                restart = false;

            }
#endif
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene(menuSceneIndex);
        }
    }
}
