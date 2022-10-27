using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFGraphicsEngine.Core.Rendering.Shading.Exceptions
{
    /// <summary>
    /// Occurs when shader has compilation errors
    /// </summary>
    public class ShaderCompilationException : Exception
    {
        public ShaderCompilationException(string log) : base($"Failed to compile shader: {log}")
        {

        }
    }
}
