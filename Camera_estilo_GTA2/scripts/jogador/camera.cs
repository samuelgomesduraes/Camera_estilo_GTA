using Godot;
using System;
using System.Collections.Generic;
public class camera : Camera
{
    Spatial cabeca;
    private Spatial ponto1,ponto2,ponto3;
    public RayCast raio;
    public List<int> posicoes=new List<int>{0,1,2};//por enquanto nao esta sendo usado
    public int indice=0;//por enquanto nao esta sendo usado
    SpringArm pivo;//por enquanto nao esta sendo usado
    enum cam_estado{cam1,cam2,cam3}//esta sendo usado no process
    cam_estado cam_atual=cam_estado.cam2;//esta sendo usado no process
    public override void _Ready(){
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
        switch (cam_atual){
            case cam_estado.cam1:trocar_camera1();break;
            case cam_estado.cam2:trocar_camera2();break;
            case cam_estado.cam3:trocar_camera3();break;
        }
	}
    public void camera_lookat(){//funcao que faz a camera olha pra cabeca do player
        LookAt(cabeca.GlobalTransform.origin,Vector3.Up);
        raio.LookAt(cabeca.GlobalTransform.origin,Vector3.Up);//raio sai da camera e aponta pra cabeca
    }
    public void transicao_camera(){
        if(Input.IsActionJustPressed("ui_left")){
            cam_atual=cam_estado.cam1;
        }
        if(Input.IsActionJustPressed("ui_down")){
            cam_atual=cam_estado.cam2;
        }
        if(Input.IsActionJustPressed("ui_right")){
            cam_atual=cam_estado.cam3;
        }
    }
    public void trocar_camera1(){//essa funcao muda a camera pra posicao do node ponto2
        float tempo=0.1f;
        Vector3 posicao_ponto1=ponto1.GetGlobalTranslation();
        GlobalTranslation=GlobalTranslation.LinearInterpolate(posicao_ponto1,tempo);
        raio.CastTo=new Vector3(0,0,-4);// aqui vc define o tamanho do raycast
    }
    public void trocar_camera2(){//essa funcao muda a camera pra posicao do node ponto2
        float Tempo=0.1f;
        Vector3 PosicaoPonto2=ponto2.GetGlobalTranslation();
        GlobalTranslation=GlobalTranslation.LinearInterpolate(PosicaoPonto2,Tempo);
        // a camera esta no node ponto2
        if(raio.IsColliding()){// se raycast colidir 
             cam_atual=cam_estado.cam1;//mude pra camera no ponto1
        }
    }
    public void trocar_camera3(){//essa funcao muda a camera pra posicao do node ponto3
        float tempo=0.1f;
        Vector3 posicao_ponto3=ponto3.GetGlobalTranslation();
        GlobalTranslation=GlobalTranslation.LinearInterpolate(posicao_ponto3,tempo);
         // a camera esta no node ponto3
        if(raio.IsColliding()){//se raycast colidir 
             cam_atual=cam_estado.cam2;//mude pra camera no ponto2
        }
        raio.CastTo=new Vector3(0,0,-9);// aqui vc define o tamanho do raycast
    }
    
//SINAIS >
//SINAIS >  
}