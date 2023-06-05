using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Prototype.Pistol{
	public class PistolRecoil : MonoBehaviour{
		private Vector3 _originRotationEuler;
		private Vector3 _currentRotationEuler;
		private Vector3 _targetRotationEuler;

		[SerializeField] private Vector3 recoilRotate;
		[SerializeField] private float snappiness;
		[SerializeField] private float returnSpeed;


		private void Start(){
			_originRotationEuler = transform.eulerAngles;
		}

		private void Update(){
			_targetRotationEuler =
					Vector3.Lerp(_targetRotationEuler, _originRotationEuler, returnSpeed * Time.deltaTime);
			_currentRotationEuler = Vector3.Slerp(_currentRotationEuler, _targetRotationEuler,
				snappiness * Time.fixedDeltaTime);
			transform.localRotation = Quaternion.Euler(_currentRotationEuler);
		}

		public void Recoil(){
			_targetRotationEuler += new Vector3(recoilRotate.x, Random.Range(-recoilRotate.y, recoilRotate.y),
				Random.Range(-recoilRotate.z, recoilRotate.z));
		}
	}
}