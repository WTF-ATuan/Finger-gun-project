using UnityEngine;
using UnityEngine.Events;

namespace Game.Prototype.Project_Update{
	public class ProjectUpdater : MonoBehaviour{
		private const string
				GithubApiUrl =
						"https://api.github.com/repos/WTF-ATuan/Finger-gun-project/releases/latest";

		public string releaseTag = "Testing Stone Boss V0.9";
		public UnityEvent onNewVersion;
		private ReleaseInfo _infoTemp;

		private void Start(){
			StartCoroutine(WebSeverAdapter.GetReleaseInfo(GithubApiUrl, CheckReleaseVersion));
		}

		public void DownloadNewVersion(){
			StartCoroutine(WebSeverAdapter.DownloadAPK(_infoTemp.apk_url, _infoTemp.tag_name, InstallNewVersion));
		}

		private void CheckReleaseVersion(ReleaseInfo result){
			if(result.tag_name != releaseTag){
				onNewVersion?.Invoke();
				_infoTemp = result;
			}
			else{
				Debug.Log("Your version is newest");
			}
		}

		private void InstallNewVersion(string apkPath){
			WebSeverAdapter.InstallNewAPK(apkPath);
		}
	}
}