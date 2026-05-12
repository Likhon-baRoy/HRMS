export interface PaginationMeta {
  page: number;

  pageSize: number;

  totalCount: number;

  totalPages: number;
}

export interface PagedResult<T> {
  items: T[];

  meta: PaginationMeta;
}
