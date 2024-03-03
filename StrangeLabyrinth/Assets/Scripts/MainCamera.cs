using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public IEnumerable<Portal> portals {get; set;}

    void Awake ()
    {
        portals = FindObjectsOfType<Portal>();
    }

    void OnPreCull ()
    {
        foreach (Portal portal in portals) portal.Render();
        foreach (Portal portal in portals) portal.ProtectFromClipping();
    }
}
