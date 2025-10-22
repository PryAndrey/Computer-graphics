#version 330 core
in vec2 uv;
out vec4 FragColor;

void main() {
    vec2 coord = uv;
    
    float dist = length(coord);
    float headRadius = 0.6;
    if (dist < headRadius) {
        FragColor = vec4(1.0, 1.0, 0.0, 1.0);
        
        vec2 leftEyePos = vec2(-0.2, 0.2);
        vec2 rightEyePos = vec2(0.2, 0.2);
        float eyeRadius = 0.06;
        
        float outlineThickness = 0.02;
    
        if (dist < headRadius + outlineThickness && dist > headRadius - outlineThickness) {
            FragColor = vec4(0.0, 0.0, 0.0, 1.0);
            return;
        }
        
        if (length(coord - leftEyePos) < eyeRadius) {
            FragColor = vec4(0.0, 0.0, 0.0, 1.0);
        }
        
        if (length(coord - rightEyePos) < eyeRadius) {
            FragColor = vec4(0.0, 0.0, 0.0, 1.0);
        }

        
        float smileRadius = 0.3;
        vec2 smileCenter = vec2(0.0, -0.1);
        float smileWidth = 0.5;
        
        float smileThickness = mix(0.001, 0.1, abs(coord.y - smileCenter.y));
       
        if (dist > smileRadius - smileThickness && 
            dist < smileRadius + smileThickness && 
            coord.y < smileCenter.y && 
            abs(coord.x) < smileWidth) {
            FragColor = vec4(0.0, 0.0, 0.0, 1.0);
        }
    } else {
        FragColor = vec4(1.0, 1.0, 1.0, 1.0);
    }
}