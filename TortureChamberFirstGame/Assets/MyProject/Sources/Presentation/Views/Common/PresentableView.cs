using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

//TODO какой это слой?
public class PresentableView<T> : MonoBehaviour where T : IPresenter
{
    protected T Presenter { get; private set; }

    public void OnEnable() => 
        Presenter?.Enable();

    public void OnDisable() => 
        Presenter?.Disable();

    public virtual void Construct(T presenter)
    {
        Hide();
        Presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        Show();
    }

    private void Hide() => 
        gameObject.SetActive(false);

    private void Show() => 
        gameObject.SetActive(true);
}