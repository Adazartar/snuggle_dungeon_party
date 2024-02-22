using Sandbox;
using System;

public sealed class Player : Component
{
	Rigidbody rb;
	[Property] int speed = 10;
	[Property] int rotation_speed = 10;
	Vector3 vel;
	Rotation target_rotation;
	[Property] bool using_controller = true;
	protected override void OnStart(){
		Log.Info("We have started");
		rb = GameObject.Components.Get<Rigidbody>();
	}
	protected override void OnFixedUpdate()
	{
		rb.Velocity = vel.Normal * Time.Delta * 1000 * speed;
	}
	protected override void OnUpdate()
	{
		updateMove();
		updateRotate();
		
		
	}

	private void updateMove()
	{
		vel = Vector3.Zero;
		if(using_controller)
		{
			vel += Input.AnalogMove;
		}
		else
		{
			if(Input.Down("forward")){
				vel += new Vector3(1, 0, 0);
			}
			if(Input.Down("backward")){
				vel += new Vector3(-1,0, 0);
			}
			if(Input.Down("left")){
				vel += new Vector3(0, 1, 0);
			}
			if(Input.Down("right")){
				vel += new Vector3(0, -1, 0);
			}
		}
		
	}

	private void updateRotate()
	{
		float angle;
		if(vel.x == 0 && vel.y == 0)
		{
			return;
		}
		if(vel.x != 0)
		{
			angle = (float)(Math.Atan2(vel.y, vel.x) * 180/Math.PI);
			target_rotation = new Angles(0, angle, 0).ToRotation();
			Transform.Rotation = Rotation.Slerp(Transform.Rotation, target_rotation, Time.Delta * rotation_speed, true);
		}
		else
		{
			angle = vel.y > 0 ? 90 : -90;
			target_rotation = new Angles(0, angle, 0).ToRotation();
			Transform.Rotation = Rotation.Slerp(Transform.Rotation, target_rotation, Time.Delta * rotation_speed, true);
		}
	}
}