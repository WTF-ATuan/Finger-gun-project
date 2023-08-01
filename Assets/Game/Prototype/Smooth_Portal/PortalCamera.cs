using System;
using UnityEngine;

namespace Game.Prototype.Smooth_Portal{
	public class PortalCamera : MonoBehaviour{
		public Transform playerCamera;
		public Transform portal;
		public Transform otherPortal;


		private void LateUpdate(){
			var playerOffsetFromPortal = playerCamera.position - otherPortal.position;
			transform.position = portal.position + playerOffsetFromPortal;

			var angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);
			var portalRotationsDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
			var needFaceToDirection = portalRotationsDifference * playerCamera.forward;
			transform.rotation = Quaternion.LookRotation(needFaceToDirection, Vector3.up);
		}
	}
}