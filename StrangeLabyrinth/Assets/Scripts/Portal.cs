using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor.SearchService;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal;
    
    private MeshRenderer screen;
    private Camera portalCam;
    private Camera playerCam;
    private RenderTexture viewTexture;
    
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
        if (!VisibleFromCamera()) return;
        screen.enabled = false;
        CreateViewTexture();

        Matrix4x4 transMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * playerCam.transform.localToWorldMatrix;
        portalCam.transform.SetPositionAndRotation(transMatrix.GetColumn(3), transMatrix.rotation);

        portalCam.Render();
        screen.enabled = true;
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

    private bool VisibleFromCamera ()
    {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(playerCam);
        return GeometryUtility.TestPlanesAABB(frustumPlanes, linkedPortal.screen.bounds);
    }
}
