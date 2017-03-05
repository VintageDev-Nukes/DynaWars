using System;
using UnityEngine;
using System.Linq;

public enum Alignment
{
	None,
	LeftTop,
	RightTop,
	LeftBot,
	RightBot
}

public class RadarSystem : MonoBehaviour
{
	public float Distance = 100;
	public string[] EnemyTag;
	private Vector2 inposition;
	public bool MapRotation;
	public Texture2D NavBG;
	public Texture2D NavCompass;
	public Texture2D[] Navtexture;
	public Alignment PositionAlignment;
	public Vector2 PositionOffset = Vector2.zero;
	public float Size = 400;

	[HideInInspector]
	float Scale = 1;

	private Matrix4x4 tempMatrix;

	private bool EnableGUI = true;
	private bool bigMap;

	private Vector2 ConvertToNavPosition(Vector3 pos)
	{
		Vector2 vector;
		vector.x = inposition.x + (((pos.x - Player.PlayerObj.transform.position.x) + ((Size * Scale) / 2f)) / Scale);
		vector.y = inposition.y + ((-(pos.z - Player.PlayerObj.transform.position.z) + ((Size * Scale) / 2f)) / Scale);
		return vector;
	}
	
	private void DrawNav(GameObject[] enemylists, Texture2D navtexture)
	{
		for (int i = 0; i < enemylists.Length; i++)
		{
			if (Vector3.Distance(Player.PlayerObj.transform.position, enemylists[i].transform.position) <= (Distance * Scale))
			{
				Vector2 a = ConvertToNavPosition(enemylists[i].transform.position);
				if ((Vector2.Distance(a, inposition + new Vector2(Size / 2f, Size / 2f)) + (navtexture.width / 2)) < (Size / 2f))
				{

					float scale = Scale;

					if (scale < 1f)
					{
						scale = 1f;
					}

					Texture2D finalTexture = navtexture;

					if(enemylists[i].tag == "Waypoint") {
						finalTexture = TextureExt.ReplaceColor(navtexture, Color.black, WaypointLib.waypointList.FirstOrDefault(x => x.name == enemylists[i].GetComponent<WaypointIndexer>().WaypointName).color);
					}

					GUIUtility.RotateAroundPivot(enemylists[i].transform.eulerAngles.y, new Vector2(a.x - ((((float)finalTexture.width) / scale) / 2f) + finalTexture.width/2, a.y - ((((float) finalTexture.height) / scale) / 2f) + finalTexture.height/2));
					GUI.DrawTexture(new Rect(a.x - ((((float) finalTexture.width) / scale) / 2f), a.y - ((((float)finalTexture.height) / scale) / 2f), finalTexture.width, finalTexture.height), finalTexture);
					GUI.matrix = tempMatrix;

				} else {
					if(enemylists[i].tag == "Waypoint" && WaypointLib.waypointList.FirstOrDefault(x => x.name == enemylists[i].GetComponent<WaypointIndexer>().WaypointName).state != WaypointState.Hidden) {

						GUI.depth = 2;

						Texture2D triangleWaypoint = Resources.Load<Texture2D>("images/wpTriangle");
						Texture2D finalTexture = new Texture2D(triangleWaypoint.width, triangleWaypoint.height);
						finalTexture = TextureExt.ReplaceColor(triangleWaypoint, Color.black, WaypointLib.waypointList.FirstOrDefault(x => x.name == enemylists[i].GetComponent<WaypointIndexer>().WaypointName).color);
						GUI.matrix = tempMatrix;

						Vector3 dir = enemylists[i].transform.position - Player.PlayerObj.transform.position;
						dir.y = 0;
						dir.Normalize();

						Vector3 forw = enemylists[i].transform.position;
						forw.y = 0;
						forw.Normalize();

						float rotation = Vector3Ext.AngleOffAroundAxis(dir, forw, Vector3.up);
						rotation *= 180 / Mathf.PI;
						rotation -= 90;

						GUIUtility.RotateAroundPivot(295, new Vector2(Screen.width-266, 100));
						GUIUtility.RotateAroundPivot(rotation, new Vector2(Screen.width-138, 138));
						GUI.DrawTexture(new Rect(Screen.width-266, 100, finalTexture.width, finalTexture.height), finalTexture); //a.x - ((((float) finalTexture.width) / scale) / 2f), a.y - ((((float)finalTexture.height) / scale) / 2f)
						GUI.matrix = tempMatrix;

						GUI.depth = 0;

					}
				}
			}
		}
	}
	
	private void OnGUI()
	{

		tempMatrix = GUI.matrix;

		if(!bigMap && EnableGUI) {

			if (MapRotation)
			{
				GUIUtility.RotateAroundPivot(-Player.PlayerObj.transform.eulerAngles.y, inposition + new Vector2(Size / 2f, Size / 2f));
			} else
			{
				GUIUtility.RotateAroundPivot(90, inposition + new Vector2(Size / 2f, Size / 2f));
			}

			for (int i = 0; i < EnemyTag.Length; i++)
			{
				DrawNav(GameObject.FindGameObjectsWithTag(EnemyTag[i]), Navtexture[i]);
			}

			GUI.depth = 1;

			GUI.DrawTexture(new Rect(inposition.x, inposition.y, Size, Size), NavBG);
			GUI.matrix = tempMatrix;
			GUIUtility.RotateAroundPivot(Player.PlayerObj.transform.eulerAngles.y, inposition + new Vector2(Size / 2f, Size / 2f));
			GUI.DrawTexture(new Rect((inposition.x + (Size / 2f)) - (((float) NavCompass.width) / 2f), (inposition.y + (Size / 2f)) - (((float) NavCompass.height) / 2f), (float) NavCompass.width, (float) NavCompass.height), NavCompass);
		
			GUI.depth = 0;

		}

	}
	
	private void Update()
	{
		//Needs to be calibrated
		Scale = transform.position.y * 0.2f / 30;

		if (Input.GetKeyDown(mInput.GetKey("EnableGUI")) && !PlayerStats.Killed) {
			EnableGUI = (EnableGUI == false) ? true : false;		
		}
		
		if(Input.GetKeyDown(mInput.GetKey("ToggleMap")) && !PlayerStats.Killed) {
			bigMap = (bigMap == false) ? true : false;
		}
		
		if (Scale <= 0f)
		{
			Scale = 1f;
		}

		switch (PositionAlignment)
		{
			case Alignment.None:
				inposition = PositionOffset;
				break;
				
			case Alignment.LeftTop:
				inposition = Vector2.zero + PositionOffset;
				break;
				
			case Alignment.RightTop:
				inposition = new Vector2(Screen.width - Size, 0f) + PositionOffset;
				break;
				
			case Alignment.LeftBot:
				inposition = new Vector2(0f, Screen.height - Size) + PositionOffset;
				break;
				
			case Alignment.RightBot:
				inposition = new Vector2(Screen.width - Size, Screen.height - Size) + PositionOffset;
				break;
		}
	}

}

