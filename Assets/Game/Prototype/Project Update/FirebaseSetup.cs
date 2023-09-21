using System;
using Firebase;
using UnityEngine;

namespace Game.Prototype.Project_Update{
	public class FirebaseSetup : MonoBehaviour{
		private FirebaseApp _app;
		private void Start(){
			FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
				var dependencyStatus = task.Result;
				if (dependencyStatus == DependencyStatus.Available) {
					_app = FirebaseApp.DefaultInstance;
				} else {
					Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
				}
			});
		}
	}
}