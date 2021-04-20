using System;
using System.Collections.Generic;

// This is a main class
// Idk what else to say about it, it's main class
class MainClass {
  public static bool skipPrompt = false;
  public static bool breakUpdate = false;

  public static string defaultRoom = "Cellar0";

  // Rooms
  public static Room Cellar0;

  // Player
  public static EntityPlayer Player;

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

  static void RegisterInstances() {
    // Rooms
      // Cellar0
      string decExts = "W";
      string decItms = "Drapes";
      string decDesc = "You are in a dark, dusty cellar. You were told there was treasure here...";
      string decRmID = "Cellar0";
      bool decCurm = false;
      Cellar0 = new Room(decExts.Split(", "), decItms.Split(", "), decDesc, decRmID, decCurm);
      Cellar0.Start();

      Room.roomList = new string[Room.roomCount];

    // Instantize player
    Player = new EntityPlayer();
    Player.SetCurrentRoom();
  }

  // DisplayPrompt writes a prompt
  // It can optionally be skipped to avoid console clutter
  static void DisplayPrompt(bool skip) {
    if(skip == false) {
      Util.Write(MainClass.Cellar0.DescribeRoom());
      Util.Write("What will you do next?");
      Util.Write("Enter 'Help' for a list of options");
    }
    skipPrompt = false;
  }
}

// InputProcessing is the factory where player input is received and dealt with
// Majority of the game's funcitonality occurs here
public class InputProcessing {

  public static string[] InputOptions = { "Help", "Move", "Inspect", "Save", "Exit", "", "", "" };

  // The input processor
  // Takes in an input, passes it through one nasty switch block, 
  // and sends control to the input behaviour methods
  public static string ProcessInput(string input) {
    if(Util.CheckForError(input, InputOptions) == true) {
      return "Not an input";
    }
    MainClass.skipPrompt = true;
    switch (input) {
      case "Help": return "Your options are: " + Util.WriteArray(InputOptions, true, false);
      case "Move": return MovePlayer();
      case "Save": return SaveGame();
      case "Exit": return "3";
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
  // These methods are called in a switch block in ProcessInput
  // Each method must return a string
  public static string MovePlayer() {
    return "You can't move right now";
  }
  public static string SaveGame() {
    return MainClass.Player.DescribeCurrentRoom();
  }
  public static string ExitGame() {
    return "Nah, you can keep playing for a bit";
  }
  public static string MoveDrapes() {
    return "You found a jewelry box!" + AddInputOption("Open Jewelry Box");
  }
  public static string OpenJewelryBox() {
    return "You found a pearl necklace" + CueEndScreen();
  }
  public static string CueEndScreen() {
    string msg = Util.newLine + Util.lineBreak + Util.newLine + "Congratulations, you found the treasure!" + Util.newLine + 
           "This is a prototype for a text adventure written in C#" + Util.newLine +
           "Thank you for playing!";
    MainClass.breakUpdate = true;
    return msg;
  }

  // Inspect action
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
  public static string AddInputOption(string option) {
    for(int i = 0; i < InputOptions.Length; i++ ) {
      if(InputOptions[i].Length == 0) {
        InputOptions[i] = option;
        return Util.newLine + Util.indent + "You can now: " + option;
      }
    }
    return null;
  }
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

  public string DescribeRoom() {
    return this.description;
  }

  public void Start() {
    roomCount++;
  }

  public Room(string[] xts, string[] itm, string dsc, string rid, bool cur) {
    exits = xts;
    items = itm;
    description = dsc;
    RoomID = rid;
    currentRoom = cur;
  }
}

// The player class is instantiated once and tracks where the player is,
// what's in their inventory, etc
public class EntityPlayer {
  public int health = 0;
  public Room currentRoom;

  public EntityPlayer() {
    health = 10;
  }

  public string DescribeCurrentRoom() {
    return currentRoom.description;
  }

  public void SetCurrentRoom() {
    currentRoom = MainClass.Cellar0;
  }
}

// Util is a toolbox with some nice functions, mostly for more Console control
// 
public static class Util {
  public static string newLine = "\r\n";
  public static string indent = "    ";
  public static string lineBreak = "--------------------------------";
  
  // Text utils
  public static void Write(string text) {
    Console.Write(FormatText(text));
  }

  private static string FormatText(string text) {
    return indent + text + newLine + lineBreak + newLine;
  }

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

  public static string WriteArray(int[] array, bool ignoreEmpty, bool format) {
    string returnString = "";
    for(var i = 0; i < array.Length; i++ ) {
      if(array[i].ToString().Length == 0 && ignoreEmpty == true) {
        continue;
      } else {
        returnString += array[i].ToString();
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
  public static bool CheckForError(string check, string[] array) {
    for(int i = 0; i < array.Length; i++){
      if(check == array[i]){
        return false;
      }
    }
    return true;
  }

  public static bool ToggleBool(bool toggle) {
    if(toggle) {
      return false;
    } else {
      return true;
    }
  }
}