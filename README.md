# m3cpy
A dead simple .m3u file extractor

## Usage
```
λ m3cpy.exe

Usage: m3cpy --m3u=<path-to-m3u> --out=<path-to-output-folder>
Options:
         -f, --m3u: Path to m3u file.
         -o, --out: Path to output folder.
               -rm: Attempt to remove the original once it has been copied.
         --replace: Do not replace the file if it already exists.
    -h, -?, --help: Display help.
      -v,--verbose: Toggle verbose mode.
```

## Notes
 * Does not support http:// URIs, nor any URI but file:// in fact. 
 * Should handle and warn you about filesystem rights issues
 * Not tested on GNU/Linux. The biggest issue might be that the required .Net assembly is 4.5, but Mono should handle it.

## License
m3cpy is licensed under the MIT License.
