using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiblioGest.Data;
using BiblioGest.Models;
using Microsoft.EntityFrameworkCore;

namespace BiblioGest.Services
{
    public class AdherentsService
    {
        private readonly BiblioGestContext _context;

        public AdherentsService()
        {
            _context = new BiblioGestContext();
        }

        public async Task<(List<Adherent> adherents, int totalCount)> GetAdherentsAsync(
            string searchText = null, 
            string status = null, 
            int page = 1, 
            int pageSize = 10)
        {
            try
            {
                // Créer la requête de base
                IQueryable<Adherent> query = _context.Adherent;

                // Appliquer le filtre de recherche
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = query.Where(a => 
                        a.Nom.Contains(searchText) || 
                        a.Prenom.Contains(searchText) || 
                        a.Email.Contains(searchText));
                }

                // Appliquer le filtre de statut
                if (!string.IsNullOrWhiteSpace(status))
                {
                    query = query.Where(a => a.Statut == status);
                }

                // Obtenir le nombre total d'éléments
                int totalCount = await query.CountAsync();

                // Appliquer la pagination
                var pagedAdherents = await query
                    .OrderBy(a => a.Nom)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (pagedAdherents, totalCount);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Erreur lors de la récupération des adhérents: {ex.Message}");
                throw;
            }
        }

        public async Task<Adherent> GetAdherentByIdAsync(int id)
        {
            return await _context.Adherent.FindAsync(id);
        }

        public async Task<bool> AddAdherentAsync(Adherent adherent)
        {
            try
            {
                _context.Adherent.Add(adherent);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAdherentAsync(Adherent adherent)
        {
            try
            {
                _context.Entry(adherent).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAdherentAsync(int id)
        {
            try
            {
                var adherent = await _context.Adherent.FindAsync(id);
                if (adherent == null)
                    return false;

                _context.Adherent.Remove(adherent);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}