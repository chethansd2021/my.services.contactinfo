using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using my.services.contactinfo.Application.Services;
using my.services.contactinfo.Domain.Models;
using System;
using System.Threading.Tasks;

namespace my.services.contactinfo.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ContactService _contactService;

        public ContactsController(ContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContactsAsync()
        {
            var contacts = await _contactService.GetAllContactsAsync();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactAsync(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact == null) return NotFound();
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContactAsync(Contact contact)
        {
            try
            {
                await _contactService.AddContactAsync(contact);
                return CreatedAtAction(nameof(GetContactAsync), new { id = contact.Id }, contact);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContactAsync(int id, Contact contact)
        {
            if (id != contact.Id) return BadRequest("ID mismatch");

            try
            {
                await _contactService.UpdateContactAsync(contact);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactAsync(int id)
        {
            try
            {
                await _contactService.DeleteContactAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}