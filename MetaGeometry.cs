using UnityEngine;

// Line vs sphere: https://stackoverflow.com/a/17499940/10725664
// Line vs circle: https://stackoverflow.com/a/23017208/10725664
// Line vs line: https://stackoverflow.com/a/46045077/10725664
// Get closest point to line: https://stackoverflow.com/a/9557244/10725664

namespace Assets.Metater
{
    public static class MetaGeometry
    {
        public static Vector3 GetClosestPointOnLineSegment(Vector3 a, Vector3 b, Vector3 p)
        {
            Vector3 ap = p - a;
            Vector3 ab = b - a;

            float magnitude = ab.sqrMagnitude;
            float dot = Vector3.Dot(ap, ab);
            float t = dot / magnitude;

            if (t < 0)
            {
                return a;
            }
            else if (t > 1)
            {
                return b;
            }
            else
            {
                return a + ab * t;
            }
        }

        // public static void LineVsLine(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2? intersection, float tolerance = 0.001f)
        // {
        //     float x1 = p1.x, y1 = p1.y;
        //     float x2 = p2.x, y2 = p2.y;

        //     float x3 = p3.x, y3 = p3.y;
        //     float x4 = p4.x, y4 = p4.y;

        //     if (Mathf.Abs(x1 - x2) < tolerance && Mathf.Abs(x3 - x4) < tolerance && Mathf.Abs(x1 - x3) < tolerance)
        //     {
        //         intersection = null;
        //         return;
        //     }

        //     if (Mathf.Abs(y1 - y2) < tolerance && Mathf.Abs(y3 - y4) < tolerance && Mathf.Abs(y1 - y3) < tolerance)
        //     {
        //         intersection = null;
        //         return;
        //     }

        //     if (Mathf.Abs(x1 - x2) < tolerance && Mathf.Abs(x3 - x4) < tolerance)
        //     {
        //         intersection = null;
        //         return;
        //     }

        //     if (Mathf.Abs(y1 - y2) < tolerance && Mathf.Abs(y3 - y4) < tolerance)
        //     {
        //         intersection = null;
        //         return;
        //     }

        //     float x, y;

        //     if (Mathf.Abs(x1 - x2) < tolerance)
        //     {
        //         float m2 = (y4 - y3) / (x4 - x3);
        //         float c2 = -m2 * x3 + y3;

        //         x = x1;
        //         y = c2 + m2 * x1;
        //     }
        //     else if (Mathf.Abs(x3 - x4) < tolerance)
        //     {
        //         float m1 = (y2 - y1) / (x2 - x1);
        //         float c1 = -m1 * x1 + y1;

        //         x = x3;
        //         y = c1 + m1 * x3;
        //     }
        //     else
        //     {
        //         float m1 = (y2 - y1) / (x2 - x1);
        //         float c1 = -m1 * x1 + y1;

        //         float m2 = (y4 - y3) / (x4 - x3);
        //         float c2 = -m2 * x3 + y3;

        //         x = (c1 - c2) / (m2 - m1);
        //         y = c2 + m2 * x;

        //         if (!(Mathf.Abs(-m1 * x + y - c1) < tolerance && Mathf.Abs(-m2 * x + y - c2) < tolerance))
        //         {
        //             intersection = null;
        //             return;
        //         }
        //     }

        //     static bool IsInsideLine(Vector2 p1, Vector2 p2, double x, double y)
        //     {
        //         return (x >= p1.x && x <= p2.x
        //                     || x >= p2.x && x <= p1.x)
        //                && (y >= p1.y && y <= p2.y
        //                     || y >= p2.y && y <= p1.y);
        //     }

        //     if (IsInsideLine(p1, p2, x, y) && IsInsideLine(p3, p4, x, y))
        //     {
        //         intersection = new(x, y);
        //         return;
        //     }

        //     intersection = null;
        //     return;
        // }

        // public static int LineVsCircle(
        //     Vector2 center, float radius,
        //     Vector2 point1, Vector2 point2,
        //     out Vector2? intersection1, out Vector2? intersection2)
        // {
        //     float cx = center.x;
        //     float cy = center.y;

        //     float dx, dy, A, B, C, det, t;

        //     dx = point2.x - point1.x;
        //     dy = point2.y - point1.y;

        //     A = dx * dx + dy * dy;
        //     B = 2 * (dx * (point1.x - cx) + dy * (point1.y - cy));
        //     C = (point1.x - cx) * (point1.x - cx) + (point1.y - cy) * (point1.y - cy) - radius * radius;

        //     det = B * B - 4 * A * C;
        //     if ((A <= 0.0000001) || (det < 0))
        //     {
        //         // No real solutions.
        //         intersection1 = null;
        //         intersection2 = null;
        //         return 0;
        //     }
        //     else if (det == 0)
        //     {
        //         // One solution.
        //         t = -B / (2 * A);
        //         intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
        //         intersection2 = null;
        //         return 1;
        //     }
        //     else
        //     {
        //         // Two solutions.
        //         t = (-B + Mathf.Sqrt(det)) / (2 * A);
        //         intersection1 = new Vector2(point1.x + t * dx, point1.y + t * dy);
        //         t = (-B - Mathf.Sqrt(det)) / (2 * A);
        //         intersection2 = new Vector2(point1.x + t * dx, point1.y + t * dy);
        //         return 2;
        //     }
        // }
    }
}