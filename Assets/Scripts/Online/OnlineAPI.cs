using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class OnlineAPI
{
	public static string Server = "https://3dms.deta.dev";
	public static string Return = "";
    public static IEnumerator NewLevel(string levelName, string level, string author) {
        UnityWebRequest www = UnityWebRequest.Get($"{Server}/newLevel?levelName={levelName}&level={level}&author={author}");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.LogError(www.error);
        } else {
            Debug.Log(www.downloadHandler.text);
			Return = www.downloadHandler.text;
        }
    }
	public static IEnumerator GetLevel(int id) {
		UnityWebRequest www = UnityWebRequest.Get($"{Server}/getLevel?levelId={id}");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError) {
            Debug.LogError(www.error);
        } else {
            Debug.Log(www.downloadHandler.text);
			Return = www.downloadHandler.text;
        }
	}
}