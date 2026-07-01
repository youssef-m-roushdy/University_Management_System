import { STORAGE_KEYS, API_ENDPOINTS } from '../constants';
import { AuthResponse, AuthData } from '../types';

// ──────────────────────────────────────────────────────────────────────────────
// TYPES
// ──────────────────────────────────────────────────────────────────────────────

interface RequestOptions extends RequestInit {
  isFormData?: boolean;
  skipAuth?: boolean; // ← NEW: skip Authorization header for login/refresh
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
  private _refreshPromise: Promise<AuthData> | null = null;

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

  /**
   * Returns true for URLs that should NEVER have an Authorization header.
   * These are public auth endpoints — sending a stale/invalid token can
   * cause the server to reject the request or behave unexpectedly.
   */
  private isAuthEndpoint(url: string): boolean {
    const authPrefixes = [
      API_ENDPOINTS.AUTH.LOGIN,
      API_ENDPOINTS.AUTH.FORGOT_PASSWORD,
      API_ENDPOINTS.AUTH.RESET_PASSWORD,
      API_ENDPOINTS.AUTH.REFRESH_TOKEN,
      API_ENDPOINTS.AUTH.VERIFY_EMAIL,
      API_ENDPOINTS.AUTH.RESEND_VERIFICATION,
    ];
    return authPrefixes.some(
      prefix => url.startsWith(prefix) || url === prefix
    );
  }

  getHeaders(
    isFormData: boolean = false,
    skipAuth: boolean = false
  ): Record<string, string> {
    const headers: Record<string, string> = {};

    if (!isFormData) {
      headers['Content-Type'] = 'application/json';
    }

    if (!skipAuth) {
      const token = this.getToken();
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
      }
    }

    return headers;
  }

  // ─── Refresh Token (single source of truth) ───────────────────────────────

  async refreshAccessToken(): Promise<AuthData> {
    if (!this._refreshPromise) {
      this._refreshPromise = this._doRefresh().finally(() => {
        this._refreshPromise = null;
      });
    }
    return this._refreshPromise;
  }

  private async _doRefresh(): Promise<AuthData> {
    const accessToken = this.getToken();
    const refreshToken = this.getRefreshToken();

    if (!refreshToken) {
      throw new Error('No refresh token.');
    }

    const res = await fetch(API_ENDPOINTS.AUTH.REFRESH_TOKEN, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      // Refresh endpoint intentionally does NOT send Authorization header
      body: JSON.stringify({ accessToken, refreshToken }),
    });

    if (!res.ok) {
      const errBody = await res.json().catch(() => ({}));
      console.error('Refresh failed:', res.status, errBody);
      throw new Error('Refresh failed.');
    }

    const body = (await res.json()) as AuthResponse;

    if (!body?.success || !body?.data) {
      throw new Error('Refresh failed: malformed response.');
    }

    const {
      accessToken: newAccessToken,
      refreshToken: newRefreshToken,
      accessTokenExpiry,
    } = body.data;

    this.setTokens(newAccessToken, newRefreshToken);
    localStorage.setItem(STORAGE_KEYS.TOKEN_EXPIRY, accessTokenExpiry);
    localStorage.setItem(STORAGE_KEYS.USER, JSON.stringify(body.data));

    return body.data;
  }

  // ─── Main Request Method ─────────────────────────────────────────────────

  async request<T = any>(
    url: string,
    options: RequestOptions = {}
  ): Promise<T> {
    // Auto-detect auth endpoints — never send stale tokens to login/refresh
    const skipAuth = options.skipAuth || this.isAuthEndpoint(url);

    const makeRequest = (token: string | null): Promise<Response> => {
      const headers = this.getHeaders(options.isFormData, skipAuth);
      // Only set Authorization if we're NOT skipping auth AND we have a token
      if (!skipAuth && token) {
        headers['Authorization'] = `Bearer ${token}`;
      }

      return fetch(url, {
        ...options,
        headers: { ...headers, ...(options.headers as Record<string, string>) },
      });
    };

    let res = await makeRequest(this.getToken());

    // ── 401 handling — refresh once (shared across concurrent 401s), retry ──
    if (res.status === 401 && !skipAuth) {
      try {
        const authData = await this.refreshAccessToken();
        res = await makeRequest(authData.accessToken);
      } catch {
        this.clearTokens();
        window.location.href = '/login';
        throw {
          status: 401,
          errorMessage: 'Session expired. Please log in again.',
        } as ApiError;
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
    const skipAuth = this.isAuthEndpoint(url);
    const headers: Record<string, string> = {};
    if (!skipAuth) {
      const token = this.getToken();
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
      }
    }

    return this.request<T>(url, {
      method: 'POST',
      body: formData,
      isFormData: true,
      headers,
    });
  }

  patchForm<T = any>(url: string, formData: FormData): Promise<T> {
    const skipAuth = this.isAuthEndpoint(url);
    const headers: Record<string, string> = {};
    if (!skipAuth) {
      const token = this.getToken();
      if (token) {
        headers['Authorization'] = `Bearer ${token}`;
      }
    }

    return this.request<T>(url, {
      method: 'PATCH',
      body: formData,
      isFormData: true,
      headers,
    });
  }
}

// ──────────────────────────────────────────────────────────────────────────────
// EXPORT
// ──────────────────────────────────────────────────────────────────────────────

const apiService = new ApiService();
export default apiService;
