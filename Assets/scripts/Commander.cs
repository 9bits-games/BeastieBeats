using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Commander
{
    public List<Command> CommandHistory { get; private set; }

    public Commander()
    {
        CommandHistory = new List<Command>();
    }

    public void AddCommand(Command command) {
        command.execute();

        this.CommandHistory.Add(command);
    }
}
