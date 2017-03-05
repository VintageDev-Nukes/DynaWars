using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DataItem<DKey, DValue> {

	public DataItem() {}

	public DataItem(DKey key, DValue value) {
		this.Key = key;
		this.Value = value;
	}

	public DKey Key;
	
	public DValue Value;

	public static List<DataItem<DKey, DValue>> ToList(Dictionary<DKey, DValue> serDict) {
		List<DataItem<DKey, DValue>> tempDataList = new List<DataItem<DKey, DValue>>();
		foreach(DKey key in serDict.Keys)
		{
			tempDataList.Add(new DataItem<DKey, DValue>(key, serDict[key]));
		}
		return tempDataList;
	}

	public static Dictionary<DKey, DValue> ToDictionary(List<DataItem<DKey, DValue>> serList) {
		Dictionary<DKey, DValue> tempDataDict = new Dictionary<DKey, DValue>();
		foreach(DataItem<DKey, DValue> listDI in serList) {
			if(!tempDataDict.ContainsKey(listDI.Key)) {
				tempDataDict.Add(listDI.Key, listDI.Value);
			}
		}
		return tempDataDict;
	}

}
