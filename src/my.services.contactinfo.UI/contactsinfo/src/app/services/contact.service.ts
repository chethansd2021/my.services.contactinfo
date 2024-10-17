import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Contact } from '../models/contact.model';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  private apiUrl = 'https://localhost:5000/api/contacts';

  constructor(private http: HttpClient) {}

  getContacts(): Observable<Contact[]> {
    return this.http.get<Contact[]>(this.apiUrl);
  }

  getContactById(id: number): Observable<Contact> {
    return this.http.get<Contact>(`${this.apiUrl}/${id}`);
  }

  addContact(contact: Contact): Observable<any> {
    return this.http.post(this.apiUrl, contact);
  }

  updateContact(contact: Contact): Observable<any> {
    return this.http.put(`${this.apiUrl}/${contact.id}`, contact);
  }

  deleteContact(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
