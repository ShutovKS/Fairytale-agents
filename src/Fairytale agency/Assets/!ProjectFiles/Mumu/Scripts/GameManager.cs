using System;
using UnityEngine;

namespace Mumu
{
    public class GameManager : MonoBehaviour
    {
        public Action Lost;
        public Action Won;
        public static GameManager Instance { get; private set; }

        [SerializeField] private EnemiesSpawner enemiesSpawner;
        [SerializeField] private Boat boat;

        public void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            enemiesSpawner.OnAllDeadEnemies = Won;

            boat.OnDead += enemiesSpawner.StopSpawn;
            boat.OnDead += Lost;
        }

        public void StartGame()
        {
            enemiesSpawner.StartSpawn();
        }
    }
}