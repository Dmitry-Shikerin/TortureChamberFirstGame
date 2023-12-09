using System;
using MyProject.Sources.PresentationInterfaces.Animations;
using UnityEngine;

namespace MyProject.Sources.Presentation.Animations
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour, IPlayerAnimation
    {
        private readonly int _speed = Animator.StringToHash("Speed");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>() ??
                        throw new NullReferenceException(nameof(_animator));
        }

        public void PlayMovementAnimation(float speed)
        {
            _animator.SetFloat(_speed, speed);
        }
    }
}