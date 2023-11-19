using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;
using System.Linq;

namespace ConsoleApp3
{
    /// <summary>
    /// The graphic window. Contains the canvas (viewport to be draw).
    /// </summary>
    class Window3D : GameWindow
    {

        private KeyboardState previousKeyboard;
        private MouseState previousMouse;
        private readonly Randomizer rando;
        private readonly Axes ax;
        private readonly Grid grid;
        private readonly Camera3DIsometric cam;
        private bool displayMarker;
        private ulong updatesCounter;
        private ulong framesCounter;
        private MassiveObject objy;
        private FallingObject fallingObject;
        private float elapsedTime = 0.0f;
        private bool isObjectMoving = false;
    
        private Vector3 targetNearPosition = new Vector3(0.0f, 0.0f, 0.0f);
        private Vector3 upNearPosition = new Vector3(0.0f, 1.0f, 0.0f);
        private Vector3 targetFarPosition = new Vector3(0.0f, 0.0f, 10.0f);
        private Vector3 upFarPosition = new Vector3(0.0f, 1.0f, 0.0f);
        private Vector3 cameraNearPosition = new Vector3(0.0f, 0.0f, 5.0f);
        private Vector3 cameraFarPosition = new Vector3(0.0f, 0.0f, 20.0f);
        private bool isCameraNear = true; 



        // DEFAULTS
        private readonly Color DEFAULT_BKG_COLOR = Color.FromArgb(49, 50, 51);

        /// <summary>
        /// Parametrised constructor. Invokes the anti-aliasing engine. All inits are placed here, for convenience.
        /// </summary>
        public Window3D() : base(1280, 768, new GraphicsMode(32, 24, 0, 8))
        {
            VSync = VSyncMode.On;

            // inits
            rando = new Randomizer();
            ax = new Axes();
            grid = new Grid();
            cam = new Camera3DIsometric();
            objy = new MassiveObject(Color.Green);
            

            DisplayHelp();
            displayMarker = false;
            updatesCounter = 0;
            framesCounter = 0;

            

        }

        /// <summary>
        /// OnLoad() method. Part of the control loop of the OpenTK API. Executed only once.
        /// </summary>
        /// <param name="e">event parameters that triggered the method;</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);
            fallingObject = new FallingObject(ax.Coordinates.ToList(), 0.1f);

        }

        /// <summary>
        /// OnResize() method. Part of the control loop of the OpenTK API. Executed at least once (after OnLoad()).
        /// </summary>
        /// <param name="e">event parameters that triggered the method;</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // set background
            GL.ClearColor(DEFAULT_BKG_COLOR);

            // set viewport
            GL.Viewport(0, 0, this.Width, this.Height);

