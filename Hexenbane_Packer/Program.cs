namespace Hexenbane_Packer
{
    class Program
    {
        public const string OUTPUT_LOCATION = @"";
        public const string IMAGE_EXTENSION = "png";  
        public const string DATA_EXTENSION = "hxb";                                                                                  

        static void Main(string[] args)
        {
            Log("Starting Processing Images...\n");
            string[] images = FileCrawler.GetAllFilesByExt("png", System.IO.Directory.GetCurrentDirectory() + @"\Sprites\");
            if(FileProcessor.CreateFile("ImageData", System.IO.Directory.GetCurrentDirectory() + @"\Sprites\", images))
            {
                Log(" Processing Images Complete!\n", System.ConsoleColor.Green);
            }
            System.Console.ReadKey(true);
        }

        static public void Log(string msg, System.ConsoleColor? color = null)
        {
            System.ConsoleColor _defaultColor = System.Console.ForegroundColor;
            if(color != null)
            {
                System.Console.ForegroundColor = (System.ConsoleColor)color;
            }                                                 
            System.Console.Write(msg);
            System.Console.ForegroundColor = _defaultColor;                                 
        }
    }
}
