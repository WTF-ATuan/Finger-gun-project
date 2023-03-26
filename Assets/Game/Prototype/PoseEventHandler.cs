using Oculus.Interaction;
using UnityEngine;

namespace Game.Prototype{
	public class PoseEventHandler : MonoBehaviour{
		[SerializeField, Interface(typeof(ISelector))]
		private MonoBehaviour fingerGunPoseWrapper;

		[SerializeField, Interface(typeof(ISelector))]
		private MonoBehaviour triggerPoseWrapper;

		private ISelector _fingerGunPoseSelector;
		private ISelector _triggerPoseSelector;
		private bool _gunPose;
		private bool _trigger;

		private void Start(){
			_fingerGunPoseSelector = fingerGunPoseWrapper as ISelector;
			_triggerPoseSelector = triggerPoseWrapper as ISelector;
			if(_fingerGunPoseSelector != null){
				_fingerGunPoseSelector.WhenSelected += () => { _gunPose = true; };
			}

			if(_triggerPoseSelector != null){
				_triggerPoseSelector.WhenSelected += () => { _trigger = true; };
				_triggerPoseSelector.WhenUnselected += () => { _trigger = false; };
			}
		}

		private void Update(){
			if(_gunPose && _trigger){
				Debug.Log($"Fire");
			}
		}
	}
}