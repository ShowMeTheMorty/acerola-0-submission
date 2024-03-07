using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGroup : MonoBehaviour
{
    public Transform heirarchy;
    public PlayerDetector detector; // optional

    void Start ()
    {
        if (detector == null) // permenantly add portals
        {
            MainCamera.AddPortals(heirarchy.GetComponentsInChildren<Portal>());
        }
        else
        {
            detector.OnPlayerEnter += PlayerEnter;
            detector.OnPlayerExit += PlayerExit;
        }
    }

    void PlayerEnter (PlayerDetector _detector)
    {
        heirarchy.gameObject.SetActive(true);
        MainCamera.AddPortals(heirarchy.GetComponentsInChildren<Portal>());
    }

    void PlayerExit (PlayerDetector _detector)
    {
        heirarchy.gameObject.SetActive(false);
        MainCamera.RemovePortals(heirarchy.GetComponentsInChildren<Portal>());
    }
}
