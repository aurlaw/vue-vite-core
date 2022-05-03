using System;
using System.Collections.Generic;

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VueViteCore
{
    public class Manifest
    {
        private readonly string rootPath;
        public Manifest(IWebHostEnvironment _environment)
        {
            rootPath = _environment.WebRootPath;
        }


        public ManifestFile? GetManifest()
        {
            var manifest = Path.Combine(rootPath, "dist/manifest.json");
            if(!File.Exists(manifest))
            {
                return null;
            }
            using (var file = File.Open(manifest, FileMode.Open)) {
                using (var stream = new StreamReader(file))
                {
                    var data = stream.ReadToEnd();
                    return JsonSerializer.Deserialize<ManifestFile>(data);

                }
            }
        }
    }


    public partial class ManifestFile
    {
        [JsonPropertyName("src/main.js")]
        public SrcMainJs? SrcMainJs { get; set; }
    }

    public partial class SrcMainJs
    {
        [JsonPropertyName("file")]
        public string? JsFile { get; set; }

        [JsonPropertyName("css")]
        public string[]? Css { get; set; }

        public string? CssFile => Css?.FirstOrDefault();

    }


}
