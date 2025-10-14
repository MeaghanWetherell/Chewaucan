necessary prefabs under prefabs/dating

1. add coursemanager prefab near the desired course location. use this as a folder for the other components.
adjust the exposed number parameters as desired
2. add a start prefab under the coursemanager at the desired start location. adjust the text on the sign as desired. 
Add a DatingHelpInteractable prefab in front to allow the user to access the help menu.
3. add coursewalls under the coursemanager, setting rotations, positions, and scales as desired to fence in the course area
4. create empty folder objects for the spawn locations of the date rocks (carbonate and tuffa) and snakes. 
optionally, also create one for rock obstacles. assign these to the appropriate transform fields on the coursemanager
5. fill the folders with empty gameobjects. the location of these gameobjects indicates a valid spawn location
for the corresponding object to the manager, which will then randomly spawn the objects each game at some of these
locations
optionally, use cubes instead of empty objects; they will be disabled automatically on start (note that the spawn location
is the center of the cube)
6. add any other desired obstacles
7. bake nav mesh