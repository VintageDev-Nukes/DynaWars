using UnityEngine;
using System.Collections;

public class GUIExt {

	public static void Window(string title, Vector2 padding, bool background = false) {

		if(background) {
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
		}

		Texture2D winTitle = Resources.Load<Texture2D>("images/winTitle");

		Texture2D winContent = Resources.Load<Texture2D>("images/winContent");

		GUIStyle titleStyle = new GUIStyle();

		titleStyle.alignment = TextAnchor.MiddleCenter;
		titleStyle.normal.textColor = Color.white;
		titleStyle.fontStyle = FontStyle.Bold;

		GUI.DrawTexture(new Rect(padding.x, padding.y, 506, 36), winTitle);
		GUI.Label(new Rect(padding.x, padding.y, 506, 26), title, titleStyle);

		GUI.DrawTexture(new Rect(padding.x, padding.y+23, 506, 506), winContent);

	}

	public static bool Hover(Rect rect) {
		return rect.Contains(Event.current.mousePosition);
	}

	public static string NumUpDown(Rect rect, ref int value, string[] strArray) {
		value = NumUpDown(rect, value, 0, strArray.Length);
		return strArray[value];
	}

	public static int NumUpDown(Rect rect, int value, int minValue, int maxValue) {
		value = NumUpDown(rect, value);
		while(value < minValue) {
			value++;
		}
		while(value > maxValue) {
			value--;
		}
		return value;
	}

	public static int NumUpDown(Rect rect, int value) {
		if(GUI.Button(new Rect(rect.x, rect.y, 20, rect.height), "<")) {
			value--;
		}
		int.TryParse(GUI.TextField(new Rect(rect.x+20, rect.y, rect.width-40, rect.height), value.ToString()), out value);
		if(GUI.Button(new Rect(rect.x+20+rect.width-40, rect.y, 20, 20), ">")) {
			value++;
		}
		return value;
	}

}

public class AdvancedRepeatButton
{
	private float downTime, lastStep;
	private bool wasClicked;
	public AdvancedRepeatButton() { downTime = -1f; lastStep = -1f; wasClicked = false; }
	
	public bool Draw(Rect r, string txt, float delayBeforeRepeat, float repeatSteps) { return Draw(r, new GUIContent(txt), GUI.skin.button, delayBeforeRepeat, repeatSteps); }
	public bool Draw(Rect r, Texture2D tex, float delayBeforeRepeat, float repeatSteps) { return Draw(r, new GUIContent(tex), GUI.skin.button, delayBeforeRepeat, repeatSteps); }
	public bool Draw(Rect r, GUIContent content, float delayBeforeRepeat, float repeatSteps) { return Draw(r, content, GUI.skin.button, delayBeforeRepeat, repeatSteps); }
	public bool Draw(Rect r, string txt, GUIStyle style, float delayBeforeRepeat, float repeatSteps) { return Draw(r, new GUIContent(txt), style, delayBeforeRepeat, repeatSteps); }
	public bool Draw(Rect r, Texture2D tex, GUIStyle style, float delayBeforeRepeat, float repeatSteps) { return Draw(r, new GUIContent(tex), style, delayBeforeRepeat, repeatSteps); }
	public bool Draw(Rect r, GUIContent content, GUIStyle style, float delayBeforeRepeat, float repeatSteps)
	{
		return ARBLogic( GUI.RepeatButton(r, content, style), delayBeforeRepeat, repeatSteps );
		
	}
	
	public bool DrawLayout(string txt, float delayBeforeRepeat, float repeatSteps, params GUILayoutOption[] options) { return DrawLayout(new GUIContent(txt), GUI.skin.button, delayBeforeRepeat, repeatSteps, options); }
	public bool DrawLayout(Texture2D tex, float delayBeforeRepeat, float repeatSteps, params GUILayoutOption[] options) { return DrawLayout(new GUIContent(tex), GUI.skin.button, delayBeforeRepeat, repeatSteps, options); }
	public bool DrawLayout(GUIContent content, float delayBeforeRepeat, float repeatSteps, params GUILayoutOption[] options) { return DrawLayout(content, GUI.skin.button, delayBeforeRepeat, repeatSteps, options); }
	public bool DrawLayout(string txt, GUIStyle style, float delayBeforeRepeat, float repeatSteps, params GUILayoutOption[] options) { return DrawLayout(new GUIContent(txt), style, delayBeforeRepeat, repeatSteps, options); }
	public bool DrawLayout(Texture2D tex, GUIStyle style, float delayBeforeRepeat, float repeatSteps, params GUILayoutOption[] options) { return DrawLayout(new GUIContent(tex), style, delayBeforeRepeat, repeatSteps, options); }
	public bool DrawLayout(GUIContent content, GUIStyle style, float delayBeforeRepeat, float repeatSteps, params GUILayoutOption[] options)
	{
		return ARBLogic(GUILayout.RepeatButton(content, style, options), delayBeforeRepeat, repeatSteps);
	}
	
	private bool ARBLogic(bool clic, float delayBeforeRepeat, float repeatSteps)
	{
		bool shallPass = false;
		if( Event.current.type == EventType.Repaint )
		{
			if( clic )
			{
				if( !wasClicked )
				{
					wasClicked = true;
					downTime = Time.time;
					lastStep = Time.time;
					shallPass = true;
				}
				else if( Time.time - downTime > delayBeforeRepeat )
				{
					float currStep = Time.time - lastStep;
					if( currStep > repeatSteps )
					{
						currStep = -1f;
						lastStep = Time.time;
						shallPass = true;
					}
				}
			}
			else
			{
				wasClicked = false;
				downTime = -1f;
				lastStep = -1f;
			}
		}
		
		return clic && shallPass;
	}
}