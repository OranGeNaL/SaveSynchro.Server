namespace SaveSyncro.Models;

public class SaveFile
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    
    //Получить хэш файла
    public SaveFile() { }

    public SaveFile(string name, string path)
    {
        Name = name;
        Path = path;
        ID = new Guid();
    }
}