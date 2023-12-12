using UnityEngine;

namespace MyProject.Sources.Infrastructure.Services
{
    public class LinecastService
    {
        public bool Linecast(Vector3 position, Vector3 colliderPosition, int obstacleLayerMask) => 
            Physics.Linecast(position, colliderPosition, obstacleLayerMask);
    }
}