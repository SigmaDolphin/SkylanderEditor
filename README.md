# SkylanderEditor
VB.NET program to edit/backup/restore Skylanders

Presenting my Skylander Editor, still work in progress and alpha version so far
-----------------------------------------------------------------------------------------
DISCLAIMER
due to the alpha nature of my program i will do my best to assist in recovering corrupt/damaged skylanders
but im not responsible for any permanent damage to figures/computers/dogs/parrots my program can cause
-----------------------------------------------------------------------------------------

Special thanks to silicontrip for his Skyreader which gave me inspiration to make this

Special thanks to Jan Axelson for making the code examples to controlling HID devices from VB.NET




ALWAYS KEEP A VALID BACKUP OF YOUR SKYLANDER BEFORE MODIFYING DATA

ALWAYS KEEP A VALID BACKUP OF YOUR SKYLANDER BEFORE MODIFYING DATA

ALWAYS KEEP A VALID BACKUP OF YOUR SKYLANDER BEFORE MODIFYING DATA

ALWAYS KEEP A VALID BACKUP OF YOUR SKYLANDER BEFORE MODIFYING DATA

ALWAYS KEEP A VALID BACKUP OF YOUR SKYLANDER BEFORE MODIFYING DATA



# What this program CAN'T and WON'T do


Clone Skylanders onto mifare cards

Change the identity of a Skylander figure



# What this program CAN do

Backup/Restore Skylanders

Edit Hats/Money/Lvl/Skill Path

Fix (perhaps) corrupt Skylanders

Reset Skylanders

Show some important hex data that could only be meaningful to devs




# Stuff that right now doesn't work and may or may not be added

Read Traps

Identify Skylander variants

Heroic Challenges

Hero Points

Nicknames

Complete Skill trees

Read/edit Imaginators

and more....

>Trap Team trinkets removed from ToDo list since they are only visible and relevant in Trap Team


database needed for the program to run is included

the image folder along the executable is optional and accepts 150x150 pictures which i cannot include for obvious reasons



# CHANGELOG
1.0.0 (VB6) - 

Initial Release


1.0.1 - 

added lvl 20 support and a nice About window


1.5.0 - 

added Trap Team Hats, some cleanup code, Superchargers indexes, 

Imaginators indexes(up to wave 2) and minis indexes,

added images from all the new indexes
	
added some experimental Swappable reading


1.8.0 - 

added Superchargers Hats

added support for most portals (Trap Team portals may still misbehave)


1.9.0 - 

Reset Figure option now correctly resets imaginators figures/crystals


1.0.0 (.NET) - 

all code was ported to VB.NET

added encrypt and decrypt functions to main program

added HID support for the Portal to main program

added functions to dump without mapping figures

added support to recognize new/blank skylanders

Swapper reading is more reliable
		
