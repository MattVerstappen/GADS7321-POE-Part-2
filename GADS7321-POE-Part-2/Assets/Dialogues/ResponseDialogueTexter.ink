INCLUDE globalVariableTracker.ink

{lastSelectedChoice == "": -> main | -> NPCHasBeenSpokenTo}

<disruption>UH OH someone new...</disruption> #speaker: Test NPC #portrait:dr_green_neutral #layout:left
===main===
<disruption><color=\#F8FF30>Welcome</color> to the decision tree </disruption>, make your Choice! #portrait:dr_green_happy #layout:right
    +[Wubba Lubba Dub Dub]
        -> chosen("Getting <color=\#FF1E35>Rickety</color> WREAAAAKED") 
    +[AH Geez Rick not again]
        -> chosen("GODDAMNIT <disruption> MORTY YOU SELFISH </disruption> LITTLE SHIT") 
    +[What is my purpose?]
        -> chosen("You pass me the butter, now experience exisitential dread")

=== chosen(response) ===
~ lastSelectedChoice = response
{response}
-> END

=== NPCHasBeenSpokenTo ===
Oh hey you again remember: {lastSelectedChoice}! #speaker: Test NPC #portrait:dr_green_happy #layout:left
-> END
