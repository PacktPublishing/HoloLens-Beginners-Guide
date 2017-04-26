using HoloToolkit.Unity;
using HoloToolkit.Sharing.Spawning;
using UnityEngine;
using HoloToolkit.Sharing;

public class SyncSpawnSkee : Singleton<SyncSpawnSkee>
{
    // public GameObject SpawnParent;
    public GameObject skeePrefab;
    public GameObject skeeMachine;
    public PrefabSpawnManager SpawnManager;

    public void SyncSpawnSkeeBallMachine()
    {

        SyncSpawnedObject spawnedObject = new SyncSpawnedObject();
        Vector3 position = skeePrefab.gameObject.transform.position;
        SpawnManager.Spawn(spawnedObject, position, skeePrefab.gameObject.transform.rotation, null, "skee", false);
        ApplicationManager.Instance.skeeballmachine = spawnedObject.GameObject;
        skeeMachine = spawnedObject.GameObject;
        skeeMachine.transform.parent = null;
        skeeMachine.name = "skee";  //spawnedObject.GameObject.name = "skee";



    }
}
//LocalUserID = SharingStage.Instance.Manager.GetLocalUser().GetID();