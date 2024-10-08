# VGDC-12

## How to create a dialogue tree:

![image](https://github.com/user-attachments/assets/0f540ded-535a-40c9-aadc-bee143c63c6c)

Each NPC has a starting dialogue, dialogue button (found in Resources/UIElements), option button container (currently in scene hierarchy under Canvas), and dialog panel (also in scene hierarchy)

Also, word speed can be changed, which is the speed at which text gets typed out into the dialogue box. Smaller number = faster.

All these fields must be filled and you can do so by dragging those elements into the corresponding fields in the NPC script section

Currently, Dialogue is stored in Assets/Scripts/Dialogue, and is formatted like this:

![image](https://github.com/user-attachments/assets/36e94b17-0e9d-4e2e-981d-8f9f68447cc3)

So far, the naming convention for a dialogue node is Name_of_NPC, Scene Number, Node number (how deep is it in the dialogue tree), a letter signifying if the dialogue node is a result of choosing dialogue option a-z (if applicable)

![image](https://github.com/user-attachments/assets/2a2855c2-17da-471e-aac3-68ad0e571d8c)

To create a new dialogue node, right click in the project window, hover over create, then dialogue, then click node.

Now you can configure the various dialogue options by dragging other nodes into them, filling them out or checking them.

### Options List: (Do not fill in the other next node field if you are using this for the current node): 
This option is for branching dialogue. You can set this by clicking the number to the right of Options and setting it to how many options you want, each option has a text field that will display that text as the option you are responding to the NPC with and a next node field which is the dialogue that the npc will respond with after selecting that option. You can fill this in by dragging a dialogue node into that field or pressing the plus button and selecting the one you want to add. If you want multiple dialogue options to have the same response you can simply reuse dialogue nodes.

### Next Node: (Do not set the options list count > 0 if you are using this for the current node):
This option is for linear dialogue. You can set this by dragging the dialogue node you want to be displayed after the NPC finishes their current line.

### Pause: (Be careful not to set a node as both a pause and a bookmark node, as that will cause dialogue to be unable to continue past the pause node)
Check this option to set a dialogue node as a pause node. This option is for pausing the dialogue and letting the player out of the dialogue menu. If the player talks to the NPC again the dialogue will continue from next node. (More specifically, it sets the start node to the node after the current one and closes the dialogue box) For example, you can have an npc get increasingly more annoyed the more you go back and talk to them. 

### Bookmark: (Be careful not to set a node as both a pause and a bookmark node, as that will cause dialogue to be unable to continue past the pause node)
Check this option to set a dialogue node as a bookmark node. When you talk to an NPC again, dialogue will restart from the bookmarked node (More specifically, it sets the start node to the current node). This option is mostly for when the player exhausts the NPC's current available dialogue and you want the dialogue to restart from a prior point in the dialogue tree. You can also set a bookmark before a dialogue choice is made and see the responses to each individual dialogue option. NOTE: A pause node after a bookmark will override the bookmark and set the next dialogue to start from the node after the pause node.

### Dialogue: The text that will be displayed when the node is reached

That is all for now.
