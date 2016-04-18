using System.IO;
using System.Collections.Generic;
using System.Text;
using System;

namespace Hexenbane_Packer
{
    static class FileProcessor
    {
        static public bool CreateFile(string name, string location, string[] content)
        {
            if(content == null)
            {
                return false;
            }
            if(InitializeFile(Program.OUTPUT_LOCATION + name + "." + Program.DATA_EXTENSION))
            {
                Program.Log("Compiling data...\n");
                byte[] _data = ProcessData(content, location);
                if(_data.Length > 0)
                {
                    Program.Log("Data compiled!\n");

                    if(FinalizeFile(Program.OUTPUT_LOCATION + name + "." + Program.DATA_EXTENSION, _data))
                    {
                        Program.Log(name + "." + Program.DATA_EXTENSION + " Complete!", ConsoleColor.Green);
                    }
                }
            }
            return true;
        }

        static byte[] ReadFile(string file)
        {
            byte[] _buffer = null;
            try
            {   
                FileStream _fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                BinaryReader _binStream = new BinaryReader(_fileStream);
                long _totalSize = new FileInfo(file).Length;
                _buffer = _binStream.ReadBytes((int)_totalSize);
            }
            catch(Exception e)
            {
                Program.Log("ERROR: Could not read file!\n" + e.Message, ConsoleColor.Red);
                
            }
            return _buffer;
        }

        static bool InitializeFile(string file)
        {
            if(File.Exists(file))
            {
                Program.Log("ERROR: " + file + " already exists!\nAborting operation...\n", ConsoleColor.Red);
                return false;
            }
            return true;
        }

        static byte[] ProcessData(string[] content, string location)
        {       
            byte[] _packMeta;
            List<byte> _packHeader = new List<byte>();
            List<byte> _packBody = new List<byte>();
            int _headerOffset = 0;
            int _ignoredFiles = 0;
            ASCIIEncoding _encoding = new ASCIIEncoding();

            for(int i = 0; i < content.Length; i++)
            {
                if(!File.Exists(location + "\\" + content[i]))
                {
                    Program.Log("ERROR: " + content[i] + " was not found! Ignoring file.\n", ConsoleColor.Red);
                    _ignoredFiles++;
                    continue;
                }
                byte[] _body = ReadFile(location + content[i]);
                if(_body == null)
                {
                    Program.Log("ERROR: While reading " + content[i] + "! Ignoring file.\n", ConsoleColor.Red);
                    _ignoredFiles++;
                    continue;
                }
                _packBody.AddRange(_body);
                byte[] itemNameBytes = _encoding.GetBytes(content[i]);
                _packHeader.AddRange(BitConverter.GetBytes(itemNameBytes.Length));
                _packHeader.AddRange(BitConverter.GetBytes(_body.Length));
                _packHeader.AddRange(itemNameBytes);
            }
            _packMeta = BitConverter.GetBytes(content.Length - _ignoredFiles);
            _headerOffset = _packHeader.Count + _packMeta.Length;
            _headerOffset = BitConverter.GetBytes(_headerOffset).Length + 9;

            List<byte> _completeFile = new List<byte>();
            _completeFile.AddRange(BitConverter.GetBytes(_headerOffset));
            _completeFile.AddRange(_packMeta);
            _completeFile.AddRange(_packHeader);
            _completeFile.AddRange(_packBody);
            return _completeFile.ToArray();

        }

        static bool FinalizeFile(string file, byte[] content)
        {
            try
            {
                FileStream _fileStream = new FileStream(file, FileMode.Create, FileAccess.Write);
                _fileStream.Write(content, 0, content.Length);
                _fileStream.Close();
                return true; 
            }
            catch(Exception e)
            {
                Program.Log("ERROR: Could not Create file! Aborting operation...\n" + e.Message, ConsoleColor.Red);
                return false;
            }
        }
    }
}
