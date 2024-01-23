using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK_winforms_z03
{
    public class Butterfly
    {
        private float wingRotationAngle = 0.0f;
        private float wingRotationSpeed = 0.1f;

        public void Update()
        {
            // Logica de animație: rotație periodica a aripilor
            wingRotationAngle += wingRotationSpeed;
            if (wingRotationAngle > 360.0f)
            {
                wingRotationAngle -= 360.0f;
            }
        }

        
    }
}
