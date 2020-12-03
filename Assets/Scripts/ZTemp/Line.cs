using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Line : MonoBehaviour
{
    private List<Vector3> pointList = new List<Vector3>();
    private List<Vector3> centerList = new List<Vector3>();
    [SerializeField] float width;
    [SerializeField] float chamferRadious;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private void Awake() 
    {
        meshFilter = this.GetComponent<MeshFilter>();
        meshRenderer = this.GetComponent<MeshRenderer>();
    }

    private void SetPoint(Vector3[] points)
    {
        pointList.Clear();
        centerList.Clear();
        centerList.Clear();
        pointList.AddRange(points);
        for (int i = 1; i < pointList.Count - 1; i += 2)
        {
            var lastPoint = pointList[i - 1];
            var point = pointList[i];
            var nextPoint = pointList[i + 1];
            var dir1 = lastPoint - point;
            var dir2 = nextPoint - point;
            var angle = Vector3.Angle(dir1, dir2);
            var halfTan = Mathf.Tan(angle / 2.0f / 180.0f * Mathf.PI);
            //最大内切圆半径
            var maxRadious = Mathf.Min(dir1.magnitude, dir2.magnitude) / halfTan;
            //内切圆半径
            var radious = Mathf.Max( Mathf.Min(maxRadious, chamferRadious), width * 0.5f);
            var d = radious / halfTan;
            var p1 = point + dir1.normalized * d;
            var p2 = point + dir2.normalized * d;
            pointList.RemoveAt(i);
            pointList.InsertRange(i, new Vector3[] { p1, p2 });
            var halfSin = Mathf.Sin(angle / 2.0f / 180.0f * Mathf.PI);
            var center = point + (dir1 * 0.5f + dir2 * 0.5f) * radious / halfSin;
            centerList.Add(center);
        }
    }

    private void DrawMeshs()
    {
        var vertList = new List<Vector3>();
        var uvList = new List<Vector2>();
        var indexList = new List<int>();
        bool isLine = false;
        var half = width * 0.5f;
        Vector3 zdir = Vector3.forward;
        float t = 0;
        Vector3 start;
        var indexs = new int[]
        {
            0, 1, 2,
            0, 2, 3,
        };
        for (int i = 0; i < pointList.Count - 1; i++)
        {
            isLine = i % 2 == 0;
            if(isLine)
            {
                var point = pointList[i];
                var nextPoint = pointList[i + 1];
                start = point;
                var dir = (nextPoint - point).normalized;
                var tdir = Vector3.Cross(dir, zdir).normalized;
                bool isEnd = false;
                while (!isEnd)
                {
                    var verts = new Vector3[4];
                    var uvs = new Vector2[4];
                    verts[0] = start - half * tdir;
                    verts[1] = start + half * tdir;
                    uvs[0] = new Vector2(t, 0);
                    uvs[1] = new Vector2(t, 1);
                    
                    var dis = Vector3.Distance(start, nextPoint);
                    isEnd = dis / width < 1 - t;
                    if(!isEnd)
                    {
                        start += (1 - t) * width * dir;
                        t = 0;
                        uvs[2] = new Vector2(1, 1);
                        uvs[3] = new Vector2(1, 0);
                    }
                    else
                    {
                        start = nextPoint;
                        t = dis / width + t;
                        uvs[2] = new Vector2(t, 1);
                        uvs[3] = new Vector2(t, 0);
                    }   

                    verts[2] = start + half * tdir;
                    verts[3] = start - half * tdir;
                    
                    vertList.AddRange(verts);
                    uvList.AddRange(uvs);
                    indexList.AddRange(indexs);
                }
            }
            else
            {
                var center = centerList[i / 2];
                var point = pointList[i];
                var nextPoint = pointList[i + 1];
            }
        }
        
        var mesh = new Mesh();
        
    }
}
