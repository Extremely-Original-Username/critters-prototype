## Critters - a simple evolution sim
An in-progress evolution simulator heavily inspired by Biogenesis (https://github.com/sullerandras/biogenesis) - I wanted to have a crack at building something similar myself in C# before looking at it's source code.

The simulation is currently feature-sparse, just consisting of agents made up of various parts, who can reproduce and randomly mutate, changing those parts. There is also a crude day/night cycle and carbon system that can be visualised with the "1" key.

![Critters](https://github.com/user-attachments/assets/dfa5992b-65f4-42ac-a936-16582376b251)

The simulation is currently quite poorly optimised, so performance can be poor at high carbon levels when more critters can survive. I am currently working on a more advanced version in Godot, making more efficient use of compute shaders after my experience developing [a simple sand simulation in C++](https://github.com/Extremely-Original-Username/simple-sand-game)
