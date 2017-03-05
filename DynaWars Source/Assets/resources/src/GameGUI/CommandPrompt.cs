using UnityEngine;
using System.Collections;

public class CommandPrompt : MonoBehaviour {

	bool _enabled = true;

	[HideInInspector]
	public string CommandString;
	
	private string CommandResults;
	//private float version = 0.1f;
	private bool __Enabled = false;
	private string currentCommand = "";
	
	void Update() {
		
		/*if(__Enabled == true) { // stop the player from walking when pressing the W button
			pController.setNoUpdate(true);
		} else {
			pController.setNoUpdate(false);
		}*/
		
		if(Input.GetKeyDown(mInput.GetKey("Console"))) {
			__Enabled = (__Enabled == false) ? true : false; // Opening command prompt
		}

		if(Input.GetKeyDown(mInput.GetKey("SendCommand"))) { // Send the command
			currentCommand = CommandString;
			CommandString = ""; 
		}
		
		if(currentCommand != "") {
			//  Executing command
			switch(currentCommand) {

				case ".help" :
					callHelp();
					break;

				case ".exit" :
					callExit();
					break;

				default:
					callHelp();
					break;

			}
		}
		
	}
	
	void  OnGUI() {
		
		if(_enabled && __Enabled) {
			
			CommandString = GUI.TextField( new Rect(10, Screen.height - 25 - 10, Screen.width / 4, 25), CommandString); // the type prompt
			GUI.Box( new Rect(10, Screen.height - 205 - 10, Screen.width / 4, 175), CommandResults); // results
			
		}
		
	}
	
	private void callHelp (){
		CommandResults += "\n/exit - Exit the command prompt";
	}

	private void callExit() {
		__Enabled = false;
	}

}
