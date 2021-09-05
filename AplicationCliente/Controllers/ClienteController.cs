using AplicationCliente.Data;
using AplicationCliente.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicationCliente.Controllers
{
    public class ClienteController : Controller
    {
        public readonly ApplicationDbContext _db;
        public ClienteController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Crear(int? id)
        {
            Cliente cliente = new Cliente();
            if (id==null)
            {
                return View(cliente);
            }
            else
            {
                cliente = await _db.Clientes.FindAsync(id);
                return View(cliente);
            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Crear(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (cliente.Id==0)//Crea el Registro
                {
                    await _db.Clientes.AddAsync(cliente);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Crear));
                }
                else
                {
                    _db.Clientes.Update(cliente);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Crear),new { id=0});
                }


            }
            return View(cliente);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos() 
        {
            var todos = await _db.Clientes.ToListAsync();
            return Json(new { data = todos });
        
        }

        [HttpDelete]
        public async Task<IActionResult>Delete(int id)
        {
            var clienteDb = await _db.Clientes.FindAsync(id);
            if (clienteDb==null)
            {
                return Json(new { success = false, message = "Error al Borrar" });
            }
            _db.Clientes.Remove(clienteDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Cliente borrado exitosamente" });
        }
    }
}
