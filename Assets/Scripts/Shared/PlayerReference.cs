using UnityEngine;

namespace Shared
{
    public class PlayerReference : MonoBehaviour
    {
        [SerializeField] private Transform player;

        public Transform GetPlayerTransform() => player;
    }
}