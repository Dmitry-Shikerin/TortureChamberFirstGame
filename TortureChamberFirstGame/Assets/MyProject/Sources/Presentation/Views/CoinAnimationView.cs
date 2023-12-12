using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CoinAnimationView : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _offsetYFinishPoint;

    private bool _canMove;

    // private CancellationTokenAwaitable _cancellationTokenAwaitable = new CancellationTokenAwaitable();
    //TODO работает ли тут обычный CancelationToken и как его правильно использовать
    private readonly CancellationToken _cancellationToken = new CancellationToken();

    private float _currentTime;
    private float _totalTime;

    //TODO сделать через он тригер ентер
    private void Start()
    {
        _totalTime = _animationCurve.keys[_animationCurve.keys.Length - 1].time;
    }

    private void OnEnable()
    {
        Collect();
    }

    //TODO как прокинуть канцелетион токен?
    private void OnDisable()
    {
        Debug.Log("Монетка выключилась");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _canMove = true;
        }
    }

    private async void Collect()
    {
        await RotateCoinAsync();
        await MoveToPlayer();
    }

    private async UniTask RotateCoinAsync()
    {
        while (_canMove == false)
        {
            transform.Rotate(0, _rotationSpeed, 0);

            await UniTask.Yield(_cancellationToken);
        }
    }

    private async UniTask MoveToPlayer()
    {
        // Vector3 targetPosition = new Vector3(_playerTransform.position.x,
        //     _playerTransform.position.y + _offsetYFinishPoint,
        //     _playerTransform.position.z);

        
        while (Vector3.Distance(transform.position, new Vector3(_playerTransform.position.x,
                   _playerTransform.position.y + _offsetYFinishPoint,
                   _playerTransform.position.z)) > 0.04f)
        {
            
            _currentTime += Time.deltaTime;

            // if (_currentTime >= _totalTime)
            //     _currentTime = 0;


            //TODO как убрать этот дубляж?
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(_playerTransform.position.x,
                    _playerTransform.position.y + 
                    _animationCurve.Evaluate(_currentTime),
                    _playerTransform.position.z), _movementSpeed * Time.deltaTime);

            await UniTask.Yield();            
        }
    }
}