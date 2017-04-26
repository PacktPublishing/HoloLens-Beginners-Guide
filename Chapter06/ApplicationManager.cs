using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity.InputModule;

public class ApplicationManager : Singleton<ApplicationManager> ,IInputClickHandler, ISpeechHandler
{
    public GameObject soundLayer1;
    public GameObject soundLayer2;
    public GameObject soundLayer3;
    public GameObject soundLayer4;

    public GameObject skeeballmachine;

    public bool spatialMapSet = false;
    public bool skeeBallMachinePlacementSet = false;
    public bool toInit = true;  

    // Use this for initialization
    void Start () {

        InputManager.Instance.PushModalInputHandler(this.gameObject);
		
	}
	
	// Update is called once per frame
	void Update () {

        if (spatialMapSet)
        {
            skeeballmachine.SetActive(true);
            if (toInit)
            {
                //this is the first time through only
                skeeballmachine.transform.position = Camera.main.transform.position;
                skeeballmachine.transform.position += new Vector3(skeeballmachine.transform.position.x, -1.25f, 3.0f);
                //we have finished the initiallization of the skeeball machine set the toInit flag to false
                toInit = false;
            }            
        }
        else
        {
            skeeballmachine.SetActive(false);
        }

        //if the skeeball machine placement is set then we want to run all of the layers of sound.
        if (skeeBallMachinePlacementSet)
        {

            soundLayer1.SetActive(true);
            soundLayer2.SetActive(true);
            soundLayer3.SetActive(true);
            soundLayer4.SetActive(true);


        }
        else
        {
            soundLayer1.SetActive(false);
            soundLayer2.SetActive(false);
            soundLayer3.SetActive(false);
            soundLayer4.SetActive(false);

        }
		
	}

    void SetSpatialMap()
    {

        spatialMapSet = true;
        SpatialMappingManager.Instance.DrawVisualMeshes = false;
        InputManager.Instance.PopModalInputHandler();

    }

    public void OnInputClicked(InputEventData eventData)
    {

        SetSpatialMap();

    }

    public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
    {
        switch (eventData.RecognizedText.ToLower())
        {

            case "done":
                SetSpatialMap();
                break;
        }           

    }
}
