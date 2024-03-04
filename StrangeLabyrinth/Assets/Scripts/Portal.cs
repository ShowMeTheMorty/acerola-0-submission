using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor.SearchService;
using UnityEngine;

/// <summary>
/// Heavily modelled on Sebastian Lague's Portals project
/// I don't fully understand how the clipping issues were solved, 
// nor how the near clipping plane is manipulated for oblique projection
// nor how recursion works :)
/// </summary>
public class Portal : MonoBehaviour
{
    public Portal linkedPortal;
    
    internal MeshRenderer screen;
    internal Camera portalCam;
    private Camera playerCam;
    private RenderTexture viewTexture;

    const int recursionLimit = 1;
    
    void Awake ()
    {
        portalCam = GetComponentInChildren<Camera>();
        playerCam = Camera.main;
        screen = GetComponent<MeshRenderer>();

        portalCam.fieldOfView = playerCam.fieldOfView;
        portalCam.enabled = false;
    }

    void CreateViewTexture ()
    {
        if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height)
        {
            if (viewTexture != null) viewTexture.Release();
            viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
            portalCam.targetTexture = viewTexture;
            linkedPortal.screen.material.SetTexture("_MainTex", viewTexture);
        }
    }

    public void Render ()
    {
        if (!MainCamera.portals[this].linkedSceenVisibleToPlayer) return;
        screen.enabled = false;
        CreateViewTexture();

        Matrix4x4 localToWorld = MainCamera.portals[this].bestPerspective.transform.localToWorldMatrix;
        Matrix4x4 matrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * localToWorld;

        screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

        portalCam.transform.SetPositionAndRotation(matrix.GetColumn(3), matrix.rotation);
        SetNearClipPlane();
        linkedPortal.ProtectFromClipping();
        portalCam.Render();

        screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

        screen.enabled = true;
    }

    private void SetNearClipPlane()
    {
        int dot = Math.Sign(Vector3.Dot(transform.forward, transform.position - portalCam.transform.position));
        Vector3 camSpacePos = portalCam.worldToCameraMatrix.MultiplyPoint(transform.position);
        Vector3 camSpaceNormal = portalCam.worldToCameraMatrix.MultiplyVector(transform.forward) * dot;
        float camSpaceDist = -Vector3.Dot(camSpacePos, camSpaceNormal) + 0.01f;
        Vector4 clipPlaneCamSpace = new Vector4(camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDist);
        portalCam.projectionMatrix = MainCamera.portals[this].bestPerspective.CalculateObliqueMatrix(clipPlaneCamSpace);
    }

    public void ProtectFromClipping ()
    {
        float halfHeight = playerCam.nearClipPlane * Mathf.Tan(playerCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * playerCam.aspect;
        float distToClipPlaneCorner = new Vector3(halfWidth, halfHeight, playerCam.nearClipPlane).magnitude;
        bool camPotalViewAligned = Vector3.Dot(transform.forward, transform.position - playerCam.transform.position) > 0;
        screen.transform.localScale = new Vector3(screen.transform.localScale.x, screen.transform.localScale.y, distToClipPlaneCorner);
        screen.transform.localPosition = Vector3.forward * distToClipPlaneCorner * (camPotalViewAligned ? 0.5f : -0.5f);
    }
}
