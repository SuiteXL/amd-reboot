
# AMD-Reboot Utility

I made this small utility using C# as the AMD Adrenalin Software commonly likes to bug out on my system and fail to load. The issue from what I could tell is related to the CN folder in the Local AppData, and whilst I haven't found a fix for the CN folder, I realized killing the AMD processes, and restarting them fixed the issue.

That's where this utility came from.




## What this does

- Checks if AMD processes are running
- Kills the AMD processes
- Restarts the AMD Adrenalin Software

## Installation / Usage

Using this is as simple as building it from the source using Visual Studio 2022, or running the release binary 

The way I have it set up on machine is using Wootomation, and binding it to my minus key (-) on the numpad.
    
## Why not just delete the CN folder?

Deleting the CN folder DOES also resolve this issue, however all you're really doing is just deleting the folder which gets remade anyways when AMD Adrenalin restarts, meaning the problem will likely come back, on top of which you will also delete all your settings (or at least game-specific ones), playtime hour tracking, etc.
and using this utility avoids those problems, and is much easier than manually deleting the folder all the time.
