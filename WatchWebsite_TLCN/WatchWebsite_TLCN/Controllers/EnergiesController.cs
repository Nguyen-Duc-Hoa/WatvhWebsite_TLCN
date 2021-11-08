﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchWebsite_TLCN.DTO;
using WatchWebsite_TLCN.Entities;
using WatchWebsite_TLCN.Intefaces;
using WatchWebsite_TLCN.IRepository;
using WatchWebsite_TLCN.Models;

namespace WatchWebsite_TLCN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnergiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEnergy _energy;

        public EnergiesController(IUnitOfWork unitOfWork, IMapper mapper, IEnergy energy)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _energy = energy;
        }

        // GET: api/Energies?currentPage=1
        [HttpGet]
        public async Task<IActionResult> GetEnegies(int currentPage)
        {
            var result = await _unitOfWork.Energies.GetAllWithPagination(
                expression: null,
                orderBy: x=>x.OrderBy(a=>a.EnergyId),
                pagination: new Pagination { CurrentPage = currentPage }
                );
            List<Energy> list = new List<Energy>();
            
            foreach( var item in result.Item1)
            {
                list.Add(item);
            }
            var listEnergyDTO = _mapper.Map<List<EnergyDTO>>(list);

            return Ok(new
            {
                Energies = listEnergyDTO,
                CurrentPage = result.Item2.CurrentPage,
                TotalPage = result.Item2.TotalPage
            });
        }

        // GET: api/Energies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Energy>> GetEnergy(int id)
        {
            var energy = await _unitOfWork.Energies.Get(x=>x.EnergyId == id);

            if (energy == null)
            {
                return NotFound();
            }

            return energy;
        }

        // PUT: api/Energies/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnergy(int id, Energy energy)
        {
            if (id != energy.EnergyId)
            {
                return BadRequest();
            }

            _unitOfWork.Energies.Update(energy);

            try
            {
                await _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EnergyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(GetEnegies), new { currentPage = 1 });
            //return NoContent();
        }

        // POST: api/Energies
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Energy>> PostEnergy(Energy energy)
        {
            try
            {
                await _unitOfWork.Energies.Insert(energy);
                await _unitOfWork.Save();

                return Ok();
            }
            catch
            { }

            return BadRequest();
            //return CreatedAtAction("GetEnergy", new { id = energy.EnergyId }, energy);
        }

        // DELETE: api/Energies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Energy>> DeleteEnergy(int id)
        {
            var energy = await _unitOfWork.Energies.Get(x=>x.EnergyId==id);
            if (energy == null)
            {
                return NotFound();
            }

            await _unitOfWork.Energies.Delete(energy);
            await _unitOfWork.Save();

            return energy;
        }


        [HttpDelete]
        [Route("Delete")]
        // Delete nhieu san pham
        // 
        /* DELETE: api/Energies/delete
         * JSON
           [6,7]
            */
        public IActionResult Delete(List<int> id)
        {
            foreach (int item in id)
            {
                if (!_energy.DeleteEnergy(item))
                {
                    return BadRequest("Something was wrong");
                }
            }
            return RedirectToAction(nameof(GetEnegies), new { currentPage = 1 });
        }

        private Task<bool> EnergyExists(int id)
        {
            return _unitOfWork.Energies.IsExist<int>(id);
        }

        
    }
}
