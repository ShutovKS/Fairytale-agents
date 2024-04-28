using System;
using Infrastructure.Services.Input;
using UI.BeanstalkScreen;
using UnityEngine;

namespace Beanstalk
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public Action OnLost;
        public Action OnWon;
        public CameraFollow cameraScript;
        public GameObject boxPrefab;

        [SerializeField] private PlayerInputActionReader inputActionReader;
        [field: SerializeField] public int CountBox { get; private set; } = 15;
        [field: SerializeField] public int NumberRemainingBox { get; private set; }

        private BeanstalkUI _beanstalkUI;
        private BoxScript _boxScript;
        private int _moveCount;
        private bool _isGameOver;

        public void Awake()
        {
            Instance = this;
        }

        public void StartGame(BeanstalkUI beanstalkUI)
        {
            _beanstalkUI = beanstalkUI;
            inputActionReader.Jump += DropBox;
            SpawnBox();
        }

        private void OnDestroy()
        {
            inputActionReader.Jump -= DropBox;
        }

        private void SpawnBox()
        {
            if (_boxScript != null)
            {
                _boxScript.OnLanded -= MoveCamera;
                _boxScript.OnLanded -= SpawnBox;
            }
            
            if (CountBox == 0)
            {
                OnWon?.Invoke();
                return;
            }

            _boxScript = Instantiate(boxPrefab, new Vector3(0f, cameraScript.targetPos.y, 0f), Quaternion.identity)
                .GetComponent<BoxScript>();

            _beanstalkUI.SetLeft(--CountBox);
            _beanstalkUI.SetBuilt(++NumberRemainingBox);

            _boxScript.OnLanded += MoveCamera;
            _boxScript.OnLanded += SpawnBox;
            _boxScript.OnGameOver += GameOver;
        }

        private void MoveCamera()
        {
            cameraScript.targetPos.y += 1f;
        }

        private void GameOver()
        {
            if (_isGameOver == false)
            {
                _isGameOver = true;
                OnLost?.Invoke();
            }
        }

        private void DropBox()
        {
            _boxScript.DropBox();
        }
    }
}