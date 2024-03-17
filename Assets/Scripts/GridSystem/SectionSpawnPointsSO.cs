using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new sectionSpawnPointsSO",menuName ="Section/Spawn Point")]
public class SectionSpawnPointsSO: ScriptableObject
{
    public List<Vector3> points = new List<Vector3>();

}