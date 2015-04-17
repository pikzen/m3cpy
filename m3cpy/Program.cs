// The MIT License (MIT)
//
// Copyright (c) 2015, Florian EULA
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using System.Web;

namespace M3Cpy
{
    class Program
    {
        static void Main(string[] args)
        {
            M3CpyFlags flags = new M3CpyFlags();

            // Parse the options and setup the M3CpyFlags struct for later use.
            var opt = new NDesk.Options.OptionSet()
            {
                {"f|m3u=",   v => flags.M3uPath    = (v)},
                {"o|out=",   v => flags.OutputPath = (v)},
                {"h|?|help", v => flags.ShowHelp   = (v != null)},
                {"rm",       v => flags.TryErase   = (v != null)},
                {"v|verbose",v => flags.Verbose    = (v != null)},
                {"replace",v => flags.Overwrite    = (v != null)}
            };
            opt.Parse(args);            

            // First sanity checks, ensure that some arguments were passed and that they're not empty.
            if (args.Length == 0 || flags.M3uPath == "" || flags.OutputPath == "")
                flags.ShowHelp = true;

            if (flags.ShowHelp)
                DisplayHelp();
            else
                ReadM3UAndCopy(flags);
        }

        /// <summary>
        /// Read the files from a .m3u and apply the appropriate operation on each one.
        /// </summary>
        /// <param name="settings">Settings to use.</param>
        static void ReadM3UAndCopy(M3CpyFlags settings)
        {
            // Check if both handles exist
            if (!Directory.Exists(settings.OutputPath))
            {
                Console.WriteLine("The folder '{0}' does not exist. Aborting.", settings.OutputPath);
                return;
            }
            if (!File.Exists(settings.M3uPath))
            {
                Console.WriteLine("The file '{0}' does not exist. Aborting.", settings.M3uPath);
                return;
            }
            
            // Ensure we have a trailing /
            if (settings.OutputPath[settings.OutputPath.Length - 1] != '/')
                settings.OutputPath += '/';

            // Parse line per line, copying files as it goes.
            using (StreamReader m3uHandle = File.OpenText(settings.M3uPath))
            {
                int filesCopied = 0;
                int filesTotal = 0;
                bool res = false;
                string line = "";
                while (!m3uHandle.EndOfStream)
                {
                    line = m3uHandle.ReadLine();
                    // Ignore comment lines and empty lines
                    if (line.Length == 0 || 
                        (line.Length != 0 && line[0] == '#'))
                        continue; 

                    res = HandleSingleFile(line, settings);
                    if (res) filesCopied++;
                    filesTotal++;
                }

                Console.WriteLine("Operation successful on {0}/{1} files", filesCopied, filesTotal);
            }
        }

        /// <summary>
        /// Apply the appropriate operations on a single file from its path (as a string).
        /// </summary>
        /// <param name="line">Path to the original file.</param>
        /// <param name="settings">Settings to use.</param>
        /// <returns></returns>
        static bool HandleSingleFile(string line, M3CpyFlags settings)
        {
            // File.Copy does not handle http:// links, and we don't want to support them.
            // TODO: support them ?
            if (line.StartsWith("http://"))
            {
                Console.WriteLine("HTTP URIs are not supported. ({0})", line);
                return false;
            }

            string log = "";
            // Remove file:/// URIs because File.Copy chokes on them.
            string file = HttpUtility.UrlDecode(line).Replace("file:///", "")
                                                     .Replace("file://", "");
            FileInfo finfo = new FileInfo(file);
            string fullpath = settings.OutputPath + finfo.Name;

            // Check for an already existing file in case the user does not want to overwrite
            // Otherwise, File.Copy throws an IOException if we don't overwrite.
            if (File.Exists(fullpath) && !settings.Overwrite)
            {
                Console.WriteLine("{0} already exists, skipping (ow is {1})", finfo.Name, settings.Overwrite);
                return true; // Consider it as a success, we can assume it's gonna be 99% of the time.
            }

            File.Copy(file, fullpath, settings.Overwrite);
            log += (String.Format("{0} copied", finfo.Name));

            // Deletes the original if the user wants to.
            if (settings.TryErase)
            {
                File.Delete(file);
                log += ", deleted";
            }

            if (settings.Verbose)
                Console.WriteLine(log);

            return true;
        }

        /// <summary>
        /// Display the help message for m3cpy
        /// </summary>
        static void DisplayHelp()
        {
            Console.WriteLine(@"
Usage: m3cpy --m3u=<path-to-m3u> --out=<path-to-output-folder>
Options:
         -f, --m3u: Path to m3u file.
         -o, --out: Path to output folder.
               -rm: Attempt to remove the original once it has been copied.
         --replace: Do not replace the file if it already exists.
    -h, -?, --help: Display help.
      -v,--verbose: Toggle verbose mode.");
        }
    }
}
