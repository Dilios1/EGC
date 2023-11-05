using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Lab4Tema
{
    public class MyApplication : GameWindow
    {
        private Cube cube;


        public MyApplication() : base(800, 600, GraphicsMode.Default, "Cub 3D")
        {
            VSync = VSyncMode.On;
            cube = new Cube("coordonate_cub.txt", Color4.Red, Color4.Red, Color4.Red, Color4.Red, Color4.Red, Color4.Red);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color4.Black);
            GL.Enable(EnableCap.DepthTest);
            GL.Disable(EnableCap.CullFace);


            // Setează o perspectivă pentru a vedea cubul
            float aspectRatio = Width / (float)Height;
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(45.0f), aspectRatio, 1.0f, 100.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);

            // Setează poziția și orientarea camerei
            Matrix4 lookat = Matrix4.LookAt(
                -1, 1, 5,   // poziția camerei (x, y, z)
                0, 0, 0,   // punctul la care se uită camera (x, y, z)
                0, 1, 0);  // direcția "susu" a camerei (în general pe axa y)
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard[Key.R])
            {
                cube.BackFaceColor = new Color4(1.0f, 0.0f, 0.0f, cube.Transparency);
            }
            else if (keyboard[Key.G])
            {
                cube.BackFaceColor = new Color4(0.0f, 1.0f, 0.0f, cube.Transparency);
            }
            else if (keyboard[Key.B])
            {
                cube.BackFaceColor = new Color4(0.0f, 0.0f, 1.0f, cube.Transparency);
            }
            else if (keyboard[Key.T])
            {
                cube.Transparency = 0.5f;
            }

            Console.WriteLine($"BackFaceColor: {cube.BackFaceColor}");
            Console.WriteLine($"Transparency: {cube.Transparency}");
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            cube.Draw();
            /* cube.InitializeColors(); //Cerinta 1 */
            cube.RandomizeColors(); //Cerinta 3

            Triangle triangle = new Triangle(
    new Vector3(1.0f, 0.0f, 0.0f),  
    new Vector3(2.0f, 0.0f, 0.0f),  
    new Vector3(1.5f, 1.0f, 0.0f),  
    new Color4(1.0f, 0.0f, 0.0f, 1.0f),  //  (Rosu)
    new Color4(0.0f, 1.0f, 0.0f, 1.0f),  //  (Verde)
    new Color4(0.0f, 0.0f, 1.0f, 1.0f)   // (Albastru)
    );

            // Afișarea valorilor RGB în consolă pentru fiecare vertex
            triangle.Draw();

            SwapBuffers();
        }

        [STAThread]
        public static void Main()
        {
            using (var window = new MyApplication())
            {
                window.Run(60.0, 60.0);
            }
        }

    }
}
