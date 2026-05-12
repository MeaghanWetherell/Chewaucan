README ON HOW TO USE THE AUDIO SYSTEM

Creation Information:
	This tool was created by Charlie Scott in Fall 2025/Spring 2026 for the game Finding Lake Chewaucan, as
	part of the broader impacts for NSF Award # 2228632, "The Small Mammals of the Paisley and Connley Caves: 
	Disentangling Drivers of Diversity in Pleistocene Extinction Survivors"
	https://www.nsf.gov/awardsearch/showAward?AWD_ID=2228632

Version Information:
	This tool was prepared using Unity version 2022.3.62f3
	This tool uses the TextMeshPro package (Unity Registry, Version 3.0.7) https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/license/LICENSE.html
	This tool uses the Unity.UI package (Unity Registry, Version 1.0.0)


	This system involves four scripts, which in turn require several other custom scripts listed here.
		SoundManager.cs (HUDManager.cs, SaveHandler.cs, PauseCallback.cs)
		SetSubtitles.cs (SoundManager.cs)
		SubtitleButtonEnabler.cs (SoundManager.cs)
		ChangeVol.cs (SoundManager.cs)

General Description:

	The core of the sound system is the SoundManager.cs script, which is attached to the SoundManager game object in the PersistentObjects 
	scene. If you need the sliders to start out at lower volume, find this object and adjust the stand vol variable.

	This script is a singleton; duplicate instances that are created will destroy themselves. 
	It will automatically place an object it is attached to into DontDestroyOnLoad. Attached to this
	object should be an audio source for music and an audio source for narration, to which the SoundManager should be provided
	references. The sound manager requires a main mixer for the project. This is an Audio Mixer object created in assets.
	
	The main mixer should have a single group with a 'master mixer' as the parent and subordinate music, narration, and effects mixers.
	Ensure that all audio sources in the project are set to use the appropriate subordinate mixer.

	Fill the references on the SoundManager as appropriate. The ChangeVol and SetSubtitles scripts are easy means of using	
	the SoundManager to update volume and subtitles settings. The SoundManager exposes several public methods that can be used
	to manage audio. Further details on narration management can be found in the Narration system.

	See the Ambient Noises section for details on how to make randomized sound effects.




Troubleshooting:
	Having trouble with your narration sounding fuzzy or odd despite controlling the loudness normalization? 
	This is an issue with the maximum volume as set by the audio mixer. But the SoundManager.cs script does overwrite this, so you can't
	just change the audio mixer settings. Instead, go to line 442 and edit as needed. Currently, this ensures that a standvol of 1 has a 	maximum dB of 0.