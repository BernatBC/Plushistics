# Plushistics
The way to distribute plushies across UK.

## Inspiration
When we discovered that a Shark Plushie was one of the prizes of the Hackaway we realized that the Hackathons UK needed a way to optimally distribute the shark flushies across the country.

## What it does
You can create cities, with a number of sharks already on them, a number of sharks that they require and their name. Then you can link cities with roads choosing the _gas spent_ of traveling by them. Last, but not least, you may select one or more cities to have a plushie van, with its own capacity for shark plushies, which will travel through, and only through, linked cities loading and unloading plushies in order to satisfy the demands of each city. When you have everything you want just press the __Run__ button.

## How we built it
The graphical side of the project, and the one that the user sees, is made with Unity.
The main algorithms and the search of an optimized solution is made with Metric-FF, using the Planning Domain Definition Langauge (PDDL). It uses A* to guide itself to the optimal solution. If finding such solution becomes algorithmically expensive, then the heuristic will change in a way that we will find an optimized solution, maybe not the most optimized, but it will get it in a reasonable amount of time.
Finally, the communication between the graphical side and the PDDL one is made by C# code (how could be otherwise).

## Challenges we ran into
We had problems related to the OS that was going to be used. Some code had to be remade to work with C# and Unity in order to work with Windows. Also, we had trouble with large number of cities and connections will were resolved using by decrementing the optimism of the heuristic function.

## Accomplishments that we're proud of
Mainly, we made it work! We found ourselves stuck several times during the project, finding new bugs. It is true that it is usually always the same process: you solve one bug, you get three more! But given the limited time we thought to ourselves, during the beginning of the night, that we may not get anything working at all. But, after all, we found our way through and with patience and hard work we ended up getting where we wanted. Such a relief.

## What we learned
We have improved our c# coding skills and learned how to communicate Unity, c#, pdll and testing c++ files.

## What's next for Plushistics
We would like to not only expand Plushistics to new countries, but also make it playable.    
Everyone around the world deserves a shark plushie. 

Try it now on Windows ([download link](https://github.com/Loparc/Plushistics/releases/download/release-1.1/Plushistics.zip))
