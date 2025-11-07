[![Build Status](https://dev.azure.com/tkolb80/MouseJiggler/_apis/build/status%2FCI%20Build?branchName=main)](https://dev.azure.com/tkolb80/MouseJiggler/_build/latest?definitionId=7&branchName=main)

MouseJiggler
============

Mouse Jiggler is a simple piece of software whose sole function is to "fake" mouse input to Windows,
and jiggle the mouse pointer back and forth.

Useful for avoiding screensavers, screen locking or other things triggered by idle detection that, for
whatever reason, you can't turn off any other way; or as a quick way to stop a screensaver activating
during an installation or when monitoring a long operation without actually having to muck about with
the screensaver settings.


Installation
============

The Mouse Jiggler is a single executable. There is no need for installation.
Download and place it anywhere you like. However, NET8 runtime needs to be installed.


Operation
=========

Simply run the MouseJiggler.exe. It runs minimized to the notification area.

In the context menu of the task bar icon use "De-/Activate" to start or stop jiggling at runtime. 
The jiggle is slight enough that you should be able to use the computer normally 
even with jiggling enabled. In every mode, the pointer returns to its original position.
When the activity check is enabled, the jiggle won't even interfere with your manual mouse usage.


Settings
========

Open Settings using the context menu of the task bar icon. 
In settings, check the "Autostart jiggle" checkbox to start jiggling the mouse when starting the application.
You can also choose whether you want to jiggle at the selected interval or check mouse activity and only jiggle 
after a certain period of inactivity.

Select a jiggle mode from the following:

* Zen: the pointer is jiggled 'virtually' - the system believes it to be moving and thus
  screen saver activation, etc., is prevented, but the pointer does not actually move.
  This may not work with a few applications which chose to implement their own idle detection.
* ZigZag: the pointer is moved some pixels in every axis and back immediately
* Circle: the pointer is moved in a circle of some pixels diameter
* Smooth: the pointer is moved in a horizontal line right and left in multiple steps

The amount of pixel (or the diameter of the circle) to move can be controlled by the `Distance` setting.

These settings are remembered from session to session. 
The settings are reused as default values for the command line arguments.

Command-line
============

They can also be overridden by command-line options:

```
Description:
  Virtually jiggles the mouse, making the computer seem not idle.

Usage:
  MouseJiggler [options]

Options:
  -j, --jiggle                           Start with jiggling enabled.
  -a, --activity                         Enable activity check and only jiggle when inactive.
  -m, --mode <Circle|Smooth|Zen|ZigZag>  Set a jiggle mode. (Zen/ZigZag/Circle/Smooth) [default: Zen]
  -d, --distance <distance>              Set distance in pixel for the jiggle. [default: 20]
  -s, --seconds <seconds>                Set number of seconds for the jiggle interval. [default: 15]
  -?, -h, --help                         Show help and usage information
  --version                              Show version information
```


Features That Will Not Be Implemented
=====================================

This is a list of feature requests which I've decided won't be implemented in Mouse Jiggler for one reason or another,
along with what those reasons are, just for reference:

* Autorun on startup (because that's what the Startup group, Task Scheduler, etc. are for; it's inelegant to duplicate
  system facilities in a minimal app).
* Timed startup/shutdown (again, Task Scheduler is for this).


Support
=======

Mouse Jiggler is a free product provided without warranty or support.
