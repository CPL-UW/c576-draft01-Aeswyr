# c576-draft01-Aeswyr
First draft of Final project for Pascal King

Project is a tower defense game with the goal of teaching about clocks, specifically, the process of winding/tuning mechanical clocks, utilizing some AI.

In its current state, the game is a single level showcasing the basics of each feature ready to be expanded upon:
 - The player can move around, place, and reload towers
 - Enemies will spawn and path towards the exit, and if too many make it, the player will lose
 - Defeating all the enemies will result in a win
 - The player can utilize their personal clock, tuning it in order to wind back time and gain an advantage

Known issues
 - Temp assets are in use for most of the personal clock abilities
 - time rewinds aren't 100% accurate when performed while an enemy is falling
 - mechanics will need more exploration and expansion to encourage the player to interact with the clock more and truly get a feel for how it works.
 - Traditionally, these mechanical clocks have all major gears on a single axis for space efficiency, but due to the 3-dimensional nature of that structure and the difficulties of moving it into a 2D format, the game displays the gears adjacent to each other
 - Game is severely lacking in any form of tutorialization. This will be fixed in a future build but as for now, the controls are:
        - A/D to move left and right
        - Space to jump
        - E to interact with and reload towers (if you have a tuning key to reload them)
        - Shift (Hold) to bring up mode menu
        - While in mode menu, use A/D to select desired mode, and release shift to finalize decision
        - In tower placement mode, E to place towers (if you have the parts)
        - in clock mode, click on either gear to begin tuning that gear. (You cannot tune a gear below time 00:00, or beyond the current time)

 Builds can be found inside the GameBuilds folder
