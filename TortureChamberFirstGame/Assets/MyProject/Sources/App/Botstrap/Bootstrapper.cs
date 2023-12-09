using System;
using System.Collections;
using System.Collections.Generic;
using MyProject.Sources.Domain.PlayerCameras;
using MyProject.Sources.Domain.PlayerMovement;
using MyProject.Sources.Domain.PlayerMovement.PlayerMovementCharacteristics;
using MyProject.Sources.Infrastructure.Factorys.Controllers;
using MyProject.Sources.Infrastructure.Factorys.Views;
using MyProject.Sources.Presentation.Animations;
using MyProject.Sources.Presentation.Views;
using MyProject.Sources.PresentationInterfaces.Animations;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private const string PlayerMovementCharacteristicsPath = "Configs/PlayerMovementCharacteristics";

    [SerializeField] private Transform _cameraMain;
    //TODO сделать через инстантиэйт
    [SerializeField] private InputService _inputService;
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerCameraView _playerCameraView;
    
    private void Awake()
    {
        //PlayerCamera
        PlayerCamera playerCamera = new PlayerCamera(_cameraMain);
        PlayerCameraPresenterFactory playerCameraPresenterFactory =
            new PlayerCameraPresenterFactory(_inputService);
        PlayerCameraViewFactory playerCameraViewFactory = 
            new PlayerCameraViewFactory(playerCameraPresenterFactory);
        playerCameraViewFactory.Create(_playerCameraView, playerCamera);
        _playerCameraView.SetTransform(_player.transform);
        
        //PlayerMovement
        IPlayerAnimation playerAnimation = _player.GetComponent<PlayerAnimation>() ??
                                           throw new NullReferenceException(nameof(PlayerAnimation));
        PlayerMovementView playerMovementView = _player.GetComponent<PlayerMovementView>();
        PlayerMovementCharacteristic playerMovementCharacteristic =
            Resources.Load<PlayerMovementCharacteristic>(PlayerMovementCharacteristicsPath);
        PlayerMovement playerMovement = new PlayerMovement(
            playerMovementCharacteristic, _cameraMain);
        PlayerMovementPresenterFactory playerMovementPresenterFactory =
            new PlayerMovementPresenterFactory(_inputService, playerAnimation);
        PlayerMovementViewFactory playerMovementViewFactory =
            new PlayerMovementViewFactory(playerMovementPresenterFactory);
        playerMovementViewFactory.Create(playerMovement, playerMovementView);
    }
}
