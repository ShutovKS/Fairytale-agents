using UnityEngine;

namespace Beanstalk
{
    public class BoxScript : MonoBehaviour
    {
        private float _minX = -2.2f, _maxX = 2.2f;
        private bool _canMove;
        private float _moveSpeed = 2f;
        private Rigidbody2D _myBody;
        private bool _gameOver;
        private bool _ignoreCollision;
        private bool _ignoreTrigger;

        private void Awake()
        {
            _myBody = GetComponent<Rigidbody2D>();
            _myBody.gravityScale = 0.0f;
        }

        private void Start()
        {
            _canMove = true;

            if (Random.Range(0, 2) > 0)
            {
                _moveSpeed *= -1.0f;
            }

            GameplayController._instance.currentBox = this;
        }

        private void Update()
        {
            MoveBox();
        }

        private void MoveBox()
        {
            if (_canMove)
            {
                Vector3 temp = transform.position;

                temp.x += _moveSpeed * Time.deltaTime;

                if (temp.x > _maxX)
                {
                    _moveSpeed *= -1.0f;
                }
                else if (temp.x < _minX)
                {
                    _moveSpeed *= -1.0f;
                }

                transform.position = temp;
            }
        }

        public void DropBox()
        {
            _canMove = false;
            _myBody.gravityScale = Random.Range(2, 4);
        }

        private void Landed()
        {
            if (_gameOver)
            {
                return;
            }

            _ignoreCollision = true;
            _ignoreTrigger = true;

            GameplayController._instance.SpawnNewBox();
            GameplayController._instance.MoveCamera();
        }

        private void RestartGame()
        {
            GameplayController._instance.Restart();
        }

        private void OnCollisionEnter2D(Collision2D target)
        {
            if (_ignoreCollision)
            {
                return;
            }

            if (target.gameObject.CompareTag($"Platform"))
            {
                Invoke(nameof(Landed), 2.0f);
                _ignoreCollision = true;
            }

            if (target.gameObject.CompareTag($"Box"))
            {
                Invoke(nameof(Landed), 2.0f);
                _ignoreCollision = true;
            }
        }


        private void OnTriggerEnter2D(Collider2D target)
        {
            if (_ignoreTrigger)
            {
                return;
            }

            if (target.gameObject.CompareTag($"GameOver"))
            {
                CancelInvoke(nameof(Landed));
                _gameOver = true;
                _ignoreTrigger = true;
                Invoke(nameof(RestartGame), 2.0f);
            }
        }
    }
}