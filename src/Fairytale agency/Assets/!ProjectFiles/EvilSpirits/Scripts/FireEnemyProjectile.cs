using UnityEngine;

namespace EvilSpirits
{
    public class FireEnemyProjectile : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed = 200.0f;
        [SerializeField] private float projectileDamage = 50.0f;
        [SerializeField] private Rigidbody rigidbody;

        private Transform _playerTransform;

        private void Awake()
        {
            _playerTransform = GameObject.Find("Player").transform;
        }

        private void Start()
        {
            rigidbody.AddForce((_playerTransform.transform.position - rigidbody.position).normalized * projectileSpeed);
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                col.gameObject.GetComponent<PlayerController>().TakeDamage(projectileDamage);
                Debug.Log("Hit");
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}