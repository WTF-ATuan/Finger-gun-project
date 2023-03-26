using System;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Prototype{
	public class PoseEventHandler : MonoBehaviour{
		[SerializeField, Interface(typeof(ISelector))]
		private MonoBehaviour fingerGunPoseWrapper;

		private ISelector _fingerGunPoseSelector;

		[SerializeField, Interface(typeof(ISelector))]
		private MonoBehaviour triggerPoseWrapper;

		private ISelector _triggerPoseSelector;

		/// <summary>
		/// State 
		/// </summary>
		private bool _gunPose;

		private bool _trigger;

		private void Start(){
			_fingerGunPoseSelector = fingerGunPoseWrapper as ISelector;
			_triggerPoseSelector = triggerPoseWrapper as ISelector;
			this.AssertField(_fingerGunPoseSelector, nameof(_fingerGunPoseSelector));
			this.AssertField(_triggerPoseSelector, nameof(_triggerPoseSelector));
			if(_fingerGunPoseSelector != null){
				_fingerGunPoseSelector.WhenSelected += () => FingerGunPoseActive(true);
				_fingerGunPoseSelector.WhenUnselected += () => FingerGunPoseActive(false);
			}

			if(_triggerPoseSelector != null){
				_triggerPoseSelector.WhenSelected += () => TriggerPoseActive(true);
				_triggerPoseSelector.WhenUnselected += () => TriggerPoseActive(false);
			}
		}

		private void FingerGunPoseActive(bool active){
			_gunPose = active;
		}

		private void TriggerPoseActive(bool active){
			_trigger = active;
		}

		private void Update(){
			
		}
	}
}