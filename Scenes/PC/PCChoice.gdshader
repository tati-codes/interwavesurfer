shader_type canvas_item;

vec2 rotate(vec2 uv, vec2 pivot, float angle)
{
	mat2 rotation = mat2(vec2(sin(angle), -cos(angle)),
						vec2(cos(angle), sin(angle)));
	
	uv -= pivot;
	uv = uv * rotation;
	uv += pivot;
	return uv;
}

void vertex()
{
	vec2 rotated_uv = rotate(UV, vec2(0.5), 9.4);
	VERTEX = vec2(VERTEX.x + sin(TIME * 3.5), VERTEX.y);
}


//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
