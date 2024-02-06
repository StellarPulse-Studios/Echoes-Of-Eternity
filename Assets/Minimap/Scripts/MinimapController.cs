using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minimap
{
    public class MinimapController : MonoBehaviour
    {
        [SerializeField] private float m_LandscapeSize = 1024.0f;
        [SerializeField] private Vector2 m_LandscapeMin = new Vector2(-512.0f, -512.0f);
        [SerializeField] private Vector2 m_LandscapeMax = new Vector2(512.0f, 512.0f);

        [SerializeField] private Transform m_PlayerTransform;
        [SerializeField] private Transform m_PlayerCameraTransform;
        [SerializeField] private Camera m_MinimapCamera;
        [SerializeField] private Material m_MinimapMaterial;

        private Transform m_MinimapCameraTransform;

        private void Start()
        {
            m_MinimapCameraTransform = m_MinimapCamera.transform;
        }

        private void Update()
        {
            float playerCameraYRot = m_PlayerCameraTransform.eulerAngles.y;
            Vector3 minimapCameraRot = m_MinimapCameraTransform.eulerAngles;

            Vector3 playerPos = m_PlayerTransform.position;
            playerPos.y = m_MinimapCameraTransform.position.y;
            m_MinimapCameraTransform.rotation = Quaternion.Euler(minimapCameraRot.x, playerCameraYRot, minimapCameraRot.z);
            m_MinimapCameraTransform.position = playerPos;

            float size = (2.0f / m_LandscapeSize) * m_MinimapCamera.orthographicSize;
            m_MinimapMaterial.mainTextureScale = new Vector2(size, size);

            float x = Mathf.InverseLerp(m_LandscapeMin.x, m_LandscapeMax.x, playerPos.x);
            float y = Mathf.InverseLerp(m_LandscapeMin.y, m_LandscapeMax.y, playerPos.z);
            m_MinimapMaterial.mainTextureOffset = new Vector2(x, y);
            m_MinimapMaterial.SetFloat("_Rotation", playerCameraYRot * Mathf.Deg2Rad);
        }
    }
}
