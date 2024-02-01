
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class AnimalSpawnner : MonoBehaviour
{
    public List<GameObject> animals;
    public Transform player;
    private List<GameObject> availableAnimals = new List<GameObject>();
    private List<GameObject> activeAnimals = new List<GameObject>();
    public int maxPoolCount = 20;//total animal count
    public float spawnRadius = 100.0f;
    public float despawnRadius = 150.0f;
    public int maxSpawnCount = 10;//before eye
    public float spawnInterval = 3.0f;

    private float timePassed = 0.0f;


    private void Start()
    {
        for(int i = 0; i < maxPoolCount; i++)
        {
            GameObject animal = Instantiate(animals[Random.Range(0,animals.Count)]);
            animal.SetActive(false);
            availableAnimals.Add(animal);
            animal.transform.SetParent(transform);
            animal.GetComponent<MobBlackboard>().Target = player.gameObject;
        }
    }
    private void Update()
    {
        if (timePassed >= spawnInterval)
        {
            despawnAnimal();
            spawnAnimal();

        }
        timePassed += Time.deltaTime;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!player) return;
        Handles.color = Color.green * 0.5f;
        Handles.DrawWireArc(player.position, Vector3.up, player.forward, 90.0f, spawnRadius);

        Handles.color = Color.red * 0.5f;
        Handles.DrawWireArc(player.position, Vector3.up, player.forward, 90.0f, despawnRadius);
    }
#endif
    private void spawnAnimal()
    {
        int count = Mathf.Clamp(maxSpawnCount - activeAnimals.Count, 0, Mathf.Clamp(maxSpawnCount,0,maxPoolCount));
        count = Mathf.Clamp(count, 0, availableAnimals.Count);
        for(int i = 0; i < count; i++)
        {
            GameObject animal = availableAnimals[availableAnimals.Count - 1];          
            Vector3 samplePos = player.position + onUnitCircle()*spawnRadius; 
            if(Physics.Raycast(samplePos+Vector3.up*10.0f,Vector3.down,out RaycastHit rayhit))
            {
                if (NavMesh.SamplePosition(rayhit.point + Vector3.up * 3.0f, out NavMeshHit navHit, 5.0f, -1))
                {
                    animal.transform.position = navHit.position;
                    availableAnimals.RemoveAt(availableAnimals.Count - 1);
                    activeAnimals.Add(animal);
                    animal.SetActive(true);
                }
            }
        }
    }

    private Vector3 onUnitCircle()
    {
        float angle = Random.Range(0.0f, 360.0f);
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(angle * Mathf.Deg2Rad));
    }

    private void despawnAnimal()
    {
        for (int i = activeAnimals.Count - 1; i >= 0; i--)
        {
            GameObject animal = activeAnimals[i];
            if (Vector3.Distance(player.position, animal.transform.position) > despawnRadius)
            {
                animal.SetActive(false);
                availableAnimals.Add(animal);
                activeAnimals.RemoveAt(i);
            }
        }
    }

}
