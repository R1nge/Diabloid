using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
        public float movementSpeed;
    }
}