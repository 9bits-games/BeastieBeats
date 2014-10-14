using UnityEngine;
using System;

public class Command
{
    protected bool executed;
    protected float execTime;
    public bool Executed { get {return executed;} }

    public Command()
    {
        executed = false;
        execTime = -1f;
    }

    public virtual void execute() {
        executed = true;
        //TODO: This should be handled or related to MainSC
        execTime = Time.timeSinceLevelLoad;
    }
}

public class PlayNoteCommand : Command
{
    public delegate void NotePlayed(PlayNoteCommand playNoteCommand);
    public static event NotePlayed OnNotePlayed;

    public Note Note { get; private set; }

    public PlayNoteCommand(Note note) : base() {
        this.Note = note;
    }

    public override void execute() {
        base.execute();

        if(OnNotePlayed != null)
            PlayNoteCommand.OnNotePlayed(this);
        Debug.Log("Played " + this.Note.Name);
    }
}
