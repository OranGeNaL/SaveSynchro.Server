using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveSyncro.Contexts;
using SaveSyncro.Models;

namespace SaveSyncro.Controllers;

[ApiController]
[Route("api/save")]
public class SaveController : ControllerBase
{
    private SaveContext db;

    private readonly ILogger<SaveController> _logger;

    public SaveController(ILogger<SaveController> logger, SaveContext context)
    {
        _logger = logger;
        db = context;
    }

    [HttpGet("info/all")]
    [Authorize]
    public async Task<ActionResult<IResult>> GetSavesInfo()
    {
        var saves = db.Saves.Where(x => x.Owner.Login == HttpContext.User.Identity.Name);
        if (saves != null)
            return Ok(saves);
        
        return NotFound();
    }

    [HttpGet("info/{id}")]
    [Authorize]
    public async Task<ActionResult<IResult>> GetSaveInfo(string id)
    {
        var save = db.Saves.FirstOrDefault(x => x.ID.ToString() == id);
        if (save != null)
            return Ok(save);
        
        return NotFound();
    }

    [HttpPost("new")]
    [Authorize]
    public async Task<ActionResult<IResult>> CreateNewSave(Save save)
    {
        return BadRequest();
    }

    [HttpPost("new/{gameName}/{saveName}")]
    [Authorize]
    public async Task<ActionResult<IResult>> CreateNewSave(string gameName, string saveName, IFormFile saveFile)
    {
        if (saveFile != null)
        {
            // путь к папке Files
            string path = string.Join('/', ConfigManager.RootDir, HttpContext.User.Identity.Name, gameName, saveName);
            
            // сохраняем файл в папку Files в каталоге wwwroot
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await saveFile.CopyToAsync(fileStream);
            }
            
            Console.WriteLine(path);
            // FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
            // _context.Files.Add(file);
            // _context.SaveChanges();
        }
        
        
        return Ok();
    }
}