using my.services.contactinfo.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my.services.contactinfo.Application.Services
{
    public class ContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<List<Contact>> GetAllContactsAsync()
        {
            return await _contactRepository.GetAllContactsAsync();
        }

        public async Task AddContactAsync(Contact newContact)
        {
            var contacts = await _contactRepository.GetAllContactsAsync();

            // Auto-increment ID
            newContact.Id = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1;

            // Ensure email is unique
            if (contacts.Any(c => c.Email == newContact.Email))
                throw new Exception("Email already exists");

            await _contactRepository.AddContactAsync(newContact);
        }

        public async Task<Contact> GetContactByIdAsync(int id)
        {
            return await _contactRepository.GetContactByIdAsync(id);
        }

        public async Task UpdateContactAsync(Contact updatedContact)
        {
            var contacts = await _contactRepository.GetAllContactsAsync();
            var contact = contacts.FirstOrDefault(c => c.Id == updatedContact.Id);
            if (contact == null)
                throw new Exception("Contact not found");

            // Ensure email is unique if changed
            if (contacts.Any(c => c.Email == updatedContact.Email && c.Id != updatedContact.Id))
                throw new Exception("Email already exists");

            await _contactRepository.UpdateContactAsync(updatedContact);
        }

        public async Task DeleteContactAsync(int id)
        {
            var contact = await _contactRepository.GetContactByIdAsync(id);
            if (contact == null)
                throw new Exception("Contact not found");

            await _contactRepository.DeleteContactAsync(id);
        }
    }


}
