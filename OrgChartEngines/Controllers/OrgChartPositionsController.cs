using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrgChartEngines.Data;
using OrgChartEngines.Models.OrgChart;
using OrgChartEngines.Models.Users;

namespace OrgChartEngines.Controllers
{
    public class OrgChartPositionsController : Controller
    {
        private readonly Engines_PCContext _context;
        private readonly UsersContext _usersContext;

        public OrgChartPositionsController(Engines_PCContext context, UsersContext usersContext)
        {
            _context = context;
            _usersContext = usersContext;
        }

        // GET: OrgChartPositions
        public async Task<IActionResult> Index()
        {
              return _context.OrgChartPositions != null ? 
                          View(await _context.OrgChartPositions.ToListAsync()) :
                          Problem("Entity set 'Weld_PCContext.OrgChartPositions'  is null.");
        }

        // GET: OrgChartPositions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrgChartPositions == null)
            {
                return NotFound();
            }

            var orgChartPosition = await _context.OrgChartPositions
                .FirstOrDefaultAsync(m => m.IdPosition == id);
            if (orgChartPosition == null)
            {
                return NotFound();
            }

            return View(orgChartPosition);
        }

        // GET: OrgChartPositions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrgChartPositions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPosition,Title,IdRol,IdUser,IdDepartment,PositionLevel,SuperiorPosition,IdLine,IdLineSub,Assemblies")] OrgChartPosition orgChartPosition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orgChartPosition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orgChartPosition);
        }

        // GET: OrgChartPositions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrgChartPositions == null)
            {
                return NotFound();
            }

            var orgChartPosition = await _context.OrgChartPositions.FindAsync(id);
            if (orgChartPosition == null)
            {
                return NotFound();
            }
            return View(orgChartPosition);
        }

        // POST: OrgChartPositions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPosition,Title,IdRol,IdUser,IdDepartment,PositionLevel,SuperiorPosition,IdLine,IdLineSub,Assemblies")] OrgChartPosition orgChartPosition)
        {
            if (id != orgChartPosition.IdPosition)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orgChartPosition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrgChartPositionExists(orgChartPosition.IdPosition))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(orgChartPosition);
        }

        // GET: OrgChartPositions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrgChartPositions == null)
            {
                return NotFound();
            }

            var orgChartPosition = await _context.OrgChartPositions
                .FirstOrDefaultAsync(m => m.IdPosition == id);
            if (orgChartPosition == null)
            {
                return NotFound();
            }

            return View(orgChartPosition);
        }

        // POST: OrgChartPositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrgChartPositions == null)
            {
                return Problem("Entity set 'Weld_PCContext.OrgChartPositions'  is null.");
            }
            var orgChartPosition = await _context.OrgChartPositions.FindAsync(id);
            if (orgChartPosition != null)
            {
                _context.OrgChartPositions.Remove(orgChartPosition);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrgChartPositionExists(int id)
        {
          return (_context.OrgChartPositions?.Any(e => e.IdPosition == id)).GetValueOrDefault();
        }
        [HttpPost]
        public JsonResult getUsers(int positionId)
        {
            var users = _usersContext.AspNetUsers.ToList();
            var org = _context.PositionUsers.ToList();
            var data = org.Join(users, pos => pos.IdUser, us => us.Id, (pos, us) => new { pos, us })
                .Where(w => w.pos.IdPosition == positionId && w.us.UserActive == true)
                .Select(s => new { user = s.us.FirstName + " " + s.us.LastName, idPositionUser = s.pos.IdPositionUser })
                .ToList();

            return Json(data);
        }
        [HttpPost]
        public JsonResult getPosition(int departmentId)
        {
            var org = _context.OrgChartPositions.ToList();
            var user = _usersContext.AspNetUsers.ToList();

            var data2 = _context.OrgChartPositions
                 .Where(w => w.IdDepartment == departmentId)
                 .Select(s => new { Id = s.IdPosition, Title = s.Title, PositionLevel = s.PositionLevel, SuperiorPosition = s.SuperiorPosition, IdDepartment = s.IdDepartment, IdLine =  s.IdLine, IdRol = s.IdRol })
                 .OrderBy(o => o.PositionLevel).ThenBy(o => o.IdLine).ToList();

            return Json(data2);
        }
        public JsonResult addPosition(string title, int departmentId, string rolid, int lineid, int positionlevel, int SuperiorPosition)
        {
            OrgChartPosition positions = new OrgChartPosition();
            positions.Title = title;
            positions.IdDepartment = departmentId;
            positions.IdRol = rolid;
            positions.IdLine = lineid;
            positions.PositionLevel = positionlevel;
            positions.SuperiorPosition = SuperiorPosition;
            _context.OrgChartPositions.Add(positions);
            _context.SaveChanges();

            var data = 1;

            return Json(data);
        }

        public JsonResult editPosition(int id, string title, int departmentId, string rolid, int lineid, int positionlevel, int SuperiorPosition, string assembly)
        {
            
            OrgChartPosition positions = _context.OrgChartPositions.Find(id);
            positions.Title = title;
            positions.IdDepartment = departmentId;
            positions.IdRol = rolid;
                positions.IdLine = lineid;
            positions.PositionLevel = positionlevel;
            positions.SuperiorPosition = SuperiorPosition;
            positions.Assemblies = assembly;
            _context.Entry(positions).State = EntityState.Modified;
            _context.SaveChanges();


            var data = 1;

            return Json(data);
        }
        public ActionResult addPosition_Modal(int iddepartment, int level, int idposition)
        {
            var rol = _usersContext.AspNetRoles.Select(s => new { id = s.Id, name = s.Name }).ToList();
            var org = _context.OrgChartPositions.ToList();
            var user = _usersContext.AspNetUsers.Select(s => new { id = s.Id, name = s.FirstName + " " + s.LastName }).ToList();

            var line = _context.Lines.Select(s => new { id = s.line_id, name = s.line_name }).ToList();

            if (idposition == 0)
            {
                var userlst = _usersContext.AspNetUsers.ToList();
                var position = org.
                GroupJoin(user, pos => pos.IdUser, us => us.id, (pos, us) => new { pos, us })
                .Where(w => w.pos.IdDepartment == iddepartment && w.pos.IdPosition == idposition).OrderBy(o => o.pos.PositionLevel)
                //.SelectMany(temp => temp.us.Join(db.AspNetUserRoles,usr => usr.Id, sr => sr.UserId,(usr,sr)=> new {usr,sr })
                .SelectMany(temp => temp.us.DefaultIfEmpty(),
                (temp, s) => new { id = temp.pos.IdPosition, name = temp.pos.Title + " - " + (s == null ? "" : s.name) }).ToList();
                ViewBag.Id = idposition;
                ViewBag.SuperiorPosition = new SelectList(position, "id", "name");
                ViewBag.Level = level;
            }
            else
            {
                var position = org.
                                GroupJoin(user, pos => pos.IdUser, us => us.id, (pos, us) => new { pos, us })
                                .Where(w => w.pos.IdDepartment == iddepartment && w.pos.IdPosition == idposition).OrderBy(o => o.pos.PositionLevel)
                                //.SelectMany(temp => temp.us.Join(db.AspNetUserRoles,usr => usr.Id, sr => sr.UserId,(usr,sr)=> new {usr,sr })
                                .SelectMany(temp => temp.us.DefaultIfEmpty(),
                                (temp, s) => new { id = temp.pos.IdPosition, name = temp.pos.Title + " - " + (s == null ? "" : s.name) }).ToList();
                ViewBag.Id = idposition;
                ViewBag.SuperiorPosition = new SelectList(position, "id", "name");
                ViewBag.Level = level + 1;
            }




            ViewBag.Rol = new SelectList(rol, "id", "name");
            ViewBag.User = new SelectList(user, "id", "name");
            ViewBag.Line = new SelectList(line, "id", "name");

            ViewBag.Department = iddepartment;

            //ViewBag.idMachine = idMachine;
            //var listmachine = db.Machines.ToList().Select(a => new
            //{
            //    id = a.Id_Machine,
            //    name = a.Name,
            //    asset = a.AssetNumber,
            //    Description = string.Format("{0} - {1}", a.Name, a.AssetNumber)
            //}).ToList();

            //ViewBag.Machine = new SelectList(listmachine, "id", "Description");
            return PartialView();
        }

        public ActionResult EditPosition_Modal(int iddepartment, int level, int idposition)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            OrgChartPosition orgChartPositions = _context.OrgChartPositions.Find(idposition);
            
            //ViewBag.IdUser = new SelectList(db.AspNetUsers, "Id", "Email", orgChart_Positions.IdUser);
            //ViewBag.IdDepartment = new SelectList(db.Departaments, "Id", "DepartamentName", orgChart_Positions.IdDepartment);
            var rol = _usersContext.AspNetRoles.Select(s => new { id = s.Id, name = s.Name }).ToList();
            var org = _context.OrgChartPositions.ToList();
            var user = _usersContext.AspNetUsers.Select(s => new { id = s.Id, name = s.FirstName + " " + s.LastName }).ToList();
            var line = _context.Lines.Select(s => new { id = s.line_id, name = s.line_name }).ToList();


            var position = org.
                            GroupJoin(user, pos => pos.IdUser, us => us.id, (pos, us) => new { pos, us })
                            .Where(w => w.pos.IdDepartment == iddepartment).OrderBy(o => o.pos.PositionLevel)
                            //.SelectMany(temp => temp.us.Join(db.AspNetUserRoles,usr => usr.Id, sr => sr.UserId,(usr,sr)=> new {usr,sr })
                            .SelectMany(temp => temp.us.DefaultIfEmpty(),
                            (temp, s) => new { id = temp.pos.IdPosition, name = temp.pos.Title + " - " + (s == null ? "" : s.name) }).ToList();
            ViewBag.Id = idposition;
            ViewBag.SuperiorPosition = new SelectList(position, "id", "name", orgChartPositions.SuperiorPosition);




            ViewBag.RolId = orgChartPositions.IdRol == "" ? "0" : orgChartPositions.IdRol;
            ViewBag.TitlePosition = orgChartPositions.Title;
            ViewBag.User = new SelectList(user, "id", "name", orgChartPositions.IdUser);
            ViewBag.Rol = new SelectList(rol, "id", "name", orgChartPositions.IdRol);
            
                ViewBag.Line = new SelectList(line, "id", "name", orgChartPositions.IdLine);
            

            ViewBag.Level = level;
            ViewBag.Department = iddepartment;

            //ViewBag.idMachine = idMachine;
            //var listmachine = db.Machines.ToList().Select(a => new
            //{
            //    id = a.Id_Machine,
            //    name = a.Name,
            //    asset = a.AssetNumber,
            //    Description = string.Format("{0} - {1}", a.Name, a.AssetNumber)
            //}).ToList();

            //ViewBag.Machine = new SelectList(listmachine, "id", "Description");
            return PartialView();
        }
        public ActionResult deletePosition_Modal(int iddepartment, int level, int idposition)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            OrgChartPosition orgChart_Positions = _context.OrgChartPositions.Find(idposition);
            var org = _context.OrgChartPositions.ToList();
            var user = _usersContext.AspNetUsers.Where(w => w.Id == orgChart_Positions.IdUser).Select(s => s.FirstName + " " + s.LastName).FirstOrDefault();
            var line = _context.Lines.Where(w => w.line_id == orgChart_Positions.IdLine).Select(s => s.line_name).FirstOrDefault();
            


            var department = _context.Departaments.Where(w => w.Id == orgChart_Positions.IdDepartment).Select(s => s.DepartamentName).FirstOrDefault();

            var position = org.
                            GroupJoin(_usersContext.AspNetUsers, pos => pos.IdUser, us => us.Id, (pos, us) => new { pos, us })
                            .Where(w => w.pos.SuperiorPosition == orgChart_Positions.SuperiorPosition).OrderBy(o => o.pos.PositionLevel)
                            //.SelectMany(temp => temp.us.Join(db.AspNetUserRoles,usr => usr.Id, sr => sr.UserId,(usr,sr)=> new {usr,sr })
                            .SelectMany(temp => temp.us.DefaultIfEmpty(),
                            (temp, s) => temp.pos.Title + " - " + (s == null ? "" : s.FirstName) + " " + (s == null ? "" : s.LastName)).FirstOrDefault();
            ViewBag.Id = idposition;
            ViewBag.SuperiorPosition = position;
            ViewBag.TitlePosition = orgChart_Positions.Title;
            ViewBag.User = user;
            ViewBag.Line = line;
            ViewBag.Level = level;
            ViewBag.Department = department;
            ViewBag.IdDepartment = iddepartment;
            
            return PartialView();
        }
        public JsonResult deletePosition(int id)
        {

            var positionUser = _context.PositionUsers.Where(w => w.IdPosition == id).ToList();
            if (positionUser.Count > 0)
            {
                _context.PositionUsers.RemoveRange(positionUser);
            }


            OrgChartPosition orgChart_Positions = _context.OrgChartPositions.Find(id);

            _context.OrgChartPositions.Remove(orgChart_Positions);
            _context.SaveChanges();

            var data = 1;

            return Json(data);
        }
        public ActionResult addUser_Modal(int idposition, string idrol)
        {
            string rolposition = _context.OrgChartPositions
                .Where(w => w.IdPosition == idposition)
                .Select(s => s.Title + " " + (s.IdLine == 0 ? "All Lines" : "Line" + s.IdLine)).FirstOrDefault();
            var user = _usersContext.AspNetUsers.Include(include => include.Roles)
                .Where(w => w.Roles.Select(s => s.Id).Contains(idrol))
                .Select(s => new { id = s.Id, name = s.FirstName + " " + s.LastName }).ToList();
            
            ViewBag.Rol = rolposition;
            ViewBag.User = new SelectList(user, "id", "name");
            ViewBag.IdPosition = idposition;
            ViewBag.IdRol = idrol;

            return PartialView();
        }
        public JsonResult addUser(int positionId, string[] userid, int shiftid)
        {
            foreach (var user in userid)
            {
                PositionUser positionsUser = new PositionUser();
                positionsUser.IdPosition = positionId;
                positionsUser.IdUser = user;
                positionsUser.Shift = shiftid;
                _context.PositionUsers.Add(positionsUser);
                _context.SaveChanges();
            }
            var data = 1;

            return Json(data);
        }

        public ActionResult EditUser_Modal(int idpositionuser, string idrol, int idposition)
        {

            PositionUser positionUser = _context.PositionUsers.Find(idpositionuser);
            List<string> shifts = new List<string>();
            shifts.Add("1");
            shifts.Add("2");

            string rolposition = _context.OrgChartPositions
                .Where(w => w.IdPosition == idposition)
                .Select(s => s.Title + " " + (s.IdLine == 0 ? "All Lines" : "Line" + s.IdLine)).FirstOrDefault();
            var user = _usersContext.AspNetUsers.Include(x => x.Roles)
                .Where(w => w.Roles.Select(s => s.Id).Contains( idrol))
                .Select(s => new { id = s.Id, name = s.FirstName + " " + s.LastName }).ToList();

            ViewBag.Shift = new SelectList(shifts, positionUser.Shift);
            ViewBag.User = new SelectList(user, "id", "name", positionUser.IdUser);
            ViewBag.Rol = rolposition;
            ViewBag.IdPosition = idposition;
            ViewBag.IdPositionUser = idpositionuser;
            ViewBag.IdRol = idrol;

            return PartialView();
        }

        public JsonResult editUser(int idpositionuser, string userid, int shiftid)
        {
            PositionUser positionuser = _context.PositionUsers.Find(idpositionuser);
            positionuser.Shift = shiftid;
            positionuser.IdUser = userid;
            _context.Entry(positionuser).State = EntityState.Modified;
            _context.SaveChanges();
            var data = 1;

            return Json(data);
        }

        public ActionResult deleteUser_Modal(int idpositionuser, string idrol)
        {
            PositionUser positionUser = _context.PositionUsers.Find(idpositionuser);
            string rolposition = _context.OrgChartPositions
                .Where(w => w.IdPosition == positionUser.IdPosition)
                .Select(s => s.Title + " " + (s.IdLine == 0 ? "All Lines" : "Line" + s.IdLine)).FirstOrDefault();
            var user = _usersContext.AspNetUsers.Where(w => w.Id == positionUser.IdUser).Select(s => s.FirstName + " " + s.LastName).FirstOrDefault();
            
            ViewBag.IdPositionUser = idpositionuser;
            ViewBag.User = user;
            ViewBag.Rol = rolposition;
            ViewBag.IdPosition = positionUser.IdPosition;
            ViewBag.Shift = positionUser.Shift;
            ViewBag.IdRol = idrol;

            return PartialView();
        }

        public JsonResult deleteUser(int id)
        {

            PositionUser positionUser = _context.PositionUsers.Find(id);
            _context.PositionUsers.Remove(positionUser);
            _context.SaveChanges();

            var data = 1;

            return Json(data);
        }
    }
}
