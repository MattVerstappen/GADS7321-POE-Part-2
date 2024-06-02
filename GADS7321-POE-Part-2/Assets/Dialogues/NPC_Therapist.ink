INCLUDE globalVariableTracker.ink

{lastSelectedChoice == "": -> firstSession | -> subsequentSession}

=== firstSession ===
Welcome to your first therapy session. #speaker: Therapist #portrait:therapist_neutral #layout:right #audio:animal_crossing_low
Today, we'll work on understanding ADHD and building your skills to manage it. 
Let's start by understanding what ADHD is.
ADHD stands for Attention Deficit Hyperactivity Disorder. 
It’s a neurodevelopmental disorder that affects both children and adults.
-> diagnosis

===diagnosis===
Can you tell me about some challenges you've faced that make you suspect you might have ADHD?
+ [Difficulty focusing on tasks]
    -> focusIssues
+ [Impulsivity]
    -> impulsivityIssues
+ [Hyperactivity]
    -> hyperactivityIssues
+ [Forgetfulness]
    -> forgetfulnessIssues
+ [Other]
    -> otherIssues

=== focusIssues ===
Having difficulty focusing on tasks is a common symptom of ADHD. 
It can manifest as trouble staying on task, easily getting distracted, 
or being unable to complete tasks.
Can you give me an example of a situation where you struggled with focus?
+ [At work or school]
    -> focusAtWorkOrSchool
+ [At home]
    -> focusAtHome
+ [In social situations]
    -> focusInSocialSituations

