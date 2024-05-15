using System;
using System.Collections.Generic;
namespace Sandbox.ExampleComponents.ViewModel
{
	public class ViewModelCamera : Component
	{
		public NetworkTest GetOwner()
		{
			return GameObject.Components.GetInAncestors<NetworkTest>();
		}

		protected override void OnStart()
		{
			base.OnStart();

			var viewModelCamera = GameObject.Components.GetOrCreate<CameraComponent>();
			viewModelCamera.ClearFlags = ClearFlags.Depth | ClearFlags.Stencil;
			viewModelCamera.BackgroundColor = Color.FromRgba( 0xFFFFFF00 );
			viewModelCamera.IsMainCamera = false;
			viewModelCamera.Priority = 2;
			viewModelCamera.FieldOfView = 110f;
			viewModelCamera.ZNear = 1;
			viewModelCamera.RenderTags.Add( "viewmodel" );
			viewModelCamera.RenderTags.Add( "light" );
			viewModelCamera.RenderExcludeTags.Add( "hide" );
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
