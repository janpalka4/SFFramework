using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFGUIFramework.Core.Shaders
{
    public static class StandardShaders
    {

        public static string Vertex = "#version 330 core \n" +
                                      "layout(location = 0) in vec3 pos;\n" +
                                      "layout(location = 1) in vec2 uv;\n" +
                                      "out vec2 texCoord;\n" +
                                      "void main(void){\n" +
                                      " texCoord = uv;" +
                                      " gl_Position = vec4(pos, 1.0);" +
                                      "}";

        public static string Fragment = "#version 330 core \n" +
            "out vec4 outputColor;" +
            "in vec2 texCoord;" +
            "uniform sampler2D texture0;" +
            "void main()" +
            "{" +
            " outputColor = texture(texture0, texCoord);" +
            "}";
    }
}
