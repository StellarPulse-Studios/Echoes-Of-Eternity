using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerOnMob : MonoBehaviour
{
    public GameObject player;
    private List<GameObject> MobSpawned = null;

    private void Start()
    {
        MobSpawned = this.GetComponent<DynamicItemSpawnerManager>().spawnedItems;
        foreach( var item in MobSpawned)
        {
            item.GetComponent<MobBlackboard>().Target = player;
        }
    }
}
