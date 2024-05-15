using Sandbox.Network;
using System;
public static class Reload
{
	public static bool Reloaded;
}

public sealed class NetworkSession : Component
{
	protected override void OnAwake()
	{
		Reload.Reloaded = false;
	}

	protected override void OnStart()
	{
		ChangeScene();
	}

	async private void ChangeScene()
	{
		await GameTask.DelaySeconds( 5f );

		//
		// Create a lobby if we're not connected
		//
		if ( !GameNetworkSystem.IsActive )
		{

			if ( !Reload.Reloaded )
			{
				Log.Info( "Je reload" );
				// Reload the same scene (the `9f832399-4887-46b0-8f21-2ee284f538e2` guid is `minimal.scene` Id)
				SceneFile sceneGamemode = ResourceLibrary.GetAll<SceneFile>().FirstOrDefault( x => x.Title.Equals( "minimal" ) );
				Game.ActiveScene.Load( sceneGamemode );

				Reload.Reloaded = true;
			}

			GameNetworkSystem.CreateLobby();
		}

		if ( Scene is not null )
		{
			Log.Info( Scene.NetworkFrequency );
			Log.Info( Scene.FixedUpdateFrequency );
			Log.Info( Scene.Id );
		}
	}
}
