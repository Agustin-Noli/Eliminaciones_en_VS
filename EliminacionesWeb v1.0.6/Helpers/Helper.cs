using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EliminacionesWeb.Models;
using EliminacionesWeb.ModelsDTO;

namespace EliminacionesWeb.Helpers
{
    public class Helper
    {
        private EliminacionesContext _context;

        public Helper()
        {
        }

        //public async void AgregarHistorial(Historial historial)
        //{
        //    _context = new EliminacionesContext();

        //    _context.Historial.Add(historial);
        //    await _context.SaveChangesAsync();
        //}

    }
}
