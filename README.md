MouseJiggler
============

Mouse Jiggler is a very simple piece of software whose sole function is to "fake" mouse input to Windows,
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


Settings
========

Open Settings using the context menu of the task bar icon. In settings, check the "Autostart jiggle" checkbox to start 
jiggling the mouse when starting. Select a jiggle mode from the following:

* Zen: the pointer is jiggled 'virtually' - the system believes it to be moving and thus
  screen saver activation, etc., is prevented, but the pointer does not actually move.
  This may not work with a few applications which chose to implement their own idle detection.
* ZigZag: the pointer is moved 4 pixels in every axis and back immediately
* Circle: the point is moved in a circle of 5 pixels radius

These settings are remembered from session to session. 

Command-line
============

They can also be overridden by command-line options:

```
Usage:
  MouseJiggler [options]

Options:
  -j, --jiggle               Start with jiggling enabled.
  -m, --mode                 Sets the jiggling mode (Zen/ZigZag/Circle). [default: Zen]
  -s, --seconds <seconds>    Set number of seconds for the jiggle interval. [default: 15]
  --version                  Show version information
  -?, -h, --help             Show help and usage information
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
