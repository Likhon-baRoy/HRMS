import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult } from '../models/paged-result.model';
import { Position } from '../models/position.model';

@Injectable({
  providedIn: 'root'
})
export class PositionService {
  private http = inject(HttpClient);

  private apiUrl = `${environment.apiUrl}/position`;

  getAll():
    Observable<PagedResult<Position>> {
    return this.http.get<PagedResult<Position>>(
      this.apiUrl
    );
  }
}
