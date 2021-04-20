using System;
using System.Collections.Generic;

// This is a main class
// Idk what else to say about it, it's main class
class MainClass {
  public static bool skipPrompt = false;
  public static bool breakUpdate = false;

  // Player
  public static EntityPlayer Player;

  // Rooms
  public static Room Cellar0;
  public static string defaultRoom = "Cellar0";
  
  public static void Main (string[] args) {
    // Setup
    Console.Clear();
    RegisterInstances();

    /* Start text (hidden because it's long as hell) */ {
      Util.Write("Welcome to Pearls! This is a prototype/ framework for a text adventure game. This iteration of the game may just have one path, but there's no telling what it could become in the future. At any point if you need help, type 'Help' into the console.");
    }

    // Start the UpdateScreen() loop
    UpdateScreen(breakUpdate);
  }

    // UpdateScreen is a loop that displays the prompt and room description, then asks for the player to input something.
    // Whatever the player inputs is handled, which could potentially be a lot of tasks, before the code finally returns to UpdateScreen();
    // Once the code comes back to UpdateScreen, the output of the player's actions are displayed, and UpdateScreen calls itself
  static void UpdateScreen(bool breakLoop) {
    if(breakLoop == false) {
      DisplayPrompt(skipPrompt);
      string input = Console.ReadLine();
      Util.Write(InputProcessing.ProcessInput(input));
      UpdateScreen(breakUpdate);
    }
  }

    // RegisterInstances() moves all of the clutter involved with 
    // constructing different instances of rooms and stuff out of Main(), for clarity
  static void RegisterInstances() {
    // Rooms
      // Cellar0
      string decExts = "W";
      string decItms = "Drapes";
      string decDesc = "You are in a dark, dusty cellar. You were told there was treasure here...";
      string decRmID = "Cellar0";
      bool decCurm = false;
      Cellar0 = new Room(decExts.Split(", "), decItms.Split(", "), decDesc, decRmID, decCurm);

      Room.roomList = new string[Room.roomCount];

    // Instantize player
    Player = new EntityPlayer();
    Player.SetCurrentRoom();
  }

    // DisplayPrompt writes a prompt
    // It can optionally be skipped to avoid console clutter
  static void DisplayPrompt(bool skip) {
    if(skip == false) {
      Util.Write(Player.currentRoom.DescribeRoom());
      Util.Write("What will you do next?");
      Util.Write("Enter 'Help' for a list of options");
    }
    skipPrompt = false;
  }
}

// InputProcessing is the factory where player input is received and dealt with
// Majority of the game's funcitonality occurs here
public class InputProcessing {

  public static string[] InputOptions = { "Help", "", "Inspect", "", "", "", "", "" };

  // The input processor
  // Takes in an input, passes it through one nasty switch block, 
  // and sends control to the input behaviour methods
  public static string ProcessInput(string input) {
    if(Util.CheckForError(input, InputOptions) == true) {
      return "Not an input";
    }
    MainClass.skipPrompt = true;
    switch (input) {
      case "Help": return WriteOptions();
      case "Inspect": return BeginInspectLoop(MainClass.Cellar0.items);
      case "Move Drapes": 
        RemoveInputOption("Move Drapes");
        return MoveDrapes();
      case "Open Jewelry Box":
        RemoveInputOption("Open Jewelry Box");
        return OpenJewelryBox();
      default: return "Option not found";
    }
  }

  // Input behaviour methods 
  // These methods are called in a switch block in ProcessInput and each must return a string
    // Writes out all of the player's current options
  public static string WriteOptions() {
    return "Your options are: " + Util.WriteArray(InputOptions, true, false);
  }
    // Reveals the jewelry box
  public static string MoveDrapes() {
    return "You found a jewelry box!" + AddInputOption("Open Jewelry Box");
  }
    // Shows text for opening the box and calls CueEndScreen()
  public static string OpenJewelryBox() {
    return "You found a pearl necklace" + CueEndScreen();
  }
    // Shows end text
  public static string CueEndScreen() {
    string msg = Util.newLine + Util.lineBreak + Util.newLine + "Congratulations, you found the treasure!" + Util.newLine + 
           "This is a prototype for a text adventure written in C#" + Util.newLine +
           "Thank you for playing!";
    MainClass.breakUpdate = true;
    return msg;
  }

