The narration system is a system designed to manage what narrations can play and store ones that have already played for display
in the journal. Also provides a simple event hook to run code when a narration is completed. To set up a Narration, create
a Narration scriptable object in the Resources folder. The name of this object should begin with the folder name and be followed
by a number. Add the narration clip associated with the object and set whether it is playable by default (if not, it will 
not be able to be played by most narration scripts until set playable in code). Optionally, create a subtitle script for the
Narration. Each line of the script should begin with a timestamp during the narration (ex. 1:20 for 1 minute 20 seconds) at which 
the line should stop displaying. It should be followed by a space, then the character "|", then another space, then the text
that should be displayed until the preceding timestamp is reached. See examples in subtitles folder for format. This Narration
object can then be slotted into several scripts under the "Triggers" folder for simple triggers, such as playing when the player 
enters a trigger volume, or passed off to a programmer to trigger with bespoke code. 