=== focusAtWorkOrSchool ===
Many people with ADHD find it challenging to stay focused at work or school. 
This can lead to incomplete assignments or missed deadlines.
Have you tried any strategies to help with this?
+ [Yes, but they didn't work]
    -> strategiesDidNotWork
+ [No, I haven't tried anything specific]
    -> noStrategiesTried

=== focusAtHome ===
Difficulty focusing at home can make it hard to complete household chores or personal projects.
Have you tried any strategies to help with this?
+ [Yes, but they didn't work]
    -> strategiesDidNotWork
+ [No, I haven't tried anything specific]
    -> noStrategiesTried

=== focusInSocialSituations ===
Struggling to focus in social situations can affect your ability 
to engage in conversations or remember details.
Have you tried any strategies to help with this?
+ [Yes, but they didn't work]
    -> strategiesDidNotWork
+ [No, I haven't tried anything specific]
    -> noStrategiesTried

=== impulsivityIssues ===
Impulsivity is another symptom of ADHD. 
It might show up as speaking out of turn, making hasty decisions, 
or struggling with self-control.
Can you tell me about a time when impulsivity caused you problems?
+ [At work or school]
    -> impulsivityAtWorkOrSchool
+ [At home]
    -> impulsivityAtHome
+ [In social situations]
    -> impulsivityInSocialSituations

=== impulsivityAtWorkOrSchool ===
Impulsivity at work or school can lead to interrupting others, 
making quick decisions without considering consequences, 
or difficulty waiting your turn.
Have you tried any strategies to help with this?
+ [Yes, but they didn't work]
    -> strategiesDidNotWork
+ [No, I haven't tried anything specific]
    -> noStrategiesTried

=== impulsivityAtHome ===
At home, impulsivity can affect relationships with family 
or roommates by causing conflicts or misunderstandings.
Have you tried any strategies to help with this?
+ [Yes, but they didn't work]
    -> strategiesDidNotWork
+ [No, I haven't tried anything specific]
    -> noStrategiesTried

=== impulsivityInSocialSituations ===
In social situations, impulsivity can lead to difficulties in conversations, 
such as interrupting others or sharing inappropriate comments.
Have you tried any strategies to help with this?
+ [Yes, but they didn't work]
    -> strategiesDidNotWork
+ [No, I haven't tried anything specific]
    -> noStrategiesTried

=== hyperactivityIssues ===
Hyperactivity often means feeling restless, fidgeting, 
or finding it hard to sit still. 
It’s more noticeable in children but can persist into adulthood.
How does hyperactivity affect your daily life?
+ [Difficulty sitting still]
    -> difficultySittingStill
+ [Constant fidgeting]
    -> constantFidgeting
+ [Feeling restless]
    -> feelingRestless

=== difficultySittingStill ===
Finding it difficult to sit still can impact your ability to 
focus and participate in activities that require prolonged sitting.
Have you tried any strategies to help with this?
+ [Yes, but they didn't work]
    -> strategiesDidNotWork
+ [No, I haven't tried anything specific]
    -> noStrategiesTried

=== constantFidgeting ===
Constant fidgeting can be distracting for you and others, 
making it hard to concentrate on tasks.
Have you tried any strategies to help with this?
+ [Yes, but they didn't work]
    -> strategiesDidNotWork
+ [No, I haven't tried anything specific]
    -> noStrategiesTried

=== feelingRestless ===
Feeling restless can make it challenging to relax and unwind, 
affecting your overall well-being.
Have you tried any strategies to help with this?
+ [Yes, but they didn't work]
    -> strategiesDidNotWork
+ [No, I haven't tried anything specific]
    -> noStrategiesTried

=== forgetfulnessIssues ===
Forgetfulness is a common ADHD symptom. 
It can mean misplacing things, forgetting appointments, 
or having difficulty following through on tasks.
Can you give me an example of a time when forgetfulness caused you problems?
+ [At work or school]
    -> forgetfulnessAtWorkOrSchool
+ [At home]
    -> forgetfulnessAtHome
+ [In social situations]
    -> forgetfulnessInSocialSituations

=== forgetfulnessAtWorkOrSchool ===
Forgetfulness at work or school can lead to missed deadlines, 
lost items, or difficulty keeping track of assignments.
Have you tried any strategies to help with this?
+ [Yes, but they didn't work]
    -> strategiesDidNotWork
+ [No, I haven't tried anything specific]
    -> noStrategiesTried

=== forgetfulnessAtHome ===
At home, forgetfulness can cause issues like misplacing items, 
forgetting important dates, or neglecting household tasks.
Have you tried any strategies to help with this?
+ [Yes, but they didn't work]
    -> strategiesDidNotWork
+ [No, I haven't tried anything specific]
    -> noStrategiesTried

=== forgetfulnessInSocialSituations ===
In social situations, forgetfulness can lead to missing appointments, 
forgetting names, or not remembering details from conversations.
Have you tried any strategies to help with this?
+ [Yes, but they didn't work]
    -> strategiesDidNotWork
+ [No, I haven't tried anything specific]
   -> noStrategiesTried

=== strategiesDidNotWork ===
It's common for strategies to not work the first time.
ADHD management often requires a combination of approaches tailored to your unique needs.
Would you like to continue discussing your symptoms?

+[Yes]
-> diagnosis
+[No]
-> selfAwareness

=== noStrategiesTried ===
That's okay.
There are many strategies we can explore together to help manage ADHD symptoms.
Would you like to learn about some of these strategies now?

+[Yes]
-> diagnosis
+[No]
-> selfAwareness

=== otherIssues ===
ADHD symptoms can vary widely. It's not just about hyperactivity;
it can also include emotional regulation issues,
time management difficulties, and more.
Can you tell me more about the challenges you’re facing?

+[Emotional regulation issues]
-> emotionalRegulationIssues
+[Time management difficulties]
-> timeManagementDifficulties
+[Other]
-> otherSpecificIssues
=== emotionalRegulationIssues ===
Emotional regulation issues can mean difficulty managing emotions,
experiencing intense feelings, or having mood swings.
Have you tried any strategies to help with this?

+[Yes, but they didn't work]
-> strategiesDidNotWork
+[No, I haven't tried anything specific]
-> noStrategiesTried
=== timeManagementDifficulties ===
Time management difficulties can include procrastination,
poor planning, or trouble estimating how long tasks will take.
Have you tried any strategies to help with this?

+[Yes, but they didn't work]
-> strategiesDidNotWork
+[No, I haven't tried anything specific]
-> noStrategiesTried
=== otherSpecificIssues ===
Tell me more about the specific challenges you're facing with ADHD.
-> otherDetailedChallenges

=== otherDetailedChallenges ===
Understanding the unique ways ADHD affects you can help us
tailor strategies to your needs.
Let's work on identifying effective coping mechanisms together.
-> selfAwareness

=== selfAwareness ===
Since you feel that the symptoms you have informed me about lead me
to believe you have ADHD, one immediate skill I can teach you is developing again
sense of Self awareness.
Developing self-awareness involves understanding your strengths, weaknesses, and patterns of behavior.
By becoming more self-aware, you can recognize ADHD symptoms and triggers, empowering you to
implement effective coping strategies and manage your symptoms more effectively.
Would you like to continue learning new skills in the next session?

+[Yes]
-> subsequentSession

+[No]
-> exitFirstSession

=== chosen(response) ===
~ lastSelectedChoice = response
{response}
-> END

=== exitFirstSession ===
Thank you for the session today.
Remember to practice what you've learned!
Come back when you're ready to learn more.
-> END

=== subsequentSession ===
Welcome back to your therapy session.
Today, we can work on building more skills to manage ADHD.
What do you want to focus on?

+[Social Skills]
-> socialSkills
+[Mindfulness]
-> mindfulness
+[Stress Management]
-> stressManagement
+[Exit]
-> exit
=== socialSkills ===
Improving your social skills can help you navigate social situations with greater ease.
By learning how to communicate effectively and read social cues, you can manage ADHD-related
challenges in social interactions more confidently.
-> repeatPrompt

=== mindfulness ===
Mindfulness techniques can assist you in staying present and focused, reducing the impact of
distractions commonly associated with ADHD. By practicing mindfulness, you can cultivate greater
awareness of your thoughts and emotions, allowing you to better regulate your attention.
-> repeatPrompt

=== stressManagement ===
Learning stress management techniques can help you mitigate the impact of stress on your ADHD symptoms.
By identifying stressors and implementing coping mechanisms such as relaxation exercises and time management
strategies, you can reduce overwhelm and maintain focus even in demanding situations.
-> repeatPrompt

=== exit ===
Thank you for the session today. Remember to practice what you've learned!
-> END

=== repeatPrompt ===
Do you want to learn about another skill?

+[Yes]
-> subsequentSession
+[No]
-> exit
