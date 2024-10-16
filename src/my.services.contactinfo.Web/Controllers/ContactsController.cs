using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using my.services.contactinfo.Application.Services;
using my.services.contactinfo.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace my.services.contactinfo.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ContactService _contactService;
        private readonly IValidator<Contact> _contactValidator;

        public ContactsController(ContactService contactService, IValidator<Contact> contactValidator)
        {
            _contactService = contactService;
            _contactValidator = contactValidator;
        }

        // GET: api/contact
        [HttpGet]
        public async Task<ActionResult<List<Contact>>> GetContacts()
        {
            var contacts = await _contactService.GetAllContactsAsync();
            return Ok(contacts);
        }

        // GET: api/contact/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact == null) return NotFound();
            return Ok(contact);
        }

        // POST: api/contact
        [HttpPost]
        public async Task<ActionResult> AddContact([FromBody] Contact contact)
        {
            var validationResult = await _contactValidator.ValidateAsync(contact);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _contactService.AddContactAsync(contact);
            return Ok();
        }

        // PUT: api/contact/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateContact(int id, [FromBody] Contact contact)
        {
            var existingContact = await _contactService.GetContactByIdAsync(id);
            if (existingContact == null) return NotFound();

            var validationResult = await _contactValidator.ValidateAsync(contact);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            contact.Id = id;
            await _contactService.UpdateContactAsync(contact);
            return Ok();
        }

        // DELETE: api/contact/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContact(int id)
        {
            var contact = await _contactService.GetContactByIdAsync(id);
            if (contact == null) return NotFound();

            await _contactService.DeleteContactAsync(id);
            return Ok();
        }
    }
}