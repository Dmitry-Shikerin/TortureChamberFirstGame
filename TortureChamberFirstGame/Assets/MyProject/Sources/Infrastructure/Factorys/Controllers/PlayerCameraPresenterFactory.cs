using System;
using JetBrains.Annotations;
using MyProject.Sources.Controllers;
using MyProject.Sources.Domain.PlayerCameras;
using MyProject.Sources.PresentationInterfaces.Views;

namespace MyProject.Sources.Infrastructure.Factorys.Controllers
{
    public class PlayerCameraPresenterFactory
    {
        private readonly InputService _inputService;

        public PlayerCameraPresenterFactory(InputService inputService)
        {
            _inputService = inputService ? inputService : throw new ArgumentNullException(nameof(inputService));
        }

        public PlayerCameraPresenter Create(PlayerCamera playerCamera, IPlayerCameraView playerCameraView)
        {
            return new PlayerCameraPresenter
            (
                playerCamera,
                playerCameraView,
                _inputService
            );
        }
    }
}