  // Inspect action
    // Asks the player which item they want to inspect, makes sure that item is available to be inspected, and then hands over control to Inspect()
  public static string BeginInspectLoop(string[] items) {
    string returnString = "";
    Util.Write("What will you inspect?");
    for(int i = 0; i < items.Length; i++) {
      returnString += "* ";
      returnString += items[i];
    }
    Util.Write(returnString);
    string input = Console.ReadLine();
    return Inspect(input, items);
  }

    // Takes in the item which the player has asked to inspect and processes + returns that information
  public static string Inspect(string input, string[] items) {
    if(Util.CheckForError(input, items) == true) {
      return "Not an input";
    }
    switch(input) {
      case "Drapes":
        return "Some drapes are piled up in the corner of the room." + AddInputOption("Move Drapes");
      default: 
        return "Nothing to see here";
    }
  }

  // Input utils
    // Adds an input option to the list of options, and tells the player that the option has been added 
  public static string AddInputOption(string option) {
    for(int i = 0; i < InputOptions.Length; i++ ) {
      if(InputOptions[i].Length == 0) {
        InputOptions[i] = option;
        return Util.newLine + Util.indent + "You can now: " + option + Util.newLine + Util.indent + WriteOptions();
      }
    }
    return null;
  }

    // Removes an input option from the list of options
  public static void RemoveInputOption(string option) {
    for(int i = 0; i < InputOptions.Length; i++ ) {
      if(InputOptions[i] == option) {
        InputOptions[i] = "";
      }
    }
  }
}

// Room is used to instantiate different rooms with 
// related data like the room's description, possible exits, etc 
public class Room {
  public static int roomCount;
  public static string[] roomList;
  public string[] exits;
  public string[] items;
  public string description;
  public string RoomID;
  public bool currentRoom;

    // Returns the room's description
  public string DescribeRoom() {
    return description;
  }

    // Instance constructor
    // Sets instance variables and also increments the total amount of rooms
  public Room(string[] xts, string[] itm, string dsc, string rid, bool cur) {
    roomCount++;
    exits = xts;
    items = itm;
    description = dsc;
    RoomID = rid;
    currentRoom = cur;
  }
}

// The player class is instantiated once and will track where the player is,
// what's in their inventory, etc
public class EntityPlayer {
  public Room currentRoom;

    // Instance constructor
  public EntityPlayer() {
  }

    // Returns the description field of the room the player is currently in 
  public string DescribeCurrentRoom() {
    return currentRoom.description;
  }

    // Sets the current room
    // Since the only room in the game right now is Cellar0, this is hardcoded for simplicity
  public void SetCurrentRoom() {
    currentRoom = MainClass.Cellar0;
  }
}

// Util is a toolbox with some nice functions, mostly for more array and console writing control
public static class Util {
  public static string newLine = "\r\n";
  public static string indent = "    ";
  public static string lineBreak = "--------------------------------";
  
  // Text utils
    // Glorified Console.Write for more control over what gets written
    // Also abstracts away formatting: instead of Console.Write-ing everying with the text passed thru FormatText,
    // this method does it for me
  public static void Write(string text) {
    Console.Write(FormatText(text));
  }

    // Takes a block of text and formats it with an indent, new line, and a horizontal line above it
  private static string FormatText(string text) {
    return indent + text + newLine + lineBreak + newLine;
  }

    // Takes an array, traverses the contents, and writes each
    // ignoreEmpty (bool): whether to write out empty entries from the array
    // format (bool): whether to apply formatting (indents, newlines, etc)
  public static string WriteArray(string[] array, bool ignoreEmpty, bool format) {
    string returnString = "";
    for(var i = 0; i < array.Length; i++ ) {
      if(array[i].Length == 0 && ignoreEmpty == true) {
        continue;
      } else {
        returnString += array[i];
        if(i != array.Length - 1) {
          returnString += ", ";
        }
      }
    }
    if(returnString.Substring(returnString.Length-2) == ", ") {
      returnString = returnString.Substring(0, returnString.Length-2);
    }
    return returnString;
  }

  // Variable utils
    // Checks if a selected option (string check) is in an array (string[] array)
  public static bool CheckForError(string check, string[] array) {
    for(int i = 0; i < array.Length; i++){
      if(check == array[i]){
        return false;
      }
    }
    return true;
  }
    // Toggles a boolean
  public static bool ToggleBool(bool toggle) {
    if(toggle) {
      return false;
    } else {
      return true;
    }
  }
}