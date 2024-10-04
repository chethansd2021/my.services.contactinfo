using my.services.contactinfo.Application.Services;
using my.services.contactinfo.Domain.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace my.services.contactinfo.Infrastructure.Respositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly JsonFileDataAccess _dataAccess;

        public ContactRepository()
        {
            _dataAccess = new JsonFileDataAccess("contacts.json");
        }

        public async Task<List<Contact>> GetAllContactsAsync()
        {
            return await _dataAccess.LoadContactsAsync();
        }

        public async Task<Contact> GetContactByIdAsync(int id)
        {
            var contacts = await GetAllContactsAsync();
            return contacts.FirstOrDefault(c => c.Id == id);
        }

        public async Task AddContactAsync(Contact newContact)
        {
            var contacts = await GetAllContactsAsync();
            contacts.Add(newContact);
            await _dataAccess.SaveContactsAsync(contacts);
        }

        public async Task UpdateContactAsync(Contact updatedContact)
        {
            var contacts = await GetAllContactsAsync();
            var contact = contacts.FirstOrDefault(c => c.Id == updatedContact.Id);
            if (contact != null)
            {
                contact.FirstName = updatedContact.FirstName;
                contact.LastName = updatedContact.LastName;
                contact.Email = updatedContact.Email;
                await _dataAccess.SaveContactsAsync(contacts);
            }
        }

        public async Task DeleteContactAsync(int id)
        {
            var contacts = await GetAllContactsAsync();
            var contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact != null)
            {
                contacts.Remove(contact);
                await _dataAccess.SaveContactsAsync(contacts);
            }
        }
    }

}
