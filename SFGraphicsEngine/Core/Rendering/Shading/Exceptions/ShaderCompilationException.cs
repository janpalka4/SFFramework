using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFGraphicsEngine.Core.Rendering.Shading.Exceptions
{
    public class ShaderCompilationException : Exception
    {
        public ShaderCompilationException(string log) : base($"Failed to compile shader: {log}")
        {

        }
    }
}
