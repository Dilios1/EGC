using System;
using System.Drawing;
using System.IO;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

public class Cube
{
    private Vector3 position;
    private Color4 color;
    public Color4 FrontFaceColor { get; set; }
    public Color4 BackFaceColor { get; set; }
    public Color4 TopFaceColor { get; set; }
    public Color4 BottomFaceColor { get; set; }
    public Color4 LeftFaceColor { get; set; }
    public Color4 RightFaceColor { get; set; }
    private float transparency = 1.0f;  // 
    public Color4 minColor = new Color4(0.0f, 0.0f, 0.0f, 1.0f);
    public  Color4 maxColor = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }


    public Cube(Vector3 position, Color4 color)
    {
        this.position = position;
        this.color = color;
        InitializeColors();
    }

    public Cube(string filePath, Color4 frontColor, Color4 backColor, Color4 topColor, Color4 bottomColor, Color4 leftColor, Color4 rightColor)
    {
        LoadCoordinatesFromFile(filePath);
        FrontFaceColor = frontColor;
        BackFaceColor = backColor;
        TopFaceColor = topColor;
        BottomFaceColor = bottomColor;
        LeftFaceColor = leftColor;
        RightFaceColor = rightColor;
        
    }

    public void InitializeColors()
    {
        FrontFaceColor = Color4.Red;
        BackFaceColor = Color4.Green;
        TopFaceColor = Color4.Blue;
        BottomFaceColor = Color4.Yellow;
        LeftFaceColor = Color4.Pink;
        RightFaceColor = Color4.Purple;
    }
    private void LoadCoordinatesFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] coordinates = line.Split(' ');
                    if (coordinates.Length == 3)
                    {
                        float x = float.Parse(coordinates[0]);
                        float y = float.Parse(coordinates[1]);
                        float z = float.Parse(coordinates[2]);
                    }
                }
            }
        }
        else
        {
            Console.WriteLine($"Fișierul {filePath} nu există.");
        }
    }

   


    public float Transparency
    {
        get { return transparency; }
        set { transparency = MathHelper.Clamp(value, 0.0f, 1.0f); }
    }

    public void RandomizeColors()
    {
        Random rand = new Random();

        FrontFaceColor = new Color4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1.0f);
        BackFaceColor = new Color4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1.0f);
        TopFaceColor = new Color4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1.0f);
        BottomFaceColor = new Color4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1.0f);
        LeftFaceColor = new Color4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1.0f);
        RightFaceColor = new Color4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1.0f);
    }


    public void Draw()
    {
        GL.Begin(PrimitiveType.Quads);


        // Front 
        GL.Color4(FrontFaceColor);
        GL.Vertex3(Position.X - 0.5f, Position.Y - 0.5f, Position.Z - 0.5f);
        GL.Vertex3(Position.X + 0.5f, Position.Y - 0.5f, Position.Z - 0.5f);
        GL.Vertex3(Position.X + 0.5f, Position.Y + 0.5f, Position.Z - 0.5f);
        GL.Vertex3(Position.X - 0.5f, Position.Y + 0.5f, Position.Z - 0.5f);
        

        
        // Back
        GL.Color4(BackFaceColor);
        GL.Vertex3(Position.X - 0.5f, Position.Y - 0.5f, Position.Z + 0.5f);
        GL.Vertex3(Position.X + 0.5f, Position.Y - 0.5f, Position.Z + 0.5f);
        GL.Vertex3(Position.X + 0.5f, Position.Y + 0.5f, Position.Z + 0.5f);
        GL.Vertex3(Position.X - 0.5f, Position.Y + 0.5f, Position.Z + 0.5f);
       
        // Left

        
        GL.Color4(LeftFaceColor);

        GL.Vertex3(Position.X - 0.5f, Position.Y - 0.5f, Position.Z - 0.5f);
        GL.Vertex3(Position.X - 0.5f, Position.Y + 0.5f, Position.Z - 0.5f);
        GL.Vertex3(Position.X - 0.5f, Position.Y + 0.5f, Position.Z + 0.5f);
        GL.Vertex3(Position.X - 0.5f, Position.Y - 0.5f, Position.Z + 0.5f);
        
        // Right
        
        GL.Color4(RightFaceColor);

        GL.Vertex3(Position.X + 0.5f, Position.Y - 0.5f, Position.Z - 0.5f);
        GL.Vertex3(Position.X + 0.5f, Position.Y + 0.5f, Position.Z - 0.5f);
        GL.Vertex3(Position.X + 0.5f, Position.Y + 0.5f, Position.Z + 0.5f);
        GL.Vertex3(Position.X + 0.5f, Position.Y - 0.5f, Position.Z + 0.5f);
        
        // Top
        
        GL.Color4(TopFaceColor); 
        GL.Vertex3(Position.X - 0.5f, Position.Y + 0.5f, Position.Z - 0.5f);
        GL.Vertex3(Position.X + 0.5f, Position.Y + 0.5f, Position.Z - 0.5f);
        GL.Vertex3(Position.X + 0.5f, Position.Y + 0.5f, Position.Z + 0.5f);
        GL.Vertex3(Position.X - 0.5f, Position.Y + 0.5f, Position.Z + 0.5f);
        
        // Bottom
        GL.Color4(BottomFaceColor);
        GL.Vertex3(Position.X - 0.5f, Position.Y - 0.5f, Position.Z - 0.5f);
        GL.Vertex3(Position.X + 0.5f, Position.Y - 0.5f, Position.Z - 0.5f);
        GL.Vertex3(Position.X + 0.5f, Position.Y - 0.5f, Position.Z + 0.5f);
        GL.Vertex3(Position.X - 0.5f, Position.Y - 0.5f, Position.Z + 0.5f);


        GL.End();
    }

    
}

