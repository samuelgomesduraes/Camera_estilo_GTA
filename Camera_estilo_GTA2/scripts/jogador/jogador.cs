using Godot;
using System;

public class jogador : KinematicBody
{
	[Export] float gravidade = 100.0f;[Export] float speed = 32f;
	[Export] float jumpforce = 45f;[Export] float mouse_sensi = 0.5f;
	private Vector3 motion=new Vector3(Vector3.Zero);
	private Vector3 snap_vector=new Vector3(Vector3.Down);
	//booleanas
	private bool esta_andando=false;private bool is_jumping=false;
	enum statemachine{IDLE,WALK,JUMP}
	statemachine state=statemachine.IDLE;
	//nodes
	private SpringArm pivo;
	private AnimationPlayer godot_player_animation;
	public override void _Ready(){
		Input.SetMouseMode(Input.MouseModeEnum.Captured);
		//nodes
		pivo=GetNode<SpringArm>("pivo");
		godot_player_animation=GetNode<AnimationPlayer>("bunny/AnimationPlayer");
	}
	public override void _Input(InputEvent @event){
		if (@event is InputEventMouseMotion motionEvent)
		{
			Vector3 rotDeg = RotationDegrees;
			rotDeg.y -= motionEvent.Relative.x * mouse_sensi;
			
			RotationDegrees = rotDeg;
			
			//rotDeg = pivo.RotationDegrees;//rotaciona em y

			rotDeg.x -= motionEvent.Relative.y * mouse_sensi;
			
			rotDeg.x = Mathf.Clamp(rotDeg.x, -90, 50);
			//pivo.RotationDegrees = rotDeg;//rotaciona em y
	
		}
	}
	public override void _Process(float delta){
		//mouse visivel controle
		if(Input.IsActionJustPressed("ui_cancel")){
			Input.SetMouseMode(Input.MouseModeEnum.Visible);
		}
		else if(Input.IsActionJustPressed("ui_accept")){
			Input.SetMouseMode(Input.MouseModeEnum.Captured);
		}
		else if(Input.IsActionJustPressed("ui_end")){
			GetTree().Quit();
		}//mouse visivel controle
		motion=MoveAndSlide(motion,Vector3.Up,true);
		//ESTADOS
		switch (state)
		{
			case statemachine.IDLE:estado_idle(delta);break;
			case statemachine.WALK:estado_walk(delta);break;
			case statemachine.JUMP:estado_jump(delta);break;	
		}
		
	}//----process

	private void estado_idle(float delta){
		motion.x = 0;
		motion.z = 0;
		aplicar_gravidade(delta);
		godot_player_animation.Play("Idle");
		if (Input.IsActionPressed("a") || Input.IsActionPressed("d")
		|| Input.IsActionPressed("w") || Input.IsActionPressed("s")){

			state=statemachine.WALK;
		}
		else if (Input.IsActionPressed("space")&& IsOnFloor() ){
		
			state=statemachine.JUMP;
		}
	}
	private void estado_walk(float delta){
		aplicar_gravidade(delta);
		movimentacao();
		
		godot_player_animation.Play("Run");
		if (! esta_andando){
			state=statemachine.IDLE;
		}
		else if (Input.IsActionPressed("space") && IsOnFloor()){
			state=statemachine.JUMP;
		}
	}
	private void estado_jump(float delta){
		aplicar_gravidade(delta);
		motion.y=jumpforce;
		godot_player_animation.Play("Jump");
		if (!esta_andando && IsOnFloor())
		{
			state=statemachine.IDLE;
		}
		else if (Input.IsActionPressed("a") || Input.IsActionPressed("d")
		|| Input.IsActionPressed("w") || Input.IsActionPressed("s")){
			state=statemachine.WALK;
		}
	}
	//<----------------ESTADOS
//PROPIEDADES----------------
private void aplicar_gravidade(float delta){motion.y+=-gravidade *delta;}
private void  movimentacao(){
	Vector3 mot=new Vector3(Vector3.Zero);
	if (Input.IsActionPressed("a")){
		mot -= Transform.basis.x * speed;
		esta_andando=true;}
	else if (Input.IsActionPressed("d")){
		mot += Transform.basis.x * speed;
		esta_andando=true; }
	else if (Input.IsActionPressed("w")){
		mot -= Transform.basis.z * speed;
		 esta_andando=true;}
	else if (Input.IsActionPressed("s")){
		mot += Transform.basis.z * speed;
		esta_andando=true;}
	else{esta_andando=false;}
	//mot = motion.Normalized();
	mot=MoveAndSlideWithSnap(mot,snap_vector,Vector3.Up,true);
}
//PROPIEDADES----------------
//SINAIS----------------->

//<-----------------SINAIS
}