using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputService : MonoBehaviour
{

    public event Action<Vector2> MovementAxisChanged;
    public event Action<float> RunAxisChanged;
    public event Action<bool, bool> RotationChanged; 
    
    private void Update()
    {
        UpdateMovementAxis();
        UpdateRunAxis();
        UpdateRotation();
    }

    private void UpdateMovementAxis()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movementInput = new Vector2(horizontalInput, verticalInput);
        
        MovementAxisChanged?.Invoke(movementInput);
    }

    private void UpdateRunAxis()
    {
        float runInput = Input.GetAxis("Run");
        
        RunAxisChanged?.Invoke(runInput);
    }

    //TODO перенести в фикседапдейт
    private void UpdateRotation()
    {
        bool isLeftRotation = Input.GetKey(KeyCode.Q);
        bool isRightRotation = Input.GetKey(KeyCode.E);
        
        RotationChanged?.Invoke(isLeftRotation, isRightRotation);
    }
}
