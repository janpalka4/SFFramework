#version 330

in vec2 texCoord;

uniform sampler2D texture0;

out vec4 outputColor;

void main(void){
	vec4 col = texture(texture0,texCoord);
	outputColor = vec4(col.r);
}