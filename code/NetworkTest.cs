using Sandbox.ExampleComponents;
using Sandbox.ExampleComponents.ViewModel;

public sealed class NetworkTest : Component
{
	[Property] public CameraComponent Camera { get; set; }
	[Property] public ViewModelCamera ViewModelCamera { get; set; }
	[Property] public ViewModel ViewModel { get; set; }
	[Property] public ViewModelArms ViewModelArms { get; set; }

	protected override void OnStart()
	{
		base.OnStart();

		Camera = Game.ActiveScene.GetAllComponents<CameraComponent>().FirstOrDefault();

		var goViewmodelCamera = new GameObject();
		goViewmodelCamera.Name = "ViewModelCamera";
		goViewmodelCamera.Parent = GameObject;
		goViewmodelCamera.NetworkMode = NetworkMode.Never;
		ViewModelCamera = goViewmodelCamera.Components.GetOrCreate<ViewModelCamera>();

		/* VIEWMODEL */

		var goViewmodel = new GameObject();
		goViewmodel.Name = "ViewModel";
		goViewmodel.Parent = GameObject;
		goViewmodel.NetworkMode = NetworkMode.Never;
		ViewModel = goViewmodel.Components.GetOrCreate<ViewModel>();

		/* ARMS */

		var goViewmodelArms = new GameObject();
		goViewmodelArms.Name = "ViewModelArms";
		goViewmodelArms.Parent = goViewmodel;
		goViewmodelArms.NetworkMode = NetworkMode.Never;
		ViewModelArms = goViewmodelArms.Components.GetOrCreate<ViewModelArms>();
	}

	// I dunno why, but you will increase the chance of geting an unsync viewmodel transform
	// by creating a new gameobject and attach it a Component
	public void MuzzleFlashEffect()
	{
		var goParticle = new GameObject();
		goParticle.Name = "Particle MuzzleFlash";
		goParticle.SetParent( ViewModel.GameObject, false );

		var tempMuzzleFlash = goParticle.Components.Create<BlankComponent>();
	}

	protected override void OnFixedUpdate()
	{
		if ( IsProxy )
			return;

		if ( Input.MouseWheel.y != 0 ) {

			ViewModelCamera.GameObject.Enabled = !ViewModelCamera.GameObject.Enabled;
		}

		if ( Input.Pressed( "attack1" ) )
		{
			ViewModel.SkinnedModelRenderer.Set( "b_attack", true );
			MuzzleFlashEffect();
		}

	}
}
