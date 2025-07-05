shader_type spatial;
#include "debug_number.gdshaderinc"
render_mode blend_mix, depth_draw_opaque, cull_back, diffuse_lambert, specular_schlick_ggx;
uniform sampler2D tex_vtx_5;
uniform vec4 ColorParameter : source_color = vec4(0.231373, 0.768627, 0.870588, 1.000000);
uniform vec3 bg : source_color = vec3(0.0, 0.57, 0.72);
uniform vec3 text : source_color;
uniform sampler2D tex_frg_3;
uniform sampler2D tex_frg_9;
uniform sampler2D depth_tex_frg_14 : hint_depth_texture;



void vertex() {
// Input:3
	float n_out3p0 = TIME;


// UVFunc:4
	vec2 n_in4p1 = vec2(0.10000, 0.02000);
	vec2 n_out4p0 = vec2(n_out3p0) * n_in4p1 + UV;


// Texture2D:5
	vec4 n_out5p0 = texture(tex_vtx_5, n_out4p0);


// Input:7
	vec3 n_out7p0 = NORMAL;


// VectorOp:8
	vec3 n_in8p1 = vec3(2.00000, 2.00000, 2.00000);
	vec3 n_out8p0 = n_out7p0 + n_in8p1;


// Input:6
	vec3 n_out6p0 = VERTEX;


// MultiplyAdd:2
	vec4 n_out2p0 = fma(n_out5p0, vec4(n_out8p0, 0.0), vec4(n_out6p0, 0.0));


// Output:0
	VERTEX = vec3(n_out2p0.xyz);


}

void fragment() {
// ColorParameter:2
	vec4 n_out2p0 = ColorParameter;


// Input:6
	float n_out6p0 = TIME;


// UVFunc:5
	vec2 n_in5p1 = vec2(0.05000, 0.05000);
	vec2 n_out5p0 = vec2(n_out6p0) * n_in5p1 + UV;


// Texture2D:3
	vec4 n_out3p0 = texture(tex_frg_3, n_out5p0);


// Input:7
	float n_out7p0 = TIME;


// UVFunc:8
	vec2 n_in8p1 = vec2(-0.01000, -0.03000);
	vec2 n_out8p0 = vec2(n_out7p0) * n_in8p1 + UV;


// Texture2D:9
	vec4 n_out9p0 = texture(tex_frg_9, n_out8p0);


// MultiplyAdd:12
	vec4 n_in12p2 = vec4(0.00000, 0.00000, 0.00000, 0.00000);
	vec4 n_out12p0 = fma(n_out3p0, n_out9p0, n_in12p2);


// VectorOp:11
	vec4 n_in11p1 = vec4(1.80000, 1.21000, 1.25000, 1.50000);
	vec4 n_out11p0 = pow(n_out12p0, n_in11p1);


// VectorOp:13
	vec4 n_out13p0 = n_out2p0 + n_out11p0;


	float n_out14p0;
// ProximityFade:14
	float n_in14p0 = 3.00000;
	{
		float __depth_tex = texture(depth_tex_frg_14, SCREEN_UV).r;
		vec4 __depth_world_pos = INV_PROJECTION_MATRIX * vec4(SCREEN_UV * 2.0 - 1.0, __depth_tex, 1.0);
		__depth_world_pos.xyz /= __depth_world_pos.w;
		n_out14p0 = clamp(1.0 - smoothstep(__depth_world_pos.z + n_in14p0, __depth_world_pos.z, VERTEX.z), 0.0, 1.0);
	}


// FloatFunc:15
	float n_out15p0 = 1.0 - n_out14p0;


// FloatOp:22
	float n_in22p1 = 50.00000;
	float n_out22p0 = pow(n_out15p0, n_in22p1);


// Input:18
	float n_out18p0 = TIME;


// MultiplyAdd:24
	float n_in24p1 = 1.50000;
	float n_in24p2 = 10.00000;
	float n_out24p0 = fma(n_out18p0, n_in24p1, n_in24p2);


// FloatOp:19
	float n_out19p0 = n_out15p0 + n_out24p0;


// FloatFunc:17
	float n_out17p0 = sin(n_out19p0);


// FloatFunc:20
	float n_out20p0 = abs(n_out17p0);


// FloatOp:21
	float n_out21p0 = n_out15p0 * n_out20p0;


// FloatOp:23
	float n_out23p0 = n_out22p0 + n_out21p0;


// Mix:25
	vec3 n_in25p1 = vec3(2.00000, 2.00000, 2.00000);
	vec3 n_out25p0 = mix(vec3(n_out13p0.xyz), n_in25p1, vec3(n_out23p0));


// Output:0
    bool pixel = am_ndbg_display_float(float(n_out23p0), vec2(50,50), int(2), int(2), int(10));
	
	ALBEDO = pixel ? text : bg;


}
