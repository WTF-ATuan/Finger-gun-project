using UnityEngine;

namespace Game.Prototype.Project_Update{
	public class ProjectUpdater : MonoBehaviour{
		private const string
				GithubApiUrl =
						"https://api.github.com/repos/WTF-ATuan/Finger-gun-project/releases/latest";

		private const string ReleaseTag = "Testing Stone Boss V0.9"; 
		private void Start(){
			StartCoroutine(WebSeverAdapter.GetReleaseInfo(GithubApiUrl, CheckReleaseVersion));
		}
		
		private void CheckReleaseVersion(ReleaseInfo result){
			if(result.tag_name != ReleaseTag){
				
			}
		}
	}
}