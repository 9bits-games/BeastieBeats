using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Util {
    private Util() {}

    public static Rect InflateRect(Rect rect, float width, float height) {
        Rect inflated = new Rect(0f, 0f, rect.width + width, rect.height + height);
        inflated.center = rect.center;
        return inflated;
    }

    public static Rect InflateRectByFactor(Rect rect, float widthFactor, float heightFactor) {
        float invertedFactor = widthFactor - 1f;
        return Util.InflateRect(rect, rect.width * invertedFactor, rect.height * invertedFactor);
    }
}

