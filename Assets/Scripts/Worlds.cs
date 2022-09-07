using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public class Worlds {
	private static void ClearFile(string p) {
		if (!File.Exists(p)) {File.Create(p);}
		using (TextWriter tw = new StreamWriter(p, false)) {
			tw.Write(string.Empty);
			tw.Close();
		}
	}
	public static string Save() {
		string path = Application.streamingAssetsPath + "/worlds/world.w";
		ClearFile(path);
		Transform a = GameObject.FindGameObjectsWithTag("Player")[0].transform;
		string data = $"Player;{a.position.x};{a.position.y};{a.position.z};{a.eulerAngles.x};{a.eulerAngles.y};{a.eulerAngles.z}|";
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("cell")) {
			if(obj.name != "ground" && obj.name != "inventoryCell") {
				string name = obj.name.Split("(Clone)")[0];
				data += $"{name};";
				data += $"{obj.transform.position.x};";
				data += $"{obj.transform.position.y};";
				data += $"{obj.transform.position.z};";
				
				data += $"{obj.transform.eulerAngles.x};";
				data += $"{obj.transform.eulerAngles.y};";
				data += $"{obj.transform.eulerAngles.z}|";
			}
		}
		using (StreamWriter writer = new StreamWriter(path, true)) {
			writer.Write(data);
			writer.Close();
		}
		return data;
	}
	public static void Load() {
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("cell")) {if(obj.name != "ground" && obj.name != "inventoryCell") {Object.Destroy(obj);}}
		string[] output = GUIUtility.systemCopyBuffer.Split('|');
		foreach (string o in output) {
			string[] oo = o.Split(';');
			if(oo[0] != "") {
				string name = oo[0];
				float x 	= float.Parse(oo[1]);
				float y 	= float.Parse(oo[2]);
				float z 	= float.Parse(oo[3]);
				if (name != "Player") {
					GameObject a = Object.Instantiate(UnityEngine.Resources.Load(name), new Vector3(x,y,z), Quaternion.identity) as GameObject;
					a.transform.rotation = Quaternion.Euler(float.Parse(oo[4]),float.Parse(oo[5]),float.Parse(oo[6]));
				} else {
					GameObject.FindGameObjectsWithTag("Player")[0].transform.position = new Vector3(x, y, z);
					GameObject.FindGameObjectsWithTag("Player")[0].transform.rotation = Quaternion.Euler(float.Parse(oo[4]),float.Parse(oo[5]),float.Parse(oo[6]));
				}
				//Debug.Log($"{name} _ {x} _ {y} _ {z}");
			}
		}
	}
}
public static class Extensions {
    public static string[] Split(this string toSplit, string splitOn) {
        return toSplit.Split(new string[] { splitOn }, System.StringSplitOptions.None);
    }
}