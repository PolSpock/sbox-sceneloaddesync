using Sandbox.Network;
using Sandbox.UI;

namespace Sandbox
{
	public class LobbyUiComponent : PanelComponent
	{

		protected override void OnStart()
		{
			base.OnStart();

			Panel.Style.PointerEvents = PointerEvents.All;

			var button = new Button();
			button.Text = "CLICK ME TO LOAD THE SCENE & START A NETWORK LOBBY";
			button.AddEventListener( "onclick", e => LoadScene() );
			button.Style.Width = Length.Percent( 100f );
			button.Style.FontColor = Color.White;

			Panel.AddChild( button );

		}

		private void LoadScene()
		{
			Log.Info( "LoadScene" );

			if ( !GameNetworkSystem.IsActive )
			{
				Log.Info( "Je load" );
				// Reload the same scene (the `9f832399-4887-46b0-8f21-2ee284f538e2` guid is `game.scene` Id)
				SceneFile sceneGamemode = ResourceLibrary.GetAll<SceneFile>().FirstOrDefault( x => x.Title.Equals( "game" ) );

				Game.ActiveScene.Load( sceneGamemode );

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
}
