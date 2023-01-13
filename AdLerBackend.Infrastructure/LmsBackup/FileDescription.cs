using System.Xml.Serialization;
#pragma warning disable CS8618

namespace AdLerBackend.Infrastructure.LmsBackup;

[XmlRoot(ElementName = "file")]
public class File
{
    [XmlElement(ElementName = "contenthash")]
    public string Contenthash { get; set; }

    [XmlElement(ElementName = "component")]
    public string Component { get; set; }

    [XmlElement(ElementName = "filename")] public string Filename { get; set; }

    [XmlElement(ElementName = "contextid")]
    public int Contextid { get; set; }
}

[XmlRoot(ElementName = "files")]
public class Files
{
    [XmlElement(ElementName = "file")] public List<File> File { get; set; }
}