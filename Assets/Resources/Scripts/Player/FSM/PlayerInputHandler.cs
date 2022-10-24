using System;
using UnityEngine;

/// <summary>Responsible for storing all input from the player. Inputs are acquired from an Action Map using
/// Unity's New Input System.</summary>
public class PlayerInputHandler : MonoBehaviour
{
    private ActionMap _actionMapScript;
    
    [Header("Inputs")]
    public Vector2 Movement;
    public bool JumpPress;
    public bool JumpRelease;
    public bool DashPress;
    public bool DashRelease;
    public bool DashDownPress;
    public bool DashDownRelease;

    private void Awake()
    {
        // Generate action map to read user inputs:
        _actionMapScript = new ActionMap();
        _actionMapScript.Enable();
    }

    private void Update()
    {
        ProcessInput();
    }

    /// <summary>Stores input from player into member variables.</summary>
    private void ProcessInput(){
        
        JumpPress = _actionMapScript.Player.JumpPress.triggered;
        JumpRelease = _actionMapScript.Player.JumpRelease.triggered;
        DashPress = _actionMapScript.Player.DashPress.triggered;
        DashRelease = _actionMapScript.Player.DashRelease.triggered;
        DashDownPress = _actionMapScript.Player.DashDown.triggered;
        DashDownRelease = _actionMapScript.Player.DashDownRelease.triggered;
        Movement = _actionMapScript.Player.Movement.ReadValue<Vector2>();
    }
}
