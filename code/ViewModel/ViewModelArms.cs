namespace Sandbox.ExampleComponents.ViewModel
{
	public class ViewModelArms : Component
	{
		public NetworkTest GetOwner()
		{
			return GameObject.Components.GetInAncestors<NetworkTest>();
		}

		public SkinnedModelRenderer SkinnedModelRenderer { get; set; }

		protected override void OnStart()
		{
			base.OnStart();

			SkinnedModelRenderer ??= GameObject.Components.GetOrCreate<SkinnedModelRenderer>();
			SkinnedModelRenderer.Model = Model.Load( "models/first_person/first_person_arms.vmdl" );
			SkinnedModelRenderer.BoneMergeTarget = GetOwner().ViewModel.SkinnedModelRenderer;

			GameObject.Tags.Add( "viewmodel" );

		}
	}
}
