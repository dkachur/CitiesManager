import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RegisterUser } from '../models/register-user';

const API_BASE_URL: string = "https://localhost:7176/api/v1/account/";
@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpClient: HttpClient) {

  }

  public postRegister(user: RegisterUser): Observable<RegisterUser> {
    return this.httpClient.post<RegisterUser>(`${API_BASE_URL}register`, user);
  }
}
