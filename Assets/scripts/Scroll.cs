using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour {

	public bool IniciarEnMovimiento = false;
	public float velocidad = 0f;
	private bool enMovimiento = false;
	private float tiempoInicio = 0f;

	// Use this for initialization
	void Start () {
		if (IniciarEnMovimiento) {
			enMovimiento = true;
		}
	}


	// Update is called once per frame
	void Update () {
		if(enMovimiento){
			renderer.material.mainTextureOffset = new Vector2(((Time.time - tiempoInicio) * velocidad) % 1, 0);
		
		}
	}
}
