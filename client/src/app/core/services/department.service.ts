import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment }
from '../../../environments/environment';

import { Department }
from '../models/department.model';

import { PagedResult }
from '../models/paged-result.model';

@Injectable({
  providedIn: 'root'
})

export class DepartmentService {

  private http = inject(HttpClient);

  getAll(page = 1, pageSize = 10): Observable<PagedResult<Department>>
  {
    return this.http.get<PagedResult<Department>>
    (
      `${environment.apiUrl}/department?page=${page}&pageSize=${pageSize}`
    );
  }
}
