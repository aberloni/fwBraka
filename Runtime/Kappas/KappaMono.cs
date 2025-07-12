using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
	/// <summary>
	/// a kappa attached in a brain hierarchy
	/// </summary>
	public class KappaMono : Mono, iKappa
	{
		public BrainBase brain;

		virtual public iKappa assoc(iBrain owner)
		{
			brain = owner as BrainBase;
			return this;
		}

		virtual public iKappa prime()
		{
			return this;
		}

		public void update(float dt)
		{
			updateKappa(dt);
		}

		virtual protected void updateKappa(float dt)
		{ }
	}
}