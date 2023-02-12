using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    public class CharacterMovement : MonoBehaviour
    {
        private NavMeshAgent _agent;

        private void Awake() => _agent = GetComponent<NavMeshAgent>();

        public void MoveTo(Vector3 pos)
        {
            _agent.isStopped = false;
            _agent.SetDestination(pos);
        }

        public void Stop() => _agent.isStopped = true;
    }
}