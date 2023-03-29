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

		[SerializeField] private Transform indexPoint;
		[SerializeField] private GameObject projectile;
		
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

		private bool _triggerDownFlag;

		private void Update(){
			if(!_trigger) _triggerDownFlag = false;
			if(_gunPose && _trigger && !_triggerDownFlag){
				_triggerDownFlag = true;
				Fire();
			}
		}

		private void Fire(){
			var forward = indexPoint.forward;
			var bullet = Instantiate(projectile, indexPoint.position, Quaternion.Euler(forward));
			bullet.GetComponent<Rigidbody>().AddForce(forward * 1000);
		}
	}
}