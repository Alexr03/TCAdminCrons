# TCAdmin Crons

A little program to automatically run some cron jobs that uses TCAdmin like automatically updating Game Updates when a new update comes out for minecraft.

## Features
 - Automatic Minecraft Vanilla game updates.
 - Automatic Minecraft Paper game updates.
 
 ## How to use
 1. Head to releases page and download the latest version.
 2. Extract somewhere
 3. Open `/Config/TcAdminConfig.json` and fill in the information required.
 4. Open `/Config/MinecraftUpdates.json` and fill in the information required.
 5. Open TCAdminCrons.exe - If all goes well you should see updates being added to Minecraft in TCAdmin.
 
### How to obtain connection string
1. Goto **C:/Program Files/TCAdmin2/Monitor/TCAdminMonitor.exe.Config** _(Or linux equivalent)_ and open using a text editor.
2. Find **TCAdmin.Database.MySql.ConnectionString** and copy the value

### How to obtain Game ID
1. Goto TCAdmin
2. Click "Settings" -> "Game & Other Voice Servers" -> Choose the game you want to use.
3. The first line should be the Game ID, copy this value and place into the config.