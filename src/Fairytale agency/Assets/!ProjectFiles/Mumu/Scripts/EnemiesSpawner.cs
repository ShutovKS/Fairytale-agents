using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mumu
{
    public class EnemiesSpawner : MonoBehaviour
    {
        public Action<int> OnNumberDeadEnemies;
        public Action<int> OnNumberRemainingEnemies;
        public Action OnAllDeadEnemies;

        [SerializeField] private Transform playerTransform;
        [SerializeField] private GameObject[] enemiesPrefabs;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private float frequencySpawn;
        [SerializeField] private int countEnemies;

        public int NumberDeadEnemies { get; private set; }
        public int NumberRemainingEnemies => _numberLivingEnemies + countEnemies;

        private int _numberLivingEnemies;
        private bool _isSpawn;

        public void StartSpawn()
        {
            _isSpawn = true;
            StartCoroutine(Spawn());
        }

        public void StopSpawn()
        {
            _isSpawn = false;
        }

        private IEnumerator Spawn()
        {
            while (_isSpawn)
            {
                countEnemies--;
                _numberLivingEnemies++;
                var instance = Instantiate(
                    enemiesPrefabs[Random.Range(0, enemiesPrefabs.Length)],
                    spawnPoints[Random.Range(0, spawnPoints.Length)].position,
                    Quaternion.identity);
                instance.GetComponent<TurnTowardsTarget>().targetTransform = playerTransform;
                instance.GetComponent<Enemy>().targetTransform = playerTransform;
                instance.GetComponent<Enemy>().OnDead = EnemyDead;

                OnNumberRemainingEnemies?.Invoke(NumberRemainingEnemies);

                if (countEnemies <= 0)
                {
                    yield break;
                }

                yield return new WaitForSeconds(frequencySpawn);
            }
        }

        private void EnemyDead()
        {
            NumberDeadEnemies++;
            _numberLivingEnemies--;
            OnNumberDeadEnemies?.Invoke(NumberDeadEnemies);

            if (NumberRemainingEnemies <= 0)
            {
                OnAllDeadEnemies?.Invoke();
            }
        }
    }
}