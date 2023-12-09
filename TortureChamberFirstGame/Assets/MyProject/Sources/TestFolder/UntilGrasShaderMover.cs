using UnityEngine;

namespace MyProject.Sources.TestFolder
{
    public class UntilGrasShaderMover : MonoBehaviour
    {
        [SerializeField] private Material _material;
        [SerializeField, Range(0.01f, 2f)] private float _speed;

        private const string PropertyKey = "_PointOffset";

        private Vector2 _offset;

        private void Update()
        {
            if(_material == null)
                return;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                _offset.y += Time.deltaTime * _speed;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                _offset.y -= Time.deltaTime * _speed;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _offset.x -= Time.deltaTime * _speed;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _offset.x += Time.deltaTime * _speed;
            }
            
            _material.SetVector(PropertyKey, _offset);
        }
    }
}