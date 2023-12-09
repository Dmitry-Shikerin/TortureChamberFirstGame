using System;
using JetBrains.Annotations;
using MyProject.Sources.Domain.PlayerMovement.PlayerMovementCharacteristics;
using UnityEngine;

namespace MyProject.Sources.Domain.PlayerMovement
{
    public class PlayerMovement
    {
        private readonly Transform _cameraPosition;
        private readonly PlayerMovementCharacteristic _characteristic;

        public PlayerMovement
        (
            PlayerMovementCharacteristic playerMovementCharacteristic,
            Transform cameraPosition //TODO перенести в сервис камеры
        )
        {
            _characteristic = playerMovementCharacteristic
                ? playerMovementCharacteristic
                : throw new ArgumentNullException(nameof(playerMovementCharacteristic));
            _cameraPosition = cameraPosition ? cameraPosition : 
                throw new ArgumentNullException(nameof(cameraPosition));
        }

        public Vector3 GetDirection(float runInput, Vector3 cameraDirection)
        {
            float speed = runInput * _characteristic.RunSpeed + _characteristic.MovementSpeed;
            Vector3 direction = cameraDirection * speed * Time.deltaTime;
            direction.y = cameraDirection.y;

            return direction;
        }

        public Vector3 GetCameraDirection(Vector2 moveInput)
        {
            Vector3 direction = _cameraPosition.TransformDirection(
                moveInput.x, 0, moveInput.y).normalized; //TODO это тоже сервис камеры

            direction.y -= _characteristic.Gravity * Time.deltaTime;

            return direction;
        }

        public bool IsIdle(Vector2 moveInput)
        {
            return moveInput.x == 0.0f && moveInput.y == 0.0f;
        }

        public Quaternion GetDirectionRotation(Vector3 direction)
        {
            return Quaternion.LookRotation(direction).normalized;
        }

        public float GetSpeedRotation()
        {
            return _characteristic.AngularSpeed * Time.deltaTime;
        }

        public float GetMaxSpeed(Vector2 moveInput, float runInput)
        {
            float maxMovementValue = Mathf.Max(Mathf.Abs(moveInput.x), Mathf.Abs(moveInput.y));
            return runInput * maxMovementValue + maxMovementValue; 
        }
    }
}