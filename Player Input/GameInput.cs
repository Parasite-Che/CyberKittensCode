using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnJumpActionPerformed;
    public event EventHandler OnShootActionPerformed;
    public event EventHandler OnInteractActionPerformed;
    public event EventHandler OnFirstKittenChosePerformed;
    public event EventHandler OnSecondKittenChosePerformed;
    public event EventHandler OnThirdKittenChosePerformed;
    public event EventHandler OnEscClickedPerformed;


    public PlayerControls inputActions;


    // Start is called before the first frame update
    void Start()
    {
        inputActions = new PlayerControls();
        inputActions.Enable();

        inputActions.Player.Jump.performed += JumpActionPerformed;
        inputActions.Player.Shoot.performed += ShootActionPerformed;
        inputActions.Player.Interact.performed += InteractActionPerformed;
        inputActions.Player.ChooseFirstKitten.performed += FirstKittenChosePerformed;
        inputActions.Player.ChooseSecondKitten.performed += SecondKittenChosePerformed;
        inputActions.Player.ChooseThirdKitten.performed += ThirdKittenChosePerformed;
        inputActions.Player.InGameMenu.performed += EscClickPerformed;
    }

    private void InteractActionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractActionPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void ShootActionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnShootActionPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void FirstKittenChosePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnFirstKittenChosePerformed?.Invoke(this, EventArgs.Empty);
    }
    
    private void SecondKittenChosePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSecondKittenChosePerformed?.Invoke(this, EventArgs.Empty);
    }

    private void ThirdKittenChosePerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnThirdKittenChosePerformed?.Invoke(this, EventArgs.Empty);
    }

    private void JumpActionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJumpActionPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void EscClickPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnEscClickedPerformed?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        //Debug.Log("Moved performed");
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

    
    
}
