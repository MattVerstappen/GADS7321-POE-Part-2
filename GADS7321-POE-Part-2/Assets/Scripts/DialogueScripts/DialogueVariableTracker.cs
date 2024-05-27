using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueVariableTracker
{
    // Dictionary to store dialogue variables
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

    private Story globalVariablesStory;
    private const string saveVariablesKey = "INK_VARIABLES";

    // Constructor to initialize dialogue variables
    public DialogueVariableTracker(TextAsset loadGlobalsJSON) 
    {
        // Initialize Ink story for global variables
        globalVariablesStory = new Story(loadGlobalsJSON.text);
        
        // Initialize dictionary to store variables
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState)
        {
            // Add global variables to the dictionary
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
        }
    }

    // Saves the state of dialogue variables
    public void SaveVariables() 
    {
        if (globalVariablesStory != null) 
        {
            // Save variables to Ink story
            VariablesToStory(globalVariablesStory);
            // Store variables in PlayerPrefs (temporary)
            PlayerPrefs.SetString(saveVariablesKey, globalVariablesStory.state.ToJson());
        }
    }

    // Starts listening for changes in dialogue variables
    public void StartListening(Story story) 
    {
        // Update variables in the story
        VariablesToStory(story);
        // Listen for variable changes
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    // Stops listening for changes in dialogue variables
    public void StopListening(Story story) 
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    // Updates dialogue variables in response to changes
    private void VariableChanged(string name, Ink.Runtime.Object value) 
    {
        // Update variables if they were initialized from global variables
        if (variables.ContainsKey(name)) 
        {
            variables.Remove(name);
            variables.Add(name, value);
        }
    }

    // Copies dialogue variables to a specific Ink story
    private void VariablesToStory(Story story) 
    {
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables) 
        {
            // Set global variables in the story
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
