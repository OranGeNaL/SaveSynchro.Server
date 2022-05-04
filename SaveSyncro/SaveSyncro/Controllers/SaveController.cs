using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveSyncro.Contexts;
using SaveSyncro.Models;

namespace SaveSyncro.Controllers;

[ApiController]
[Route("api/save")]
public class SaveController : ControllerBase
{
    private SaveContext saveDb;

    private readonly ILogger<SaveController> _logger;

    public SaveController(ILogger<SaveController> logger, SaveContext context)
    {
        _logger = logger;
        saveDb = context;
    }

    [HttpGet("info/all")]
    [Authorize]
    public async Task<ActionResult<IResult>> GetSavesInfo()
    {
        var saves = saveDb.Saves.Where(x => x.Owner == HttpContext.User.Identity.Name);

        if (saves != null)
            return Ok(saves);
        
        return NotFound();
    }

    [HttpGet("info/save/{id}")]
    [Authorize]
    public async Task<ActionResult<IResult>> GetSaveInfo(string id)
    {
        var save = saveDb.Saves.FirstOrDefault(x => x.ID.ToString() == id);
        if (save != null)
            return Ok(save);
        
        return NotFound();
    }
    
    [HttpGet("info/file/{id}")]
    [Authorize]
    public async Task<ActionResult<IResult>> GetFileInfo(string id)
    {
        var file = saveDb.SaveFiles.FirstOrDefault(x => x.ID.ToString() == id);
        if (file != null)
            return Ok(file);
        
        return NotFound();
    }

    [HttpPost("new/{gameName}/{saveName}")]
    [Authorize]
    public async Task<ActionResult<IResult>> CreateNewSave(string gameName, string saveName, IFormFile? saveFile)
    {
        if (saveFile != null)
        {
            string path = string.Join('/', ConfigManager.RootDir, HttpContext.User.Identity.Name, gameName, saveName);
            
            Directory.CreateDirectory(string.Join('/', ConfigManager.RootDir, HttpContext.User.Identity.Name, gameName));

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await saveFile.CopyToAsync(fileStream);
            }

            SaveFile newFile = new SaveFile(saveName, path);
            newFile.UpdateChecksum();
            
            Console.WriteLine(newFile.ID.ToString());
            Save newSave = new Save(HttpContext.User.Identity.Name, gameName, saveName, DateTime.Now.ToUniversalTime(), newFile.ID.ToString());

            await saveDb.AddAsync(newSave);
            await saveDb.AddAsync(newFile);
            await saveDb.SaveChangesAsync();

            return Ok(newSave);
        }
        return BadRequest();
        
    }

    [HttpPut("update/{saveID}")]
    [Authorize]
    public async Task<ActionResult<IResult>> UpdateSave(string saveID, IFormFile? saveFile)
    {
        var save = saveDb.Saves.FirstOrDefault(x => x.ID.ToString() == saveID);
        if (save != null)
        {
            if (saveFile != null)
            {
                var file = saveDb.SaveFiles.FirstOrDefault(x => x.ID.ToString() == save.FileID);

                if (file != null)
                {
                    using (var fileStream = new FileStream(file.Path, FileMode.Create))
                    {
                        await saveFile.CopyToAsync(fileStream);
                    }
                    
                    save.LastUpdateTime = DateTime.Now.ToUniversalTime();
                    
                    file.UpdateChecksum();
                    
                    saveDb.Update(file);
                    saveDb.Update(save);
                    
                    return Ok(save);
                }
            }
        }

        return BadRequest();
    }
}