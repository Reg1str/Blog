﻿using Microsoft.IdentityModel.Tokens;
using PhotoSauce.MagicScaler;

namespace Blog.Data.FileManager;

public class FileManager : IFileManager
{
    private string _imagePath;

    public FileManager(IConfiguration config)
    {
        _imagePath = config["Path:Images"];
    }

    public FileStream ImageStream(string image)
    {
        return new FileStream(Path.Combine(_imagePath, image), FileMode.Open, FileAccess.Read);
    }
    
    [Obsolete("Obsolete")]
    public async Task<string> SaveImage(IFormFile image)
    {
        try
        {



            var save_path = Path.Combine(_imagePath);
            if (!Directory.Exists(_imagePath))
            {
                Directory.CreateDirectory(save_path);
            }

            //Internet Explorer Error
            //var fileName = image.FileName;

            var mime = image.FileName.Substring(image.FileName.LastIndexOf('.'));
            var fileName = $"img_{DateTime.Now:dd-MM-yyyy-HH-mm-ss}{mime}";

            using( var fileStream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
            {
                MagicImageProcessor.ProcessImage(image.OpenReadStream(), fileStream, ImageOptions());
            }

            return fileName;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return "Error";
        }
    }

    public bool RemoveImage(string image)
    {
        try
        {
            var file = Path.Combine(_imagePath, image);
            if (File.Exists(file))
                File.Delete(file);
            return true;
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
            return false;
        }
    }

    [Obsolete("Obsolete")]
    private ProcessImageSettings ImageOptions() => new ProcessImageSettings
    {
        Width = 800,
        Height = 500,
        ResizeMode = CropScaleMode.Crop,
        SaveFormat = FileFormat.Jpeg,
        JpegQuality = 100,
        JpegSubsampleMode = ChromaSubsampleMode.Subsample420
    };
}