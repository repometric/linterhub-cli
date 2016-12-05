namespace Linterhub.Engine.Extensions
{
    using System.IO;
    using System.Text;

    public static class CommonExtensions
    {
        public static Stream GetMemoryStream(this string self)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(self));
        }
    }
}