import { STORAGE_KEYS, API_ENDPOINTS } from '../constants';
import { AuthResponse } from '../types';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

interface RequestOptions extends RequestInit {
  isFormData?: boolean;
}

interface FailedQueueItem {
  resolve: (value: string) => void;
  reject: (reason?: any) => void;
}

interface ApiError {
  status: number;
  errorMessage?: string;
  [key: string]: any;
}

// ──────────────────────────────────────────────────────────────────────────────
// API SERVICE
// ──────────────────────────────────────────────────────────────────────────────

class ApiService {
  private _isRefreshing: boolean = false;
  private _failedQueue: FailedQueueItem[] = [];

  constructor() {
    this._isRefreshing = false;
    this._failedQueue = [];
  }

  // ─── Token Management ──────────────────────────────────────────────────────

  getToken(): string | null {
    return localStorage.getItem(STORAGE_KEYS.ACCESS_TOKEN);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(STORAGE_KEYS.REFRESH_TOKEN);
  }

  setTokens(accessToken: string, refreshToken: string): void {
    localStorage.setItem(STORAGE_KEYS.ACCESS_TOKEN, accessToken);
    localStorage.setItem(STORAGE_KEYS.REFRESH_TOKEN, refreshToken);
  }

  clearTokens(): void {
    localStorage.removeItem(STORAGE_KEYS.ACCESS_TOKEN);
    localStorage.removeItem(STORAGE_KEYS.REFRESH_TOKEN);
    localStorage.removeItem(STORAGE_KEYS.USER);
    localStorage.removeItem(STORAGE_KEYS.TOKEN_EXPIRY);
  }

  // ─── Headers ──────────────────────────────────────────────────────────────

  getHeaders(isFormData: boolean = false): Record<string, string> {
    const headers: Record<string, string> = {};

    if (!isFormData) {
      headers['Content-Type'] = 'application/json';
    }

    const token = this.getToken();
    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    return headers;
  }

  // ─── Queue Management ─────────────────────────────────────────────────────

  private _enqueue(): Promise<string> {
    return new Promise((resolve, reject) => {
      this._failedQueue.push({ resolve, reject });
    });
  }

  private _flushQueue(
    error?: ApiError | null,
    newToken: string | null = null
  ): void {
    this._failedQueue.forEach(({ resolve, reject }) => {
      if (error) {
        reject(error);
      } else if (newToken) {
        resolve(newToken);
      }
    });
    this._failedQueue = [];
  }

  // ─── Refresh Token ────────────────────────────────────────────────────────

  private async _tryRefresh(): Promise<string> {
    const refreshToken = this.getRefreshToken();
    console.log('🔄 Attempting refresh...');
    console.log('Refresh token exists:', !!refreshToken);
    console.log('Refresh token value:', refreshToken?.substring(0, 20) + '...');

    if (!refreshToken) {
      throw new Error('No refresh token.');
    }

    const res = await fetch(API_ENDPOINTS.AUTH.REFRESH_TOKEN, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ refreshToken }),
    });

    console.log('Refresh response status:', res.status);

    if (!res.ok) {
      const errBody = await res.json().catch(() => ({}));
      console.error('Refresh failed response:', errBody);
      throw new Error('Refresh failed.');
    }

    const data = (await res.json()) as AuthResponse;
    console.log(
      '✅ Refresh success, new token:',
      data.data?.accessToken?.substring(0, 20) + '...'
    );

    this.setTokens(data.data.accessToken, data.data.refreshToken);
    return data.data.accessToken;
  }

  // ─── Main Request Method ─────────────────────────────────────────────────

  async request<T = any>(
    url: string,
    options: RequestOptions = {}
  ): Promise<T> {
    // Build and fire the request
    const makeRequest = (token: string | null): Promise<Response> => {
      const headers = this.getHeaders(options.isFormData);
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
      }

      return fetch(url, {
        ...options,
        headers: { ...headers, ...(options.headers as Record<string, string>) },
      });
    };

    let res = await makeRequest(this.getToken());

    // ── 401 handling — try refresh then retry ────────────────────────────
    if (res.status === 401) {
      // If a refresh is already running, wait for it then retry
      if (this._isRefreshing) {
        try {
          const newToken = await this._enqueue();
          res = await makeRequest(newToken);
        } catch {
          this.clearTokens();
          window.location.href = '/login';
          throw { status: 401, errorMessage: 'Session expired.' } as ApiError;
        }
      } else {
        // This request triggers the refresh
        this._isRefreshing = true;

        try {
          const newToken = await this._tryRefresh();
          this._flushQueue(null, newToken); // unblock queued requests
          res = await makeRequest(newToken); // retry this request
        } catch (err) {
          this._flushQueue(err as ApiError);
          this.clearTokens();
          window.location.href = '/login';
          throw {
            status: 401,
            errorMessage: 'Session expired. Please log in again.',
          } as ApiError;
        } finally {
          this._isRefreshing = false;
        }
      }
    }

    // ── Normal response handling ─────────────────────────────────────────
    if (!res.ok) {
      const err = await res
        .json()
        .catch(() => ({ errorMessage: res.statusText }));
      throw { status: res.status, ...(err || {}) } as ApiError;
    }

    if (res.status === 204) {
      return null as T;
    }

    return res.json() as Promise<T>;
  }

  // ─── HTTP Methods ────────────────────────────────────────────────────────

  get<T = any>(url: string): Promise<T> {
    return this.request<T>(url);
  }

  post<T = any>(url: string, data?: any): Promise<T> {
    return this.request<T>(url, {
      method: 'POST',
      body: JSON.stringify(data),
    });
  }

  put<T = any>(url: string, data?: any): Promise<T> {
    return this.request<T>(url, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  }

  patch<T = any>(url: string, data?: any): Promise<T> {
    return this.request<T>(url, {
      method: 'PATCH',
      body: JSON.stringify(data),
    });
  }

  delete<T = any>(url: string): Promise<T> {
    return this.request<T>(url, {
      method: 'DELETE',
    });
  }

  // ─── Form Data Methods ──────────────────────────────────────────────────

  postForm<T = any>(url: string, formData: FormData): Promise<T> {
    return this.request<T>(url, {
      method: 'POST',
      body: formData,
      isFormData: true,
      headers: {
        Authorization: `Bearer ${this.getToken()}`,
      } as Record<string, string>,
    });
  }

  patchForm<T = any>(url: string, formData: FormData): Promise<T> {
    return this.request<T>(url, {
      method: 'PATCH',
      body: formData,
      isFormData: true,
      headers: {
        Authorization: `Bearer ${this.getToken()}`,
      } as Record<string, string>,
    });
  }
}

// ──────────────────────────────────────────────────────────────────────────────
// EXPORT
// ──────────────────────────────────────────────────────────────────────────────

const apiService = new ApiService();
export default apiService;
