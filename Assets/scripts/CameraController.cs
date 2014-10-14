using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour9Bits {

    public float wL = 2f;
    public float wS = 0.1f;
    public float hL = 3.2f;
    public float hS = 0.05f;

    public Transform player;
    public Vector3 lookAtOffset;

    private Vector3 initPos;

	// Use this for initialization
	void Start () {
        initPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (player) transform.LookAt(player.position + lookAtOffset);
        
        transform.position = initPos + new Vector3(
            Mathf.Sin(Time.time / wL) * wS,
            Mathf.Sin(Time.time / hL) * hS,
            0
        );
	}
}
