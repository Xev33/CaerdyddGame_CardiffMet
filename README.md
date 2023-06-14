# README #

### Project summary ###

This project has been developped in three weeks.
As an exchange student spending one year abroad, I was required to team up with other british students during the year I spent in Cardiff Metropolitan University for the module **Designing and Implementing Game Mechanics** in order to 
1. Design a game taking place and using the welsh folklore and culture.
2. Write its game design document.
3. Make people playtest the game and write a report about it.

I decided to mix two games:
- Donkey kong country tropical freeze for the game structure and 2D platforming aspect,
- Spyro the dragon for the character controller.

# Welcome to Caerdydd Game #

![Flag_of_Wales svg](https://github.com/Xev33/CaerdyddGame_CardiffMet/assets/106018428/b42fb651-f2bf-47d7-8868-a3a7f30d29a0)


This readme is splitted in three parts
- The game navigation part in order to understand how to play the game.
- The technical part to explain the key algorithms.
- The game design/assessment part will teach you all the possible inputs and will give you the game's documentation.

# How to play the game #

This game is meant to be played with a gamepad, however using a keyboard is possible as well.

### Menu navigation ###

* On the gamepad: use the four buttons onto the right side of your controller
* On keyboard: Use the keyboard arrows

### Game controls ###

* Gamepad controller inputs:
	* Move: left joystick
	* Jump/Glide/Hover: The bottom button from the side of your controller (Xbox: A || Playstation: X)
![DragonStates](https://github.com/Xev33/CaerdyddGame_CardiffMet/assets/106018428/940add09-db70-4295-995a-c4045620193c)


* Keyboard inputs:
	* Move: A->left D-> right
	* Jump/Glide/Hover: Space bar
 
# Technical part #

### Key algorithms ###
|**Algorithm**|**Description**|
|:------:|:----------------:|
| [Singleton pattern](https://gameprogrammingpatterns.com/singleton.html) |This algorithm ensure a class has one instance, and provide a global point of access to it. Here, I use a template of singletons. Thus, each signleton will share the same behaviour|
| [Command pattern](https://gameprogrammingpatterns.com/command.html) |**Command** is a behavioral design pattern that turns a request into a stand-alone object that contains all information about the request. Since our character is based on a small state machine, we use the input pattern to swap between them.|
| [Observer pattern](https://gameprogrammingpatterns.com/observer.html) |The **Observer** pattern lets you define a subscription mechanism to notify multiple objects about any events that happen to the object they’re observing.|

### My programming work on the game ###
I was in charge of all the game programing but the menus.

# Game design part #

### Elevator pitch ###

Caerdydd Rush is a 2D side scrolling platform game which has a distinct Welsh focus. The player 
controls the Welsh dragon spirit throughout the game. With its great gliding ability, the dragon 
has been set on a venture to restore nature’s balance by finding and collecting the holy daffodil.
This game truly offers players an opportunity to ride into the Welsh folklore and culture.

### Game Design Document ###
Please find here the [Caerdydd **game design document**](https://github.com/Xev33/CaerdyddGame_CardiffMet/files/11750287/Caerdydd.Rush.GDD.pdf) the team and I submitted for our assessment.

### Playtest report ###

We asked 20 people to playtest our game and tried to get a varied range of people who often play video games and those that never do.
Here is the [PDF report](https://github.com/Xev33/CaerdyddGame_CardiffMet/files/11750403/st20242612.GroupA.CIS5011.PRAC1.pdf) which shows the results.
	
### Code documentation ###

To see the entire code documentation:

* just go to the following folder: ```Doxygen/html```
* Search for a file named “index.html” 
* Double click on it 
* If asked: Select your internet navigator 
* Done! You should now have all our class attributes and methods with some inheritance trees.

  # Acknowledgments #
I would like to thank all the playtesters who really helped me to understand what worked and what wasn't good enough.

It helped me as a game designer as well and gave me another nice experience in that country that I now really love: Wales.
  
