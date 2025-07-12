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
		public iKappa assoc(Brain owner);

		/// <summary>
		/// setup stuff after all kappas are assocs
		/// </summary>
		public iKappa prime();

		/// <summary>
		/// to be called externally
		/// </summary>
		public void update(float dt);
	}

}