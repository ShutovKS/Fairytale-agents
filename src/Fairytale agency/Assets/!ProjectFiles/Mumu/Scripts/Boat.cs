using System;
using UnityEngine;

namespace Mumu
{
    public class Boat : MonoBehaviour
    {
        public Action OnDead;
        
        [SerializeField] private float health = 100f;

        public void TakeDamage(float damage)
        {
            health -= damage;

            if (health <= 0)
            {
                OnDead?.Invoke();
            }
        }
    }
}