            // set perspective
            Matrix4 perspectiva = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)this.Width / (float)this.Height, 1, 1024);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspectiva);

            // set the eye
            cam.SetCamera();
        }

        /// <summary>
        /// OnUpdateFrame() method. Part of the control loop of the OpenTK API. Executed periodically, with a frequency determined when launching
        /// the graphics window (<see cref="GameWindow.Run(double, double)"/>). In this case should be 30.00 (if unmodified).
        ///
        /// All logic should reside here!
        /// </summary>
        /// <param name="e">event parameters that triggered the method;</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            updatesCounter++;
            elapsedTime += (float)e.Time;
            



            if (displayMarker)
            {
                TimeStampIt("update", updatesCounter.ToString());
            }

            // LOGIC CODE
            KeyboardState currentKeyboard = Keyboard.GetState();
            MouseState currentMouse = Mouse.GetState();

            if (currentKeyboard[Key.Escape])
            {
                Exit();
            }

            if (currentKeyboard[Key.H] && !previousKeyboard[Key.H])
            {
                DisplayHelp();
            }

            if (currentKeyboard[Key.R] && !previousKeyboard[Key.R])
            {
                GL.ClearColor(DEFAULT_BKG_COLOR);
                ax.Show();
                grid.Show();
                
            }

            if (currentKeyboard[Key.K] && !previousKeyboard[Key.K])
            {
                ax.ToggleVisibility();
            }

            if (currentKeyboard[Key.B] && !previousKeyboard[Key.B])
            {
                GL.ClearColor(rando.RandomColor());
            }

            if (currentKeyboard[Key.V] && !previousKeyboard[Key.V])
            {
                grid.ToggleVisibility();
            }

            if (currentKeyboard[Key.O] && !previousKeyboard[Key.O])
            {
                objy.ToggleVisibility();
            }

            // camera control (isometric mode)
            if (currentKeyboard[Key.W])
            {
                cam.MoveForward();
            }
            if (currentKeyboard[Key.S])
            {
                cam.MoveBackward();
            }
            if (currentKeyboard[Key.A])
            {
                cam.MoveLeft();
            }
            if (currentKeyboard[Key.D])
            {
                cam.MoveRight();
            }
            if (currentKeyboard[Key.Q])
            {
                cam.MoveUp();
            }
            if (currentKeyboard[Key.E])
            {
                cam.MoveDown();
            }

            // helper functions
            if (currentKeyboard[Key.L] && !previousKeyboard[Key.L])
            {
                displayMarker = !displayMarker;
            }
            fallingObject.Update((float)e.Time);

            if (isObjectMoving)
            {
                float objectSpeed = 100.0f; 
                float distance = objectSpeed * (float)e.Time;

                
                fallingObject.MoveDown(distance);

                
                if (fallingObject.MinY() <= 0.0f)
                {
                    
                    isObjectMoving = false;
                }
            }

            
            if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released)
            {
                
                fallingObject.StartObjectMovement();
                isObjectMoving = true; 
            }

            float initialY = fallingObject.MinY(); 
            if (currentMouse.RightButton == ButtonState.Pressed && !previousMouse[MouseButton.Right])
            {
                isObjectMoving = true;
                float objectSpeed = 300.0f; 

                
                fallingObject.MoveUp(objectSpeed * (float)e.Time);

                float finalY = fallingObject.MinY(); 
                Console.WriteLine($"Initial Y: {initialY}, Final Y: {finalY}");
            }

            if (currentKeyboard[Key.Space] && !previousKeyboard[Key.Space])
            {
                
                isCameraNear = !isCameraNear;

                
                if (isCameraNear)
                {
                    Vector3 objectPosition = fallingObject.GetObjectPosition();
                    cam.SetCameraPosition(objectPosition + new Vector3(0, 0, 5), objectPosition, upNearPosition);

                }
               
            }


            if (currentKeyboard[Key.Y] && !previousKeyboard[Key.Y])
            {
                Vector3 objectPosition = fallingObject.GetObjectPosition();
                cam.SetCameraPosition(objectPosition + new Vector3(0, 0, 1000), objectPosition, upFarPosition);
            }

            





            previousKeyboard = currentKeyboard;
            previousMouse = currentMouse;
            // END logic code
        }

        /// <summary>
        /// OnRenderFrame() method. Part of the control loop of the OpenTK API. Executed periodically, with a frequency determined when launching
        /// the graphics window (<see cref="GameWindow.Run(double, double)"/>). In this case should be 0.00 (if unmodified) - the rendering is triggered
        /// only when the scene is modified.
        ///
        /// All render calls should reside here!
        /// </summary>
        /// <param name="e">event parameters that triggered the method;</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            framesCounter++;

            if (displayMarker)
            {
                TimeStampIt("render", framesCounter.ToString());
            }

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            // RENDER CODE
            grid.Draw();
            ax.Draw();
            objy.Draw();
            fallingObject.Draw(Color.Yellow);


            // END render code

            SwapBuffers();
        }

        /// <summary>
        /// Internal method, used to dump the menu on the console window (text mode!)...
        /// </summary>
        private void DisplayHelp()
        {
            Console.WriteLine("\n      MENIU");
            Console.WriteLine(" (H) - meniul");
            Console.WriteLine(" (ESC) - parasire aplicatie");
            Console.WriteLine(" (K) - schimbare vizibilitate sistem de axe");
            Console.WriteLine(" (R) - resteaza scena la valori implicite");
            Console.WriteLine(" (B) - schimbare culoare de fundal");
            Console.WriteLine(" (V) - schimbare vizibilitate linii");
            Console.WriteLine(" (W,A,S,D) - deplasare camera (izometric)");
            Console.WriteLine("(SPACE) -  Camera aproape de obiect");
            Console.WriteLine("(Y)- Camera departe de obiect");
            Console.WriteLine("(O) - schimba visibilitate obiect .obj");
        }

        private void TimeStampIt(String source, String counter)
        {
            String dt = DateTime.Now.ToString("hh:mm:ss.ffff");
            Console.WriteLine("     TSTAMP from <" + source + "> on iteration <" + counter + ">: " + dt);
        }

        





    }
}
