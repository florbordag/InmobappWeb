﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_.Net_Core.Controllers
{
    public class InquilinosController : Controller
    {
		private readonly IRepositorio<Inquilino> repositorio;

		public InquilinosController(IRepositorio<Inquilino> repositorio)
		{
			this.repositorio = repositorio;
		}

        // GET: Inquilino
        [Authorize]
        public ActionResult Index()
        {
			var lista = repositorio.ObtenerTodos();
			return View(lista);
        }

        // GET: Inquilino/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Inquilino/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino propietario)
        {
            try
            {
                TempData["Nombre"] = propietario.Nombre;
				if (ModelState.IsValid)
				{
					repositorio.Alta(propietario);
					return RedirectToAction(nameof(Index));
				}
				else
					return View();
			}
            catch
            {
                return View();
            }
        }

        // GET: Inquilino/Edit/5
        public ActionResult Edit(int id)
        {
            var inqui = repositorio.ObtenerPorId(id);
            return View(inqui);
        }

        // POST: Inquilino/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inquilino i = null;
            try
            {
                i = repositorio.ObtenerPorId(id);
                i.Nombre = collection["Nombre"];
                i.Apellido = collection["Apellido"];
                i.Dni = collection["Dni"];
                i.Email = collection["Email"];
                i.Telefono = collection["Telefono"];
                i.DireccionTrabajo = collection["DireccionTrabajo"];
                i.NombreGarante= collection["NombreGarante"];
                i.DniGarante = collection["DniGarante"];
                i.TelGarante = collection["TelGarante"];
                repositorio.Modificacion(i);
                TempData["Mensaje"] = "Inquilino actualizado correctamente";
            

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(i);
            }
        }

        // GET: Inquilino/Delete/5
        public ActionResult Delete(int id)
        {
            var i = repositorio.ObtenerPorId(id);
            if (TempData.ContainsKey("Mensaje"))
                ViewBag.Mensaje = TempData["Mensaje"];
            if (TempData.ContainsKey("Error"))
                ViewBag.Error = TempData["Error"];
            return View(i);
        }

        // POST: Inquilino/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repositorio.Baja(id);
                TempData["Mensaje"] = "Se eliminó el inquilino correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(collection);
            }
        }
    }
}