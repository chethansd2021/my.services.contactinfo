import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ContactService } from '../../services/contact.service';
import { Contact } from '../../models/contact.model';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.css']
})
export class ContactFormComponent implements OnInit {
  contactForm: FormGroup;
  isEdit: boolean;

  constructor(
    private fb: FormBuilder,
    private contactService: ContactService,
    public dialogRef: MatDialogRef<ContactFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { contact: Contact; isEdit: boolean }
  ) {
    this.isEdit = data.isEdit;
    this.contactForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });

    if (this.isEdit && data.contact) {
      this.contactForm.patchValue(data.contact);
    }
  }

  ngOnInit(): void {}

  onSubmit(): void {
    if (this.contactForm.valid) {
      if (this.isEdit) {
        this.contactService.updateContact({ ...this.data.contact, ...this.contactForm.value }).subscribe(() => {
          this.dialogRef.close(true);
        });
      } else {
        this.contactService.addContact(this.contactForm.value).subscribe(() => {
          this.dialogRef.close(true);
        });
      }
    }
  }
}
