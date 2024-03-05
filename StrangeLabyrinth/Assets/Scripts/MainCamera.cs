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
        public bool linkedSceenVisibleToPlayer;
    }

    public static Dictionary<Portal, PrepassData> portals {get; private set;}


    public static void RemovePortals (IEnumerable<Portal> _portals)
    {
        foreach (Portal portal in _portals)
        {
            portals.Remove(portal);
        }
#if DEBUG
        Debug.Log($"{_portals.Count()} portals removed");
        Debug.Log($"{portals.Count()} active portals");
#endif
    }
    
    public static void AddPortals (IEnumerable<Portal> _portals)
    {
        foreach (Portal portal in _portals)
        {
            portals[portal] = new PrepassData() {bestPerspective = Camera.main};
        }
#if DEBUG
        Debug.Log($"{_portals.Count()} portals added");
        Debug.Log($"{portals.Count()} active portals");
#endif
    }

    void Awake ()
    {
        portals = new Dictionary<Portal, PrepassData>();
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
            data.linkedSceenVisibleToPlayer = false;
            data.bestPerspective = Camera.main;
        }
        foreach (KeyValuePair<Portal, PrepassData> portalData in portals) 
        {
            bool directlyVisible = GeometryUtility.TestPlanesAABB(playerFrustum, portalData.Key.linkedPortal.screen.bounds);
            if (!directlyVisible) continue;

            portalData.Value.linkedSceenVisibleToPlayer = true;
            
            // just do one for now
            foreach (Portal.ProxyPortal proxy in portalData.Key.proxies)
            {
                if (proxy.detector.containsPlayer)
                {
                    portals[proxy.proxy.linkedPortal].linkedSceenVisibleToPlayer = true;
                    portals[proxy.proxy.linkedPortal].bestPerspective = portalData.Key.portalCam;
                }
            }
        }
    }
}
