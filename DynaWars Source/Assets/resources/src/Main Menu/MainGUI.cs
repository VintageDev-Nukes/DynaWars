using UnityEngine;
using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CorruptedSmileStudio.MessageBox;

public class MainGUI : MonoBehaviour {

	public static List<Worlds> allWorlds = new List<Worlds>();

	Texture2D logo;
	Texture2D mainmenuTexture;
	
	GameObject waterObject;
	CharacterMotor chMotor;
	float WaterLevel;
	WaterSimple water;

	bool mainmenu = true;
	bool SelectGame;
	bool CreateWorld;
	bool recreate;
	bool rename;
	bool deleteMenu;

	bool optionsMenu;

	string WorldName = "";
	string lvlSeed = "";

	string currentfolder;
	string[] savesfolders;

	int MenuMaxItems;
	float MenuvScroll = 0;

	Rect windowRect;

	int y = 0;
	int selectedIndex = 0;

	private int seedBeta;

	private bool[] clicked;

	public GameObject personaje;

	BlurEffect BE;

	string selectedWorld;
	string renamedWorld;

	bool WorldIsSelected = false;

	GUIStyle buttonStyle;

	private List<string> svf;

	private Vector3 InputfixPos;

	//private MessageBox msgbox;

	void CrearFolders()
	{
		//Create Saves directory
		if(!Directory.Exists(Application.dataPath + "/saves/")) {
			Directory.CreateDirectory(Application.dataPath + "/saves/");
		}

		//Create Config folder
		if(!Directory.Exists(Application.dataPath + "/config/")) {
			Directory.CreateDirectory(Application.dataPath + "/config/");
		}

		//Create Screnshots folder
		if(!Directory.Exists(Application.dataPath + "/screenshots/")) {
			Directory.CreateDirectory(Application.dataPath + "/screenshots/");
		}

	}

	/*void CreateConfig() {
		string datanew = "QualityLvl=3" + Environment.NewLine + "FOV=60" + Environment.NewLine + "Sensibility=15" + Environment.NewLine + "Brightness=0.3" + Environment.NewLine + "RenderDistance=1500" + Environment.NewLine + "MaxFPS=-1" + Environment.NewLine + "InvertMouse=0" + Environment.NewLine + "VSync=1" + Environment.NewLine + "MinimapZoom=100";
		
		if (!System.IO.File.Exists (Application.dataPath + "/appConfig.cfg")) {
			using (System.IO.FileStream fs = System.IO.File.Create(Application.dataPath + "/appConfig.cfg")) {
				Byte[] info = new System.Text.UTF8Encoding(true).GetBytes(datanew);
				// Add some information to the file.
				fs.Write (info, 0, info.Length);
				fs.Close();
			}
		}
	}*/

	int windowRectH(int y) 
	{
		int i = (int)(Screen.height*0.7f/100);
		if (y > i) {
			return i;
		}
		return y;
	}

	void Awake() {
		ApplySettings();
	}

