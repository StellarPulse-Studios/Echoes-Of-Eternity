using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ShowSection : MonoBehaviour
{
    public Color color;
    void OnDrawGizmos()
    {
        BoxCollider collider= GetComponent<BoxCollider>();
        if(collider != null)
            DrawBoxColliderOutline(collider,transform);
    }

    void DrawBoxColliderOutline(BoxCollider boxCollider,Transform transform)
    {
        // Get a random color
        Color randomColor = color;
        Gizmos.matrix= transform.localToWorldMatrix;
        // Draw gizmos around the borders of the BoxCollider
        Gizmos.color = randomColor;
        Gizmos.DrawCube(boxCollider.center, boxCollider.size);
    }
    [ContextMenu("Generate Color")]
    private void GenerateRandomColor()
    {
        color = Random.ColorHSV();
        color.a = .5f;
    }
}
