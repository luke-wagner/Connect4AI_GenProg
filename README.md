# Connect 4 AI with Genetic Programming

## Description:

ADD MORE HERE
"res" folder stores the output from running AIDueler.Play().

## Class Hierarchy Diagram:

<img src="./docs/ClassDiagram.png" alt="diagram" width="450"/>

## Class Hierarchy Explanation

### DuelerConfig class
* Contains constants used by `AIDueler` for generating AI tuning data
* Changing this config file will change the process for generating this tuning data

### AIDueler class
* Derives from `IPlayable` interface
* Its purpose is to make `AITrainer` objects compete and do something with the result (output to appropriate .json file(s))
* Run `AIDueler` by creating a new `AIDueler` object and calling `AIDueler.Play()`
* The tuning used by `C4SmartAI` comes from the data in `extra/res/finalTuning.json` (generated from `AIDueler.Play()`)

### Process for generating `finalTuning.json`:
* `AIDueler.findNewWinner()` starts with two bots with random tuning. These bots play each other the winner's tuning is saved to the next round, where it fights another random AI. The previous winner's tuning is overridden with the winner of this round and this process continues for `NUM_TRIALS_FOR_NEW_WINNER` rounds. The tuning of the final resulting winner is returned by the function
* `AIDueler.generateRandomWinners()` runs `AIDueler.findNewWinner()` `NUM_WINNERS_TO_GENERATE` times and pushes all winners to the file `randWinners.json`
* `AIDueler` then performs a series of gene pool repopulations and reductions until we reach a final gene pool. `NUM_CYCLES` is the number of times that the gene pool is repopulated and reduced. Gene pool repopulations are done by breeding the organisms in the gene pool using uniform crossover
* The final gene pool is then divided in half through a series of matches where the loser is removed from the pool
* When only one organism remains, its tuning is pushed to the file `finalTuning.json`

#### Critiques:
* I might have gotten a better tuning if I added the possibility for mutation in `AIDueler.repopulatePool()`. Currently, the gene pool homogenizes fairly quickly so running more cycles is useless

### AITrainer class
* Derives from `C4SmartAI` class
* Has public methods `Move()` and `OpponentMove()` which can be used by `AIDueler` to put two `AITrainer` objects against each other