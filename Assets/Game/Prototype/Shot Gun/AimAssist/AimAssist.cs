using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Game.Prototype.Pistol{
	[RequireComponent(typeof(LineRenderer))]
	public class AimAssist : MonoBehaviour{
		[SerializeField] private GameObject aimTargetPrefab;
		[SerializeField] private float maxDistance = 5;
		[SerializeField] private float maxLineDistance = 2;
		[SerializeField] private float smoothValue = 20;
		[SerializeField] private bool isRight = true;
		private LineRenderer _lineRenderer;
		private GameObject _aimTarget;
		private MeshRenderer _targetMesh;


		private void Start(){
			_aimTarget = Instantiate(aimTargetPrefab, transform.position, Quaternion.identity);
			_lineRenderer = GetComponent<LineRenderer>();
			_targetMesh = _aimTarget.GetComponent<MeshRenderer>();
			_lineRenderer.positionCount = 2;
		}

		private void Update(){
			TrackTarget();
		}

		[SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
		private void TrackTarget(){
			var originTransform = transform;
			var targetPosition = originTransform.position + originTransform.forward * maxDistance;
			var lineTargetPosition = originTransform.position + originTransform.forward * maxLineDistance;
			_lineRenderer.SetPositions(new[]
					{ Vector3.zero, originTransform.InverseTransformPoint(lineTargetPosition) });
			_aimTarget.transform.position = Vector3.Lerp(_aimTarget.transform.position, targetPosition,
				Time.deltaTime * smoothValue);
		}
	}
}