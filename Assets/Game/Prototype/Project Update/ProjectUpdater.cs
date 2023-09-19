using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Prototype.Project_Update{
	public class ProjectUpdater : MonoBehaviour{
		private const string GithubApiUrl = "https://api.github.com/repos/WTF-ATuan/Finger-gun-project/releases/latest";

		private const string PackageName = "com.Atuan.StoneBossTest";

		public string releaseTag = "Testing Stone Boss V0.9";
		public UnityEvent<string> onNewVersion;
		public UnityEvent<string> onDetectNewVersion;
		public UnityEvent onDownloadFinish;
		private ReleaseInfo _infoTemp;

		private Firebase.FirebaseApp _app;
		private void Start(){
			StartCoroutine(WebSeverAdapter.GetReleaseInfo(GithubApiUrl, CheckReleaseVersion));
			Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
				var dependencyStatus = task.Result;
				if (dependencyStatus == Firebase.DependencyStatus.Available) {
					// Create and hold a reference to your FirebaseApp,
					// where app is a Firebase.FirebaseApp property of your application class.
					_app = Firebase.FirebaseApp.DefaultInstance;
					// Set a flag here to indicate whether Firebase is ready to use by your app.
					Firebase.Analytics.FirebaseAnalytics.LogEvent("player_death", "amount", "0");
					Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventTutorialBegin);

				} else {
					UnityEngine.Debug.LogError(System.String.Format(
						"Could not resolve all Firebase dependencies: {0}", dependencyStatus));
					// Firebase Unity SDK is not safe to use here.
				}
			});
			
		}
		[Button]
		public void DownloadNewVersion(){
			StartCoroutine(WebSeverAdapter.DownloadAPK(_infoTemp.apk_url, _infoTemp.tag_name, DownloadComplete));
		}

		private void CheckReleaseVersion(ReleaseInfo result){
			if(result.tag_name != releaseTag){
				onNewVersion?.Invoke(result.tag_name);
				_infoTemp = result;
			}
			else{
				Debug.Log("Your version is newest");
			}
			onDetectNewVersion?.Invoke(releaseTag);
		}

		private void DownloadComplete(string apkPath){
			onDownloadFinish?.Invoke();
		}
	}
}