using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

public class XMLTools {

	public static string Serialize<T>(T value, bool indented = false)
	{
		if (value == null) {
			Debug.LogError("XMLSerializer - The value passed is null!");
			return "";
		}

		try {
			XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
			string serializeXml = "";

			using (StringWriter stringWriter = new StringWriter()) {

				using (XmlWriter writer = XmlWriter.Create(stringWriter)) {
					xmlserializer.Serialize(writer, value);
					serializeXml = stringWriter.ToString();
				}

				if (indented) {
					serializeXml = Beautify(serializeXml);
				}

			}

			return serializeXml;
		} catch(Exception ex) {
			Debug.LogError(ex.Message);
			return "";
		}
	}

	public static T Deserialize<T>(string value)
	{

		try {
			object returnvalue = new object();
			XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
			TextReader reader = new StringReader(value);

			returnvalue = xmlserializer.Deserialize(reader);

			reader.Close();
			return (T)returnvalue;
		} catch(Exception ex) {
			Debug.LogError(ex.Message);
			return default(T);
		}

	}

	public static void SerializeToFile<T>(T value, string filePath, bool indented = false)
	{
		if (value == null) {
			Debug.LogError("XMLSerializer - The value passed is null!");
		}
		try {
			XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
			using (StreamWriter fileWriter = new StreamWriter(filePath)) {
				if (indented) {
					using (StringWriter stringWriter = new StringWriter()) {
						using (XmlWriter writer = XmlWriter.Create(stringWriter)) {
							xmlserializer.Serialize(writer, value);
							fileWriter.WriteLine(Beautify(stringWriter.ToString()));
						}
					}
				} else {
					using (XmlWriter writer = XmlWriter.Create(fileWriter)) {
						xmlserializer.Serialize(writer, value);
					}
				}
			}

		} catch(Exception ex) {
			Debug.LogError(ex.Message);
		}
	}

	public static T DeserializeFromFile<T>(string filePath)
	{

		try {
			object returnvalue = new object();
			XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
			using (TextReader reader = new StreamReader(filePath)) {
				returnvalue = xmlserializer.Deserialize(reader);
			}
			return (T)returnvalue;
		} catch(Exception ex) {
			Debug.LogError(ex.Message);
			return default(T);
		}

	}

	public static string Beautify(object obj)
	{
		XmlDocument doc = new XmlDocument();
		if(obj.GetType() == typeof(string)) {
			if(!String.IsNullOrEmpty((string)obj)) {
				try {
					doc.LoadXml((string)obj);
				}catch(Exception ex) {
					Debug.Log("XMLIndenter - Wrong string format! ["+ex.Message+"]");
					return "";
				}
			} else {
				Debug.LogError("XMLIndenter - String is null!");
				return "";
			}
		} else if(obj.GetType() == typeof(XmlDocument)) {
			doc = (XmlDocument)obj;
		} else {
			Debug.LogError("XMLIndenter - Not supported type!");
			return "";
		}
		MemoryStream w = new MemoryStream();
		XmlTextWriter writer = new XmlTextWriter(w, Encoding.Unicode);
		writer.Formatting = Formatting.Indented;
		doc.WriteContentTo(writer);

		writer.Flush();
		w.Seek(0L, SeekOrigin.Begin);

		StreamReader reader = new StreamReader(w);
		return reader.ReadToEnd();
	}

}
