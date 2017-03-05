using System;
using System.Text;
using System.IO;
// Add references to Soap and Binary formatters. 
using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class MyItemType : ISerializable
{
	public MyItemType()
	{
		// Empty constructor required to compile.
	}
	
	// The value to serialize. 
	private string myProperty_value;
	
	public string MyProperty
	{
		get { return myProperty_value; }
		set { myProperty_value = value; }
	}
	
	// Implement this method to serialize data. The method is called  
	// on serialization. 
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		// Use the AddValue method to specify serialized values.
		info.AddValue("props", myProperty_value, typeof(string));
		
	}
	
	// The special constructor is used to deserialize values. 
	public MyItemType(SerializationInfo info, StreamingContext context)
	{
		// Reset the property value using the GetValue method.
		myProperty_value = (string) info.GetValue("props", typeof(string));
	}
}

// This is a console application.  
public static class Serializer
{
	/*static void Serialize()
	{
		// This is the name of the file holding the data. You can use any file extension you like. 
		string fileName = "dataStuff.myData";
		
		// Use a BinaryFormatter or SoapFormatter.
		IFormatter formatter = new BinaryFormatter();
		//IFormatter formatter = new SoapFormatter();

		Serializer.SerializeItem(fileName, formatter); // Serialize an instance of the class.
		Serializer.DeserializeItem(fileName, formatter); // Deserialize the instance.
		Console.WriteLine("Done");
		Console.ReadLine();
	}*/
	
	public static string SerializeItem(string ToSerialize)
	{
		// Create an instance of the type and serialize it.
		MyItemType t = new MyItemType();
		t.MyProperty = ToSerialize;

		using (MemoryStream ms = new MemoryStream())
		{
			new BinaryFormatter().Serialize(ms, t);
			return Convert.ToBase64String(ms.ToArray());

		}        

	}
	
	
	public static object DeserializeItem(string ToDeserialize)
	{

		using(MemoryStream s = new MemoryStream(Convert.FromBase64String(ToDeserialize))) { 
			return new BinaryFormatter().Deserialize(s); 
		}
	}       
}

/*public class VectorSerializer {

	static int first = 0;
	static int last = 0;

	public static string Searialize2(Vector2 vector2) {

		try {
			
			return "[X:" + vector2.x + ",Y:" + vector2.y + "]";
				
		} catch (Exception ex) {
			UnityEngine.Debug.Log(ex.Message);
		}

		return null;
		
	}

	public static string Searialize3(Vector3 vector3) {

		try {

			return "[X:" + vector3.x + ",Y:" + vector3.y + ",Z:" + vector3.z + "]";
			
		} catch (Exception ex) {
			UnityEngine.Debug.Log(ex.Message);
		}

		return null;

	}

	public static string Searialize4(Vector4 vector4) {

		try {
			
			return "[W:" + vector4.w + ",X:" + vector4.x + ",Y:" + vector4.y + ",Z:" + vector4.z + "]";

		} catch (Exception ex) {
			UnityEngine.Debug.Log(ex.Message);
		}

		return null;
		
	}

	public static Vector3 Unserialize3(string serialized) {

			int X = 0;
			int Y = 0;
			int Z = 0;
			
			if(!serialized.StartsWith("[X:")) {

				throw new Exception("That isn't a Vector3.");

				return new Vector3(0, 0, 0);

			} 

			first = serialized.IndexOf("[X:") + "[X:".Length;
			last = serialized.LastIndexOf(",Y");

			X = Convert.ToInt32(serialized.Substring(first, last - first));

			first = serialized.IndexOf(",Y:") + ",Y:".Length;
			last = serialized.LastIndexOf(",Z");

			Y = Convert.ToInt32(serialized.Substring(first, last - first));

			first = serialized.IndexOf(",Z:") + ",Z:".Length;
			last = serialized.LastIndexOf("]");

			Z = Convert.ToInt32(serialized.Substring(first, last - first));

			Vector3 result = new Vector3(X, Y, Z);

			return result;

	}

}*/
