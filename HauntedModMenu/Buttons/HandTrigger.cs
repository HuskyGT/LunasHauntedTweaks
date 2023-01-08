﻿using System.Collections;

using UnityEngine;
using UnityEngine.XR;

namespace HauntedModMenu.Buttons
{
	internal class HandTrigger : MonoBehaviour
	{
		protected bool triggered = false;
		protected Collider handCollider = null;

		protected static bool leftHand = true;
		protected static float handSensitivity = 1f;
		protected static Utils.ObjectTracker leftHandTracker = null;
		protected static Utils.ObjectTracker rightHandTracker = null;

		private Coroutine timerRoutine = null;

		protected virtual void Awake()
		{
			this.gameObject.layer = LayerMask.NameToLayer("GorillaInteractable");

			if(leftHandTracker == null)
				leftHandTracker = Utils.RefCache.LeftHandFollower?.AddComponent<Utils.ObjectTracker>();

			if(rightHandTracker == null)
				rightHandTracker = Utils.RefCache.RightHandFollower?.AddComponent<Utils.ObjectTracker>();
		}

		protected virtual void OnDisable()
		{
			triggered = false;
			if(timerRoutine != null)
				StopCoroutine(timerRoutine);
		}

		// TODO: Rebind the hand trigger to either a face button or a grip button instead.
		private void Update()
		{
			if (triggered)
				return;

			bool canTrigger = true;

			if (canTrigger)
			{
				triggered = true;
				if (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.secondaryButton, out bool tButton))
				{
					timerRoutine = StartCoroutine(Timer());
					HandTriggered();
				}
			}
		}
		private IEnumerator Timer()
		{
			yield return new WaitForSeconds(1.5f);
			triggered = false;
			timerRoutine = null;
		}

		protected virtual void HandTriggered() { }
	}
}
