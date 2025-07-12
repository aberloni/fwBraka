using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{

	public interface iKappa
	{
		/// <summary>
		/// assoc kappa to a brain
		/// </summary>
		public iKappa assoc(iBrain owner);

		/// <summary>
		/// setup stuff after all kappas are assocs
		/// </summary>
		public iKappa prime();

		/// <summary>
		/// to be called externally by parent brain
		/// </summary>
		public void update(float dt);
	}

	public interface iKappaUpdate : iKappa
	{
		public iKappa setActivity(bool activate);

	}

	public interface iKappaDebug : iDebug, iLogger
	{
		public void drawGizmos();
	}

}