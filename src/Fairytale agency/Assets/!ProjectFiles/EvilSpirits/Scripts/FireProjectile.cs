using UnityEngine;
using UnityEngine.Serialization;

namespace EvilSpirits
{
    public class FireProjectile : MonoBehaviour
    {
        [SerializeField] private Transform startPosition;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float projectileDamage;
        [SerializeField] private Rigidbody rigidbody;

        private void Awake()
        {
            projectileSpeed = 15000.0f;
            projectileDamage = 50.0f;
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            rigidbody.AddForce(rigidbody.transform.forward * projectileSpeed);
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.CompareTag($"Enemy"))
            {
                col.gameObject.GetComponent<ImpController>().TakeDamage(projectileDamage);
                Debug.Log("Hit");
                Destroy(gameObject);
            }

            if (col.gameObject.CompareTag($"Level"))
            {
                Destroy(gameObject);
            }
        }
    }
}