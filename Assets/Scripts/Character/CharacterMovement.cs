using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private LayerMask rayLayer;
        private NavMeshAgent _agent;
        private Camera _camera;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _camera = Camera.main;
        }

        private Vector3 GetClickPosition()
        {
            Ray mousePosition = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mousePosition, out var hit, rayLayer))
            {
                return hit.point;
            }

            return Vector3.zero;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _agent.SetDestination(GetClickPosition());
            }
        }
    }
}