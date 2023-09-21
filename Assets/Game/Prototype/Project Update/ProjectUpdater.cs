using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Prototype.Project_Update{
	public class ProjectUpdater : MonoBehaviour{
		private const string GithubApiUrl = "https://api.github.com/repos/WTF-ATuan/Finger-gun-project/releases/latest";

		public string releaseTag = "Testing Stone Boss V0.9";
		public UnityEvent<string> onNewVersion;
		public UnityEvent<string> onDetectNewVersion;
		public UnityEvent onDownloadFinish;
		private ReleaseInfo _infoTemp;

		private void Start(){
			StartCoroutine(WebSeverAdapter.GetReleaseInfo(GithubApiUrl, CheckReleaseVersion));
		}

		public void DownloadNewVersion(){
			//because upload app lab will not allow to access write and read;
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