shader_type canvas_item;
uniform float crt_curve: hint_range(0.0, 1.0) = 0.05;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec2 crt_curve_shift = (vec2(1.0) - sin(UV.yx * PI)) * crt_curve;
	vec2 crt_curve_scale = vec2(1.0) + crt_curve_shift * 2.0;
	vec2 screen_uv = SCREEN_UV * crt_curve_scale - crt_curve_shift;
	vec2 uv = UV * crt_curve_scale - crt_curve_shift;
	float crt_mask = 0.0;
	if (uv.x >= 0.0 && uv.x <= 1.0 && uv.y >= 0.0 && uv.y <= 1.0) {
		crt_mask = 1.0;
	} 
	if (crt_mask > 0.0) {
		COLOR = COLOR;
	} else {
		COLOR = vec4(0.0);
	}
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
