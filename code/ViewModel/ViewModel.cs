namespace Sandbox.ExampleComponents.ViewModel
{
	public class ViewModel : Component
	{
		public NetworkTest GetOwner()
		{
			return GameObject.Components.GetInAncestors<NetworkTest>();
		}

		[Property] public SkinnedModelRenderer SkinnedModelRenderer { get; set; }

		protected override void OnAwake()
		{
			base.OnAwake();

			SkinnedModelRenderer ??= GameObject.Components.GetOrCreate<SkinnedModelRenderer>();
			//SkinnedModelRenderer.Model = Model.Load( "models/weapons/sbox_smg_mp5/v_mp5.vmdl" );
			SkinnedModelRenderer.Model = Cloud.Model( "facepunch/v_mp5" );

			GameObject.Tags.Add( "viewmodel" );
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			var camera = GetOwner().Camera;

			GameObject.Transform.Position = camera.GameObject.Transform.Position;
			GameObject.Transform.Rotation = camera.GameObject.Transform.Rotation;
		}
	}
}
