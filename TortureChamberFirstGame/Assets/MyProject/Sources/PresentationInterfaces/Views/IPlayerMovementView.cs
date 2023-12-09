using UnityEngine;

namespace MyProject.Sources.PresentationInterfaces.Views
{
    public interface IPlayerMovementView
    {
        public void Move(Vector3 direction);
        public void Rotate(Quaternion look, float speed);
    }
}