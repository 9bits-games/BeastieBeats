using UnityEngine;
using System.Collections;

public class MainSC : SceneController {

    Commander commander;

	// Use this for initialization
	void Start () {
        this.commander = new Commander();

        GUIManager guiManager = this.GetComponent("GUIManager") as GUIManager;
        guiManager.commander = this.commander;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
