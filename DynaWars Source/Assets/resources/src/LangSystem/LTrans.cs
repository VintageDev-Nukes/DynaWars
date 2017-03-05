using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Langs {ES, EN, FR, DE}

public class LangMotor {
	public static void Load(ref Languages langs) {
		//langs = Deserialize(Langs.xml);
	}
}

public class Languages {
	public List<LTrans> DataBase;
}

public class LTrans {

	public Langs currentLang;

	public string str1 = "Hi!/Hola!/Salut!";

}
