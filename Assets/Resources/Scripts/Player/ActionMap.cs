// GENERATED AUTOMATICALLY FROM 'Assets/Resources/ActionMap/ActionMap.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ActionMap : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ActionMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ActionMap"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""c61b91db-47c1-4d67-9107-711c533d20f0"",
            ""actions"": [
                {
                    ""name"": ""JumpPress"",
                    ""type"": ""Button"",
                    ""id"": ""1d57e9cd-fc81-4320-b248-ef770417d43e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.1)""
                },
                {
                    ""name"": ""JumpRelease"",
                    ""type"": ""Button"",
                    ""id"": ""b38f9270-0fa3-4299-bf99-25d4feb764a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""f6e62f51-d207-423d-b536-14a33ad48db5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DashPress"",
                    ""type"": ""Button"",
                    ""id"": ""34ffe674-74b8-4de4-af0d-0f7c7eeb5447"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.1)""
                },
                {
                    ""name"": ""DashRelease"",
                    ""type"": ""Button"",
                    ""id"": ""c3c9af74-998f-4924-87d7-f7d686bfa73e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.1,behavior=1)""
                },
                {
                    ""name"": ""DiveBounce"",
                    ""type"": ""Button"",
                    ""id"": ""1fb01794-f4ee-4403-8d46-5d8527888e3c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bc0e77be-4b37-4f9a-9076-e6e389b62589"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""JumpPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""92e9f923-2573-4bd8-b8f7-19e3a84b1575"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""JumpRelease"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""130d9858-664d-4a44-8f44-ff7c7f124541"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""83e1ab47-939b-4f09-b1e2-e3cbd7ec3884"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0fb8c19e-9cbb-4f9e-9f61-bf44d1def63b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4295c9a7-a305-43ac-948d-8eb6b961cc58"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9e2d8d18-c330-4e73-81d6-eb981d0db84c"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""efd7a264-919f-4ade-bda2-d80914c3d2bb"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DashPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a490bcf6-7e12-47df-b9e9-7f1c04656f7e"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press(pressPoint=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DiveBounce"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f4a7385d-ae01-4da9-a60a-242ff3cf2202"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DashRelease"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_JumpPress = m_Player.FindAction("JumpPress", throwIfNotFound: true);
        m_Player_JumpRelease = m_Player.FindAction("JumpRelease", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_DashPress = m_Player.FindAction("DashPress", throwIfNotFound: true);
        m_Player_DashRelease = m_Player.FindAction("DashRelease", throwIfNotFound: true);
        m_Player_DiveBounce = m_Player.FindAction("DiveBounce", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_JumpPress;
    private readonly InputAction m_Player_JumpRelease;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_DashPress;
    private readonly InputAction m_Player_DashRelease;
    private readonly InputAction m_Player_DiveBounce;
    public struct PlayerActions
    {
        private @ActionMap m_Wrapper;
        public PlayerActions(@ActionMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @JumpPress => m_Wrapper.m_Player_JumpPress;
        public InputAction @JumpRelease => m_Wrapper.m_Player_JumpRelease;
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @DashPress => m_Wrapper.m_Player_DashPress;
        public InputAction @DashRelease => m_Wrapper.m_Player_DashRelease;
        public InputAction @DiveBounce => m_Wrapper.m_Player_DiveBounce;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @JumpPress.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJumpPress;
                @JumpPress.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJumpPress;
                @JumpPress.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJumpPress;
                @JumpRelease.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJumpRelease;
                @JumpRelease.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJumpRelease;
                @JumpRelease.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJumpRelease;
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @DashPress.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDashPress;
                @DashPress.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDashPress;
                @DashPress.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDashPress;
                @DashRelease.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDashRelease;
                @DashRelease.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDashRelease;
                @DashRelease.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDashRelease;
                @DiveBounce.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDiveBounce;
                @DiveBounce.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDiveBounce;
                @DiveBounce.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDiveBounce;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @JumpPress.started += instance.OnJumpPress;
                @JumpPress.performed += instance.OnJumpPress;
                @JumpPress.canceled += instance.OnJumpPress;
                @JumpRelease.started += instance.OnJumpRelease;
                @JumpRelease.performed += instance.OnJumpRelease;
                @JumpRelease.canceled += instance.OnJumpRelease;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @DashPress.started += instance.OnDashPress;
                @DashPress.performed += instance.OnDashPress;
                @DashPress.canceled += instance.OnDashPress;
                @DashRelease.started += instance.OnDashRelease;
                @DashRelease.performed += instance.OnDashRelease;
                @DashRelease.canceled += instance.OnDashRelease;
                @DiveBounce.started += instance.OnDiveBounce;
                @DiveBounce.performed += instance.OnDiveBounce;
                @DiveBounce.canceled += instance.OnDiveBounce;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnJumpPress(InputAction.CallbackContext context);
        void OnJumpRelease(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnDashPress(InputAction.CallbackContext context);
        void OnDashRelease(InputAction.CallbackContext context);
        void OnDiveBounce(InputAction.CallbackContext context);
    }
}
