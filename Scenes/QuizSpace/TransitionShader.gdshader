shader_type spatial;


uniform sampler2D texture_1 : source_color, repeat_enable, filter_nearest;
uniform sampler2D texture_2 : source_color, repeat_enable, filter_nearest;
uniform float transform_ratio : hint_range(0.0, 1.0, 0.01) = 1.0;

void fragment() {
	vec4 texture_1_color = texture(texture_1, UV );
	vec4 texture_2_color = texture(texture_2, UV );
	ALBEDO.xyz = (texture_1_color.xyz * transform_ratio) + (texture_2_color.xyz * abs(transform_ratio - 1.0) );
}
