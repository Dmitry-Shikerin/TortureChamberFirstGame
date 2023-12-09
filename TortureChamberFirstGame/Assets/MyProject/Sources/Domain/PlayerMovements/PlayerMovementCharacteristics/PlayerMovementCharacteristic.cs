using UnityEngine;

namespace MyProject.Sources.Domain.PlayerMovement.PlayerMovementCharacteristics
{
    [CreateAssetMenu(fileName = "PlayerMovementCharacteristics", 
        menuName = "Characteristics/PlayerMovementCharacteristic", order = 51)]
    public class PlayerMovementCharacteristic : ScriptableObject
    {
        [field: SerializeField] public bool CursorVisible { get; private set; } = true;
        [field: SerializeField, Range(1, 10)] public float MovementSpeed { get; private set; } = 1f;
        [field: SerializeField] public float RunSpeed { get; private set; } = 3f;
        [field: SerializeField] public float AngularSpeed { get; private set; } = 350f;
        [field: SerializeField] public float Gravity { get; private set; } = 2f;
    }
}