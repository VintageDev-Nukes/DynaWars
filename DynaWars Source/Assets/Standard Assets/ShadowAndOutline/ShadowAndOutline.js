#pragma strict
 
public static class ShadowAndOutlineJS
{
    function DrawOutline(rect : Rect, text : String, style : GUIStyle, outColor : Color, inColor : Color, size : float)
    {
        var halfSize : float = size * 0.5;
        var backupStyle : GUIStyle = GUIStyle(style);
        var backupColor : Color = GUI.color;
 
        style.normal.textColor = outColor;
        GUI.color = outColor;
 
        rect.x -= halfSize;
        GUI.Label(rect, text, style);
 
        rect.x += size;
        GUI.Label(rect, text, style);
 
        rect.x -= halfSize;
        rect.y -= halfSize;
        GUI.Label(rect, text, style);
 
        rect.y += size;
        GUI.Label(rect, text, style);
 
        rect.y -= halfSize;
        style.normal.textColor = inColor;
        GUI.color = backupColor;
        GUI.Label(rect, text, style);
 
        style = backupStyle;
    }
 
    function DrawShadow(rect : Rect, content : GUIContent, style : GUIStyle, txtColor : Color, shadowColor : Color,
                                    direction : Vector2)
    {
        var backupStyle : GUIStyle = style;
 
        style.normal.textColor = shadowColor;
        rect.x += direction.x;
        rect.y += direction.y;
        GUI.Label(rect, content, style);
 
        style.normal.textColor = txtColor;
        rect.x -= direction.x;
        rect.y -= direction.y;
        GUI.Label(rect, content, style);
 
        style = backupStyle;
    }
    function DrawLayoutShadow(content : GUIContent, style : GUIStyle, txtColor : Color, shadowColor : Color,
                                    direction : Vector2, options : GUILayoutOption[] )
    {
        DrawShadow(GUILayoutUtility.GetRect(content, style, options), content, style, txtColor, shadowColor, direction);
    }
 
    function DrawButtonWithShadow( r : Rect, content : GUIContent, style : GUIStyle, shadowAlpha : float, direction : Vector2) : boolean
    {
        var letters : GUIStyle = GUIStyle(style);
        letters.normal.background = null;
        letters.hover.background = null;
        letters.active.background = null;
 
        var result : boolean = GUI.Button(r, content, style);
 
        var color : Color = r.Contains(Event.current.mousePosition) ? letters.hover.textColor : letters.normal.textColor;
 
        DrawShadow(r, content, letters, color, Color(0.0, 0.0, 0.0, shadowAlpha), direction);
 
        return result;
    }
 
    function DrawLayoutButtonWithShadow(content : GUIContent, style : GUIStyle, shadowAlpha : float,
                                                   direction : Vector2, options : GUILayoutOption[] ) : boolean
    {
        return DrawButtonWithShadow(GUILayoutUtility.GetRect(content, style, options), content, style, shadowAlpha, direction);
    }
}