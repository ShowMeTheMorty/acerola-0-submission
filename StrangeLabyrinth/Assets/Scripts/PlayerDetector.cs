using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public bool containsPlayer;
    private bool didContainPlayer;

    public delegate void PlayerEnterHandler (PlayerDetector playerDetector);
    public delegate void PlayerExitHandler (PlayerDetector playerDetector);

    public event PlayerExitHandler OnPlayerExit;
    public event PlayerEnterHandler OnPlayerEnter;

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

        if (containsPlayer && !didContainPlayer) OnPlayerEnter?.Invoke(this);
        if (!containsPlayer && didContainPlayer) OnPlayerExit?.Invoke(this);

        didContainPlayer = containsPlayer;
    }
}
