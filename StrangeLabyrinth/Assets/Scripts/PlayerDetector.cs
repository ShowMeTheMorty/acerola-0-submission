using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public bool setMusic;
    public AudioClip track;
    public float trackVolume;

    public bool containsPlayer;
    private bool didContainPlayer;

    public bool setSpawnPoint;

    public delegate void PlayerEnterHandler (PlayerDetector playerDetector);
    public delegate void PlayerExitHandler (PlayerDetector playerDetector);

    public event PlayerExitHandler OnPlayerExit;
    public event PlayerEnterHandler OnPlayerEnter;

    private int timeInArea;

    void Start ()
    {
        Destroy(GetComponent<MeshRenderer>());
    }

    void LateUpdate ()
    {
        Vector3 toPlayer = transform.position - Camera.main.transform.position;
        toPlayer = transform.InverseTransformVector(toPlayer);
        containsPlayer = Mathf.Abs(toPlayer.x) < 0.5f
            && Mathf.Abs(toPlayer.y) < 0.5f
            && Mathf.Abs(toPlayer.z) < 0.5f;

        if (containsPlayer && !didContainPlayer) 
        {
            OnPlayerEnter?.Invoke(this);
            if (setMusic) AudioManager.singleton.SetDesiredMusicClip(track, trackVolume);
        }
        if (!containsPlayer && didContainPlayer) 
        {
            OnPlayerExit?.Invoke(this);
            setSpawnPoint = false;
            if (setMusic) AudioManager.singleton.SetDesiredMusicClip(null, 0f);
        }

        if (containsPlayer && Player.player.isGrounded && Player.player.respawnFrame == 0) timeInArea++;
        if (setSpawnPoint && timeInArea == 120) Player.SetSpawn();

        didContainPlayer = containsPlayer;
    }
}
