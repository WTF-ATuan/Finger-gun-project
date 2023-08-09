using System;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game.Prototype.Bomb{
	public class BombBinder : MonoBehaviour{
		[SerializeField] private GameObject bombPrefab;
		[SerializeField] private Transform bombSpawnPoint;
		[SerializeField] private GameObject explosionVFX;

		private Vector3 _previousFrameControllerPosition;
		private GameObject _bombClone;


		public void Update(){
			var activeBomb = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);
			var rightControllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
			var controllerOffset = rightControllerPosition - _previousFrameControllerPosition;
			var controllerSpeed = controllerOffset.magnitude / Time.fixedDeltaTime;
			_previousFrameControllerPosition = rightControllerPosition;
			if(activeBomb){
				if(!_bombClone){
					_bombClone = Instantiate(bombPrefab, bombSpawnPoint.position, bombSpawnPoint.rotation,
						bombSpawnPoint);
				}

				var targetPosition = SelectTarget();
				_bombClone.OnCollisionEnterAsObservable().Subscribe(BombCloneHit);
				BombMovement(controllerSpeed, controllerOffset, targetPosition);
			}
			else{
				if(!_bombClone || _bombClone.transform.parent != bombSpawnPoint) return;
				DestroyImmediate(_bombClone);
			}
		}

		private Vector3 SelectTarget(){
			var sphereCast = Physics.SphereCast(new Ray(bombSpawnPoint.position, bombSpawnPoint.forward), 3,
				out var hitInfo);
			return sphereCast ? hitInfo.point : Vector3.zero;
		}

		private void BombCloneHit(Collision obj){
			if(obj.gameObject.TryGetComponent<Projectile>(out var projectile)){
				Destroy(_bombClone);
				var vfxClone = Instantiate(explosionVFX, obj.GetContact(0).point,
					Quaternion.Euler(obj.GetContact(0).normal));
				Destroy(vfxClone, 1.5f);
			}

			if(obj.gameObject.CompareTag("Concrete")){
				var bombRigid = _bombClone.GetComponent<Rigidbody>();
				bombRigid.velocity = Vector3.zero;
				_bombClone.transform.eulerAngles = -obj.GetContact(0).normal;
			}
		}

		private void BombMovement(float controllerSpeed, Vector3 controllerOffset, Vector3 targetPosition){
			if(controllerSpeed < 1f) return;
			_bombClone.transform.SetParent(null);
			var bombRigid = _bombClone.GetComponent<Rigidbody>();
			if(targetPosition == Vector3.zero){
				bombRigid.AddForce(controllerOffset * 100, ForceMode.Impulse);
			}
			else{
				bombRigid.DOPath(GenerateCurvePathPoints(targetPosition, controllerOffset), 1f);
			}
		}


		private Vector3[] GenerateCurvePathPoints(Vector3 targetPosition, Vector3 controllerOffset){
			const int numPoints = 50;
			const int height = 100;
			var pathPoints = new Vector3[numPoints + 2]; // 加2是为了包括起始点和目标位置
			pathPoints[0] = transform.position;
			pathPoints[numPoints + 1] = targetPosition;
			for(var i = 1; i <= numPoints; i++){
				var t = i / (float)(numPoints + 1);
				var offset = height * (t - t * t); // 控制曲线的高度

				pathPoints[i] = Vector3.Lerp(pathPoints[0], pathPoints[numPoints + 1], t) +
								controllerOffset * offset;
			}

			return pathPoints;
		}
	}
}