#version 330

layout(location=0) in vec2 pos;

layout(location=1) in vec2 uv;

uniform mat4 Projection;
uniform mat4 View;
uniform mat4 Model;

out vec2 texCoord;

void main(void){
	//Uv vertical flip
	//texCoord = vec2(uv.x,abs(uv.y-1));
	texCoord = uv;

	gl_Position = Projection * View * Model * vec4(pos.x,pos.y,0.0,1.0);
}