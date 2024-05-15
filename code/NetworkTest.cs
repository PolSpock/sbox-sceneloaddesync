using Sandbox.ExampleComponents;
using Sandbox.ExampleComponents.ViewModel;

public sealed class NetworkTest : Component
{
	[Property] public GameObject HoldRelative { get; set; }
	[Property] public CameraComponent Camera { get; set; }
	[Property] public ViewModelCamera ViewModelCamera { get; set; }
	[Property] public ViewModel ViewModel { get; set; }
	[Property] public ViewModelArms ViewModelArms { get; set; }

	[Sync]
	GameObject Carrying { get; set; }
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

		UpdatePickup();
	}

	protected override void OnPreRender()
	{
		if ( Carrying.IsValid() && !Carrying.IsProxy )
		{
			var offset = new Vector3( 0, 0, 40 );

			var pc = Components.Get<PlayerController>();

			Carrying.Transform.Position = HoldRelative.Transform.Position + HoldRelative.Parent.Transform.Rotation * offset;
			Carrying.Transform.Rotation = pc.Body.Transform.Rotation;
		}
	}

	void UpdatePickup()
	{
		if ( Carrying.IsValid() )
		{
			if ( Carrying.IsProxy )
			{
				Drop();
				return;
			}

			var offset = new Vector3( 0, 0, 40 );

			var pc = Components.Get<PlayerController>();

			Carrying.Transform.Position = HoldRelative.Transform.Position + HoldRelative.Parent.Transform.Rotation * offset;
			Carrying.Transform.Rotation = pc.Body.Transform.Rotation;
			//Carrying.Components.Get<Rigidbody>().Velocity = 0;
			//Carrying.Components.Get<Rigidbody>().AngularVelocity = 0;
		}

		if ( Input.Pressed( "use" ) )
		{
			var pc = Components.Get<PlayerController>();
			var lookDir = pc.EyeAngles.ToRotation();

			if ( Carrying is not null )
			{
				var rb = Carrying.Components.Get<Rigidbody>( true );
				if ( rb is not null )
				{
					rb.Enabled = true;
					rb.Velocity = lookDir.Forward * 300.0f + Vector3.Up * 200.0f;
				}

				Drop();
				return;
			}

			TryPickup();
		}
	}

	void TryPickup()
	{
		var pc = Components.Get<PlayerController>();
		var lookDir = pc.EyeAngles.ToRotation();
		var eyePos = Transform.Position + Vector3.Up * 60;

		var tr = Scene.Trace.WithoutTags( "player" ).Sphere( 16, eyePos, eyePos + lookDir.Forward * 100 ).Run();
		if ( !tr.Hit ) return;

		if ( tr.Body.GetGameObject() is not GameObject go )
			return;

		if ( !go.Tags.Has( "pickup" ) )
			return;

		go.Network.TakeOwnership();

		Carrying = go;
		Carrying.SetParent( GameObject, true );
		Carrying.Tags.Add( "carrying" );

		var rb = Carrying.Components.Get<Rigidbody>( true );
		if ( rb is not null )
		{
			rb.Enabled = false;
		}

		var ca = Components.Get<Sandbox.Citizen.CitizenAnimationHelper>();

		ca.IkLeftHand = Carrying.Children.FirstOrDefault( x => x.Name == "hand_left" ) ?? Carrying;
		ca.IkRightHand = Carrying.Children.FirstOrDefault( x => x.Name == "hand_right" ) ?? Carrying;
	}

	void Drop()
	{
		if ( !Carrying.IsValid() )
			return;

		var rb = Carrying.Components.Get<Rigidbody>( true );
		if ( rb is not null )
		{
			rb.Enabled = true;
		}

		Carrying.SetParent( null, true );
		Carrying.Tags.Remove( "carrying" );
		Carrying.Network.DropOwnership();
		Carrying = null;

		var ca = Components.Get<Sandbox.Citizen.CitizenAnimationHelper>();
		ca.IkLeftHand = null;
		ca.IkRightHand = null;
	}
}
