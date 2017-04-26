using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class OptionsButton : MonoBehaviour, IInputClickHandler {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnInputClicked(InputClickedEventData eventData)
    {


        ApplicationManager.Instance.debugInfoString += "\n Options Button Clicked";
    }
}
