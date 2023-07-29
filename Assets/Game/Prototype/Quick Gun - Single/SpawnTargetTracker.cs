using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core{
	public class SpawnTargetTracker : MonoBehaviour{
		[TitleGroup("Target Setting")] [EnumToggleButtons]
		public TargetType targetType;

		[ShowIf("IsTransformType")] [TitleGroup("Target Setting")]
		public Transform targetTransform;

		[ShowIf("IsTransformType")] [TitleGroup("Target Setting")]
		public Vector3 targetOffset;

		[ShowIf("IsVectorType")] [TitleGroup("Target Setting")]
		public Vector3 targetVector;

		[TitleGroup("Target Setting")] public Vector3 targetBoxSize = Vector3.one;

		[TitleGroup("Movement Setting")] [EnumToggleButtons]
		public MovementType movementType = MovementType.Linear;

		[TitleGroup("Movement Setting")] [ShowIf("@movementType", MovementType.Curve)]
		public float height = 5;

		[TitleGroup("Movement Setting")] [ShowIf("@movementType", MovementType.Curve)]
		public Vector3 direction = Vector3.up;

		[TitleGroup("Duration Setting")] [EnumToggleButtons]
		public RotationType rotateType = RotationType.FaceTarget;

		[TitleGroup("Duration Setting")] public float during = 2;
		[TitleGroup("Duration Setting")] public AnimationCurve movingCurve = AnimationCurve.Linear(0, 0, 1, 1);

		[TitleGroup("Complete Action")] [EnumToggleButtons]
		public CompleteAction completeAction;

		[TitleGroup("Complete Action")] [ShowIf("IsDelayAction")]
		public float delayTime = 1f;

		[TitleGroup("Debug")] public bool debugLine;
		[TitleGroup("Debug")] public Color debugColor = Color.green;

		private ISpawner _spawner;

		private void OnEnable(){
			_spawner = GetComponent<ISpawner>();
			if(_spawner == null) throw new Exception($"Can,t Get Spawner in {gameObject}");
			_spawner.OnSpawn += OnSpawn;
		}

		private void OnDisable(){
			_spawner.OnSpawn -= OnSpawn;
		}

		private void OnSpawn(GameObject obj){
			var objTransform = obj.transform;
			var targetPosition = GetTargetPosition();
			var targetRandomPosition = GetTargetRandomPosition(targetPosition);
			var currentScale = objTransform.localScale;
			obj.transform.DOScale(Vector3.zero, 0f);
			obj.transform.DOScale(currentScale, 0.5f);
			SetRotation(objTransform, targetRandomPosition);
			DoMovement(obj, targetRandomPosition);
		}

		private void DoMovement(GameObject obj, Vector3 targetRandomPosition){
			switch(movementType){
				case MovementType.Linear:
					obj.transform.DOMove(targetRandomPosition, during)
							.SetEase(movingCurve)
							.OnComplete(() => OnCompleteMoving(obj));
					break;
				case MovementType.Curve:
					var pathPoints = GenerateCurvePathPoints(targetRandomPosition);
					obj.transform.DOPath(pathPoints, during, PathType.CatmullRom, PathMode.Full3D, 10, debugColor)
							.SetEase(movingCurve)
							.OnComplete(() => OnCompleteMoving(obj));
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void SetRotation(Transform objTransform, Vector3 targetRandomPosition){
			switch(rotateType){
				case RotationType.Default:
					break;
				case RotationType.Zero:
					objTransform.rotation = Quaternion.identity;
					break;
				case RotationType.FaceTarget:
					objTransform.DOLookAt(targetRandomPosition, 0.1f);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void OnCompleteMoving(GameObject obj){
			switch(completeAction){
				case CompleteAction.None:
					break;
				case CompleteAction.Destroy:
					obj.transform.DOScale(Vector3.zero, delayTime)
							.OnComplete(() => Destroy(obj));
					break;
				case CompleteAction.Inactive:
					obj.transform.DOScale(Vector3.zero, delayTime)
							.OnComplete(() => obj.SetActive(false));
					break;
				case CompleteAction.MoveAround:
					var randomOffset = Random.Range(2, -2);
					var targetPoint = obj.transform.position + new Vector3(randomOffset, 0, 0);
					obj.transform.DOJump(targetPoint, 3, 1, during)
							.SetLoops(-1, LoopType.Yoyo);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private Vector3 GetTargetRandomPosition(Vector3 targetPosition){
			var randomX = Random.Range(-targetBoxSize.x / 2, targetBoxSize.x / 2);
			var randomY = Random.Range(-targetBoxSize.y / 2, targetBoxSize.y / 2);
			var randomZ = Random.Range(-targetBoxSize.z / 2, targetBoxSize.z / 2);
			var randomPosition = new Vector3(randomX, randomY, randomZ);
			return targetPosition + randomPosition;
		}

		private Vector3 GetTargetPosition(){
			switch(targetType){
				case TargetType.MainCamera:
					var main = Camera.main;
					var current = Camera.current;
					if(!main && !current) throw new Exception("Can,t find Camera");
					var cameraPosition = main ? main.transform.position : current.transform.position;
					return cameraPosition;
				case TargetType.Transform:
					var position = targetTransform.position + targetOffset;
					return position;
				case TargetType.Vector:
					var targetPosition = transform.position + targetVector;
					return targetPosition;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private Vector3[] GenerateCurvePathPoints(Vector3 targetPosition){
			const int numPoints = 50;
			var pathPoints = new Vector3[numPoints + 2]; // 加2是为了包括起始点和目标位置
			pathPoints[0] = transform.position;
			pathPoints[numPoints + 1] = targetPosition;

			for(var i = 1; i <= numPoints; i++){
				var t = i / (float)(numPoints + 1);
				var offset = height * (t - t * t); // 控制曲线的高度

				pathPoints[i] = Vector3.Lerp(pathPoints[0], pathPoints[numPoints + 1], t) +
								direction * offset;
			}

			return pathPoints;
		}

		private void OnDrawGizmosSelected(){
			if(!debugLine){
				return;
			}

			if(!targetTransform && IsTransformType()) return;

			var targetPosition = GetTargetPosition();
			Gizmos.color = debugColor;
			if(_spawner == null){
				Gizmos.DrawLine(transform.position, targetPosition);
				Gizmos.DrawWireCube(targetPosition, targetBoxSize);
			}
			else{
				foreach(var point in _spawner.SpawnPoint){
					Gizmos.DrawLine(point, targetPosition);
				}

				Gizmos.DrawWireCube(targetPosition, targetBoxSize);
			}
		}


		private bool IsTransformType() => targetType == TargetType.Transform;
		private bool IsVectorType() => targetType == TargetType.Vector;

		private bool IsDelayAction() =>
				completeAction == CompleteAction.Destroy || completeAction == CompleteAction.Inactive;
	}

	public enum TargetType{
		MainCamera,
		Transform,
		Vector
	}

	public enum CompleteAction{
		None,
		Destroy,
		Inactive,
		MoveAround,
	}

	public enum RotationType{
		Default,
		Zero,
		FaceTarget
	}

	public enum MovementType{
		Linear,
		Curve,
	}
}