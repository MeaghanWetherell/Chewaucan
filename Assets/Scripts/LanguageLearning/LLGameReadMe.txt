Set up for the Language Learning Game requires multiple bits of data to be set up and fed to the Language Learning Manager,
which is attached to the Manager object in the LanguageMinigame scene. The minigame was not set up to handle multiple different runs
with different words and scripts, but this can be achieved easily enough by duplicating the scene for each new instance.
The data the manager needs are as follows:
1. The words the player can unlock, in the form of DraggableImgData scriptable objects. Examples for French as the alt language
can be found under LanguageLearning/SOs
2. A script that the player is unlocking words from, set up as a text file. Details for the formatting on the text file can
be found in the example text file under the LanguageLearning folder
3. A list of valid 3-word simple sentences that allow the player to unlock words such as "Cat Hunt Mammoth" written in a text file.
Formatting details can be found in the example text file. The list should be written in English, but can handle sentences arranged
to match how they are written in the alt language.
4. A list of audio clips representing each individual sentence in the script, for both languages. They should be added to the
LanguageLearningManager in the order they appear in the script.