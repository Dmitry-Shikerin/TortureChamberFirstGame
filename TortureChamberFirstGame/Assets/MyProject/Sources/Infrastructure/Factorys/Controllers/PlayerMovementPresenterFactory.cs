using System;
using JetBrains.Annotations;
using MyProject.Sources.Controllers;
using MyProject.Sources.Domain.PlayerMovement;
using MyProject.Sources.Presentation.Animations;
using MyProject.Sources.Presentation.Views;
using MyProject.Sources.PresentationInterfaces.Animations;
using MyProject.Sources.PresentationInterfaces.Views;

namespace MyProject.Sources.Infrastructure.Factorys.Controllers
{
    public class PlayerMovementPresenterFactory
    {
        private readonly InputService _inputService;
        private readonly IPlayerAnimation _playerAnimation;

        public PlayerMovementPresenterFactory
        (
            InputService inputService,
            IPlayerAnimation playerAnimation//TODO запрашивать ли его в конструктор?
        )
        {
            _inputService = inputService ? inputService : 
                throw new ArgumentNullException(nameof(inputService));
            _playerAnimation = playerAnimation ??
                               throw new ArgumentNullException(nameof(playerAnimation));
        }

        public PlayerMovementPresenter Create(PlayerMovement playerMovement, IPlayerMovementView playerMovementView)
        {
            if (playerMovement == null) 
                throw new ArgumentNullException(nameof(playerMovement));
            if (playerMovementView == null) 
                throw new ArgumentNullException(nameof(playerMovementView));
            
            return new PlayerMovementPresenter
            (
                playerMovementView,
                _playerAnimation,
                playerMovement,
                _inputService
            );
        }
    }
}