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
        foreach (Portal portal in portals.Keys) portal.Reposition();
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
            data.bestPerspective = Camera.main;
        }
        foreach (KeyValuePair<Portal, PrepassData> portalData in portals) 
        {
            bool directlyVisible = GeometryUtility.TestPlanesAABB(playerFrustum, portalData.Key.linkedPortal.screen.bounds);
            if (!directlyVisible) continue;

            // portalData.Value.bestPerspective = Camera.main;
            portalData.Value.linkedSceenVisibleToPlayer = true;
            Plane[] portalCamFrustum = GeometryUtility.CalculateFrustumPlanes(portalData.Key.portalCam);
            
            // look at second level portal screens
            foreach (Portal deepPortal in portalData.Key.secondDepthPortals)
            {
                if (GeometryUtility.TestPlanesAABB(portalCamFrustum, deepPortal.screen.bounds))
                {
                    portals[deepPortal.linkedPortal].linkedSceenVisibleToPlayer = true;
                    float directDistance = (Camera.main.transform.position - deepPortal.transform.position).magnitude;
                    float secondDistance = (portalData.Key.transform.position - deepPortal.transform.position).magnitude;
                    if (secondDistance < directDistance) 
                    {
                        portals[deepPortal.linkedPortal].bestPerspective = portalData.Key.portalCam;
                    }



                    // float nearPortalDistance = (Camera.main.transform.position - portalData.Key.linkedPortal.transform.position).magnitude;
                    // if (secondDistance < directDistance && nearPortalDistance < portals[deepPortal].bestNearPortalDistance) 
                    // {
                    //     portals[deepPortal].bestNearPortalDistance = nearPortalDistance;
                    //     portals[deepPortal].bestPerspective = portalData.Key.portalCam;
                    // }
                }
            }
        }
    }
}
