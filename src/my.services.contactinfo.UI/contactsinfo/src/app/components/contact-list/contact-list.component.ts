import { Component, OnInit } from '@angular/core';
import { ContactService } from '../../services/contact.service';
import { Contact } from '../../models/contact.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
})
export class ContactListComponent implements OnInit {
  contacts: Contact[] = [];

  constructor(private contactService: ContactService, private router: Router) {}

  ngOnInit(): void {
    this.getContacts();
  }

  getContacts(): void {
    this.contactService.getContacts().subscribe((data) => {
      this.contacts = data;
    });
  }

  onEditContact(contactId: number): void {
    this.router.navigate([`/contacts/edit/${contactId}`]);
  }

  onDeleteContact(contactId: number): void {
    this.contactService.deleteContact(contactId).subscribe(() => {
      this.getContacts();
    });
  }

  onAddContact(): void {
    this.router.navigate(['/contacts/add']);
  }
}
