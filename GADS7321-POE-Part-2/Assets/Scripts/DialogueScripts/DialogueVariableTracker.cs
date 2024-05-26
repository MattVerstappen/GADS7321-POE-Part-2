using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using Object = Ink.Runtime.Object;

public class DialogueVariableTracker
{
    private Dictionary<string, Object> variables;
    private Story globalVariablesInStory;
    private const string saveCurrentVariablesKey = "INK_VARIABLES_STATE";
    public DialogueVariableTracker(TextAsset loadGlobalsFromJSON)
    {
        globalVariablesInStory = new Story(loadGlobalsFromJSON.text);

        if (PlayerPrefs.HasKey(saveCurrentVariablesKey))
        {
            string jsonState = PlayerPrefs.GetString(saveCurrentVariablesKey);
            globalVariablesInStory.state.LoadJson(jsonState);
        }
        
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

    public void SaveVariables()
    {
        if (globalVariablesInStory != null)
        {
            StoryVariables(globalVariablesInStory);
            //Note To self this will work in conjunction with a save and load method later on.
            PlayerPrefs.SetString(saveCurrentVariablesKey, globalVariablesInStory.state.ToJson());
        }
    }
}
