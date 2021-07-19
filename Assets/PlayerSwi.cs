// GENERATED AUTOMATICALLY FROM 'Assets/PlayerSwi.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerSwi : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerSwi()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerSwi"",
    ""maps"": [
        {
            ""name"": ""PlayerSwitch"",
            ""id"": ""2f7b58bf-c451-42bb-8357-4fc38c88f319"",
            ""actions"": [
                {
                    ""name"": ""Switch"",
                    ""type"": ""Button"",
                    ""id"": ""88a7f569-7d0e-48bc-b88e-dfb7d0b8f0e0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""32f1ad35-56e2-4e44-a211-47f9ce2eb43c"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Switch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6432b0f1-c448-4e6d-808e-a0050e73dbec"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Switch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerSwitch
        m_PlayerSwitch = asset.FindActionMap("PlayerSwitch", throwIfNotFound: true);
        m_PlayerSwitch_Switch = m_PlayerSwitch.FindAction("Switch", throwIfNotFound: true);
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

    // PlayerSwitch
    private readonly InputActionMap m_PlayerSwitch;
    private IPlayerSwitchActions m_PlayerSwitchActionsCallbackInterface;
    private readonly InputAction m_PlayerSwitch_Switch;
    public struct PlayerSwitchActions
    {
        private @PlayerSwi m_Wrapper;
        public PlayerSwitchActions(@PlayerSwi wrapper) { m_Wrapper = wrapper; }
        public InputAction @Switch => m_Wrapper.m_PlayerSwitch_Switch;
        public InputActionMap Get() { return m_Wrapper.m_PlayerSwitch; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerSwitchActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerSwitchActions instance)
        {
            if (m_Wrapper.m_PlayerSwitchActionsCallbackInterface != null)
            {
                @Switch.started -= m_Wrapper.m_PlayerSwitchActionsCallbackInterface.OnSwitch;
                @Switch.performed -= m_Wrapper.m_PlayerSwitchActionsCallbackInterface.OnSwitch;
                @Switch.canceled -= m_Wrapper.m_PlayerSwitchActionsCallbackInterface.OnSwitch;
            }
            m_Wrapper.m_PlayerSwitchActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Switch.started += instance.OnSwitch;
                @Switch.performed += instance.OnSwitch;
                @Switch.canceled += instance.OnSwitch;
            }
        }
    }
    public PlayerSwitchActions @PlayerSwitch => new PlayerSwitchActions(this);
    public interface IPlayerSwitchActions
    {
        void OnSwitch(InputAction.CallbackContext context);
    }
}
