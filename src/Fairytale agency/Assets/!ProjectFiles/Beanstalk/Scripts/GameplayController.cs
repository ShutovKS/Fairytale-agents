using UnityEngine;
using UnityEngine.SceneManagement;

namespace Beanstalk
{
    public class GameplayController : MonoBehaviour
    {
        public static GameplayController _instance;

        [HideInInspector] public BoxScript currentBox;
        public CameraFollow cameraScript;
        public BoxSpawner boxSpawner;

        private int _moveCount;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        private void Start()
        {
            boxSpawner.SpawnBox();
        }

        private void Update()
        {
            DetectInput();
        }

        private void DetectInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentBox.DropBox();
            }
        }

        public void SpawnNewBox()
        {
            Invoke(nameof(NewBox), 1.0f);
        }

        private void NewBox()
        {
            boxSpawner.SpawnBox();
        }

        public void MoveCamera()
        {
            _moveCount++;
            if (_moveCount == 5)
            {
                _moveCount = 0;
                cameraScript.targetPos.y += 2f;
            }
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}