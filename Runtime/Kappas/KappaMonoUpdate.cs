using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
	public class KappaMonoUpdate : MonoUpdate, iKappa
	{
		public BrainBase brain;

		virtual public iKappa assoc(Brain owner)
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