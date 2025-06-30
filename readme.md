# OPLPS1AppUtility

A utility to automate the creation of OPL (Open PS2 Loader) Apps for PS1 games using [Popstarter](https://www.psx-place.com/threads/popstarter.19139/) and a `list.csv` game list exported from the [OPL Manager](https://oplmanager.com/) program.

## Overview

This tool scans your OPL folder structure, matches PS1 VCD images to entries in your `list.csv`, and generates OPL Apps for each game. 

It supports a test mode to preview actions before making changes.

## Requirements

Before running this tool, ensure your OPL folder contains the following:

- `APPS` folder (for generated OPL Apps)
- `POPS` folder (containing your PS1 games and Popstarter ELF)
- `POPS/POPSTARTER.ELF` (the Popstarter ELF file, [download here](https://www.psx-place.com/threads/popstarter.19139/))
- `POPS/[ID].[Title].VCD` files (your PS1 games, one per game, in VCD format)
- `list.csv` (exported from [OPL Manager](https://oplmanager.com/), must include correct IDs and Titles)

### Disclaimer

I am not responsible for any issues that arise from using this tool. Use at your own risk. Always back up your OPL folder before making changes.
