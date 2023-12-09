using System;
using MyProject.Sources.Controllers;
using MyProject.Sources.PresentationInterfaces.Views;
using UnityEngine;

namespace MyProject.Sources.Presentation.Views
{
    public class PlayerMovementView : PresentableView<PlayerMovementPresenter>, IPlayerMovementView
    {
        private CharacterController _characterController;
        
        private void Awake()
        {
            _characterController = GetComponent<CharacterController>() ??
                                   throw new NullReferenceException(nameof(_characterController));
        }

        //TODO могу ли я обобщить Update?
        private void Update() =>
            Presenter?.Update();

        public void Move(Vector3 direction) => 
            _characterController.Move(direction);

        public void Rotate(Quaternion look, float speed) =>
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, look, speed);
    }
}