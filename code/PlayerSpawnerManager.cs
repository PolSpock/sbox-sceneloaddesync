namespace Sandbox;

public sealed class PlayerSpawnerManager : Component, Component.INetworkListener
{
	static public void SpawnThePlayer()
	{
		Log.Info( "SpawnThePlayer" );

		var player = new GameObject();
		player.Parent = Game.ActiveScene;
		player.Components.Create<NetworkTest>();

		player.Tags.Add( "player" );

		var body = new GameObject();
		body.Name = "Body";
		body.Parent = player;

		var bodySkinnedModelRenderer = body.Components.Create<SkinnedModelRenderer>();
		bodySkinnedModelRenderer.Model = Model.Load( "models/citizen/citizen.vmdl" );
		bodySkinnedModelRenderer.RenderType = ModelRenderer.ShadowRenderType.ShadowsOnly;
		bodySkinnedModelRenderer.CreateBoneObjects = true;

		var eyes = new GameObject();
		eyes.Name = "Eyes";
		eyes.Transform.LocalPosition = new Vector3( 0, 0, 64f );
		eyes.SetParent( player );

		var characterController = player.Components.Create<CharacterController>();
		var playerController = player.Components.Create<PlayerController>();
		playerController.Body = body;
		playerController.Eye = eyes;
	}
}
