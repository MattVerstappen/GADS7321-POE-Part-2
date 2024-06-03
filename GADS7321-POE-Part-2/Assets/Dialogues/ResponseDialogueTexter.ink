INCLUDE globalVariableTracker.ink

-> main

=== main ===
Hello there! It's been a while since we last talked. #speaker:Mr. Blue #portrait:mr_blue_neutral #layout:left #audio:celeste_high
How have you <disruption>been?</disruption>
+ [Good]
    That's good to hear! What’s <disruption>new with you?</disruption> #portrait:mr_blue_happy#audio:celeste_high
    -> check_skills
+ [Not great]
    I'm sorry to hear that. Do you <disruption>want to talk</disruption> about it? #portrait:mr_blue_concerned#audio:celeste_high
    -> check_skills
+ [I'm managing my ADHD better now]
    That's wonderful! Have you been working <disruption>with someone on it?</disruption> #portrait:mr_blue_happy#audio:celeste_high
    -> check_skills
+ [I'll be back a bit later]
    Not a problem I'll see you a bit later my friend.
-> END

=== check_skills ===
{hasLearnedSocialSkills && hasLearnedMindfulness && hasLearnedSelfAwareness && hasLearnedStressManagement:
    You seem to have acquired some helpful <disruption>skills.</disruption> How has that been going for you? #portrait:mr_blue_happy#audio:celeste_high
    -> discuss_skills
- else:
    It looks like there's still some room to grow. Let's see if I can help with <disruption>anything.</disruption> #portrait:mr_blue_neutral#audio:celeste_high
    -> discuss_skills
}

=== discuss_skills ===
What would you <disruption>like to</disruption> talk about?
+ [Social Skills]
    {hasLearnedSocialSkills:
        Your <color=#F8FF30>social skills</color> have improved, <disruption>haven’t they?</disruption> #portrait:mr_blue_happy #audio:celeste_high
    - else:
        Social skills are important for <disruption>interacting with others.</disruption> Have you been working on them? #portrait:mr_blue_neutral#audio:celeste_high
        -> improve_social_skills
    }
+ [Mindfulness]
    {hasLearnedMindfulness:
        Practicing <color=#F8FF30>mindfulness</color> really helps, doesn’t it? #portrait:mr_blue_happy#audio:audio:celeste_high
    - else:
        Mindfulness can be very helpful in managing <disruption>ADHD.</disruption> Would you like to learn more about it? #portrait:mr_blue_neutral#audio:audio:celeste_high
        -> improve_mindfulness
    }
+ [Self-Awareness]
    {hasLearnedSelfAwareness:
        Your <color=#F8FF30>self-awareness</color> has really grown. #portrait:mr_blue_happy#audio:audio:celeste_high
    - else:
        Understanding yourself better can make a huge difference. Have you <disruption>explored self-awareness?</disruption> #portrait:mr_blue_neutral#audio:celeste_high
        -> improve_self_awareness
    }
+ [Stress Management]
    {hasLearnedStressManagement:
        Managing stress is crucial, and it looks like you’re doing well with it. #portrait:mr_blue_happy#audio:celeste_high
    - else:
        Stress management is essential. Do you want to talk about how to <disruption>handle stress better?</disruption> #portrait:mr_blue_neutral#audio:celeste_high
        -> improve_stress_management
    }
+ [None]
    That’s fine. We can <disruption>talk about something else.</disruption> #portrait:mr_blue_neutral#audio:celeste_high
    -> main

=== improve_social_skills ===
Social skills can help you connect with others more <disruption>effectively.</disruption> Would you like some tips? #portrait:mr_blue_neutral#audio:celeste_high
- [Yes]
    Great! Start by practicing <disruption>active listening</disruption> and observing social cues. #portrait:mr_blue_happy#audio:celeste_high
    -> main
- [No]
    No problem. Let’s talk about <disruption>something else.</disruption> #portrait:mr_blue_neutral#audio:celeste_high
    -> main

=== improve_mindfulness ===
Mindfulness involves staying present and aware of your <disruption>thoughts and feelings.</disruption> Would you like some exercises to try? #portrait:mr_blue_neutral#audio:celeste_high
- [Yes]
    Start with simple breathing exercises and gradually move to longer <disruption>mindfulness sessions.</disruption> #portrait:mr_blue_happy#audio:celeste_high
    -> main
- [No]
    That’s okay. We can <disruption>discuss other things.</disruption> #portrait:mr_blue_neutral#audio:celeste_high
    -> main

=== improve_self_awareness ===
Self-awareness helps you understand your emotions and reactions better. Would you like to learn more about it? #portrait:mr_blue_neutral#audio:celeste_high
- [Yes]
    Keep a journal to reflect on your daily experiences and emotions. #portrait:mr_blue_happy#audio:celeste_high
    -> main
- [No]
    Alright, let’s move on to something else. #portrait:mr_blue_neutral#audio:celeste_high
    -> main

=== improve_stress_management ===
Managing stress effectively can make a big difference in your daily life. Would you like to hear some techniques? #portrait:mr_blue_neutral#audio:celeste_high
- [Yes]
    Try incorporating regular exercise and relaxation techniques like deep <disruption>breathing.</disruption> #portrait:mr_blue_happy#audio:celeste_high
    -> main
- [No]
    Sure, we can <disruption>discuss something else.</disruption> #portrait:mr_blue_neutral#audio:audio:celeste_high
    -> main

=== END ===
Thanks for the talk!
-> END
