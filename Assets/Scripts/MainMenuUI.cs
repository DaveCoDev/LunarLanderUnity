using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class MainMenuUI : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private Button thrustUpRebindButton;
    private Button thrustLeftRebindButton;
    private Button thrustRightRebindButton;
    private Toggle debugToggle;
    private bool isDebugToggleEnabled;

    private const string RebindsPrefsName = "rebinds";
    private const string debugPrefsName = "debugEnabled";

    void Start()
    {
        playerInputActions = new PlayerInputActions();
        LoadBindings();
        isDebugToggleEnabled = PlayerPrefs.GetInt(debugPrefsName) == 1;

        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        
        var startButton = rootVisualElement.Q<Button>("startButton");
        var settingsButton = rootVisualElement.Q<Button>("settingsButton");
        var quitButton = rootVisualElement.Q<Button>("closeButton");
        
        var menuContainer = rootVisualElement.Q<VisualElement>("MenuContainer");
        var settingsContainer = rootVisualElement.Q<VisualElement>("SettingsContainer");

        var controlsBackButton = settingsContainer.Q<Button>("settingsBackButton");
        thrustUpRebindButton = settingsContainer.Q<Button>("thrustUpControl");
        thrustLeftRebindButton = settingsContainer.Q<Button>("thrustLeftControl");
        thrustRightRebindButton = settingsContainer.Q<Button>("thrustRightControl");
        debugToggle = settingsContainer.Q<Toggle>("debugToggle");

        thrustUpRebindButton.text = GetKeybindName(playerInputActions.Lander.ThrustUp);
        thrustLeftRebindButton.text = GetKeybindName(playerInputActions.Lander.ThrustLeft);
        thrustRightRebindButton.text = GetKeybindName(playerInputActions.Lander.ThrustRight);

        startButton.clicked += () =>
        {
            LoadGameScene();
        };
        
        settingsButton.clicked += () =>
        {
            menuContainer.style.display = DisplayStyle.None;
            settingsContainer.style.display = DisplayStyle.Flex;
        };

        quitButton.clicked += () =>
        {
            Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        };

        controlsBackButton.clicked += () =>
        {
            menuContainer.style.display = DisplayStyle.Flex;
            settingsContainer.style.display = DisplayStyle.None;
        };

        thrustUpRebindButton.clicked += () =>
        {
            InputAction key = playerInputActions.Lander.ThrustUp;
            RebindKey(key, thrustUpRebindButton);
        };

        thrustLeftRebindButton.clicked += () =>
        {
            InputAction key = playerInputActions.Lander.ThrustLeft;
            RebindKey(key, thrustLeftRebindButton);
        };
        
        thrustRightRebindButton.clicked += () =>
        {
            InputAction key = playerInputActions.Lander.ThrustRight;
            RebindKey(key, thrustRightRebindButton);
        };

        debugToggle.RegisterValueChangedCallback(DebugToggled);
        debugToggle.value = isDebugToggleEnabled;
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    private void RebindKey(InputAction keybind, Button button)
    {
        button.text = "Press any key...";
        keybind.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithControlsExcluding("<Gamepad>/LeftStick/X")
            .WithControlsExcluding("<Gamepad>/LeftStick/Y")
            .WithControlsExcluding("<Gamepad>/RightStick/X")
            .WithControlsExcluding("<Gamepad>/RightStick/Y")
            .OnComplete(callback =>
            {
                callback.Dispose();
                SaveBindings();
                button.text = GetKeybindName(callback.action);
            })
            .Start();
    }

    private void SaveBindings()
    {
        string rebinds = playerInputActions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(RebindsPrefsName, rebinds);
    }

    private void LoadBindings()
    {
        string rebinds = PlayerPrefs.GetString(RebindsPrefsName, string.Empty);

        if (string.IsNullOrEmpty(rebinds))
        {
            return;
        }
        else
        {
            playerInputActions.LoadBindingOverridesFromJson(rebinds);
        }
    }
    
    private string GetKeybindName(InputAction keybind)
    {
        int bindingIndex = keybind.GetBindingIndexForControl(keybind.controls[0]);
        string text = InputControlPath.ToHumanReadableString(
            keybind.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        return text;
    }

    private void DebugToggled(ChangeEvent<bool> eventData)
    {
        PlayerPrefs.SetInt(debugPrefsName, eventData.newValue ? 1 : 0);
    }

}
