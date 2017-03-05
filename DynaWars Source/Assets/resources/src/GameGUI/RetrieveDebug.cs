using UnityEngine;
using System.Collections;

public class DebugColor {

	private static Color _color;

	public static Color color {
		get {return _color;}
		set {_color = value;}
	}

}

public class RetrieveDebug : MonoBehaviour {

	static GUIStyle LogStyle;
	static string myLog;

	public static bool isChatFocused = false;

	[HideInInspector]
	public string chatStr = "";

	private string output = "";
	//private string stack = "";
	private Vector2 scrollPosition;

	void Start() {
		LogStyle = new GUIStyle();
		DebugColor.color = Color.white;
	}

	void  OnEnable() {
		Application.RegisterLogCallback(HandleLog);
	}
	
	void  OnDisable() {
		// Remove callback when object goes out of scope
		Application.RegisterLogCallback(null);
	}
	
	void HandleLog(string logString, string stackTrace, LogType type) {

		if(type.ToString().Equals("Log")) {
			DebugColor.color = new Color(0, 0.75f, 1);
		} else if(type.ToString().Equals("Warning")) {
			DebugColor.color = Color.yellow;
		} else if(type.ToString().Equals("Error")) {
			DebugColor.color = Color.red;
		}

		output = logString;
		//stack = stackTrace;
		myLog +=output+"\n";

		// setting the "y" value of scrollPosition puts the scrollbar at the bottom
		scrollPosition = new Vector2(scrollPosition.x, Mathf.Infinity);
	}
	
	void OnGUI() {

		//bool EnterPressed = Event.current.keyCode == KeyCode.Return;

		//GUI.GetNameOfFocusedControl() != "user"
		if(Event.current.isKey && !RetrieveDebug.isChatFocused) {
			chatStr += Event.current.character;
		}

		//Not setted
		LogStyle.normal.textColor = DebugColor.color;

		GUILayout.BeginArea(new Rect(10, Screen.height-10-50-200, 300, 200), GUI.skin.box);
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width (300), GUILayout.Height (200));
			
		// We just add a single label to go inside the scroll view. Note how the
		// scrollbars will work correctly with wordwrap.
		GUILayout.Label (myLog, LogStyle); //LogStyle
			
		// End the scrollview we began above.
		GUILayout.EndScrollView();

		//GUILayout.ExpandWidth(10), GUILayout.ExpandHeight(Screen.height-40)

		GUILayout.EndArea();

		//GUI.skin.textField.margin = new RectOffset(0,0,0,0);

		GUILayout.BeginArea (new Rect(10, Screen.height-46, 250, 40), GUI.skin.label);

		GUI.SetNextControlName("chat");
		
		chatStr = GUILayout.TextField(chatStr, GUILayout.Width(240), GUILayout.Height(30));

		RetrieveDebug.isChatFocused = GUI.GetNameOfFocusedControl() == "user";

		GUILayout.EndArea ();

		GUILayout.BeginArea (new Rect(255, Screen.height-47, 100, 40), GUI.skin.label);

		//GUILayout.Button("Enviar", GUILayout.Width(50), GUILayout.Height(30)) || (
		if( GUILayout.Button("Enviar", GUILayout.Width(50), GUILayout.Height(30)) || (Event.current.keyCode == KeyCode.Return && !chatStr.Equals(System.String.Empty))) {
			SendMessage();
		}
		
		GUILayout.EndArea ();

		/* Chat system can be implemented here */

	}

	void SendMessage(Object varmes = null, string user = "User") {

		string message = chatStr; //.Replace(System.Environment.NewLine, "");

		if(varmes != null) {
			message =  varmes.ToString(); //.Replace(System.Environment.NewLine, "");
		}

		Debug.Log("<"+user+"> "+message);
		chatStr = "";
	}

}
