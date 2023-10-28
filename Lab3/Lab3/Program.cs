using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.IO;

namespace TriangleColorChange
{
    class Program : GameWindow
    {
        private Vector3 color1 = new Vector3(1.0f, 1.5f, 0.0f); // Roșu
        private Vector3 color2 = new Vector3(1.0f, 1.0f, 0.0f); // Verde
        private Vector3 color3 = new Vector3(0.5f, 1.0f, 1.0f); // Albastru
       

        private float x1, y1, z1, x2, y2, z2, x3, y3, z3; // Coord triunghi
        private float cameraRotationX = 0.0f;
        private float cameraRotationY = 0.0f;
        private float previousMouseX;
        private float previousMouseY;


        public Program() : base(800, 600, new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;
            Title = "Triangle Color Change";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Color4.Black); // Color4 este folosit pentru culoarea de fundal
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            // Citire din fisier
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "triangle_coordinates.txt");
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length >= 3)
                {
                    // 
                    string[] point1 = lines[0].Split(' ');
                    string[] point2 = lines[1].Split(' ');
                    string[] point3 = lines[2].Split(' ');

                    if (point1.Length == 3 && point2.Length == 3 && point3.Length == 3)
                    {
                         x1 = float.Parse(point1[0]);
                         y1 = float.Parse(point1[1]);
                         z1 = float.Parse(point1[2]);

                         x2 = float.Parse(point2[0]);
                         y2 = float.Parse(point2[1]);
                         z2 = float.Parse(point2[2]);

                         x3 = float.Parse(point3[0]);
                         y3 = float.Parse(point3[1]);
                         z3 = float.Parse(point3[2]);

                        
                    }
                }
            }
            else
            {
                Console.WriteLine("Fișierul " + filePath + " nu există.");
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            double aspect_ratio = Width / (double)Height;

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);

            Matrix4 lookat = Matrix4.LookAt(5, 5, 5, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyboard[Key.Escape])
            {
                Exit();
            }


            if (keyboard[Key.R])
                color1 = new Vector3(1.0f, 0.0f, 0.0f); //  Roșu
            if (keyboard[Key.G])
                color2 = new Vector3(0.0f, 1.0f, 0.0f); //  Verde
            if (keyboard[Key.B])
                color3 = new Vector3(0.0f, 0.0f, 1.0f); //  Albastru

            float deltaMouseX = mouse.X - previousMouseX;
            float deltaMouseY = mouse.Y - previousMouseY;

            // Actualizam unghi camera
            cameraRotationX += deltaMouseY * 0.1f;
            cameraRotationY += deltaMouseX * 0.1f;

            
            previousMouseX = mouse.X;
            previousMouseY = mouse.Y;

           
            if (keyboard[Key.R])
            {
                color1 = new Vector3(1.0f, 0.0f, 0.0f); // Rosu
                Console.WriteLine("Culoarea Vertex 1 (RGB): " + color1.X + ", " + color1.Y + ", " + color1.Z);
            }
            if (keyboard[Key.G])
            {
                color2 = new Vector3(0.0f, 1.0f, 0.0f); //  Verde
                Console.WriteLine("Culoarea Vertex 2 (RGB): " + color2.X + ", " + color2.Y + ", " + color2.Z);
            }
            if (keyboard[Key.B])
            {
                color3 = new Vector3(0.0f, 0.0f, 1.0f); //  Albastru
                Console.WriteLine("Culoarea Vertex 3 (RGB): " + color3.X + ", " + color3.Y + ", " + color3.Z);
            }

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            Matrix4 lookat = Matrix4.LookAt(
             5.0f * (float)Math.Sin(cameraRotationY),  // X
             5.0f * (float)Math.Sin(cameraRotationX),  // Y
             5.0f * (float)Math.Cos(cameraRotationY),  // Z
        0, 0, 0, 0, 1, 0);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            

            GL.Begin(PrimitiveType.Triangles); 

            // Vertex 1 cu culoare roșie
            GL.Color3(color1);
            GL.Vertex3(x1, y1, z1);

            // Vertex 2 cu culoare verde
            GL.Color3(color2);
            GL.Vertex3(x2, y2, z2);

            // Vertex 3 cu culoare albastru
            GL.Color3(color3);
            GL.Vertex3(x3, y3, z3);
           

            GL.End();
            



            SwapBuffers();
        }

        [STAThread]
        static void Main(string[] args)
        {
            using (Program program = new Program())
            {
                program.Run(30.0, 0.0);
            }
        }
    }
}