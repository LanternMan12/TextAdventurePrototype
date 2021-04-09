using System;
using System.Collections.Generic;

class MainClass {

  public static void Main (string[] args) {
    Registry.InitializeRooms();
    
    foreach(var item in Cellar0){
      Console.Write(item);
    }
    UpdateScreen();
  }

  static void UpdateScreen() {
    DisplayPrompt();
    string input = Console.ReadLine();
    Console.Write(InputProcessing.ProcessInput(input));
    UpdateScreen();
  }

  static void DisplayPrompt() {
    Console.Write(Util.FormatText("What will you do next?"));
    Console.Write(Util.FormatText("Enter 'Help' for a list of options"));
  }
}

public class InputProcessing {

  public static string[] inputs = { "Help", "Move", "Inpsect", "Save", "Exit" };

  // Input behaviour methods 
  // These methods are called in a switch block in ProcessInput
  // Each method must return a string
  public static string WriteOptions() {
    string str = "";
    for(int i = 0; i < inputs.Length; i++){
      if(str.Length == 0) {
        str = inputs[0];
      } else {
        str = str + ", " + inputs[i];
      }
    }
    return Util.FormatText("Your options are: " + str);
  }
  public static string MovePlayer() {
    return Util.FormatText("You can't move right now");
  }
  public static string SaveGame() {
    return Util.FormatText("Game saved (jk lol)");
  }
  public static string ExitGame() {
    return Util.FormatText("Nah, you can keep playing for a bit");
  }

  public static string ProcessInput(string input) {
    if(CheckForError(input) == true) {
      return Util.FormatText("Not an input");
    }
    switch (input) {
      case "Help": return WriteOptions();
      case "Move": return MovePlayer();
      case "Save": return "2";
      case "Exit": return "3";
      default: return "0";
    }
  }

  public static bool CheckForError(string input) {
    for(int i = 0; i < inputs.Length; i++){
      if(input == inputs[i]){
        return false;
      }
    }
    return true;
  }
}

public class Room {
  public static int roomCount; 
  private string[] exits;
  private string[] items;
  private string description;
  private string RoomID;

  public Room(string[] xts, string[] itm, string dsc, string rid) {
    roomCount++;
    exits = xts;
    items = itm;
    description = dsc;
    RoomID = rid;
  }
}

public static class Util {
  public string FormatText(string text) {
    string indent = "    ";
    string newLine = "\r\n";
    return indent + text + newLine;
  }
}