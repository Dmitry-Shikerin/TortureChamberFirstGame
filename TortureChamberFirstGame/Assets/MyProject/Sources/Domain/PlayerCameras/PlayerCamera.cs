using System;
using JetBrains.Annotations;
using UnityEngine;

namespace MyProject.Sources.Domain.PlayerCameras
{
    public class PlayerCamera
    {
        private readonly Transform _cameraTransform;

        private const float AngularSpeed = 1f;

        //TODO возможно убрать этот трансформ из зависимости
        public PlayerCamera(Transform cameraTransform)
        {
            _cameraTransform = cameraTransform ? cameraTransform : 
                throw new ArgumentNullException(nameof(cameraTransform));

            AngleY = _cameraTransform.position.y;
        }
        
        public float AngleY { get; private set; }

        public void SetLeftRotation()
        {
            AngleY += AngularSpeed;
        }

        public void SetRightRotation()
        {
            AngleY -= AngularSpeed;
        }
    }
}