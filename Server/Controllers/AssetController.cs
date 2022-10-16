using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.SignalR;
using Asset_Track.Shared.Models;
using Asset_Track.Shared.StoredProcedures;
using Asset_Track.Server.Hubs;


namespace Asset_Track.Server.Controllers
{
#pragma warning disable CS0472 // Possible null reference argument.
#pragma warning disable CS8602 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8629 // Nullable value type may be null.

    
    [ApiController]
	[Route("api/[controller]")]						   
    public class AssetController : ControllerBase
    {
        private readonly AssetTrackContext _context;
        private readonly IHubContext<AssetHub, IChatClient> _hubContext;

        public AssetController(AssetTrackContext context, IHubContext<AssetHub, IChatClient> hubContext)
        {
            this._context = context;
            this._hubContext = hubContext;
        }

        #region SIGNALR
        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<string>> SendEntityToGroup(spSpot_Parameter par)
        {
            await _hubContext.Clients.Group(par.receiver).ReceiveEntity(par.sender, par.receiver, par.connectionId, par.spAsset, (short)par.increment);
            return Ok("SendEntityToGroup Success...");
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<string>> SendMessageToUser(spSpot_Parameter par)
        {
            await _hubContext.Clients.Client(par.receiver).ReceiveMessage(par.sender, par.receiver, par.connectionId, par.message);
            return Ok("SendMessageToUser Success...");
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<string>> SendMessageToGroup(spSpot_Parameter par)
        {
            await _hubContext.Clients.Group(par.receiver).ReceiveMessage(par.sender, par.receiver, par.connectionId, par.message);
            return Ok("SendMessageToGroup Success...");
        }
        #endregion

        [HttpGet("Spots")]
        //LINQ Version
        public async Task<ActionResult<List<Spot>>> Spots()
        {
            List<Spot> spots = new List<Spot>();

            ////INNER JOIN
            //spots = await ( from s in _context.Spot
            //                join a in ( from g in _context.Asset
            //                            group g by g.Spot_Id into _g
            //                            select new 
            //                            {
            //                                Spot_Id = _g.Key,
            //                                Asset_Count = _g.Count()
            //                            }) on s.Spot_Id equals a.Spot_Id
            //                select new Spot
            //                {
            //                    Spot_Id = a.Spot_Id,
            //                    Spot_Title = s.Spot_Title,
            //                    Asset_Count = a.Asset_Count,
            //                }
            //                ).ToListAsync();

            //LEFT JOIN
            spots = await (from s in _context.Spot
                           join a in (from g in _context.Asset
                                      group g by g.Spot_Id into _g
                                      select new
                                      {
                                          Spot_Id = _g.Key,
                                          Asset_Count = _g.Count()
                                      }) on s.Spot_Id equals a.Spot_Id into b
                           from _a in b.DefaultIfEmpty()
                           select new Spot
                           {
                               Spot_Id = s.Spot_Id,
                               Spot_Title = s.Spot_Title,
                               Asset_Count = _a.Asset_Count == null ? 0 : _a.Asset_Count
                           }).ToListAsync();

            return await Task.FromResult(spots);
        }

        [HttpGet("Assets/{Spot_Id}")]
        public async Task<ActionResult<List<Asset>>> Assets(string Spot_Id)
        {
            var assets = await _context.Asset.Where(a => a.Spot_Id == Spot_Id).ToListAsync();
            return assets;
        }

        [HttpGet("spSpots")]
        public async Task<ActionResult<List<spSpot>>> spSpots()
        {
            List<spSpot> spots = await _context.Set<spSpot>().FromSqlRaw("EXEC dbo.spSpots").ToListAsync();

            return await Task.FromResult(spots);
        }

        [HttpGet("spAssets/{Spot_Id}")]
        public async Task<ActionResult<List<spAsset>>> spAssets(string Spot_Id)
        {
            var assets = await _context.Set<spAsset>().FromSqlRaw("EXEC dbo.spAssets {0}", Spot_Id).ToListAsync();
            return assets;
        }

        [HttpPost("spAsset_Update")]
        //Called from SIMULATOR or RFID Device
        public async Task<ActionResult<spAsset>> spAsset_Update(spSpot_Parameter par)
        {
            // UPDATE Asset.Spot_Id & Asset.Time_Stamp AND INSERT Track.Asset_Id & Track.Spot_Id & Track.Time-Stamp
            //await _context.Database.ExecuteSqlRawAsync("EXEC dbo.xpAsset_Update {0}, {1}", par.xpAsset.Asset_Id, par.xpAsset.Spot_Id);

            spAsset? asset = par.spAsset;
            List<spAsset> assets = new List<spAsset>();
            try
            {
                assets = await _context.Set<spAsset>().FromSqlRaw("EXEC dbo.spAsset_Update {0}, {1}", par.spAsset.Asset_Id, par.spAsset.Spot_Id).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}: {1}", ex.GetType().Name, ex.Message);
            }


            if (assets is not null && assets.Count > 0)
            {
                asset = assets[0];
                await _hubContext.Clients.Group(par.receiver).ReceiveEntity(par.sender, par.receiver, par.connectionId, asset, (short)par.increment);
            }

            return Ok(asset);
        }

    }
}