	void ApplySettings() { //Esta función carga la string de Profile.LoadedProfile

		//Si estamos en el editor cargar el profile llamado test
		#if UNITY_EDITOR
			Profile.LoadedProfile = "test";
		#endif

		//Create the default config
		if(!System.IO.File.Exists(Application.dataPath + "/Profiles/"+Profile.LoadedProfile+"/profile.xml")) {
			Profile.CreateDefaultProfile(Profile.LoadedProfile);
		}

		//And load it...
		Profile.Load();

		//Si no estamos en el editor la string no se carga y por tanto este if se leerá...
		if(String.IsNullOrEmpty(Profile.LoadedProfile)) {

			try {

				//Haya o no haya comandos para leer necesitamos que en caso de que "-CustomArgs:profile=null" el código pase al catch donde se buscaría el launcher...
				Profile.LoadedProfile = CommandLineReader.GetCustomArgument("profile"); //If customarg is null then open the launcher, but where it is? %APPDATA% knows it.
				//Profile.Load();
				Profile.StaticLoad(); //If appConfig.xml doesn't exists pass to the next block
				Screen.fullScreen = Convert.ToBoolean(Convert.ToInt32(CommandLineReader.GetCustomArgument("fullscreen"))); //Siendo este el segundo comando que se lea evitaremos que se cambie dos veces la resolución al pasar al catch
				Profile.staticProfile.lastLoadedProfile = Profile.LoadedProfile;
				//INI_Manager.Set_Value(Application.dataPath + "/appConfig.cfg", "lastLoadedProfile", Profile.LoadedProfile);

			} catch {

				Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true); //Seteamos pantalla completa

				/*try {
					//Intetamos cargar el último perfil cargado desde appConfig.cfg si no existe pasamos al catch...
					Profile.LoadedProfile = Profile.currentProfile.lastLoadedProfile; //INI_Manager.Load_Value("lastLoadedProfile", Application.dataPath + "/appConfig.cfg");
				} catch {*/
					
					//Ejecutar el launcher
					
				try {

					//El primer try de este catch intentará sacar la ruta del launcher para ejecutarlo...

					string LauncherPath = ""; //INI_Manager.Load_Value("LauncherPath", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"/.dynawars/app.conf");
						
					try {
						//Si la ruta del launcher se encontró se ejecuta y la aplicación se cierra...
						Process.Start(LauncherPath);
						Application.Quit();
					} catch {
							
						//En caso de que no se pudiese ejecutar se lanza un aviso...
						MessageBox.Show(LauncherNotFound, "No puedes abrir el juego directamente, debes abrirlo desde el Launcher manualmente.", "Error", MessageBoxButtons.OK);

					}
						
				} catch {

					//Si no se ha encontrado la ruta del launcher entonces el usuario deberá ejecutar manualmente este...
					MessageBox.Show(LauncherNotFound, "No puedes abrir el juego directamente, el juego tampoco pudo encontrar el Launcher."+System.Environment.NewLine+
					        "Busca y abrelo manualmente, para que se cree la configuración permantente.", "Error", MessageBoxButtons.OK);
						
				}
					
				//}

			}
		}

	}

	// Use this for initialization
	void Start () 
	{

		WaterLevel = UnityEngine.Random.Range (0, 20);
		logo = (Texture2D)Resources.Load("images/logo");
		waterObject = (GameObject)Instantiate(Resources.Load("prefabs/Water4Example"));
		water = waterObject.AddComponent<WaterSimple>();
		water.transform.position = new Vector3(0, WaterLevel, 0);
		water.transform.localScale = new Vector3 (4000, 1, 4000);

		//Crear todos los directorios
		CrearFolders();
		
		svf = new List<string>();

		if(Directory.GetDirectories (Application.dataPath + "/saves/").Length > 0) {
			foreach (string folder in Directory.GetDirectories (Application.dataPath + "/saves/")) 
			{
				if(File.Exists(folder + "/level.xml"))
				{
					allWorlds.Add(new Worlds() {path = folder, svProfile = XMLTools.DeserializeFromFile<SaveProfile>(folder + "/level.xml")});
					svf.Add(folder);
					y++;
				}
			}
		}

		clicked = new bool[y];
		savesfolders = new string[y];
		savesfolders = svf.ToArray ();
		for (int i = 0; i > y; i++) {
			clicked[i] = false;
		}

	}

	void OnGUI() 
	{

		if (WorldIsSelected) {
			buttonStyle = GUI.skin.GetStyle("button");
		} else {
			buttonStyle = GUI.skin.GetStyle("box");
			buttonStyle.alignment = TextAnchor.MiddleCenter;
		}

		if (mainmenu) {
			
			if (GUI.Button (new Rect (Screen.width / 2 - 150, 120, 300, 50), "Singleplayer")) {
				
				SelectGame = true;
				mainmenu = false;
		
			}

			if (GUI.Button (new Rect (Screen.width / 2 - 150, 200, 300, 50), "Opciones")) {
				
				optionsMenu = true;
				mainmenu = false;
				
			}

			if (GUI.Button (new Rect (Screen.width / 2 - 150, 280, 300, 50), "Salir")) {
				
				Application.Quit();
				
			}
			
			GUI.DrawTexture (new Rect (Screen.width / 2 - 519/2, 10, 519, 103), logo);
		
		} else if (SelectGame) {

			GUI.Window (0, new Rect (Screen.width * 0.1f, Screen.height * 0.05f, Screen.width * 0.8f, Screen.height * 0.9f), ShowSelectGame, "Seleccione una partida");

		} else if (CreateWorld) {

			//GUI.Box(windowRect, "");	
			GUI.Window (0, new Rect (Screen.width * 0.1f, Screen.height * 0.05f, Screen.width * 0.8f, Screen.height * 0.9f), CreateWorldMenu, "Seleccione una partida");

		} else if(optionsMenu) {

			GUI.Window (0, new Rect (Screen.width * 0.1f, Screen.height * 0.05f, Screen.width * 0.8f, Screen.height * 0.9f), OptionsWindow, "Seleccione una partida");

		} else if(deleteMenu) {

			GUI.Window (0, new Rect (Screen.width * 0.35f, Screen.height * 0.3f, Screen.width * 0.3f, Screen.height * 0.4f), DeleteWindow, "Ventana de borrado");

		} else if(recreate) {

			GUI.Window (0, new Rect (Screen.width * 0.1f, Screen.height * 0.05f, Screen.width * 0.8f, Screen.height * 0.9f), RecreateWorld, "Recrear");

		} else if(rename) {

			GUI.Window (0, new Rect (Screen.width * 0.35f, Screen.height * 0.3f, Screen.width * 0.3f, Screen.height * 0.4f), RenameWindow, "Renombrar mundo");

		}

	}

	void OptionsWindow(int WindowID) {

		if (!Options.Draw ("menu")) {
			optionsMenu = false;
			mainmenu = true;
		}

	}

	void DeleteWindow(int WindowID) {

		GUI.Label (new Rect(25, 50, Screen.width*0.3f-50, 50), "¿Deseas borrar la partida seleccionada?");

		if (GUI.Button (new Rect(Screen.width*0.15f-100, Screen.height*0.4f-75, 100, 50), "Sí")) {
			DirectoryInfo downloadedMessageInfo = new DirectoryInfo(selectedWorld);
			
			foreach (FileInfo file in downloadedMessageInfo.GetFiles())
			{
				file.Delete(); 
			}
			foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
			{
				dir.Delete(true); 
			}

			Directory.Delete(selectedWorld);

			Application.LoadLevel("Main");

			deleteMenu = false;
			mainmenu = false;
			SelectGame = true;
		}

		if (GUI.Button (new Rect(Screen.width*0.15f+15, Screen.height*0.4f-75, 100, 50), "No")) {
			deleteMenu = false;
			SelectGame = true;
		}
		
	}

	bool nameSetted = false;

	void RenameWindow(int WindowID) {

		if(!nameSetted) {
			renamedWorld = selectedWorld;
			nameSetted = true;
		}

		selectedWorld = GUI.TextField (new Rect (10, 30, Screen.width*0.3f-20, 30), selectedWorld);

		if (GUI.Button (new Rect (50, 75, Screen.width*0.3f-100, 50), "Renombrar")) {

			//INI_Manager.Set_Value(Application.dataPath + "/saves/"+renamedWorld+"/level.dat", "lvlName", selectedWorld);

			Directory.CreateDirectory(Application.dataPath + "/saves/" + selectedWorld);

			//SaveProfile svProfile = XMLTools.DeserializeFromFile<SaveProfile>(Application.dataPath + "/saves/" + renamedWorld + "/level.xml");

			SaveProfile svProfile = allWorlds.FirstOrDefault(x => x.path == Application.dataPath + "/saves/" + renamedWorld).svProfile;

			svProfile.lvlName = selectedWorld;

			XMLTools.SerializeToFile<SaveProfile>(svProfile, Application.dataPath + "/saves/" + selectedWorld + "/level.xml", true);

			Directory.Delete(Application.dataPath + "/saves/" + renamedWorld);

			rename = false;
			SelectGame = true;

		}

		if (GUI.Button (new Rect (50, 140, Screen.width*0.3f-100, 50), "Cancelar")) {
			rename = false;
			SelectGame = true;
		}

	}

	//private int times;

	void RecreateWorld(int WindowID) {

		SaveProfile svProfile = new SaveProfile();

		if (!nameSetted) {
			svProfile = XMLTools.DeserializeFromFile<SaveProfile>(Application.dataPath + "/saves/" + selectedWorld + "/level.xml");
			/*if(selectedWorld.StartsWith("Copia de ")) {
				selectedWorld = "copia de " + selectedWorld;
			} else {
				selectedWorld = "Copia de " + selectedWorld;
			}*/
			selectedWorld = "Copia de " + svProfile.lvlName.Replace("Copia", "copia");
			WorldName = selectedWorld;
			//lvlSeed = svProfile.strSeed;
			nameSetted = true;
		}

		GUI.Label (new Rect (Screen.width*0.8f/2-140, 30, 125, 30), "Nombre de la partida:");
		selectedWorld = GUI.TextField (new Rect (Screen.width*0.8f/2+15, 30, 200, 30), selectedWorld);

		//No es necesario, puesto que se supone que es solo volver a crear el mundo con la misma seed
		/*GUI.Label (new Rect (Screen.width*0.8f/2-140, 75, 125, 30), "Seed:");
		lvlSeed = GUI.TextField (new Rect (Screen.width*0.8f/2+15, 75, 200, 30), lvlSeed);*/
		
		if (GUI.Button (new Rect (Screen.width*0.8f/2 - 200, 75, 200, 50), "Volver a crear")) 
		{
			
			if(!Directory.Exists(Application.dataPath + "/saves/"+WorldName)) {
				Directory.CreateDirectory(Application.dataPath + "/saves/"+WorldName);
			} else {
				string[] paths = Directory.GetDirectories(Application.dataPath + "/saves/", WorldName);
				Directory.CreateDirectory(Application.dataPath + "/saves/"+WorldName+" ("+(paths.Length+1)+")");
				/*int times = 0;
				
				foreach (string folder in Directory.GetDirectories (Application.dataPath + "/saves/")) {
					if(folder.Replace(Application.dataPath + "/saves/", "").Substring(folder.Replace(Application.dataPath + "/saves/", "").Length - WorldName.Length) == WorldName) {
						times += 1;
					}
				}*/
				
				//WorldName = selectedWorld;
				
				//Debug.Log(WorldName);
				
				//Directory.CreateDirectory(Application.dataPath + "/saves/"+WorldName);
			}

			svProfile.lvlName = WorldName;

			svProfile.SetPlayerValues();
			
			/*INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "lvlName", WorldName);
			
			INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "playerX", UnityEngine.Random.Range(0, 100).ToString()); //VectorSerializer.Searialize3(player.transform.position)
			INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "playerY", "30");
			INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "playerZ", UnityEngine.Random.Range(0, 100).ToString());
			
			INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "playerotX", "0");
			INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "playerotY", "0");
			INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "playerotZ", "0");*/
			
			//INI_Manager.Set_Value(Application.dataPath + "/appConfig.cfg", "lastPlayedWorld", WorldName);

			//Profile.currentProfile.lastPlayedWorld = WorldName;

			SProf sProfile = XMLTools.DeserializeFromFile<SProf>(Application.dataPath + "/Profiles/"+Profile.LoadedProfile+"/profile.xml"); //Profile.currentProfile;

			sProfile.lastPlayedWorld = WorldName;

			XMLTools.SerializeToFile<SProf>(sProfile, Application.dataPath + "/Profiles/"+Profile.LoadedProfile+"/profile.xml", true);

			if(!String.IsNullOrEmpty(lvlSeed)) 
			{
				SeedExtension seedExt = new SeedExtension(lvlSeed);
				svProfile.strSeed = lvlSeed;
				svProfile.seed = seedExt.ToInt();
				//INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "seed", seedExt.ToInt().ToString());
				//INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "seedKey", lvlSeed);
			} else {
				System.Random rnd = new System.Random ();
				seedBeta = rnd.Next();
				svProfile.strSeed = seedBeta.ToString();
				svProfile.seed = seedBeta;
				//INI_Manager.Set_Value (Application.dataPath + "/saves/" + WorldName + "/level.dat", "seed", seedBeta.ToString ()); 
				//INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "seedKey", seedBeta.ToString());
			}
			
			//Debug.Log("Nombre del mundo: " + WorldName + ", Lvl Seed: " + lvlSeed); 

			XMLTools.SerializeToFile<SaveProfile>(svProfile, Application.dataPath + "/saves/"+WorldName+"/level.xml", true);

			Profile.Save();

			Application.LoadLevel("Game");
			
		}

		if(GUI.Button(new Rect(Screen.width*0.8f/2+15, 75, 200, 50), "Cancelar"))
		{
			recreate = false;
			SelectGame = true;
		}
		
	}

	void ShowSelectGame(int WindowID)
	{

		MenuMaxItems = y;

		MenuvScroll = GUI.VerticalSlider (new Rect (Screen.width*0.8f-50, Screen.height*0.05f, 10, Screen.height*0.7f-55), MenuvScroll, 0, 100*MenuMaxItems);
	
		GUI.BeginGroup (new Rect (Screen.width*0.05f, Screen.height*0.05f, Screen.width*0.8f, Screen.height*0.7f-55));

		GUI.Box (new Rect(0, 0, Screen.width*0.7f, Screen.height*0.7f-55), "");

		GUI.BeginGroup (new Rect (0, -MenuvScroll, Screen.width*0.8f, MenuMaxItems * 100));

		for (int x = 0; x<MenuMaxItems; x++) 
		{

			if (clicked[x]) {
				GUI.Box(new Rect(0,100*x,Screen.width*0.7f,100), "");
				WorldIsSelected = true;
			} else {
				//GUI.Button(new Rect(0,100*x,Screen.width*0.7f,100), "", "label");
				//WorldIsSelected = false;
			}

			currentfolder = savesfolders[x];

			// Make a group on the center of the screen
			GUI.BeginGroup (new Rect (0, 100*x, Screen.width*0.7f, 100));
			// All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.
			
			// We'll make a box so you can see where the group is on-screen.
			//GUI.Box(new Rect(0,0,Screen.width*0.7f,100), "", "box");
			if(GUI.Button(new Rect(0,0,Screen.width*0.7f,100), "", "label")) { //"label"
				for(int j = 0; j<y; j++) {
					clicked[j] = false;
				}
				clicked[x] = true;
			}

			//SaveProfile svProfile = XMLTools.DeserializeFromFile<SaveProfile>(currentfolder+"/level.xml");

			SaveProfile svProfile = allWorlds.FirstOrDefault(z => z.path == currentfolder).svProfile;

			GUI.Label(new Rect(30, 25, Screen.width*0.7f-60,50), svProfile.lvlName);

			// End the group we started above. This is very important to remember!
			GUI.EndGroup ();
		
		}

		GUI.EndGroup ();

		GUI.EndGroup ();

		if (Event.current.clickCount >= 2 && Input.GetMouseButton(0) && windowRect.Contains(InputfixPos))
		{
			//SaveProfile svProfile = XMLTools.DeserializeFromFile<SaveProfile>(savesfolders[selectedIndex]+"/level.xml");
			SaveProfile svProfile = allWorlds.FirstOrDefault(x => x.path == savesfolders[selectedIndex]).svProfile;
			string clickedGame = svProfile.lvlName; //INI_Manager.Load_Value("lvlName", savesfolders[selectedIndex]+"/level.dat");
			//INI_Manager.Set_Value(Application.dataPath + "/appConfig.cfg", "lastPlayedWorld", clickedGame);
			//Profile.currentProfile.lastPlayedWorld = clickedGame;
			//Profile.SetProperty(ProfileProperty.LastPlayedWorld, clickedGame); //controlar "currentProfile.lastPlayedWorld" y donde este set value hay que setearlo y load value dejarlo igual
			Profile.currentProfile.lastPlayedWorld = clickedGame;
			Profile.Save();
			Application.LoadLevel("Game");
		} else if(GUI.Button(new Rect(Screen.width*0.8f/2-215, Screen.height*0.9f-130, 215, 50), "Jugar partida seleccionada", buttonStyle) && WorldIsSelected)
		{
			//SaveProfile svProfile = XMLTools.DeserializeFromFile<SaveProfile>(savesfolders[selectedIndex]+"/level.xml");
			SaveProfile svProfile = allWorlds.FirstOrDefault(x => x.path == savesfolders[selectedIndex]).svProfile;
			string clickedGame = svProfile.lvlName; //INI_Manager.Load_Value("lvlName", savesfolders[selectedIndex]+"/level.dat");
			//INI_Manager.Set_Value(Application.dataPath + "/appConfig.cfg", "lastPlayedWorld", clickedGame);
			//Profile.lastPlayedWorld = clickedGame;
			//Profile.SetProperty(ProfileProperty.LastPlayedWorld, clickedGame);
			Profile.currentProfile.lastPlayedWorld = clickedGame;
			Profile.Save();
			Application.LoadLevel("Game");
		}

		if(GUI.Button(new Rect(Screen.width*0.8f/2+15, Screen.height*0.9f-130, 215, 50), "Crear nueva partida"))
		{
			SelectGame = false;
			CreateWorld = true;
		}

		if(GUI.Button(new Rect(Screen.width*0.8f/2-215, Screen.height*0.9f-65, 100, 50), "Renombrar", buttonStyle) && WorldIsSelected)
		{
			SelectGame = false;
			rename = true;
			selectedWorld = savesfolders[selectedIndex].Replace(Application.dataPath + "/saves/", "");
		}

		if(GUI.Button(new Rect(Screen.width*0.8f/2-100, Screen.height*0.9f-65, 100, 50), "Borrar", buttonStyle) && WorldIsSelected)
		{
			SelectGame = false;
			deleteMenu = true;
			selectedWorld = savesfolders[selectedIndex];
		}

		if(GUI.Button(new Rect(Screen.width*0.8f/2+15, Screen.height*0.9f-65, 100, 50), "Recrear", buttonStyle) && WorldIsSelected)
		{
			SelectGame = false;
			recreate = true;
			selectedWorld = savesfolders[selectedIndex].Replace(Application.dataPath + "/saves/", "");
		}

		if(GUI.Button(new Rect(Screen.width*0.8f/2+130, Screen.height*0.9f-65, 100, 50), "Cancelar"))
		{
			SelectGame = false;
			mainmenu = true;
		}

	}

	void CreateWorldMenu(int WindowID)
	{
		GUI.Label (new Rect (Screen.width*0.8f/2-140, 30, 125, 30), "Nombre de la partida:");
		WorldName = GUI.TextField (new Rect (Screen.width*0.8f/2+15, 30, 200, 30), WorldName);

		GUI.Label (new Rect (Screen.width*0.8f/2-140, 75, 125, 30), "Seed:");
		lvlSeed = GUI.TextField (new Rect (Screen.width*0.8f/2+15, 75, 200, 30), lvlSeed);

		if (GUI.Button (new Rect (Screen.width*0.8f/2 - 200, 120, 200, 50), "Crear!")) 
		{

			//CrearPartidaFolder(WorldName);

			if(!Directory.Exists(Application.dataPath + "/saves/"+WorldName)) {
				Directory.CreateDirectory(Application.dataPath + "/saves/"+WorldName);
			} else {
				int times = 0;
				
				foreach (string folder in Directory.GetDirectories (Application.dataPath + "/saves/")) {
					if(folder.Replace(Application.dataPath + "/saves/", "").Substring(folder.Replace(Application.dataPath + "/saves/", "").Length - WorldName.Length) == WorldName) {
						times += 1;
					}
				}

				selectedWorld = "";

				for(int x=0;x<times;x++) {
					selectedWorld += "Copia de ";
				}

				selectedWorld += WorldName;
				
				WorldName = selectedWorld;
				
				Directory.CreateDirectory(Application.dataPath + "/saves/"+WorldName);
			}

			/*INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "lvlName", WorldName);

			INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "playerX", UnityEngine.Random.Range(0, 100).ToString()); //VectorSerializer.Searialize3(player.transform.position)
			INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "playerY", "30");
			INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "playerZ", UnityEngine.Random.Range(0, 100).ToString());

			INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "playerotX", "0");
			INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "playerotY", "0");
			INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "playerotZ", "0");


			Profile.lastPlayedWorld = WorldName;*/

			SaveProfile svProfile = new SaveProfile();

			svProfile.lvlName = WorldName;
			
			svProfile.SetPlayerValues();

			//Profile.SetProperty(ProfileProperty.LastPlayedWorld, WorldName);

			Profile.currentProfile.lastPlayedWorld = WorldName;

			//INI_Manager.Set_Value(Application.dataPath + "/appConfig.cfg", "lastPlayedWorld", WorldName);

			if(!String.IsNullOrEmpty(lvlSeed)) 
			{
				SeedExtension seedExt = new SeedExtension(lvlSeed);
				svProfile.strSeed = lvlSeed;
				svProfile.seed = seedExt.ToInt();
				//INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "seed", seed.ToString());
				//INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "seedKey", lvlSeed);
			} else {
				System.Random rnd = new System.Random ();
				seedBeta = rnd.Next ();
				svProfile.strSeed = seedBeta.ToString();
				svProfile.seed = seedBeta;
				//INI_Manager.Set_Value (Application.dataPath + "/saves/" + WorldName + "/level.dat", "seed", seedBeta.ToString ()); 
				//INI_Manager.Set_Value(Application.dataPath + "/saves/"+WorldName+"/level.dat", "seedKey", seedBeta.ToString());
			}

			XMLTools.SerializeToFile<SaveProfile>(svProfile, Application.dataPath + "/saves/"+WorldName+"/level.xml", true);

			//Debug.Log("Nombre del mundo: " + WorldName + ", Lvl Seed: " + lvlSeed); 

			Profile.Save();

			Application.LoadLevel("Game");

		}

		if(GUI.Button(new Rect(Screen.width*0.8f/2+15, 120, 200, 50), "Cancelar"))
		{
			CreateWorld = false;
			SelectGame = true;
		}

	}


	// Update is called once per frame
	void Update () {

		UnderWaterEffects(personaje, WaterLevel - 0.905f);

		windowRect = new Rect(Screen.width * 0.15f, Screen.height * 0.1f, Screen.width*0.7f, windowRectH(y)*100);
		InputfixPos = new Vector3 (Input.mousePosition.x, Screen.height - Input.mousePosition.y, 0);

		//Debug.Log (Input.GetAxis ("Mouse ScrollWheel"));
		if (SelectGame) {
			if(Input.GetMouseButton(0) && windowRect.Contains(InputfixPos)) {
				selectedIndex = (int)((Screen.height - Input.mousePosition.y - Screen.height*0.1f + MenuvScroll) / 100);
				//Debug.Log ("(" + (Screen.height - Input.mousePosition.y) + " - " + Screen.height*0.1f + " + " + MenuvScroll + ")" + " / 100 = " + selectedIndex);
			}//&& MenuvScroll <= 100*y) { //&& Input.GetAxis ("Mouse ScrollWheel") <= 100*y) {

			MenuvScroll -= Input.GetAxis ("Mouse ScrollWheel")*25;
			//Debug.Log(MenuvScroll);

			if(MenuvScroll < 0) {
				MenuvScroll += 25;
			} else if(MenuvScroll > y * 100) {
				MenuvScroll -= 25;
			}

		}
	}

	public void UnderWaterEffects(GameObject player, float waterLevel) 
	{

		if (player.transform.position.y < waterLevel) {
			SetUnderwater();
		} else {
			SetNormal();	
		}
	
	}

	void SetNormal () 
	{

		BE = personaje.GetComponent<BlurEffect>();

		BE.enabled = false;
		
	}
	
	void SetUnderwater () 
	{

		BE = personaje.GetComponent<BlurEffect>();

		BE.enabled = true;

		
	}

	void LauncherNotFound(DialogResult result)
	{
		if(result == DialogResult.Ok)	
		{	
			Application.Quit();	
		} else {
			Application.Quit();
		}
	}

}

public class Worlds {
	public string path;
	public SaveProfile svProfile;
}
