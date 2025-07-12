using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{

	public interface Brain
	{
		public TKappa getKappa<TKappa>() where TKappa : Kappa;
		public void updateKappas(float dt);
	}

}