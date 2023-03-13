# Bachelor Project

Using Unity 2021.3.16f1 and MRTK 2.7.3
This file will detail the different commits

## hololens (master)
This branch is the main branch and holds the latest code that has been tested on the hololens 2 headset (except the first commit)
### details
v1.4 : added window to choose height, deleted spatial awareness, changed scale and position of objects. Can build.
#### issues
- menu is far and down dynamically.
- validation of height doesn't work.
- profiler is visible
- collisions are inaccurate (check mesh and box colliders)
- hard to grab
- objects fly away after being put down (even when not very dynamic, lock object in place)

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

## prototype 2
This branch is up to the second prototype, the customizable prototype, it consists of the following points.
Having a window where parameters can be changed.
The parameters are the scale of the whole, the contrast, black and white or colors, PLF, the numbers of objects.
Having the PLF (color, distance, size, orientation).
### details

## prototype 3
This branch is up to the third prototype, the feedback prototype, it consists of the following points.
Having parameters used for the task
Having number of errors
Having time recorded of pick ups individual and global and which hands
Having the percentage and global direction of the errors (up/down, left/right)
Having the capacity to save to a file
### details

## extras
If time allows these things will be added :
A way to save custom parameters (especially PLF).
An anonymous log-in