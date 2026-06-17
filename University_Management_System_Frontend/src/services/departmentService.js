import api from './api';
import { API_ENDPOINTS } from '../constants';

const departmentService = {
  getAll: () => api.get(API_ENDPOINTS.DEPARTMENTS.BASE),
  getById: id => api.get(API_ENDPOINTS.DEPARTMENTS.BY_ID(id)),
  create: data => api.post(API_ENDPOINTS.DEPARTMENTS.BASE, data),
  update: (id, data) => api.put(API_ENDPOINTS.DEPARTMENTS.BY_ID(id), data),
  del: id => api.del(API_ENDPOINTS.DEPARTMENTS.BY_ID(id)),
};

export default departmentService;
