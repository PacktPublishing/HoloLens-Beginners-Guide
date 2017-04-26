using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity.InputModule;

public class ApplicationManager : Singleton<ApplicationManager>, IInputClickHandler, ISpeechHandler
{
    public GameObject soundLayer1;
    public GameObject soundLayer2;
    public GameObject soundLayer3;
    public GameObject soundLayer4;

    public GameObject skeeballmachine;
    public GameObject sharingPrefab;
    public GameObject spatialPrefab;

    public TextMesh debugTextMesh;
    public string debugInfoString = "DebugInfo:";

    
    public enum AppState
    {
        SharingInit = 0,
        ConnectingToServer,
        SpatialMapping,
        MapComplete,
        PlaceSkeeMachine,
        PlaceSkeeMachineComplete,
        StartGame,
        EndGame,
        Reset,
    }

    public AppState myAppstate;


    // Use this for initialization
    void Start()
    {

        InputManager.Instance.PushModalInputHandler(this.gameObject);
       
    }

    void HideSkeeMachine()
    {
        foreach (Transform child in skeeballmachine.transform)
        {
            child.gameObject.SetActive(false);
        }


    }
    void ShowSkeeMachine()
    {
        foreach (Transform child in skeeballmachine.transform)
        {
            child.gameObject.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {

        debugTextMesh.text = debugInfoString;
        switch (myAppstate)
        {
            case AppState.SharingInit:
                {
                    sharingPrefab.SetActive(true);
                    myAppstate = AppState.ConnectingToServer;
                    debugInfoString = "\n" + myAppstate;
                }
                break;
            case AppState.ConnectingToServer:
                {
                    //need a test to determine that the user is connected.
                    myAppstate = AppState.SpatialMapping;
                    debugInfoString = "\n" + myAppstate;
                }
                break;
            case AppState.SpatialMapping:
                {

                    spatialPrefab.SetActive(true);
                    debugInfoString = "\n" + myAppstate;
                }
                break;
            case AppState.MapComplete:
                {
                    SyncSpawnSkee.Instance.SyncSpawnSkeeBallMachine();
                    myAppstate = AppState.PlaceSkeeMachine;
                    debugInfoString = "\n" + myAppstate;
                }
                break;
            case AppState.PlaceSkeeMachine:
                {
                    debugInfoString = "\n" + myAppstate;
                }
                break;
            case AppState.PlaceSkeeMachineComplete:
                {                    
                    debugInfoString = "\n" + myAppstate;
                    soundLayer1.SetActive(true);
                    soundLayer2.SetActive(true);
                    soundLayer3.SetActive(true);
                    soundLayer4.SetActive(true);
                    myAppstate = AppState.StartGame;
                }
                break;
            case AppState.StartGame: { debugInfoString = "\n" + myAppstate; } break;
            case AppState.EndGame: { debugInfoString = "\n" + myAppstate; } break;
            case AppState.Reset:
                {
                    debugInfoString = "\n" + myAppstate;
                }
                break;
            default:
                {

                }
                break;
        }
    }

    void SetSpatialMap()
    {
        SpatialMappingManager.Instance.DrawVisualMeshes = false;
        InputManager.Instance.PopModalInputHandler();
        myAppstate = AppState.MapComplete;

    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (myAppstate == AppState.SpatialMapping)
        {
            SetSpatialMap();
        }

    }

    public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
    {
        if (myAppstate == AppState.SpatialMapping)
        {
            switch (eventData.RecognizedText.ToLower())
            {

                case "done":
                    SetSpatialMap();
                    break;
            }
        }
    }
}
