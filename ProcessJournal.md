# Process Journal
## 2/17/2026
After the first week of work, I've set up the multiplayer functionality, and got a rudimentary selection system working, with room for fine-tuning.
- Up to four players can act independently of each other, positioned in the four corners of the screen, created upon button input from a new source.
- Players can cycle between different options, at the moment simply cycling through colors for their character.
  - The cycle system has been fine-tuned to repeat at a specific time interval when held.
- The player can "confirm," which at the moment enlargens the square and locks them out from continuing to cycle.

## 3/3/2026

Having neglected this project since the last progress update, I'll be doing some crunching this week.
[Today's playtesting](Playtesting.md), though on a rudimentary prototype, exposed some issues and consideration's I'll have to take.

### Next Iteration

I intend to undergo an additional playtesting session on the 6th. This is what I'll be prioritizing for then:

1. Making it functionally playable. The player needs to be able to win, and to compete with the other players.
   - I'll expand the gameplay so the player must sequentially select multiple components (three seems to be a good number).
   - I'll reward the player for certain behavior, like finishing first, or attaining certain combinations.
   - Players will be able to interfere with each other, and should not be able to select the same components.
   - The game should telegraph who won and then be replayed.

2. Making it clear what's going on. The player should be able to see what options they're swapping between, and be able to understand this is a game about character selection.
   - The controls should be telegraphed to the player immediately as to avoid confused button mashing. They go for buttons before sticks.
   - "Choose your Character!" text.
   - While a player waits for the other players to finish, they should be able to move their character around.

3. Implement a system where the player can see what the options are to the left and right.

## 3/6/2026

Since the last update, I've:
- Implemented the system where players must sequentially select multiple (three) components
- Made it so players cannot select the same components as each other, and will skip over each other when selecting through components
- Made it so players can see the components to the left and right (accounting for what is available)
- Added a tweening animation for cycling across components
- Added icons that indicate the controls for all of the players, which do a tweened "press" animation upon being executed
- Added control randomization to further contribute to the whole "scrambling" aspect of choosing a character

### Next Iteration

Plenty of the feedback I received related to the randomized controls, and how they could be communicated better to the player.

1. I'll work on making the control changes more clear to the player.
   - Make it so the control icons flash red when the WRONG input is used for an action
   - Do a general animation or pop up to indicate that the controls have changed.
2. I still need to make this selection system into a game.
   - Implementing multiple rounds, instead of forcing the players to restart the game.
   - Adding a scoring system that rewards certain combinations + speed
   - Telegraphing the winner
   - Showing the player how to get more points
