using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ConsoleApp3
{
    class FallingObject
    {
        private List<Vector3> coordinates;
        private bool isMoving;
        private float speed;
        private float elapsedTime;
        private bool isObjectMoving = false;
        

        public FallingObject(List<Vector3> coords, float initialSpeed)
        {
            coordinates = coords;
            isMoving = true;
            speed = initialSpeed;
            elapsedTime = 0;
        }

        public void Update(float deltaTime)
        {
            if (isMoving)
            {
                elapsedTime += deltaTime;
                MoveDown(speed * elapsedTime);

                
                if (MinY() <= 0)
                {
                    isMoving = false;
                    elapsedTime = 0;
                }
            }
        }

        public void Draw(Color color)
        {
            GL.Color3(color);
            GL.Begin(PrimitiveType.Triangles);
            foreach (var vert in coordinates)
            {
                GL.Vertex3(vert);
            }
            GL.End();
        }

        public void MoveDown(float distance)
        {
            for (int i = 0; i < coordinates.Count; i++)
            {
                coordinates[i] = new Vector3(coordinates[i].X, coordinates[i].Y - distance, coordinates[i].Z);
            }
        }

        public void MoveUp(float distance)
        {
            for (int i = 0; i < coordinates.Count; i++)
            {
                coordinates[i] = new Vector3(coordinates[i].X, coordinates[i].Y + distance, coordinates[i].Z);
            }
        }

        public float MinY()
        {
            float minY = coordinates[0].Y;
            foreach (var vertex in coordinates)
            {
                minY = Math.Min(minY, vertex.Y);
            }
            return minY;
        }

        public void StartObjectMovement()
        {
            isObjectMoving = true;
        }

        public Vector3 GetObjectPosition()
        {
            
            Vector3 sum = Vector3.Zero;

            foreach (var vertex in coordinates)
            {
                sum += vertex;
            }

            return sum / coordinates.Count;
        }


    }


    
}