using System;
using Infrastructure.Services.Input;
using UnityEngine;

namespace Beanstalk
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public Action OnGameOver;
        public CameraFollow cameraScript;
        public GameObject boxPrefab;

        [SerializeField] private PlayerInputActionReader inputActionReader;

        private BoxScript _boxScript;
        private int _moveCount;
        private bool _isGameOver;

        public void Awake()
        {
            Instance = this;
        }

        private void StartGame()
        {
            inputActionReader.Jump += DropBox;
            SpawBox();
        }

        private void OnDestroy()
        {
            inputActionReader.Jump -= DropBox;
        }

        private void SpawBox()
        {
            if (_boxScript != null)
            {
                _boxScript.OnLanded -= MoveCamera;
                _boxScript.OnLanded -= SpawBox;
            }

            _boxScript = Instantiate(boxPrefab, new Vector3(0f, cameraScript.targetPos.y, 0f), Quaternion.identity)
                .GetComponent<BoxScript>();

            _boxScript.OnLanded += MoveCamera;
            _boxScript.OnLanded += SpawBox;
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
                OnGameOver?.Invoke();
            }
        }

        private void DropBox()
        {
            _boxScript.DropBox();
        }
    }
}