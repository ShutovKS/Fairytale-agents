using System;
using System.Collections;
using System.Threading.Tasks;
using Data.Dialogue;
using Infrastructure.Services.WindowsService;
using UI.Confirmation;
using UI.DialogueScreen;
using UI.Mumu.Scrips;
using UnityEngine;
using Event = UnityEngine.Event;

namespace Mumu
{
    public class GameManager : MonoBehaviour
    {
        public event Action OnLost;
        public event Action OnWon;
        public static GameManager Instance { get; private set; }

        [SerializeField] private EnemiesSpawner enemiesSpawner;
        [SerializeField] private Boat boat;
        [SerializeField] private BoatMovement boatMovement;
        
        private MumuUI _mumuUI;

        public void Awake()
        {
            Instance = this;
        }

        public void StartGame(MumuUI mumuUI)
        {
            _mumuUI = mumuUI;

            _mumuUI.SetHealthPoints((int)boat.Health);
            _mumuUI.SetDestroyed(enemiesSpawner.NumberDeadEnemies);
            _mumuUI.SetLeft(enemiesSpawner.NumberRemainingEnemies);

            boat.OnHealthChange = _mumuUI.SetHealthPoints;
            boat.OnDead += enemiesSpawner.StopSpawn;
            boat.OnDead += () => OnLost?.Invoke();

            enemiesSpawner.OnAllDeadEnemies = MovementTowardShore;
            enemiesSpawner.OnNumberDeadEnemies = _mumuUI.SetDestroyed;
            enemiesSpawner.OnNumberRemainingEnemies = _mumuUI.SetLeft;

            enemiesSpawner.StartSpawn();
        }

        private void MovementTowardShore()
        {
            boatMovement.enabled = true;
            boatMovement.OnCompleted += () => OnWon?.Invoke();
        }
    }
}