using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpaceShooter
{
    [CreateAssetMenu(fileName = "Add LevelSettings",order = 2000)]
    public class LevelSettings : ScriptableObject
    {
#pragma warning disable CS0649
        [SerializeField] List<Hazard> asteroids = new List<Hazard>();
        [SerializeField] List<Hazard> enemyShips = new List<Hazard>();
        [SerializeField] int hazardsForWaveCount;
#pragma warning restore CS0649
        public List<Hazard> Hazards => asteroids;
        public List<Hazard> EnemyShips => enemyShips;
        public int HazardsForWaveCount => hazardsForWaveCount;
        public Hazard GetRandomAsteroid => GetRandomHazard(asteroids);
        public Hazard GetRandomEnemy=> GetRandomHazard(enemyShips);
            
        private Hazard GetRandomHazard(List<Hazard> hazardList)
        {
            if (hazardList.Count != 0)
            {
                int rand = Random.Range(0, hazardList.Count);
                return hazardList[rand];
            }
            else {
                Debug.LogError("Hazard list is empty");
                return null;
            }
        }
        


    }
}


