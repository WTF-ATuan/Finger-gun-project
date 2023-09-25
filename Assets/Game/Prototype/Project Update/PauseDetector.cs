using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Prototype.Project_Update{
	public class PauseDetector : MonoBehaviour{
		[SerializeField] private UnityEvent onPause;

		private void Start(){
			OVRManager.VrFocusLost += () => onPause?.Invoke();
			OVRManager.InputFocusLost += () => onPause?.Invoke();
			OVRManager.TrackingLost += () => onPause?.Invoke();
			OVRManager.HMDUnmounted += () => onPause?.Invoke();
			OVRManager.HMDLost += () => onPause?.Invoke();
		}
	}
}