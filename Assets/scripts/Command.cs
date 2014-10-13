using UnityEngine;
using System;

public class Command
{
    protected bool executed;
    public bool Executed { get {return executed;} }

    public Command()
    {
        executed = false;
    }

    public virtual void execute() {
        executed = true;
    }
}

public class PlayNoteCommand : Command
{
    public Note Note { get; private set; }

    public PlayNoteCommand(Note note) : base() {
        this.Note = note;
    }

    public override void execute() {
        base.execute();

        Debug.Log("Played " + this.Note.Name);
    }
}
