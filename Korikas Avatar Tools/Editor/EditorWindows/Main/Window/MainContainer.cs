using UnityEngine;
public class MainContainer {
    public static Color fromRGB(float r, float g, float b)
    {
        return new Color(r / 255f, g / 255f, b / 255);
    }
}