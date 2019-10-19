using System.IO;


namespace RubiusTest.Helpers
{
    class IconLoader
    {
        public static byte[] GetIcon(string resourceName)
        {
            using (var stream = typeof(IconLoader).Assembly.GetManifestResourceStream($"RubiusTest.Resources.{resourceName}"))
            {
                return ReadBytes(stream);
            }
        }

        private static byte[] ReadBytes(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
