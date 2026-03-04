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
