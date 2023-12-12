using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class WaterParticleController : MonoBehaviour
{
    [SerializeField] private GameObject _waterParticle;
    [SerializeField] private float _stopDelay;
    
    private async void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
            PlayParticle();
        
        //TODO правильно ли тут все?
        if(Input.GetKeyUp(KeyCode.LeftShift) && _waterParticle.activeSelf == true)
            await StopPlayParticleAsync();
    }

    private void PlayParticle()
    {
        _waterParticle.SetActive(true);
    }

    private void StopPlayParticle()
    {
        _waterParticle.SetActive(false);
    }

    private async UniTask StopPlayParticleAsync()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_stopDelay));

        StopPlayParticle();
    }
}
