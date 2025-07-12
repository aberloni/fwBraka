namespace fwp.braka
{

	public interface iBrakaUpdate
	{
		public void update(float dt);
	}

	/// <summary>
	/// object compatible with brain
	/// </summary>
	public interface iBrakaLimb
	{
		public BrainBase getBrain();
	}

	public interface iDebug
	{

		/// <summary>
		/// return string of debug information
		/// </summary>
		public string stringify();
	}
}