using UnityEngine;
using UnityEngine.AI;

public class PlayerChase : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform player;

    private void Update()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }
}