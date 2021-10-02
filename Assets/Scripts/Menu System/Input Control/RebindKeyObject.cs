using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;


// Class to be placed onto UI objects to initiate rebind of a key
public class RebindKeyObject : MonoBehaviour
{
    [SerializeField] private InputActionReference inputAction; // Input Action
    [SerializeField] private GameObject rebindButton; //button obect
    [SerializeField] private GameObject bindingDisplayNameObject; // Object with text for what action is bound to
    [SerializeField] private GameObject waitingForInputObject; //waiting for input object

    private PlayerInput playerInput;   // player input class
    private RebindingDisplay rebindingDisplay;
    private string rebindsKey = "rebinds";

    private void Start()
    {
        rebindingDisplay = FindObjectOfType<RebindingDisplay>();
        if (rebindingDisplay != null)
        {
            //load saved rebinds
            playerInput = rebindingDisplay.GetSelectedPlayerInput(); //set the player input object for loading data

            string rebinds = PlayerPrefs.GetString(rebindsKey, string.Empty);
            if (!string.IsNullOrEmpty(rebinds)) 
            {
                playerInput.actions.LoadBindingOverridesFromJson(rebinds);
            } 
        }
        else
        {
            Debug.LogError("Please set up a Rebinding Display object for rebinding key object to work!");
        }

        int bindingIndex = inputAction.action.GetBindingIndexForControl(inputAction.action.controls[0]);

        // set bound key text
        bindingDisplayNameObject.GetComponent<Text>().text = InputControlPath.ToHumanReadableString(
            inputAction.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    //Sends info to rebinding display when button pressed
    public void SendRebindingInformation()
    {
        rebindingDisplay.StartRebinding(inputAction, rebindButton, bindingDisplayNameObject, waitingForInputObject, rebindsKey);
    }
}
