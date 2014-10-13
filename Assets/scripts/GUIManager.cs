using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIManager : MonoBehaviour9Bits {

    public Commander commander;

    public float bgSize = 0.4f;
    public float paddingH = 0.1f;
    public float marginTop = 0.1f;
    public float buttonSize = 0.2f;

    Note[] notes = {
        new Note { Name = "Do" },
        new Note { Name = "Re" },
        new Note { Name = "Mi" },
        new Note { Name = "Fa" },
        new Note { Name = "Sol" }
    };


	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
//	void Update () {}

	void OnGUI () {
        Rect GUIRect = new Rect(0f, Screen.height * (1f - bgSize), Screen.width, Screen.height * bgSize);

        //Background
        GUI.Box(GUIRect, "");

        //Buttons
        float buttonPixelSize = buttonSize * Mathf.Min(Screen.height, Screen.width);
        float marginH = (Screen.width - notes.Length * buttonPixelSize - (notes.Length - 1) * paddingH * Screen.width) / 2f;
        float count = 0f;
        foreach (Note note in notes) {
            Rect btn_r = new Rect(
                left: marginH + count * paddingH * Screen.width + count * buttonPixelSize,
                top: (marginTop) * Screen.height + GUIRect.yMin,
                width: buttonPixelSize,
                height: buttonPixelSize 
            );

            if(GUI.Button(btn_r, note.Name)) {
                commander.addCommand(
                    new PlayNoteCommand(note)
                );
            }

            count++;
        }
		
	}
}
