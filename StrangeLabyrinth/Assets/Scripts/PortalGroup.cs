using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGroup : MonoBehaviour
{
    public Transform heirarchy;
    public List<PlayerDetector> detectors; // optional
    public bool active;

    void Start ()
    {
        if (detectors.Count == 0) // permenantly add portals
        {
            MainCamera.AddPortals(heirarchy.GetComponentsInChildren<Portal>());
        }
        else
        {
            foreach (PlayerDetector detector in detectors)
            {
                detector.OnPlayerEnter += PlayerEnter;
                detector.OnPlayerExit += PlayerExit;
            }
        }
        PlayerExit(null);
    }

    void PlayerEnter (PlayerDetector _detector)
    {
        if (active) return; // already active
        heirarchy.gameObject.SetActive(true);
        MainCamera.AddPortals(heirarchy.GetComponentsInChildren<Portal>());
    }

    void PlayerExit (PlayerDetector _detector)
    {
        foreach (PlayerDetector detector in detectors) if (detector.containsPlayer) return;
        heirarchy.gameObject.SetActive(false);
        MainCamera.RemovePortals(heirarchy.GetComponentsInChildren<Portal>());
    }
}
