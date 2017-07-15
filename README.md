# CatalystVR

### Introduction

The goal of the CatalystVR project is to visualize archaeological data in Virtual Reality systems, primarily CAVE systems, for public display. The system should be able to support a wide variety of data types, and more importantly, be able to visualize new data on the fly, without having to make any changes in the Unity Editor. Here's a brief overview of the existing flow:

1. Archaeologist uploads new data to online database (CAVE Base)
2. New data is pulled down onto a mounted drive on computer
3. JSON file is generated/updated with paths to the data and metadata
4. CatalystVR system pulls latest info from the JSON file and creates "POIs" on a virtual earth
5. User can select these POIs, view their metadata, and choose to actually see the data
6. Data is then pulled from the mounted drive, from the path specified in JSON file
7. User looks at data, then returns to the Earth scene when done
8. If inactive for a long period of time, the system will go into an "idle" screensaver mode


### Initial Remarks

An important file to initially look at is the "screenConfig.txt" file that must always exist in the same directory as the final ".exe" Unity build. In the project, this file is located in the same directory as the Assets folder. This screenConfig.txt file is what ensures that everything can render on any given 3D CAVE system. If, when attempting to run this project, the screens don't match up or something seems off, you will likely need to tweak the screenConfig.txt file.

Second, to actually test this project, you must always make a build of it for it to show up properly in the CAVE. It's unfortunately impossible to actually run the project while in the Unity Editor. Please remember that the screenConfig.txt file must be in the same directory as that final .exe


### Current Status and Guide

The latest working version of this project is always located in the "master" branch, though as of right now it's rather outdated, since in this version, all the data/functionality is in the Unity project itself and must be changed through the Unity Editor. This is, again, done through our POI system. Here's a step by step guide on adding new data via the system that's on master:

1. Open Unity and navigate to the "HomeScene.unity" scene.
2. You should see an Earth in the hierarchy/scene. Select the Earth, then dropdown the "Earth Main" object
3. You should see a great deal of "POI - SiteName" objects. It's recommended that you use one of these as an example.
4. POI objects have components that determine what metadata and site they will visit. Most will have at least:

   * POI component (Needed): Enter the POI name under "POI Name" here. Nothing else needs to change.
   * Scene Loader Component: Just enter the name of the Unity Scene to load when it's selected
   * Text Component: Enter the description/sentence that will appear when the POI is selected
   * Photo Component: Add a path to any pictures that can be shown for this site

5. Duplicate the object and add whichever components are most relevant for the POI you're showing
6. Build and run the project, and it should work!

This means if you're trying to load an object (3D model, point cloud, CAVECam, etc) in the old system, you must create a separate Unity scene with that object and connect it to the corresponding POI with a "Scene Loader Component", and then by entering the scene name. For 3D models, this is easy, just create a new scene, drop in the model, and you're good to go! For CAVECams, it's a little trickier, but thankfully we have a "CAVECamLoader" script that you can use in a new scene to show cavecams. Just create a new object in the scene, add the CAVE Cam Loader script, then fill in the necessary info and you're set

Once you've created the scene, make sure to add it to the scenes in build (in your build settings), create a POI with a Scene Loader Component with that scene name, and you should be good to go! Build and run the project and you should see your new POI, which can be selected. Once selected, you should see the button "Visit Site" active, and you can go to your new site.


### New Work and Dynamic Data Loading

Currently, the process of making all data dynamically load is nearly complete, and the process of actually showing this data is much, much easier. In the "DynamicDataLoading" branch you'll find all of this work.

Here, all of the data exists OUTSIDE of the Unity Editor. The most important file here is a "siteData.json" file, that should be in a folder titled "CAVEkiosk_SiteData" in the same directory as the .exe file. Right now, there's a lot of dummy data in there, but it should give a very good idea for how data loading works. Data is referenced through filepaths, all of which are in that JSON file. It really is as easy as just adding a new site, updating it's metadata, and giving paths to the file all in that JSON. 

Now, the existing issue with this branch and the reason it isn't on master is because of load times. Right now, the data is loading VERY slow. A good place to start investigating this would be the CAVECam.cs script, where you can see how CAVECams are being dynamically loaded. After that, CatalystModel.cs is where 3D models are being loaded, and is also a good place to investigate.



