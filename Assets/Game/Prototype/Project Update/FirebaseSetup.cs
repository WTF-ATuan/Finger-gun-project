using Firebase;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Prototype.Project_Update{
	public class FirebaseSetup : MonoBehaviour{
		private FirebaseApp _app;

		[SerializeField] private Text debugText;

		private void Start(){
			FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
				var status = task.Result;
				if(status == DependencyStatus.Available){
					_app = FirebaseApp.DefaultInstance;
					FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
					FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventTutorialBegin);
					FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventBeginCheckout);
					debugText.text = $"{_app.Options.ProjectId}";
				}
				else{
					debugText.text = $"Could not resolve all Firebase dependencies: {status}";
					Debug.LogError($"Could not resolve all Firebase dependencies: {status}");
				}
			});
		}

		public void PlayerDeadEvent(){
			FirebaseAnalytics.LogEvent("player_dead");
		}

		public void PassStageEvent(int stageCount = 1){
			FirebaseAnalytics.LogEvent("pass_stage", "stage", $"{stageCount}");
		}

		public void BeatBossEvent(){
			FirebaseAnalytics.LogEvent("beat_boss");
		}

		public void ReplayEvent(){
			FirebaseAnalytics.LogEvent("replay_game");
		}
	}
}