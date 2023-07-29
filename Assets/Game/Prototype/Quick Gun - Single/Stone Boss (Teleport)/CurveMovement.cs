using DG.Tweening;
using UnityEngine;

namespace Game.Prototype.Quick_Gun___Single.Stone_Boss__Teleport_{
	public class CurveMovement : MonoBehaviour{
		public Transform targetTransform;
		public float duration = 2f;
		public float height = 2f; // 控制曲线的高度
		public Vector3 curveDirection = Vector3.up;
		private const int NumPoints = 50; // 生成的路径点数量

		private void Start(){
			MoveAlongCurve();
		}

		private void MoveAlongCurve(){
			var pathPoints = GeneratePathPoints();
			transform.DOPath(pathPoints, duration, PathType.CatmullRom).SetEase(Ease.Linear)
					.OnComplete(OnMovementComplete);
		}

		private Vector3[] GeneratePathPoints(){
			var pathPoints = new Vector3[NumPoints + 2]; // 加2是为了包括起始点和目标位置
			pathPoints[0] = transform.position;
			pathPoints[NumPoints + 1] = targetTransform.position;

			for(var i = 1; i <= NumPoints; i++){
				var t = i / (float)(NumPoints + 1);
				var offset = height * (t - t * t); // 控制曲线的高度

				pathPoints[i] = Vector3.Lerp(pathPoints[0], pathPoints[NumPoints + 1], t) +
								curveDirection * offset;
			}

			return pathPoints;
		}

		private void OnMovementComplete(){
			Debug.Log("Curve movement completed!");
			// 在这里可以执行移动完成后的逻辑
		}
	}
}