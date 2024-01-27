using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace test
{
    public class BoxInteracter : MonoBehaviour
    {
        public Transform cube;
        public Transform point;

        public Color insidePoint;
        public Color outsidePoint;
        public Color sphereColor;
        public Color diagSphere;
        private void OnDrawGizmos()
        {
            Gizmos.color = isBoxPointIntersecting(cube.position,cube.rotation,cube.lossyScale,point.position)?insidePoint:outsidePoint;
            Matrix4x4 prevMat = Gizmos.matrix;
            Gizmos.matrix = cube.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
            Gizmos.matrix = prevMat;
        }

        private bool isBoxPointIntersecting(Vector3 boxPosition,Quaternion boxRotation, Vector3 boxScale,Vector3 pointPosition)
        {
            Vector3 localBoxPos = pointPosition - boxPosition;
            Quaternion localBoxRot = Quaternion.Inverse(boxRotation);
            Vector3 localBoxScale = new Vector3(1.0f / boxScale.x, 1.0f / boxScale.y, 1.0f / boxScale.z);
            Vector3 localPointPos = Vector3.Scale(localBoxRot * localBoxPos, localBoxScale);
            return localPointPos.x >= -0.5f && localPointPos.x <= 0.5f && localPointPos.y >= -0.5f && localPointPos.y <= 0.5f && localPointPos.z >= -0.5f && localPointPos.z <= 0.5f; 
        }

    }
    
}

