The core of the sound system is the SoundManager script. This script is a singleton; duplicate instances that are created
will destroy themselves. It will automatically place an object it is attached to into DontDestroyOnLoad. Attached to this
object should be an audio source for music and an audio source for narration, to which the SoundManager should be provided
references. The sound manager requires a main mixer for the project. This is an Audio Mixer object created in assets.
The main mixer should have a single group with a 'master mixer' as the parent and subordinate music, narration, and effects mixers.
Ensure that all audio sources in the project are set to use the appropriate subordinate mixer.
Fill the references on the SoundManager as appropriate. The ChangeVol and SetSubtitles scripts are easy means of using
the SoundManager to update volume and subtitles settings. The SoundManager exposes several public methods that can be used
to manage audio. Further details on narration management can be found in the Narration system.