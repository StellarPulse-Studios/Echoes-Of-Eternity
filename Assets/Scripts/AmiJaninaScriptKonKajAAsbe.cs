using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmiJaninaScriptKonKajAAsbe : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print(other.name+" Enter");
    }
    private void OnTriggerStay(Collider other)
    {
        print(other.name + " Stay");
    }
    private void OnTriggerExit(Collider other)
    {
        print(other.name + " Exit");
    }
}
