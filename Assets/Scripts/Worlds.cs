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
				string name = obj.GetComponent<Cell>().cellType.ToString();
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
				string name = oo[0].ToUpper().Replace("IMMOBILE", "WALL").Replace("FIXED ROTATOR", "FIXEDROTATOR");
				float x 	= float.Parse(oo[1]);
				float y 	= float.Parse(oo[2]);
				float z 	= float.Parse(oo[3]);
				if (oo[4] == "270") {
					oo[4] = "-90";
				}
				if (name != "PLAYER") {// GameObject c, Vector3 position, Vector3 rotation, Direction_e rotation2, bool generated
					/*GameObject a = */GridManager.instance.SpawnCell(CellFunctions.StringToCellType_e(name), new Vector3(x,y,z), new Vector3(float.Parse(oo[4]),float.Parse(oo[5]),float.Parse(oo[6])), CellFunctions.Vector3ToDirection_e(new Vector3(float.Parse(oo[4]),float.Parse(oo[5]),float.Parse(oo[6]))), false)/* as GameObject*/;
					//a.transform.rotation = Quaternion.Euler(float.Parse(oo[4]),float.Parse(oo[5]),float.Parse(oo[6]));
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
/*
Player;47,38988;5,48;77,95635;0;273,9663;0|MOVER;52;5;87;0;0;0|MOVER;49;5;85;0;0;0|MOVER;48;5;82;0;0;0|MOVER;51;5;77;0;0;0|MOVER;55;5;76;0;0;0|MOVER;58;5;75;0;0;0|MOVER;63;5;74;0;0;0|MOVER;65;5;74;0;0;0|GENERATOR;70;5;75;0;0;0|FIXEDROTATOR;70;5;73;0;0;0|FIXEDROTATOR;69;5;71;0;0;0|FIXEDROTATOR;68;5;71;0;0;0|FIXEDROTATOR;64;5;70;0;0;0|FIXEDROTATOR;65;5;70;0;0;0|FIXEDROTATOR;64;5;69;0;0;0|FIXEDROTATOR;65;5;69;0;90;0|WALL;60;5;68;90;0;0|WALL;61;5;68;0;180;0|WALL;62;5;68;0;180;0|WALL;63;5;68;0;270;0|MOVER;45;5;72;270;0;0|MOVER;45;5;73;270;0;0|MOVER;45;5;74;270;0;0|MOVER;45;6;73;270;0;0|MOVER;45;7;73;270;0;0|MOVER;39;5;76;270;0;0|MOVER;39;5;77;0;0;0|MOVER;39;5;78;0;90;0|MOVER;39;5;79;90;0;0|MOVER;39;5;80;0;180;0|MOVER;39;5;81;0;270;0|
Player;56,58104;5,48;80,60558;0;1,606082;0|MOVER;57;5;84;270;0;0|
*/