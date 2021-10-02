using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
 * Make a prefab which contains the text information, its own input action reference
 */


//Class that handles logic for rebinding a key

public class RebindingDisplay : MonoBehaviour
{
    [Tooltip("Player input component on player object")]
    [SerializeField] private PlayerInput playerInput;   // player input class
    [SerializeField] private string MenuActionMapName; // Menu Action map name...
    [SerializeField] private string GameplayActionMapName; // Gameplay Action map name...

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    // Think about making it optional to switch Action Maps. We may not want to go into the gameplay map when still in menu


    public void StartRebinding(InputActionReference inputAction, GameObject rebindButton, GameObject bindingDisplayNameObject, GameObject waitingForInputObject, string rebindsKey)
    {
        rebindButton.SetActive(false);
        waitingForInputObject.SetActive(true);

        playerInput.SwitchCurrentActionMap(MenuActionMapName); // changes action map to the menu -- temporary

        // wait for input and execute rebind
        rebindingOperation = inputAction.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete(inputAction, rebindButton, bindingDisplayNameObject, waitingForInputObject, rebindsKey))
            .Start();
    }

    private void RebindComplete(InputActionReference inputAction, GameObject rebindButton, GameObject bindingDisplayNameObject, GameObject waitingForInputObject, string rebindsKey)
    {
        UpdateBindUIObject(inputAction, bindingDisplayNameObject); //update the UI text

        // free memory
        rebindingOperation.Dispose();

        rebindButton.SetActive(true);
        waitingForInputObject.SetActive(false);

        SaveRebinds(rebindsKey); // save the rebind

        // switch back to gameplay action map -- temporary
        playerInput.SwitchCurrentActionMap(GameplayActionMapName);
    }

    public void UpdateBindUIObject(InputActionReference inputAction, GameObject bindingDisplayNameObject)
    {
        Debug.Log("Changing UI");

        int bindingIndex = inputAction.action.GetBindingIndexForControl(inputAction.action.controls[0]);

        // set bound key text
        bindingDisplayNameObject.GetComponent<Text>().text = InputControlPath.ToHumanReadableString(
            inputAction.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public PlayerInput GetSelectedPlayerInput()
    {
        return playerInput;
    }


    // Saves data
    public void SaveRebinds(string rebindsKey)
    {
        Debug.Log("Saving data");
        string rebinds = playerInput.actions.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString(rebindsKey, rebinds);
    }
}
