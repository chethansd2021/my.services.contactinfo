import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Contact } from '../../models/contact.model';


@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html',
  styleUrls: ['./contact-form.component.scss']
})
export class ContactFormComponent implements OnInit {
  @Input() contact?: Contact;
  @Output() save = new EventEmitter<Contact>();
  contactForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.contactForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]      
    });
  }

  ngOnInit() {
    if (this.contact) {
      this.contactForm.patchValue(this.contact);
    }
  }

  onSubmit() {
    if (this.contactForm.valid) {
      this.save.emit(this.contactForm.value);
      this.resetForm(); // Reset form after save
    }
  }

  resetForm() {
    this.contactForm.reset(); // Clear the form fields
  }
}
