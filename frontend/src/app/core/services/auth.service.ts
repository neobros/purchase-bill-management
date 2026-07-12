import { Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import { AuthSession, LoginRequest, LoginResponseData, UserLocation } from '../models/auth.model';

const SESSION_KEY = 'pbm_session';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly _session = signal<AuthSession | null>(this.readSession());

  readonly session = this._session.asReadonly();
  readonly isAuthenticated = computed(() => this._session() !== null);
  readonly locations = computed<UserLocation[]>(() => this._session()?.locations ?? []);
  readonly token = computed<string | null>(() => this._session()?.token ?? null);

  constructor(private readonly http: HttpClient) {}

  login(request: LoginRequest): Observable<AuthSession> {
    return this.http
      .post<ApiResponse<LoginResponseData>>(`${environment.apiBaseUrl}/auth/login`, request)
      .pipe(
        map((response) => response.data),
        tap((session) => this.setSession(session)),
      );
  }

  logout(): void {
    this._session.set(null);
    sessionStorage.removeItem(SESSION_KEY);
  }

  private setSession(session: AuthSession): void {
    this._session.set(session);
    sessionStorage.setItem(SESSION_KEY, JSON.stringify(session));
  }

  private readSession(): AuthSession | null {
    const raw = sessionStorage.getItem(SESSION_KEY);
    if (!raw) {
      return null;
    }
    try {
      return JSON.parse(raw) as AuthSession;
    } catch {
      return null;
    }
  }
}
