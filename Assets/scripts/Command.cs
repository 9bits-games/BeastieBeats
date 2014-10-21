using UnityEngine;
using System;

/**
 * Base class for the command pattern.
 * The class Commander is the responsible of executing all concrete instances of Command.
 * All other commands must inherit Command and override excecute() if needed.
 * The recommended way to override executed is:
 * public override void execute() {
 *     base.execute();
 *     //Your code...
 * }
 * 
 */
public class Command
{
    // True if the commnad was executed by the Commander.
    public bool Executed { get {return executed;} }
 
    protected bool executed;
    protected float execTime;

    //Creates a non executed command.
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

/**
 * The command to play a Note in the Game.
 * It triggers the event NotePlayed when executed.
 */
public class PlayNoteCommand : Command
{
    // This events is triggered when a note is played by the user.
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
