README ON HOW TO USE THE CLIMBABLE OBJECTS SYSTEM

Creation Information:
	This tool was created by Charlie Scott in Fall 2025 for the game Finding Lake Chewaucan, as
	part of the broader impacts for NSF Award # 2228632, "The Small Mammals of the Paisley and Connley Caves: 
	Disentangling Drivers of Diversity in Pleistocene Extinction Survivors"
	https://www.nsf.gov/awardsearch/showAward?AWD_ID=2228632

Version Information:
	This tool was prepared using Unity version 2022.3.62f3


General Description
	This set of scripts makes the player transition to climbing movement when they hit the correct triggerable object. 
	The object must have a child gameobject with the ClimbableEnter.cs script attached.

	Several of these scripts rely on other scripts to operate.
		ClimbableEnter.cs (Player.cs)
		ClimbableExit.cs (Player.cs)
		ClimbingMovement.cs (LandMovement.cs)
	
How to Set Up Climbing:
	To make an object climbable, create an empty object as its child with a box collider trigger. 
	To this child game object, add a "ClimbableEnter" script.
	Assign the box collider of the empty child object  to the exit collider field of the script and set the collider dimensions. 
	The player will be able to climb while within the bounds of the collider and stop climbing on exit. 
	As such, the collider should extend just above the lip of the climbable surface so that the player can exit by moving forward.