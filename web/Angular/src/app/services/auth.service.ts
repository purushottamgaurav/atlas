import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, tap } from 'rxjs';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly TOKEN_KEY = 'quiz_token';
  private readonly USER_KEY  = 'quiz_user';

  private _currentUser = new BehaviorSubject<AuthResponse | null>(this.loadUser());
  currentUser$ = this._currentUser.asObservable();

  get token(): string | null { return localStorage.getItem(this.TOKEN_KEY); }
  get currentUser(): AuthResponse | null { return this._currentUser.value; }
  get isLoggedIn(): boolean { return !!this.token; }

  constructor(private http: HttpClient) {}

  login(req: LoginRequest) {
    return this.http.post<AuthResponse>('/api/auth/login', req).pipe(
      tap(r => this.store(r))
    );
  }

  register(req: RegisterRequest) {
    return this.http.post<AuthResponse>('/api/auth/register', req).pipe(
      tap(r => this.store(r))
    );
  }

  logout() {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this._currentUser.next(null);
  }

  private store(r: AuthResponse) {
    localStorage.setItem(this.TOKEN_KEY, r.token);
    localStorage.setItem(this.USER_KEY, JSON.stringify(r));
    this._currentUser.next(r);
  }

  private loadUser(): AuthResponse | null {
    const raw = localStorage.getItem(this.USER_KEY);
    return raw ? JSON.parse(raw) : null;
  }
}
