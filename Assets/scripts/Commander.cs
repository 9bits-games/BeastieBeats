using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/**
 * The Commander part of the command patters and is the responsible of executing
 * all concrete instances of the class Command.
 * 
 * It also stores an historic of all commands excecuted.
 * 
 **/
public class Commander
{
    public List<Command> CommandHistory { get; private set; }

    public Commander()
    {
        CommandHistory = new List<Command>();
    }


    //Add a command to be executed.
    public void AddCommand(Command command) {
        command.execute();

        this.CommandHistory.Add(command);
    }
}
