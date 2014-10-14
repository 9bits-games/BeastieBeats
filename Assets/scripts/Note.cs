using System;
using UnityEngine;

public class Note {
    public static Note DO = new Note {Name="Do", Color = Color.red};
    public static Note RE = new Note {Name="Re", Color = Color.blue};
    public static Note MI = new Note {Name="Mi", Color = Color.yellow};
    public static Note FA = new Note {Name="Fa", Color = Color.green};
    public static Note SOL = new Note {Name="Sol", Color = Color.green};
    public static Note LA = new Note {Name="La", Color = Color.magenta};
    public static Note Si = new Note {Name="Si", Color = Color.magenta};

    public string Name;
    public Color Color;
}
