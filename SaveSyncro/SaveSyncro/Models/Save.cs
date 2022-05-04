using System.ComponentModel.DataAnnotations;

namespace SaveSyncro.Models;

public class Save
{
    [Key]
    public Guid ID { get; set; }
    public string Owner { get; set; }
    public string GameName { get; set; }
    public string SaveName { get; set; }
    public DateTime LastUpdateTime { get; set; }
    public string FileID { get; set; }
    
    public Save() { }

    public Save(string owner, string gameName, string saveName, DateTime lastUpdateTime, string fileId)
    {
        Owner = owner;
        GameName = gameName;
        SaveName = saveName;
        LastUpdateTime = lastUpdateTime;
        FileID = fileId;
        ID = Guid.NewGuid();
    }
}