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

		public static IEnumerator DownloadAPK(string apkURL, string apkName, Action<string> result){
			var request = UnityWebRequest.Get(apkURL);
			request.downloadHandler = new DownloadHandlerBuffer();

			yield return request.SendWebRequest();

			if(request.result != UnityWebRequest.Result.Success){
				Debug.LogError("Error downloading APK: " + request.error);
			}
			else{
				var apkData = request.downloadHandler.data;
				// Save the downloaded APK to a file
				var savePath = Application.persistentDataPath + $"/{apkName}.apk";
				File.WriteAllBytes(savePath, apkData);
				result?.Invoke(savePath);
			}
		}

		public static void InstallNewAPK(string apkFilePath){
			try{
				var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
				var intentClass = new AndroidJavaClass("android.content.Intent");
				var intent = new AndroidJavaObject("android.content.Intent",
					intentClass.GetStatic<string>("ACTION_VIEW"));

				// 获取 FileProvider 的 URI
				var file = new AndroidJavaObject("java.io.File", apkFilePath);
				var unityContext = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
				var packageName = unityContext.Call<string>("getPackageName");
				var fileProvider = new AndroidJavaClass("androidx.core.content.FileProvider");
				var uri = fileProvider.CallStatic<AndroidJavaObject>("getUriForFile", unityContext,
					packageName + ".fileprovider", file);

				var mimeType = new AndroidJavaObject("java.lang.String", "application/vnd.android.package-archive");
				intent.Call<AndroidJavaObject>("setDataAndType", uri, mimeType);
				intent.Call<AndroidJavaObject>("addFlags", intentClass.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK"));
				intent.Call<AndroidJavaObject>("addFlags",
					intentClass.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION")); // 授予读取 URI 的权限
				currentActivity.Call("startActivity", intent);

				GameObject.Find("TextDebug").GetComponent<TMP_Text>().text = "Success";
			}
			catch(Exception exception){
				GameObject.Find("TextDebug").GetComponent<TMP_Text>().text = "Error: " + exception.Message;
				throw;
			}
		}

		public static void ActiveApp(string packageName){
			var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			var packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");

			try{
				var launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", packageName);
				if(launchIntent != null){
					currentActivity.Call("startActivity", launchIntent);
				}
				else{
					Debug.LogError("App not found or cannot be launched.");
				}
			}
			catch(Exception exception){
				Debug.LogError("Error starting app: " + exception.ToString());
			}
		}
	}
}