# Bachelor Project

Using Unity 2021.3.16f1 and MRTK 2.7.3
This file will detail the different commits

## hololens (master)
This branch is the main branch and holds the latest code that has been tested on the hololens 2 headset (except the first commit)
### details
v1.4 : added window to choose height, deleted spatial awareness, changed scale and position of objects. Can build.
v1.6 : buttons work correctly and are in the right area, slate is lower, profiled is disabled, checkend and validate method work, objects are stopped after being placed correctly (previous angular velocity issue), house model has all faces, poke and grab only
v2.2 : same as v2.1 but tested on hololens.
#### issues
- collisions are inaccurate (check multiple primitive colliders). Especially Y.
- can't make buttons work in unity, they work on the hololens

## prototype 1
This branch is up to the first prototype, the base prototype, it consists of the following points.
Having 4 objects corresponding to 4 imprints on a tray.
Having these objects react to the right and wrong imprints (possibly with sound).
Having the right materials for the visually impaired people.
Having an end condition.
Having it place itself in the right area in space (automatic or calibration).
The objects and imprints dispositions are randomized or at least partly.
### details
- v1.0 : first and second condition are done. Note : No sound yet
- v1.1 : changed the structure of the project. Note : Gravity is not earth-like, Objects shouldn't bounce of the tray, The Collections need to have a script still.
- v1.2 : added a Script for parameters. fourth condition is done, there is a print message. Objects don't bounce of the tray no more. Note : There should be a pop up where you can start again or exit.
- v1.3 : added models for objects 1 to 3 and corresponding imprints. Note : collider is less precise, house object missing one face.
- v1.4 : same as hololens
- v1.5 : added 4th model, adapted colors, fixed window in world, changed validate action, fixed objects after put down at the right place. Note : couldn't test some features as I can't move the objects anymore. Height buttons don't work anymore.
- v1.6 : same as hololens
- v1.7 : added automatic layout of imprints and objects with randomization of order based on hierarchy
- v2.1 : same as prototype 2
- v2.8 : same as prototype 2

## prototype 2
This branch is up to the second prototype, the customizable prototype, it consists of the following points.
Having a JSON file where parameters can be changed.
The parameters are the scale of the whole, the contrast, black and white or white and black or colors, PLF, the numbers of objects, the name, the anonimity, the objects, the colors, the rotationmode, the scalingmode, the limit_to_field_of_view.
Having the PRL (color, distance, size, orientation).
### details
- v2.1 : parameters are read correctly from a JSON file.
- v2.2 : tested on hololens.
- v2.3 : added translation from JSON to Unity, currently doing apply. Note : if the JSON is not correct, or if the objects are not present, doesn't work
- v2.4 : added colors, scale, lay application. Note : check size and position of objects, potential issue of imprints' appearance in lower scale.
- v2.5 : added numbers application. Note : collision check method has to change, the clone pieces interact only with the same imprint.
- v2.6 : changed collision methods. Note : collision itself has still the same issues.
- v2.7 : added PRL.
- v2.8 : added Rotationmode and Scalemode. Note : Rotationmode "locked" has no effect in hololens, "xzlocked" has not been tested. Angle of PRL is off.
- v2.9 : added min size of PRL of 0.1, added height of PRL to be on top of object, fixed PRL angle, added one piece at a time, terminationmenu. Note : the menu doesn't do anything yet
- v2.10 : added a boolean to limit scale to field of view, fixed objects could be interacted with only once, TerminationMenu buttons should work, added JSON objects for data recording.

## prototype 3
This branch is up to the third prototype, the feedback prototype, it consists of the following points.
Having parameters used for the task
Having number of errors, and number of color and shape error
Having time recorded of pick ups individual and global
Having the difference in position of each piece (vector)
Having the difference of rotation of each piece (vector)
Having the capacity to save to a JSON file
### details
- v2.10 : same as prototype 2
- v3.1 : termination button produces a JSON file with records with all the necessary recordings (not tested)
- v3.2 : corrected the name of the file saved and multiple continuous collisions on the same object register as only one error now. Also changed the color of the white slate to be more pearly. Switch all shaders to MRTK standard from standard. Note: save files to C:\Users\berts\AppData\LocalLow\DefaultCompany\Bachelor
- v3.3 : added outline on hover and grab. Note: Meshes have to be read/write enabled for the outline to work, else crash. MeshOutline script has to be deactivated at the start and an hover color has to be referenced.
- v3.4 : added data naming and hashing.

## extras
If time allows these things will be added :
A way to save custom parameters (especially PLF).
An anonymous log-in
Safeguards when things are not present in Unity from the JSON file

## known issues
Collisions register too high (y)
Collisions for circles are very hard to register in hololens
Rotationmode "locked" doesn't work, "xzlocked" may not work (to test)

## to test
# On hololens
TerminationMenu Quit Button
RotationLock
Find FieldOfView Length
# in Unity