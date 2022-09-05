using UnityEditor;
using UnityEngine;

public class Tools : EditorWindow
{
	string cellName = "default";
	GameObject cellPrefab;
	
	[MenuItem("Tools/Cell/Create Cell")]
    public static void ShowWindow() {
		GetWindow(typeof(Tools));
	}
	
	private void OnGUI() {
		GUILayout.Label("Create Cell", EditorStyles.boldLabel);
		cellName = EditorGUILayout.TextField("Cell Name", cellName);
		cellPrefab = EditorGUILayout.ObjectField("Cell Prefab", cellPrefab, typeof(GameObject), false) as GameObject;
		
		if (GUILayout.Button("Create Cell")) {
			CreateCell(cellName, cellPrefab);
		}
	}
	private void CreateCell(string name, GameObject cell) {
		Debug.Log("cell");
	}
}
