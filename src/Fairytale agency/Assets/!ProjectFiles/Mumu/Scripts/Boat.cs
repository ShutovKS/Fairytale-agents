using System;
using UnityEngine;

namespace Mumu
{
    public class Boat : MonoBehaviour
    {
        public Action OnDead;
        public Action<int> OnHealthChange;

        [field: SerializeField] public float Health { get; private set; } = 100f;

        public void TakeDamage(float damage)
        {
            Health -= damage;
            OnHealthChange?.Invoke((int)Health);

            if (Health <= 0)
            {
                OnDead?.Invoke();
            }
        }
    }
}