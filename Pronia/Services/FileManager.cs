namespace Pronia.Services
{
    public static class FileManager
    {

        public static string ImagesPath = "C:\\Users\\l novbe\\Desktop\\Pronia\\Pronia\\wwwroot\\images\\";



        public static string Save(IFormFile file)
        {
            string filename = Guid.NewGuid().ToString() + "." + new FileInfo(file.FileName).Extension;
            FileStream fs = new FileStream(Path.Combine(ImagesPath, filename), FileMode.Create);

            file.CopyTo(fs);

            return filename;
        }


        public static void Delete(string filename)
        {
            try
            {

                File.Delete(Path.Combine(ImagesPath, filename));


            }catch (Exception ex)
            {
                //if something happens then log and give the background worker to handle or do not nothing like me :))
            }
        }

    }
}
