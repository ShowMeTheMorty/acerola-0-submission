using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class MainCamera : MonoBehaviour
{
    public class PrepassData
    {
        public Camera bestPerspective;
        public float bestNearPortalDistance;
        public bool linkedSceenVisibleToPlayer;
    }

    public static Dictionary<Portal, PrepassData> portals {get; set;}


    void Awake ()
    {
        Portal[] _portals = FindObjectsOfType<Portal>();
        portals = new Dictionary<Portal, PrepassData>();
        foreach (Portal portal in _portals)
        {
            portals[portal] = new PrepassData() {bestPerspective = Camera.main};
        }
    }

    void OnPreCull ()
    {
        VisibiliyPrepass();
        foreach (Portal portal in portals.Keys) portal.Render();
        foreach (Portal portal in portals.Keys) portal.ProtectFromClipping();
    }

    private static void VisibiliyPrepass()
    {
        Plane[] playerFrustum = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        foreach (PrepassData data in portals.Values) 
        {
            data.bestNearPortalDistance = float.MaxValue;
            data.linkedSceenVisibleToPlayer = false;
        }
        foreach (KeyValuePair<Portal, PrepassData> portalData in portals) 
        {
            bool directlyVisible = GeometryUtility.TestPlanesAABB(playerFrustum, portalData.Key.linkedPortal.screen.bounds);
            if (!directlyVisible) continue;

            portalData.Value.bestPerspective = Camera.main;
            portalData.Value.linkedSceenVisibleToPlayer = true;
            Plane[] portalCamFrustum = GeometryUtility.CalculateFrustumPlanes(portalData.Key.portalCam);
            
            // look at second level portal screens
            foreach (KeyValuePair<Portal, PrepassData> secondPortalData in portals)
            {
                if (portalData.Key == secondPortalData.Key) continue;
                if (GeometryUtility.TestPlanesAABB(portalCamFrustum, secondPortalData.Key.linkedPortal.screen.bounds))
                {
                    secondPortalData.Value.linkedSceenVisibleToPlayer = true;
                    float directDistance = (Camera.main.transform.position - secondPortalData.Key.linkedPortal.transform.position).magnitude;
                    float secondDistance = (portalData.Key.portalCam.transform.position - secondPortalData.Key.linkedPortal.transform.position).magnitude;
                    float nearPortalDistance = (Camera.main.transform.position - portalData.Key.linkedPortal.transform.position).magnitude;
                    if (secondDistance < directDistance && nearPortalDistance < secondPortalData.Value.bestNearPortalDistance) 
                    {
                        secondPortalData.Value.bestNearPortalDistance = nearPortalDistance;
                        secondPortalData.Value.bestPerspective = portalData.Key.portalCam;
                    }
                }
            }
        }
    }
}
