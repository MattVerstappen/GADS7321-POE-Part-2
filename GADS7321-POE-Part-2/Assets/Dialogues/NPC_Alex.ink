INCLUDE globalVariableTracker.ink

-> main

=== main ===
Hello again. <disruption>What do you want now?</disruption> #speaker: Alex #portrait:alex_neutral #layout:right #audio:animal_crossing_mid

+ {allSkillsLearned == false} [I'm still not sure what is wrong with me] -> notUnderstanding
+ {allSkillsLearned == true} [I want to explain something important] -> moreUnderstanding
+ [Nothing] 
    Fine. Come back when you actually have something to say. #portrait:alex_angry #audio:animal_crossing_mid
    -> END

=== notUnderstanding ===
~ temp hasLearnedSkills = (hasLearnedSocialSkills || hasLearnedMindfulness || hasLearnedSelfAwareness || hasLearnedStressManagement)

{ hasLearnedSkills:
    I see you've been learning some skills. But I'm still not sure if I can trust you. #portrait:alex_neutral #audio:animal_crossing_mid
    -> discuss_skills
- else:
    Why should I care about your problems? You've always been unreliable. #portrait:alex_angry #audio:animal_crossing_mid
    -> END
}

=== moreUnderstanding ===
So, you want to explain yourself? <disruption>Go ahead.</disruption> #portrait:alex_neutral #audio:animal_crossing_mid
-> explain_adhd

=== explain_adhd ===
#speaker: Alex #portrait:alex_neutral #audio:animal_crossing_mid
ADHD has made things difficult for you, but you claim you're learning to manage it.
+ [It affected your ability to focus] 
    So, your inability to focus wasn't because you didn't care about what I was saying, but because of your ADHD. Interesting. #portrait:alex_neutral #audio:animal_crossing_mid
    -> understand_effect
+ [It caused impulsivity]
    You're saying your impulsive actions were due to your ADHD. Convenient excuse. #portrait:alex_neutral #audio:animal_crossing_mid
    -> understand_effect
+ [It led to forgetfulness]
    Oh, so your forgetfulness wasn't intentional. Blaming it on your ADHD now? #portrait:alex_neutral #audio:calm_music_low
    -> understand_effect
+ [It made you restless]
    Restlessness, huh? That's a new one. #portrait:alex_neutral #audio:calm_music_low
    -> understand_effect

=== understand_effect ===
I see. <disruption>That explains a lot.</disruption> But it doesn't change the fact that it hurt me. #portrait:alex_sad #audio:sad_music_low
+ [I'm sorry, I'm working on it]
    I hope so. <disruption>I'll believe it when I see it.</disruption> #portrait:alex_neutral #audio:animal_crossing_mid
    -> discuss_skills
+ [I've learned some skills to manage it better]
    Show me. Maybe then I'll believe you. #portrait:alex_neutral #audio:animal_crossing_mid
    -> discuss_skills

=== discuss_skills ===
What have you been learning? #portrait:alex_neutral #audio:animal_crossing_mid
+ {hasLearnedSocialSkills} [Social Skills]
    I've learned how to improve my social skills and communicate better. #portrait:player_neutral #audio:animal_crossing_mid
    -> improve_relationship
+ {hasLearnedMindfulness} [Mindfulness]
    Mindfulness techniques help me stay present and reduce distractions. #portrait:player_neutral #audio:animal_crossing_mid
    -> improve_relationship
+ {hasLearnedSelfAwareness} [Self-Awareness]
    Developing self-awareness has made me more conscious of my actions and their impact. #portrait:player_neutral #audio:animal_crossing_mid
    -> improve_relationship
+ {hasLearnedStressManagement} [Stress Management]
    I've learned how to manage my stress better, which helps me stay more focused. #portrait:player_neutral #audio:animal_crossing_mid
    -> improve_relationship

=== improve_relationship ===
I can see you're trying. Maybe there's hope for us after all. #portrait:alex_neutral #audio:animal_crossing_mid
+ [Thank you]
    Let's see if you can keep it up. #portrait:alex_neutral #audio:animal_crossing_mid
    -> END
+ [I'll prove it to you]
    I'll be watching. Don't let me down. #portrait:alex_neutral #audio:animal_crossing_mid
    -> END

=== END ===
See you around.
-> END
