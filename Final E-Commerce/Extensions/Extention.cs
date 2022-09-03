namespace Final_E_Commerce.Extensions
{
    public static class Extention
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image/");
        }

        public static bool ValidSize(this IFormFile file, int size)
        {
            return file.Length / 1024 > size;
        }

        public static string SaveImage(this IFormFile file, IWebHostEnvironment env, string folder)
        {
            string filename = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.Combine(env.WebRootPath, folder, filename);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            };
            return filename;
        }
    }
}
