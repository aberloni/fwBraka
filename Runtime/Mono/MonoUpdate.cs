using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace fwp.braka
{
	/// <summary>
	/// add Start/Update layer on top
	/// </summary>
	abstract public class MonoUpdate : Mono
	{
		/// <summary>
		/// this behav finished its init
		/// and is running
		/// </summary>
		virtual public bool isLive() => canUpdate();

		private IEnumerator Start()
		{
			enabled = false;

			yield return null;

			//wait for everybody to Start()
			while (delaySetup()) yield return null;

			setup();

			yield return null;

			while (delaySetupLate()) yield return null;

			setupLate();

			yield return null;

			enabled = true;
		}

		/// <summary>
		/// hook to delay boot internaly
		/// </summary>
		virtual protected bool delaySetup() => false;

		virtual protected void setup()
		{ }

		virtual protected bool delaySetupLate() => false;

		/// <summary>
		/// enabled is false here
		/// </summary>
		virtual protected void setupLate()
		{ }

		private void Update()
		{
			if (!canUpdate()) return;

			update(Time.deltaTime);
		}

		private void LateUpdate()
		{
			if (!canUpdate()) return;

			updateLate();
		}

		virtual protected bool canUpdate() => enabled;

		/// <summary>
		/// to update externaly
		/// </summary>
		abstract protected void update(float dt);

		virtual protected void updateLate()
		{ }
	}

}