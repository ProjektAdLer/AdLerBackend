using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Exceptions.LMSBackupProcessor;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.World;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;

namespace AdLerBackend.Infrastructure.LmsBackup;

public class LmsBackupProcessor : ILmsBackupProcessor
{
    public IList<H5PDto> GetH5PFilesFromBackup(Stream backupFile)
    {
        var filesDescriptionStream = GetFileDescriptionFromTarStream(backupFile, "files.xml");
        var filesDescription = DeserializeToObject<Files>(filesDescriptionStream);

        var h5PFiles = StoreH5PFiles(backupFile, filesDescription);
        return h5PFiles.Select(h5PFile => new H5PDto
        {
            H5PFile = h5PFile.FileStream,
            H5PFileName = Path.GetFileNameWithoutExtension(h5PFile.FileName)
        }).ToList();
    }

    public WorldAtfResponse GetWorldDescriptionFromBackup(Stream dslStream)
    {
        dslStream.Position = 0;

        var retVal = JsonSerializer.Deserialize<WorldAtfResponse>(dslStream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new LmsBackupProcessorException("Could not deserialize DSL file");

        return retVal;
    }

    private List<H5PFile> StoreH5PFiles(Stream backupFile, Files filesDescription)
    {
        var h5PFiles = new List<H5PFile>();
        foreach (var file in filesDescription.File)
            if (file.Component == "mod_h5pactivity" && file.Filename != ".")
            {
                // Moodle Stores the H5P Files twice, so we remove one of them
                if (h5PFiles.Any(x => x.ContentHash == file.Contenthash)) continue;
                var h5PFile = new H5PFile
                {
                    FileName = file.Filename,
                    ContentHash = file.Contenthash,
                    FileStream = GetFileDescriptionFromTarStream(backupFile,
                        $"files/{file.Contenthash.AsSpan(0, 2)}/{file.Contenthash}")
                };
                h5PFiles.Add(h5PFile);
            }

        return h5PFiles;
    }

    private T DeserializeToObject<T>(Stream file) where T : class
    {
        try
        {
            file.Position = 0;
            var xmlSerializer = new XmlSerializer(typeof(T));

            var obj = (T) xmlSerializer.Deserialize(file)! ?? throw new LmsBackupProcessorException();

            return obj;
        }
        catch (Exception e)
        {
            throw new LmsBackupProcessorException("Could not deserialize file for " + nameof(T), e);
        }
    }

    private Stream GetFileDescriptionFromTarStream(Stream backupFile, string fileName)
    {
        var tarStream = GetTarInputStream(backupFile);
        while (tarStream.GetNextEntry() is { } te)
            if (te.Name == fileName)
            {
                Stream fs = new MemoryStream();
                tarStream.CopyEntryContents(fs);
                fs.Position = 0;
                return fs;
            }

        throw new NotFoundException(fileName + " not found in backup");
    }

    private static TarInputStream GetTarInputStream(Stream backupFile)
    {
        backupFile.Position = 0;
        Stream source = new GZipInputStream(backupFile);

        return new TarInputStream(source, Encoding.Default);
    }

    private class H5PFile
    {
        public Stream FileStream { get; set; }
        public string FileName { get; init; }
        public string ContentHash { get; init; }
    }
}