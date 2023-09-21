using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Prototype.Project_Update{
	public static class WebSeverAdapter{
		public static IEnumerator GetReleaseInfo(string githubApiUrl, Action<ReleaseInfo> result){
			using var webRequest = UnityWebRequest.Get(githubApiUrl);
			webRequest.SetRequestHeader("Accept", "application/vnd.github.v3+json");
			yield return webRequest.SendWebRequest();

			if(webRequest.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError){
				Debug.LogError("Error getting release info: " + webRequest.error);
			}
			else{
				var response = webRequest.downloadHandler.text;
				var releaseInfo = JsonUtility.FromJson<ReleaseInfo>(response);
				result?.Invoke(releaseInfo);
			}
		}

		// public static IEnumerator DownloadAPK(string apkURL, string apkName, Action<string> result){
		// 	var request = UnityWebRequest.Get(apkURL);
		// 	request.downloadHandler = new DownloadHandlerBuffer();
		//
		// 	yield return request.SendWebRequest();
		//
		// 	if(request.result != UnityWebRequest.Result.Success){
		// 		Debug.LogError("Error downloading APK: " + request.error);
		// 	}
		// 	else{
		// 		var apkData = request.downloadHandler.data;
		// 		// Save the downloaded APK to a file
		// 		var savePath = "/sdcard/Download/" + $"/{apkName}.apk";
		// 		File.WriteAllBytes(savePath, apkData);
		// 		result?.Invoke(savePath);
		// 	}
		// }
	}
}