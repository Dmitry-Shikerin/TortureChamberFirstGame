using System;
using JetBrains.Annotations;
using MyProject.Sources.Controllers.Common;
using MyProject.Sources.Domain.PlayerCameras;
using MyProject.Sources.PresentationInterfaces.Views;

namespace MyProject.Sources.Controllers
{
    public class PlayerCameraPresenter : PresenterBase
    {
        private readonly PlayerCamera _playerCamera;
        private readonly IPlayerCameraView _playerCameraView;
        private readonly InputService _inputService;

        public PlayerCameraPresenter
        (
            PlayerCamera playerCamera,
            IPlayerCameraView playerCameraView,
            InputService inputService
        )
        {
            _playerCamera = playerCamera ?? 
                            throw new ArgumentNullException(nameof(playerCamera));
            _playerCameraView = playerCameraView ?? 
                                throw new ArgumentNullException(nameof(playerCameraView));
            _inputService = inputService ? inputService : 
                throw new ArgumentNullException(nameof(inputService));
        }

        public override void Enable()
        {
            _inputService.RotationChanged += OnRotationChanged;
        }
        
        public override void Disable()
        {
            _inputService.RotationChanged -= OnRotationChanged;
        }

        //TODO нужно обобщить
        public void Update()
        {
            _playerCameraView.Follow();
            _playerCameraView.Rotate(_playerCamera.AngleY);
        }
        
        private void OnRotationChanged(bool isLeftRotation, bool isRightRotation)
        {
            if(isLeftRotation)
                _playerCamera.SetLeftRotation();
            
            if(isRightRotation)
                _playerCamera.SetRightRotation();
        }
    }
}