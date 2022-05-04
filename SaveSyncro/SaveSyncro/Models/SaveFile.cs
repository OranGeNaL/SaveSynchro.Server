using System.Security.Cryptography;

namespace SaveSyncro.Models;

public class SaveFile
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    
    public string CheckSum { get; set; }
    
    //Получить хэш файла
    public SaveFile() { }

    public SaveFile(string name, string path)
    {
        Name = name;
        Path = path;
        ID = Guid.NewGuid();
    }
    
    public void UpdateChecksum()
    {
        using (FileStream fs = File.OpenRead(Path))
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                byte[] checkSum = md5.ComputeHash(fs);
                string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);

                this.CheckSum = result;
                
                //Console.WriteLine(result);
            }
    }
}