import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ContactFormComponent } from '../contact-form/contact-form.component';
import { ContactService } from '../../services/contact.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Contact } from '../../models/contact.model';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.css']
})
export class ContactListComponent implements OnInit {
  contacts: Contact[] = [];
  displayedColumns: string[] = ['firstName', 'lastName', 'email', 'actions'];

  constructor(private contactService: ContactService, public dialog: MatDialog, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.loadContacts();
  }

  loadContacts(): void {
    this.contactService.getContacts().subscribe((data: Contact[]) => {
      this.contacts = data;
    });
  }

  onAddContact(): void {
    const dialogRef = this.dialog.open(ContactFormComponent, {
      width: '300px',
      data: { isEdit: false }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadContacts();
        this.snackBar.open('Contact added successfully!', 'Close', { duration: 3000 });
      }
    });
  }

  editContact(contact: Contact): void {
    const dialogRef = this.dialog.open(ContactFormComponent, {
      width: '300px',
      data: { contact, isEdit: true }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadContacts();
        this.snackBar.open('Contact updated successfully!', 'Close', { duration: 3000 });
      }
    });
  }

  deleteContact(id: number): void {
    if (confirm('Are you sure you want to delete this contact?')) {
      this.contactService.deleteContact(id).subscribe(() => {
        this.loadContacts();
        this.snackBar.open('Contact deleted successfully!', 'Close', { duration: 3000 });
      });
    }
  }
}