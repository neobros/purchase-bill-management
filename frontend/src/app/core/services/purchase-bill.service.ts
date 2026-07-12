import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import { PurchaseBillCreateRequest, PurchaseBillResponse } from '../models/purchase-bill.model';

@Injectable({ providedIn: 'root' })
export class PurchaseBillService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/purchasebills`;

  getAll(): Observable<PurchaseBillResponse[]> {
    return this.http
      .get<ApiResponse<PurchaseBillResponse[]>>(this.baseUrl)
      .pipe(map((response) => response.data));
  }

  getById(id: number): Observable<PurchaseBillResponse> {
    return this.http
      .get<ApiResponse<PurchaseBillResponse>>(`${this.baseUrl}/${id}`)
      .pipe(map((response) => response.data));
  }

  create(request: PurchaseBillCreateRequest): Observable<PurchaseBillResponse> {
    return this.http
      .post<ApiResponse<PurchaseBillResponse>>(this.baseUrl, request)
      .pipe(map((response) => response.data));
  }

  update(id: number, request: PurchaseBillCreateRequest): Observable<PurchaseBillResponse> {
    return this.http
      .put<ApiResponse<PurchaseBillResponse>>(`${this.baseUrl}/${id}`, request)
      .pipe(map((response) => response.data));
  }

  delete(id: number): Observable<void> {
    return this.http.delete<ApiResponse<object>>(`${this.baseUrl}/${id}`).pipe(map(() => undefined));
  }
}
