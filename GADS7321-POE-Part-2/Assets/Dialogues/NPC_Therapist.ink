INCLUDE globalVariableTracker.ink

{lastSelectedChoice == "":
    -> main
- else:
    -> NPCHasBeenSpokenTo
}

=== main ===
<disruption>Welcome to the therapy session.</disruption> #speaker: Therapist #portrait:therapist_neutral #layout:right
Today, we'll work on building your skills to manage ADHD.
What do you want to focus on?
+ [Social Skills]
    -> socialSkills
+ [Mindfulness]
    -> mindfulness
+ [Self-Awareness]
    -> selfAwareness
+ [Stress Management]
    -> stressManagement
+ [Exit]
    -> exit

=== socialSkills ===
~ lastSelectedChoice = "Social Skills"
Improving your social skills can help you navigate social situations with greater ease. 
By learning how to communicate effectively and read social cues, you can manage ADHD-related 
challenges in social interactions more confidently.
-> repeatPrompt

=== mindfulness ===
~ lastSelectedChoice = "Mindfulness"
Mindfulness techniques can assist you in staying present and focused, reducing the impact of 
distractions commonly associated with ADHD. By practicing mindfulness, you can cultivate greater 
awareness of your thoughts and emotions, allowing you to better regulate your attention.
-> repeatPrompt

=== selfAwareness ===
~ lastSelectedChoice = "Self-Awareness"
Developing self-awareness involves understanding your strengths, weaknesses, and patterns of behavior. 
By becoming more self-aware, you can recognize ADHD symptoms and triggers, empowering you to 
implement effective coping strategies and manage your symptoms more effectively.
-> repeatPrompt

=== stressManagement ===
~ lastSelectedChoice = "Stress Management"
Learning stress management techniques can help you mitigate the impact of stress on your ADHD symptoms. 
By identifying stressors and implementing coping mechanisms such as relaxation exercises and time management 
strategies, you can reduce overwhelm and maintain focus even in demanding situations.
-> repeatPrompt

=== exit ===
Thank you for the session today. Remember to practice what you've learned!
-> END

=== repeatPrompt ===
Do you want to learn about another skill?
+ [Yes]
    -> main
+ [No]
    -> exit

=== NPCHasBeenSpokenTo ===
Hello again, remember: {lastSelectedChoice}! #speaker: Therapist #portrait:therapist_happy #layout:left
Would you like to learn about another skill?
+ [Yes]
    -> main
+ [No]
    -> exit
