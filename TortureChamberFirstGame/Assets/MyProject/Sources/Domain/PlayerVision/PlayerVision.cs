using UnityEngine;

namespace MyProject.Sources.Domain.PlayerVision
{
    public class PlayerVision
    {
        //TODO нужно ли его делать ReadOnly или как то еще чтобы инкапсулировать?
        public Collider[] Colliders { get; private set; } = new Collider[32];
    }
}