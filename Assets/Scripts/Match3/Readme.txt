Artists and Designers:
To add a new bone model to match 3:
    Navigate to Scripts>Match3>Resources>Meshes
    Right click>create>MeshDataObj and fill its fields
    One of its fields will be a DescObj, which can be created the same way (right click>create>DescObj)
    Desc Objs are just containers for text that you can reuse for multiple different bones if you want to create variations or plan to only describe the animal and not the bone itself
    To create the flat sprite for the flat image field, navigate to the SnapshotScene in scenes, set up the model there to your liking, then go to Tools>SnapshotCreator in the top bar, enter a file name, and click "Take Snapshot"
    Once your MeshDataObj is created, add it to the main list titled Match3Meshes, found in Scripts>Match3>Resources>Meshes
    For your new mesh to appear in a level, its index in the Match3Meshes list must be added to the Meshes field of a LevelData object
    
To add a new level:
    Navigate to Scripts>Match3>Resources>Levels
    Right click>create>LevelData and fill its fields 
    Navigate to Persistent Objects in scenes and open it
    Add your new level data to the Levels field of the MatchLevelManager on the MatchLevelManager game object. make sure that your level data appears at the position in order you want it to be, ex. level 3 should be at index 3. Endless should always be at index 0.

---------------------------------------------------------------------------------
Programmers:
There shouldn't be anything here that an artist/designer can't hook up themself, so you should only be needed if the system needs a refactor.
Any performance problems are almost certainly due to inefficiencies of the design of MatchGrid that I'm not worrying about because I don't expect performance to be a problem.
If the system needs major refactoring, it's probably best to mostly just rip out MatchGrid and redesign it from scratch. 
Match grid mostly contains match detection functionality, as well as swapping bones. All the data is in MatchObject and MatchLine. 