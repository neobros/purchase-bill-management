import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import { UserLocation } from '../models/auth.model';

@Injectable({ providedIn: 'root' })
export class LocationService {
  private readonly http = inject(HttpClient);

  getAll(): Observable<UserLocation[]> {
    return this.http
      .get<ApiResponse<UserLocation[]>>(`${environment.apiBaseUrl}/locations`)
      .pipe(map((response) => response.data));
  }
}
