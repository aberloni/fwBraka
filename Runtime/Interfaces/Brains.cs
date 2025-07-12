using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
	public interface iBrain
	{

	}

	public interface iBrainKappable : iBrain
	{
		public TKappa getKappa<TKappa>() where TKappa : iKappa;
		public void updateKappas(float dt);
		public void updateLateKappas(float dt);
	}

	/// <summary>
	/// object compatible with brain
	/// </summary>
	public interface iBrainLimb
	{
		public BrainBase getBrain();
	}

}