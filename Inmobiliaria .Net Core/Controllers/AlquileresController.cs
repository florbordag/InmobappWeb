﻿using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Inmobiliaria_.Net_Core.Controllers
{
    public class AlquileresController : Controller
    {

        private readonly IRepositorio<Alquiler> repositorio;

        public AlquileresController(IRepositorio<Alquiler> repositorio, IRepositorio<Inmueble> inmuebles, IRepositorio<Inquilino> inquilinos)
        {
            this.repositorio = repositorio;
            Inmuebles = inmuebles;
            Inquilinos = inquilinos;

        }
        public IRepositorio<Inmueble> Inmuebles { get; set; }
        public IRepositorio<Inquilino> Inquilinos { get; set; }

        [Authorize]
        // GET:
        public ActionResult Index()
        {
            var lista = repositorio.ObtenerTodos();
            if (TempData.ContainsKey("Id"))
                ViewBag.Id = TempData["Id"];
            return View(lista);
        }

        public ActionResult Create()
        {
            ViewBag.propis = Inmuebles.ObtenerTodos();
            ViewBag.inquis = Inquilinos.ObtenerTodos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Alquiler alquiler)
        {
        
            try
            {
                alquiler.Vigente = true;
                repositorio.Alta(alquiler);
                TempData["Id"] = "creó el alquiler";
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewBag.propis = Inmuebles.ObtenerTodos();
                ViewBag.inquis = Inquilinos.ObtenerTodos();
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(alquiler);
            }
        }
        public ActionResult Edit(int id)
        {
            var uno = repositorio.ObtenerPorId(id);
            ViewBag.trato = uno.Inmueble.Duenio.Nombre + " " + uno.Inmueble.Duenio.Apellido + " y " + uno.Inquilino.Nombre + " " + uno.Inquilino.Apellido + " en " + uno.Inmueble.Direccion;
            return View(uno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Alquiler alquiler)
        {
            try
            {
                alquiler = repositorio.Update(alquiler);
                alquiler.calcularMulta();
                repositorio.Modificacion(alquiler);
                TempData["Id"] = "actualizó el alquiler";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(alquiler);
            }
        }

        public ActionResult Delete(int id)
        {
            var uno = repositorio.ObtenerPorId(id);
            ViewBag.trato = " el locador " + uno.Inmueble.Duenio.Nombre + " " + uno.Inmueble.Duenio.Apellido + " y el locatario " + uno.Inquilino.Nombre + " " + uno.Inquilino.Apellido + " para el inmueble con dirección en " + uno.Inmueble.Direccion;
            return View(uno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Alquiler alquiler)
        {

            try
            {
                alquiler = repositorio.FinalizarContrato(alquiler);
                TempData["Id"] = "Finalizo el contrato de alquiler";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrate = ex.StackTrace;
                return View(alquiler);
            }


        }

    }
}