using System;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Action OnPlayerSpotted;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!IsObjectBetweenPlayerAndObject(other.transform))
            {
                OnPlayerSpotted?.Invoke();
            }
        }
    }

    private bool IsObjectBetweenPlayerAndObject(Transform playerTransform)
    {
        var direction = transform.position - playerTransform.position;
        var distance = direction.magnitude;
        direction.Normalize();

        if (Physics.Raycast(playerTransform.position, direction, out var hit, distance))
        {
            if (hit.collider.gameObject != gameObject && !hit.collider.CompareTag("Untagged"))
            {
                return false;
            }
        }

        return true;
    }
}