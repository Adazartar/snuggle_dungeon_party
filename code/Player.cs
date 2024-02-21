using Sandbox;
using System;

public sealed class Player : Component
{
	Rigidbody rb;
	[Property] int speed = 10;
	Vector3 vel;
	protected override void OnStart(){
		Log.Info("We have started");
		rb = GameObject.Components.Get<Rigidbody>();
	}
	protected override void OnFixedUpdate()
	{
		vel = Vector3.Zero;
		if(Input.Down("forward")){
			vel += new Vector3(0, -1, 0);
		}
		if(Input.Down("backward")){
			vel += new Vector3(0, 1, 0);
		}
		if(Input.Down("left")){
			vel += new Vector3(1, 0, 0);
		}
		if(Input.Down("right")){
			vel += new Vector3(-1, 0, 0);
		}
		rb.Velocity = vel.Normal * Time.Delta * 1000 * speed;
	}
	protected override void OnUpdate()
	{
		if(vel != Vector3.Zero){
			Transform.Rotation = Rotation.LookAt(vel);
		}
		else{
			Transform.Rotation = Angles.Zero;
		}
	}
}