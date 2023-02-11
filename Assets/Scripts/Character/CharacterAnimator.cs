using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");

        private void Update() => animator.SetBool(IsWalking, agent.velocity.magnitude > 0);
    }
}