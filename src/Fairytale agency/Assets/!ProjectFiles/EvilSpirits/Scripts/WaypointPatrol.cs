using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform[] waypoints;

    private int _currentWaypointIndex;

    private void Start()
    {
        navMeshAgent.SetDestination(waypoints[0].position);
    }

    private void Update()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);
        }
    }
}