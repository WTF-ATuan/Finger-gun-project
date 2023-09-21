using Firebase;
using UnityEngine;

namespace Game.Prototype.Project_Update{
	public class FirebaseSetup : MonoBehaviour{
		private FirebaseApp _app;

		private void Start(){
			FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
				var dependencyStatus = task.Result;
				if(dependencyStatus == DependencyStatus.Available){
					_app = FirebaseApp.DefaultInstance;
				}
				else{
					Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
				}
			});
		}

		public void PlayerDeadEvent(){
			Firebase.Analytics.FirebaseAnalytics.LogEvent("player_dead");
		}

		public void PassStageEvent(int stageCount = 1){
			Firebase.Analytics.FirebaseAnalytics.LogEvent("pass_stage", "stage", $"{stageCount}");
		}

		public void BeatBossEvent(){
			Firebase.Analytics.FirebaseAnalytics.LogEvent("beat_boss");
		}

		public void ReplayEvent(){
			Firebase.Analytics.FirebaseAnalytics.LogEvent("replay_game");
		}
	}
}