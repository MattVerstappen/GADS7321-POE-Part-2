INCLUDE globalVariableTracker.ink

{lastSelectedChoice == "": -> main | -> NPCHasBeenSpokenTo}
UH OH someone new... #speaker: Test NPC #portrait: mr_green_neutral #layout:left

===main===
<color=\#F8FF30>Welcome</color> to the decision tree, make your Choice! #portrait: mr_green_happy #layout:right
    +[Wubba Lubba Dub Dub]
        -> chosen("Getting <color=\#FF1E35>Rickety</color> WREAAAAKED") 
    +[AH Geez Rick not again]
        -> chosen("GODDAMNIT MORTY YOU SELFISH LITTLE SHIT") 
    +[What is my purpose?]
        -> chosen("You pass me the butter, now experience exisitential dread")
=== chosen(response) ===
~ lastSelectedChoice = response
{response}
-> END

=== NPCHasBeenSpokenTo ===
Oh hey you again remember: {lastSelectedChoice}! #speaker: Test NPC #portrait: mr_green_happy #layout:left
-> END
