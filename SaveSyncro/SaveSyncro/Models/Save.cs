using System.ComponentModel.DataAnnotations;

namespace SaveSyncro.Models;

public class Save
{
    [Key]
    public Guid ID { get; set; }
    public User Owner { get; set; }
    public string GameName { get; set; }
    public string SaveName { get; set; }
    public DateTime LastUpdateTime { get; set; }
    public SaveFile Data { get; set; }
    
    public Save() { }

    public Save(User owner, string gameName, string saveName, DateTime lastUpdateTime, SaveFile data)
    {
        Owner = owner;
        GameName = gameName;
        SaveName = saveName;
        LastUpdateTime = lastUpdateTime;
        Data = data;
        ID = new Guid();
    }
}