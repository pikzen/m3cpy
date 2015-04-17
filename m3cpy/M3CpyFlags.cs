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

namespace M3Cpy
{
    struct M3CpyFlags
    {
        /// <summary>
        /// If set to true, display the help for m3cpy
        /// </summary>
        public bool ShowHelp;

        /// <summary>
        /// If set to true, attempt to erase the originals once copied.
        /// </summary>
        public bool TryErase;

        /// <summary>
        /// Enable verbose console output
        /// </summary>
        public bool Verbose;

        /// <summary>
        /// Overwrite the file if it already exists.
        /// </summary>
        public bool Overwrite;

        /// <summary>
        /// Path for the .m3u file (may be relative or absolute)
        /// </summary>
        public string M3uPath;

        /// <summary>
        /// Path for the output folder (may be relative or absolute)
        /// </summary>
        public string OutputPath;

        /// <summary>
        /// Create an M3CpyFlags struct with default values.
        /// </summary>
        public M3CpyFlags()
        {
            ShowHelp = false;
            TryErase = false;
            Verbose = false;
            Overwrite = false;
            M3uPath = "";
            OutputPath = "";
        }
    }
}
