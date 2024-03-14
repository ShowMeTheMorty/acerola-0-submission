using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDetector : MonoBehaviour
{
    public Transform unlock;
    public PlayerDetector noMusic;

    // Start is called before the first frame update
    void Start()
    {
        unlock.gameObject.SetActive(false);
        GetComponent<PlayerDetector>().OnPlayerEnter += Hooray;
    }

    private void Hooray(PlayerDetector playerDetector)
    {
        unlock.gameObject.SetActive(true);
        noMusic.setMusic = false;
    }

}
