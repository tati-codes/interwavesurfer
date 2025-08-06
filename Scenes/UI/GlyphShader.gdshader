shader_type spatial;


uniform sampler2D glyphs;
uniform sampler2D noise;
uniform int glyph_w = 1; //The number of horizontal cells on the glyph image.
uniform int glyph_h = 1; //The number of verticle cells on the glyph image.
uniform float cells_w = 10; //number of horizonatal cells on the shader
uniform float cells_h = 10; //shader verticle cells.
uniform float change_sec = 5; //frequency of changes.
//each change: choose a random glyph and a random color, change. must be offset so they dont all change at once.

//from the book of shaders.com
float random (vec2 st) {
    return fract(sin(dot(st.xy,
                         vec2(12.9898,78.233)))*
        43758.5453123);
}

vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void fragment() {
	float x = floor(UV.x * cells_w);
	float y = floor(UV.y * cells_h);
	vec2 cell_dim = vec2(1.0 / cells_w, 1.0 / cells_h );
	vec2 localUV = mod(UV, cell_dim) * vec2( cells_w, cells_h);
	ALBEDO.rg = localUV;
	ALBEDO.b = 0.0;
	ALPHA = 1.0;
	vec2 noise_val = texture(noise, vec2(x,y)  * cell_dim ).xy;
	float glyphx = floor( mod(random(vec2(x * x * y + y + floor(TIME * change_sec + noise_val * change_sec  ))) * float(glyph_w),float(glyph_w)) );
	float glyphy = floor( mod(random(vec2(y * y * x + x + floor(TIME * change_sec + noise_val * change_sec  ))) * float(glyph_h),float(glyph_h)) );
	vec2 glyphUV = vec2( glyphx / float(glyph_w), glyphy / float(glyph_h) ) + localUV * vec2((float(cells_w) / cells_w) / float(glyph_w),(float(cells_h) / cells_h) / float(glyph_h)) ;
	vec4 color = texture(glyphs,glyphUV);
	ALBEDO = color.rgb;
	vec3 hue = vec3( random(vec2(floor(TIME * noise_val * change_sec  ))), 15.0, 0.1);
	hue = hsv2rgb(hue);
	ALBEDO.rgb = mix(color.rgb, hue, 0.15);
	//COLOR.rg = glyphUV;
	//COLOR.b = 0.0;
	//COLOR.a = 1.0;
}
