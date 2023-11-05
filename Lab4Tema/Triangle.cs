using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;


namespace Lab4Tema
{
    internal class Triangle
    {
        public Vector3 Vertex1 { get; set; }
        public Vector3 Vertex2 { get; set; }
        public Vector3 Vertex3 { get; set; }
        public Color4 Color1 { get; set; }
        public Color4 Color2 { get; set; }
        public Color4 Color3 { get; set; }

        public Triangle(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Color4 color1, Color4 color2, Color4 color3)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            Vertex3 = vertex3;
            Color1 = color1;
            Color2 = color2;
            Color3 = color3;
        }
        public void Draw()
        {
            Console.WriteLine($"Vertex1: RGB({Color1.R}, {Color1.G}, {Color1.B})");
            Console.WriteLine($"Vertex2: RGB({Color2.R}, {Color2.G}, {Color2.B})");
            Console.WriteLine($"Vertex3: RGB({Color3.R}, {Color3.G}, {Color3.B})");

            GL.Begin(PrimitiveType.Triangles);

            GL.Color4(Color1);
            GL.Vertex3(Vertex1);

            GL.Color4(Color2);
            GL.Vertex3(Vertex2);

            GL.Color4(Color3);
            GL.Vertex3(Vertex3);

            GL.End();
        }
    }
}
