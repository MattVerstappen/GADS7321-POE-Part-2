using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions
{
    // Binds external functions to the Ink story
    public void Bind(Story story, Animator emoteAnimator)
    {
        story.BindExternalFunction("playEmote", (string emoteName) => PlayEmote(emoteName, emoteAnimator));
    }

    // Unbinds external functions from the Ink story
    public void Unbind(Story story) 
    {
        story.UnbindExternalFunction("playEmote");
    }

   // Plays an emote animation
    public void PlayEmote(string emoteName, Animator emoteAnimator)
    {
        if (emoteAnimator != null) 
        {
            // Trigger the specified emote animation
            emoteAnimator.Play(emoteName);
        }
        else 
        {
            Debug.LogWarning("Tried to play emote, but emote animator was " + "not initialized when entering dialogue mode.");
        }
    }
}
