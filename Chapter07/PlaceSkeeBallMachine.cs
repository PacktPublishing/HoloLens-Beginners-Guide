using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity;
using HoloToolkit.Unity.SpatialMapping;

public class PlaceSkeeBallMachine : MonoBehaviour, IInputClickHandler
{

    public string skeeBallAnchorName = "SkeeBallMachine01";
    WorldAnchorManager wAnchorManager;


    [Tooltip("Place parent on tap instead of current game object.")]
    public bool PlaceParentOnTap;

    [Tooltip("Specify the parent game object to be moved on tap, if the immediate parent is not desired.")]
    public GameObject ParentGameObjectToPlace;

    /// <summary>
    /// Keeps track of if the user is moving the object or not.
    /// Setting this to true will enable the user to move and place the object in the scene.
    /// Useful when you want to place an object immediately.
    /// </summary>
    [Tooltip("Setting this to true will enable the user to move and place the object in the scene without needing to tap on the object. Useful when you want to place an object immediately.")]
    public bool IsBeingPlaced;

    /// <summary>
    /// Controls spatial mapping.  In this script we access spatialMappingManager
    /// to control rendering and to access the physics layer mask.
    /// </summary>
    protected SpatialMappingManager spatialMappingManager;

    public float heightBuffer = 0.15f;
    public static bool placed = false;

    protected virtual void Start()
    {
        wAnchorManager = WorldAnchorManager.Instance;
                
        spatialMappingManager = SpatialMappingManager.Instance;
        if (spatialMappingManager == null)
        {
            Debug.LogError("This script expects that you have a SpatialMappingManager component in your scene.");
        }
        
        if(wAnchorManager != null && spatialMappingManager != null)
        {
            wAnchorManager.AttachAnchor(gameObject, skeeBallAnchorName);
        }
        else
        {
            Destroy(this);
        }
        

        if (PlaceParentOnTap)
        {
            if (ParentGameObjectToPlace != null && !gameObject.transform.IsChildOf(ParentGameObjectToPlace.transform))
            {
                Debug.LogError("The specified parent object is not a parent of this object.");
            }

            DetermineParent();
        }
    }

    protected virtual void Update()
    {
        // If the user is in placing mode,
        // update the placement to match the user's gaze.
        if (IsBeingPlaced)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            Vector3 headPosition = Camera.main.transform.position;
            Vector3 gazeDirection = Camera.main.transform.forward;

            RaycastHit hitInfo;
            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo, 30.0f, spatialMappingManager.LayerMask))
            {
                // Rotate this object to face the user.
                Quaternion toQuat = Camera.main.transform.localRotation;
                toQuat.x = 0;
                toQuat.z = 0;

                // Move this object to where the raycast
                // hit the Spatial Mapping mesh.
                // Here is where you might consider adding intelligence
                // to how the object is placed.  For example, consider
                // placing based on the bottom of the object's
                // collider so it sits properly on surfaces.
                if (PlaceParentOnTap)
                {
                    // Place the parent object as well but keep the focus on the current game object
                    Vector3 currentMovement = hitInfo.point - gameObject.transform.position;
                    ParentGameObjectToPlace.transform.position += new Vector3(currentMovement.x, currentMovement.y + heightBuffer, currentMovement.z);
                    ParentGameObjectToPlace.transform.rotation = toQuat;
                }
                else
                {
                    gameObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + heightBuffer, hitInfo.point.z);
                    gameObject.transform.rotation = toQuat;
                }
            }
        }
    }

    public virtual void OnInputClicked(InputClickedEventData eventData)
    {
        // On each tap gesture, toggle whether the user is in placing mode.

        IsBeingPlaced = !IsBeingPlaced;

        // If the user is in placing mode, display the spatial mapping mesh.
        if (IsBeingPlaced && !placed)
        {
            spatialMappingManager.DrawVisualMeshes = true;
            wAnchorManager.RemoveAnchor(gameObject);

        }
        // If the user is not in placing mode, hide the spatial mapping mesh.
        else
        {
            placed = true;
            ApplicationManager.Instance.skeeBallMachinePlacementSet = true;
            spatialMappingManager.DrawVisualMeshes = false;
            wAnchorManager.AttachAnchor(gameObject, skeeBallAnchorName);

        }
    }

    private void DetermineParent()
    {
        if (ParentGameObjectToPlace == null)
        {
            if (gameObject.transform.parent == null)
            {
                Debug.LogError("The selected GameObject has no parent.");
                PlaceParentOnTap = false;
            }
            else
            {
                Debug.LogError("No parent specified. Using immediate parent instead: " + gameObject.transform.parent.gameObject.name);
                ParentGameObjectToPlace = gameObject.transform.parent.gameObject;
            }
        }
    }
}

