using my.services.contactinfo.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace my.services.contactinfo.Application.Services
{
    public interface IContactRepository
    {
        Task<List<Contact>> GetAllContactsAsync();            // Retrieve all contacts
        Task<Contact> GetContactByIdAsync(int id);            // Retrieve a contact by ID
        Task AddContactAsync(Contact newContact);              // Add a new contact
        Task UpdateContactAsync(Contact updatedContact);       // Update an existing contact
        Task DeleteContactAsync(int id);                       // Delete a contact by ID
    }
}