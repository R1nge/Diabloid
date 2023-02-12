using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private LayerMask groundLayer;
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
            if (Physics.Raycast(mousePosition, out var hit, rayDistance, groundLayer))
            {
                return hit.point;
            }

            return transform.position;
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