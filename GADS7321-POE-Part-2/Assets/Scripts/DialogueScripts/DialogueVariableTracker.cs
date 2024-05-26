using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System.IO;
using Ink;
using Object = Ink.Runtime.Object;

public class DialogueVariableTracker
{
    private Dictionary<string, Ink.Runtime.Object> variables;

    public DialogueVariableTracker(string filePathFinder)
    {
        string inkFileContents = File.ReadAllText(filePathFinder);
        Compiler recompiler = new Compiler(inkFileContents);
        Story globalVariablesInStory = recompiler.Compile();

        variables = new Dictionary<string, Object>();
        foreach (string name in globalVariablesInStory.variablesState)
        {
            Object value = globalVariablesInStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("EEEEY this thing is compiling over here: " + name + " = " + value);
        }
    }
    public void ListenAtStart(Story story)
    {
        StoryVariables(story);
        story.variablesState.variableChangedEvent += hasVariableChanged;
    }

    public void ListeningEnd(Story story)
    {
        story.variablesState.variableChangedEvent -= hasVariableChanged;
    }
    private void hasVariableChanged(string name, Object value)
    {
        Debug.Log("Variable changed: " + name + " = " + value);
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }
    }

    private void StoryVariables(Story story)
    {
        foreach (KeyValuePair<string, Object> variable in variables)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
