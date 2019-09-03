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
        [Header("Enemy spawn settings")]
        [SerializeField] LevelSettings lvlSettings;
        [SerializeField] Vector3 spawnValues;
        [Tooltip("Spawn delay between hazards in seconds inside current wave")]
        [SerializeField] float spawnWait;
        [Tooltip("Start delay in seconds before waves of hazards will be spawned")]
        [SerializeField] float startWait;
        [Tooltip("Delay in second between waves")]
        [SerializeField] float waveWait;
        [Range(0.0f,1.0f)]
        [SerializeField] float enemyShipSpawnOdds =0.5f;

        [Header("Game Control settings")]
        [SerializeField] GameObject GameOverMenu;
        [SerializeField] Button go2menuBtn;
        [SerializeField] Button restartBtn;
        [SerializeField] int menuSceneIndex = 0;
#pragma warning restore CS0649
        

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

        void Start()
        {
            ActivateGameOverMenu(false);
           
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
            StopCoroutine(SpawnWaves());
            GameOver();
        }


        IEnumerator SpawnWaves()
        {
            yield return new WaitForSeconds(startWait);
            while (true)
            {
                for (int i = 0; i < lvlSettings.HazardsForWaveCount; i++)
                {
                    Try2SpawnEnemyShop();
                    SpawnHazard(lvlSettings.GetRandomAsteroid);
                    
                    
                    yield return new WaitForSeconds(spawnWait);
                }
                yield return new WaitForSeconds(waveWait);
            }
        }


        private bool Try2SpawnEnemyShop()
        {
            float rand = UnityEngine.Random.Range(0.0f, 1.0f);
            if (rand < enemyShipSpawnOdds)
            {
               SpawnHazard(lvlSettings.GetRandomEnemy);
                return true;
            }

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
            
            ActivateGameOverMenu(true);
        }

        private void ActivateGameOverMenu(bool active)
        {
            GameOverMenu.SetActive(active);
            if (active)
            {
                go2menuBtn.onClick.AddListener(LoadMenu);
                restartBtn.onClick.AddListener(RestartGame);
            }
            else
            {                
                go2menuBtn.onClick.RemoveListener(LoadMenu);
                restartBtn.onClick.RemoveListener(RestartGame);
            }

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
