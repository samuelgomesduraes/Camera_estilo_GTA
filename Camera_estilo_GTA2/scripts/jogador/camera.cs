using Godot;
using System;
using System.Collections.Generic;
public class camera : Camera
{
    KinematicBody jogador;
    Spatial cabeca;
    private Spatial ponto1 ;private Spatial ponto2;private Spatial ponto3;
    public RayCast raio;
    public Vector3 motion=new Vector3(1,0,0);
    public List<int> posicoes=new List<int>{0,1,2};
    public int indice=0;
    SpringArm pivo;
    bool pos1=false;bool pos2=false;bool pos3=false;
    enum cam_estado{cam1,cam2,cam3}
    cam_estado cam_atual=cam_estado.cam2;

    public override void _Ready()
    {
        jogador=GetNode<KinematicBody>("../../jogador");//1 forma de pegar o node do jogador 
        cabeca=GetNode<Spatial>("/root/mapa1/jogador/cabeca");
        raio=GetNode<RayCast>("raio");
        ponto1=GetNode<Spatial>("/root/mapa1/jogador/ponto1");
        ponto2=GetNode<Spatial>("/root/mapa1/jogador/ponto2");
        ponto3=GetNode<Spatial>("/root/mapa1/jogador/ponto3");
        pivo=GetNode<SpringArm>("/root/mapa1/jogador/pivo");
    }
    public override void _Process(float delta) {
		camera_lookat();
        transicao_camera();
        switch (cam_atual)
        {
            case cam_estado.cam1:trocar_camera1();break;
            case cam_estado.cam2:trocar_camera2();break;
            case cam_estado.cam3:trocar_camera3();break;
        }

	}
    public void camera_lookat(){//funcao que faz a camera olha pra cabeca do player
        LookAt(cabeca.GlobalTransform.origin,Vector3.Up);
        //raio.LookAt(cabeca.GlobalTransform.origin,Vector3.Up);//raio sai da camera e aponta pra cabeca
    }
    public void transicao_camera(){
        if(Input.IsActionJustPressed("ui_left")){
            cam_atual=cam_estado.cam1;
        }
        else if(Input.IsActionJustPressed("ui_down")){
            cam_atual=cam_estado.cam2;
        }
        else if(Input.IsActionJustPressed("ui_right")){
            cam_atual=cam_estado.cam3;
        }
        if(pos3){
            raio.CastTo=new Vector3(0,0,-8);
        }
       if(raio.IsColliding() && pos2){
            cam_atual=cam_estado.cam1;    
       }
       else if(raio.IsColliding() && pos3){
        cam_atual=cam_estado.cam2;
       }
    }
     public void trocar_camera1(){
        float tempo=0.1f;
        Vector3 posicao_ponto1=ponto1.GetGlobalTranslation();
        GlobalTranslation=GlobalTranslation.LinearInterpolate(posicao_ponto1,tempo);
    }
    public void trocar_camera2(){
        float tempo=0.1f;
        Vector3 posicao_ponto2=ponto2.GetGlobalTranslation();
        
        GlobalTranslation=GlobalTranslation.LinearInterpolate(posicao_ponto2,tempo);
    }
    public void trocar_camera3(){
        float tempo=0.1f;
        Vector3 posicao_ponto3=ponto3.GetGlobalTranslation();
        GlobalTranslation=GlobalTranslation.LinearInterpolate(posicao_ponto3,tempo);
    }
//SINAIS >
   private void _on_Area1_body_entered(Node body){
    if(body.IsInGroup("camera")){
        pos1=true;
    }
   }
   private void _on_Area1_body_exited(Node body){
    if(body.IsInGroup("camera")){
        pos1=false;
    }
   }
   private void _on_Area2_body_entered(Node body){
    if(body.IsInGroup("camera")){
        pos2=true;
    }
   }
   private void _on_Area2_body_exited(Node body){
    if(body.IsInGroup("camera")){
        pos2=false;
    }
   }
   private void _on_Area3_body_entered(Node body){
    if(body.IsInGroup("camera")){
        pos3=true;
    }
   }
   private void _on_Area3_body_exited(Node body){
    if(body.IsInGroup("camera")){
        pos3=false;
    }
   }
    



//SINAIS >  
}
