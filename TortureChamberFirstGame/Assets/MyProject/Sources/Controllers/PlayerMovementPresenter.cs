using System;
using JetBrains.Annotations;
using MyProject.Sources.Controllers.Common;
using MyProject.Sources.Domain.PlayerMovement;
using MyProject.Sources.Presentation.Animations;
using MyProject.Sources.Presentation.Views;
using MyProject.Sources.PresentationInterfaces.Animations;
using MyProject.Sources.PresentationInterfaces.Views;
using UnityEngine;

namespace MyProject.Sources.Controllers
{
    //TODO можно ли сделать всего персонажа через стейтМашину?
    //TODO если да то нужно будет сделать несколько моделей?
    public class PlayerMovementPresenter : PresenterBase
    {
        private readonly IPlayerMovementView _playerMovementView;
        private readonly IPlayerAnimation _playerAnimation;
        private readonly PlayerMovement _playerMovement;
        private readonly InputService _inputService;
        
        private float _runInput;
        private Vector2 _movementInput;

        public PlayerMovementPresenter
            (
                IPlayerMovementView playerMovementView,
                IPlayerAnimation playerAnimation,
                PlayerMovement playerMovement,
                InputService inputService
                )
        {
            _playerMovementView = playerMovementView ?? 
                                  throw new ArgumentNullException(nameof(playerMovementView));
            _playerAnimation = playerAnimation ?? 
                               throw new ArgumentNullException(nameof(playerAnimation));
            _playerMovement = playerMovement ?? 
                              throw new ArgumentNullException(nameof(playerMovement));
            _inputService = inputService ? inputService : 
                throw new ArgumentNullException(nameof(inputService));
        }

        public override void Enable()
        {
            _inputService.MovementAxisChanged += OnMovementAxis;
            _inputService.RunAxisChanged += OnRunAxis;
        }

        public override void Disable()
        {
            _inputService.MovementAxisChanged += OnMovementAxis;
            _inputService.RunAxisChanged += OnRunAxis;
        }

        public void Update()
        {
            Vector3 cameraDirection = _playerMovement.GetCameraDirection(_movementInput);
            Vector3 direction = _playerMovement.GetDirection(_runInput, cameraDirection);

            float animationSpeed = _playerMovement.GetMaxSpeed(_movementInput, _runInput);
            
            _playerAnimation.PlayMovementAnimation(animationSpeed);
            _playerMovementView.Move(direction);
            
            if(_playerMovement.IsIdle(_movementInput))
                return;

            Quaternion look = _playerMovement.GetDirectionRotation(cameraDirection);
            float speedRotation = _playerMovement.GetSpeedRotation();
            
            _playerMovementView.Rotate(look, speedRotation);
        }
        
        private void OnRunAxis(float runInput) => 
            _runInput = runInput;

        private void OnMovementAxis(Vector2 movementInput) => 
            _movementInput = movementInput;
    }
}