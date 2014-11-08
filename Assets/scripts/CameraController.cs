using UnityEngine;
using System.Collections;

/**
 * Controlls the Camera.
 */
public class CameraController : MonoBehaviour9Bits {

    public float wL = 2f;
    public float wS = 0.1f;
    public float hL = 3.2f;
    public float hS = 0.05f;

    public Transform player;
    public Vector3 lookAtOffset;

    private Vector3 initPos;

    void Awake () {
        initPos = transform.position;
	}
	
	void Update () {
        //Camera waving movement
        if (player) transform.LookAt(player.position + lookAtOffset);
        
        transform.position = initPos + new Vector3(
            Mathf.Sin(Time.time / wL) * wS,
            Mathf.Sin(Time.time / hL) * hS,
            0
        );
	}
}
