README ON HOW TO IMPLEMENT/EDIT THE BONE PILE MINI QUEST
Creation Information:
	This tool was created by Charles Scott in Spring 2024 for the game Finding Lake Chewaucan, 
As part of the broader impacts for NSF Award # 2228632, "The Small Mammals of the Paisley and Connley Caves: 
Disentangling Drivers of Diversity in Pleistocene Extinction Survivors"
https://www.nsf.gov/awardsearch/showAward?AWD_ID=2228632

Version Information:
This tool was prepared using Unity version 2022.3.10f1
It uses the following packages:
	Unity UI (Version 1.0.0)
Input System (Version 1.7.0)
		TextMeshPro (Version 3.0.6)
	It relies on scripts from the LoadGUIFolder, QuestSystem, and Misc folders.

General Description
This is a complete game system that relies on the player finding objects, clicking on them, then rotating them to compare to a second object.
There are several scripts that are directly associated with this system:
	BoneComparison/BoneInteractable.cs
	BoneComparison/BoneChecker.cs 
	BoneComparison/BoneRotatorSelector.cs 
	Interactabes/Interactable.cs
	Interactables/InteractListenManager.cs
	Interactables/InteractRaycaster.cs
	Misc/BoneRotator
	Misc/IListener.cs
	

There are several assets associated with this system:
	All objects under BonePileQuest in the ModernMap scene
	All objects under AnswerBones in the ModernMap scene

There is a specific ShaderGraph and Material for the Interactables:
	OutlineSG
	OutlineMat

This is linked up to a custom renderer, Render Objects Outlined, found in the Renderer folder as a child of CustomRenderer.

General Functioning
Bone objects have the BoneInteractable script attached. When the player looks at these bones, they light up with an outline. If the player then presses the interact key (tab by default), the BoneComparison scene is loaded into the player's view (additive load). The player can then rotate the bones in this UI and compare them to the mastodon half femur. If the bone is correct and in the correct orientation, the player completes the quest.

BoneInteractable.cs
This is added to an empty game object (e.g. CowJaw) that is the parent of your default and outlined models (the object must have a collider). It ensures that the game will register this as a bone that can be compared in the bone pile quest. It has public fields for the bone the player sees in the regular game (Default Bone), an outlined version of that bone (Outlined Bone), and then a rotatable scaled version of the bone that is sent to the Bone Comparison scene (Answer Bone). There is a boolean Is Correct box, which is should be checked on the bone which is the correct match.

This is also responsible for turning off the directional light in the main scene (called Main Light), because otherwise everything looks insanely washed out and yellow.

BoneChecker.cs
Hooked up to the View Bone object in the BoneComparison scene. This provides the text for different types of matches and completes the quest when the player gets it correct. 

Currently, this uses quaternion angles to measure how far the correct bone is from the mastodon femur. This necessitates the Answer Bone being aligned in the correct orientation, and then the script will rotate it an additional 90 degrees (which allows us to specify essentially which way is the correct orientation, without immediately giving the answer away). 

BoneRotatorSelector.cs
This allows the user to choose between rotating the mastodon femur and the bone they chose


InteractListenerManager.cs & InteractRaycaster
Both components are attached to the Player object. The latter determines if the player is looking at an interactable object, while the former calls methods on the appropriate object when the player presses the interact key


BoneComparison Scene
This scene has a canvas, two texture cameras, two “viewbone” objects, and a game object called RotSelect with the BoneRotatorSelector.cs script attached. If you need to adjust the size of the bones, you can use the scale of the ViewBone attributes. 

RenderTextures
There are two render texture files hooked up to the RenderTextureCam objects. These are both found in the BoneComparison folder and include CompareBoneTexture and MastodonBoneRtx. If you find the render texture cameras are making one or both items too low of resolution, up the size of these render textures.


Material, ShaderGraph, and CustomRenderer
To create an outline on an object when it is moused over, we added a custom rendering object (Render Objects Outlined) that links a material (OutlineMat) and custom shader graph (OutlineSG) to a particular game layer (Outlined). 

The Outlined Bone object should be set on the Outlined Layer. When BoneInteractable sets that object to active, the custom renderer turns on the shadergraph and material, adding a glowy outline to the object. 

This is dictated in the shader graph partially by the object scale, which means that it is impacted by the import scale of the prefab. So if you have very different sizes of prefab objects, they will not always have the same amount of glow. It is controlled by the “vector 3” object and also by the “thickness” input, and you can play around with these parameters to make your glow more or less influenced by the object size.

TO SETUP:
We will use the Cow Jaw as an example. Under the BonePileQuest gameobject in the Modern Map scene you will see a gameobject called CowJaw with a box collider, an audio source, and the BoneInteractable script attached. 

CowJaw has two child objects which are both versions of the prefab bone. These should have the same scale and orientation, so edit the scale and orientation of the parent to change both. Make certain that the collider on the parent fully encompasses the child bone objects. 

One of these is set active and on the Default layer, the other is set inactive and on the Outlined layer. The active, Default layer object goes in the “Default Bone” field of BoneInteractable on the parent while the inactive, outlined layer goes in the “Outlined Bone” field.

Because the answer bone might be rotated and scaled differently, we have put the answer bones as prefabs in the Scripts/BoneComparison/AnswerBones folder. The CowJaw(2) under AnswerBones is in the correct orientation so that it appears in the correct orientation and position when loaded by BoneComparison.

Add an audio source to the parent gameobject (CowJaw). Drag this into the “Pickup Audio” box. This sound plays when the player picks up that object (presses Tab to interact)

The scaling of the bones in the BoneComparison scene is related to two factors – the scale of the ViewBone object, and the scale of the answer bone itself. We scaled the answer bones to be approximately the correct size to one another (e.g., a calcaneum is smaller than a pelvis), and the ViewBone object to make all of these objects approximately the same scale to the mastodon femur (e.g., they should all be smaller than a mastodon femur!)



TROUBLESHOOTING
If you find that there is no bone popping up when the player loads the BoneComparison scene, this is usually either that there is no bone in the Answer Bone Slot, or that the Answer Bone is offscreen somehow and not visible. Check the prefab's xyz position. If it has been moved significantly, it probably is off screen and not visible in the game comparison scene.

If you find that objects have vastly different outline widths, and some are rather jagged, this is an issue with import scale of the objects and mesh complexity. Right now, we have the shader graph interpreting the position of the ‘glow’ using normal vectors and a scaling parameter. If you have one object that is 10x the size of another, it will have a much bigger thickness. Similarly, because it is using vectors if your imported mesh has many more triangles it will probably look smoother, but for some reason might also look bigger. All our bones thus came in at the same import scale for the prefab and about the same complexity (~10,000 triangles), with any scaling of the bones happening in the scene file instead of to the prefab. We found that sometimes this still didn’t work because we hadn’t pressed “apply transform” when exporting from blender, or hadn’t pressed “apply scale” in blender, or because our normals were poorly oriented in blender. See the “3D Photogrammetry Readme” document for tips.


