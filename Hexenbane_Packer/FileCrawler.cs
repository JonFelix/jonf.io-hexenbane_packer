using System.IO;
using System.Collections.Generic;

namespace Hexenbane_Packer
{
    static class FileCrawler
    {
        static public string[] GetAllFilesByExt(string ext, string loc)
        {
            List<string> _items = new List<string>();
            if(!Directory.Exists(loc))
            {
                Program.Log("ERROR: " + loc + " not found! Aborting Operation...\n", System.ConsoleColor.Red);
                return null;
            }
            _items.AddRange(CrawlFolder(loc));
            for(int i = 0; i < _items.Count; i++)
            {
                if(_items[i].Substring(_items[i].Length - ext.Length, ext.Length).ToLower() != ext)
                {
                    if(_items.Remove(_items[i]))
                    {
                        i--;
                    }
                    continue;                
                }
                _items[i] = _items[i].Substring(loc.Length, _items[i].Length - loc.Length);    
            }

            return _items.ToArray();
        }

        static string[] CrawlFolder(string loc, string[] parentItems = null)
        {
            List<string> items = new List<string>();
            
            DirectoryInfo _location = new DirectoryInfo(@loc);  

            foreach(FileInfo i in _location.GetFiles())
            {
                string _name = i.FullName;
                items.Add(_name);
                Program.Log("Found " + _name + '\n');
            }

            foreach(DirectoryInfo i in _location.GetDirectories())
            {
                items.AddRange(CrawlFolder(i.FullName, items.ToArray()));
            }

            return items.ToArray();
        }
    }
}
