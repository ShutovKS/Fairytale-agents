using System;
using UnityEngine;

namespace Mumu
{
    public class GameManager : MonoBehaviour
    {
        public event Action OnLost;
        public event Action OnWon;
        public static GameManager Instance { get; private set; }

        [SerializeField] private EnemiesSpawner enemiesSpawner;
        [SerializeField] private Boat boat;

        public void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            enemiesSpawner.OnAllDeadEnemies = OnWon;

            boat.OnDead += enemiesSpawner.StopSpawn;
            boat.OnDead += OnLost;
        }

        public void StartGame()
        {
            enemiesSpawner.StartSpawn();
        }
    }
}