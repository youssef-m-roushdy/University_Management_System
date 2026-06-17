# Frontend Folder Structure Documentation

This document provides a comprehensive overview of the AYA-UIS frontend folder structure and its organization principles.

## 📁 Directory Structure

```
src/
├── components/              # Reusable UI components
│   ├── common/             # Generic, reusable components
│   │   ├── Button.jsx      # ✅ Button component
│   │   ├── Button.css      # ✅ Button styles  
│   │   ├── Modal.jsx       # Modal component
│   │   ├── Table.jsx       # Data table component
│   │   ├── Pagination.jsx  # Pagination component
│   │   ├── LoadingSpinner.jsx
│   │   ├── ErrorBoundary.jsx
│   │   └── index.js        # Export file
│   ├── layout/             # Layout-specific components
│   │   ├── Layout.jsx      # ✅ Main layout component
│   │   ├── Layout.css      # ✅ Layout styles
│   │   ├── Header.jsx      # Header component
│   │   ├── Sidebar.jsx     # Sidebar navigation
│   │   ├── Footer.jsx      # Footer component
│   │   └── index.js        # Export file
│   └── forms/              # Form-specific components
│       ├── FormField.jsx   # Generic form field
│       ├── FormGroup.jsx   # Form group wrapper
│       ├── ValidationMessage.jsx
│       └── index.js        # Export file
├── pages/                  # Page-level components (routes)
│   ├── auth/               # Authentication pages
│   │   ├── LoginPage.jsx   # Login page
│   │   ├── RegisterPage.jsx # Registration page
│   │   ├── ForgotPasswordPage.jsx
│   │   └── index.js        # Export file
│   ├── dashboard/          # Dashboard pages
│   │   ├── DashboardPage.jsx
│   │   ├── DashboardStats.jsx
│   │   └── index.js
│   ├── departments/        # Department management
│   │   ├── DepartmentListPage.jsx
│   │   ├── DepartmentFormPage.jsx
│   │   ├── DepartmentDetailPage.jsx
│   │   └── index.js
│   ├── courses/            # Course management  
│   │   ├── CourseListPage.jsx
│   │   ├── CourseFormPage.jsx
│   │   ├── CourseDetailPage.jsx
│   │   └── index.js
│   ├── fees/               # Fee management
│   │   ├── FeeListPage.jsx
│   │   ├── FeeFormPage.jsx
│   │   ├── FeeDetailPage.jsx
│   │   └── index.js
│   └── schedules/          # Schedule management
│       ├── ScheduleListPage.jsx
│       ├── ScheduleFormPage.jsx
│       ├── ScheduleCalendarPage.jsx
│       └── index.js
├── hooks/                  # Custom React hooks
│   ├── index.js           # ✅ Common hooks (useAuth, useApi, etc.)
│   ├── useAuthGuard.js    # Authentication guard hook
│   ├── usePermissions.js  # Role-based permissions
│   └── useNotifications.js # Notification management
├── services/               # API service layer
│   ├── api.js             # ✅ Base API service
│   ├── authService.js     # ✅ Authentication services
│   ├── departmentService.js # ✅ Department CRUD
│   ├── courseService.js   # Course management
│   ├── feeService.js      # Fee management
│   ├── scheduleService.js # Schedule management
│   └── index.js           # Service exports
├── contexts/               # React context providers
│   ├── AuthContext.js     # ✅ Authentication context
│   ├── ThemeContext.js    # Theme management
│   ├── NotificationContext.js # Notifications
│   └── index.js           # Context exports
├── utils/                  # Utility functions
│   ├── index.js           # ✅ Common utilities
│   ├── validators.js      # Form validation helpers
│   ├── formatters.js      # Data formatting utils  
│   ├── constants.js       # Application constants
│   └── api-helpers.js     # API utility functions
├── styles/                 # Global styles and themes
│   ├── globals.css        # ✅ Global CSS variables and base styles
│   ├── variables.css      # CSS custom properties
│   ├── components.css     # Component-specific styles
│   ├── utilities.css      # Utility classes
│   └── themes/            # Theme variations
│       ├── light.css
│       └── dark.css
├── types/                  # TypeScript definitions
│   ├── index.d.ts         # ✅ Main type definitions
│   ├── auth.types.ts      # Authentication types
│   ├── api.types.ts       # API response types
│   └── component.types.ts # Component prop types
├── constants/              # Application constants
│   ├── index.js           # ✅ Main constants file
│   ├── routes.js          # Route definitions
│   ├── api-endpoints.js   # API endpoint constants
│   └── ui-constants.js    # UI-related constants  
└── assets/                 # Static assets
    ├── images/             # Image files
    │   ├── logo.svg
    │   ├── hero-image.jpg
    │   └── backgrounds/
    └── icons/              # Icon files
        ├── menu-icon.svg
        └── social-icons/
```

## 🎯 Organization Principles

### 1. Feature-Based Structure
- **Pages**: Organized by application features (auth, dashboard, departments, etc.)
- **Components**: Grouped by usage pattern (common, layout, forms)
- **Services**: One service per domain/feature

### 2. Separation of Concerns
- **Components**: Pure UI components with minimal logic
- **Pages**: Route-level components that compose smaller components
- **Hooks**: Reusable stateful logic
- **Services**: API calls and external integrations
- **Utils**: Pure functions and utilities

### 3. Scalability Patterns
- **Index Files**: Clean imports with barrel exports
- **CSS Modules**: Component-scoped styling
- **TypeScript**: Type safety and better development experience
- **Context API**: Global state management for cross-cutting concerns

## 📝 File Naming Conventions

### Components
- **PascalCase** for component files: `UserProfile.jsx`
- **Matching CSS files**: `UserProfile.css`
- **Index files** for barrel exports: `index.js`

### Services and Utilities  
- **camelCase** for service files: `departmentService.js`
- **Descriptive names**: `authService.js`, not `auth.js`

### Constants and Types
- **UPPER_SNAKE_CASE** for constants: `API_BASE_URL`
- **Descriptive type files**: `auth.types.ts`, `api.types.ts`

## 🗂️ Component Categories

### Common Components (`components/common/`)
**Purpose**: Reusable UI components used across the application
- ✅ `Button.jsx` - Universal button component with variants
- `Modal.jsx` - Reusable modal/dialog component
- `Table.jsx` - Data table with sorting/pagination
- `LoadingSpinner.jsx` - Loading state indicator
- `ErrorBoundary.jsx` - Error handling wrapper

### Layout Components (`components/layout/`)
**Purpose**: Structural components that define page layout
- ✅ `Layout.jsx` - Main application layout
- `Header.jsx` - Application header with navigation
- `Sidebar.jsx` - Side navigation menu
- `Footer.jsx` - Application footer
- `Breadcrumbs.jsx` - Navigation breadcrumbs

### Form Components (`components/forms/`)
**Purpose**: Form-specific reusable components
- `FormField.jsx` - Generic form input wrapper
- `FormGroup.jsx` - Form section grouping
- `ValidationMessage.jsx` - Error/validation display
- `SearchBox.jsx` - Search input component

## 🎣 Custom Hooks (`hooks/`)

### Authentication Hooks
- ✅ `useAuth()` - Authentication state and actions
- `useAuthGuard()` - Route protection logic
- `usePermissions()` - Role-based access control

### Data Management Hooks  
- ✅ `useApi()` - Generic API data fetching
- ✅ `useForm()` - Form state management
- ✅ `usePagination()` - Pagination logic
- ✅ `useDebounce()` - Performance optimization

### UI State Hooks
- ✅ `useToggle()` - Boolean state management  
- ✅ `useLocalStorage()` - Local storage integration
- `useNotifications()` - Toast/notification management

## 🔗 Services (`services/`)

### API Services
- ✅ `api.js` - Base API configuration and utilities
- ✅ `authService.js` - Authentication operations
- ✅ `departmentService.js` - Department CRUD operations
- `courseService.js` - Course management
- `feeService.js` - Fee management  
- `scheduleService.js` - Schedule operations

### Service Patterns
```javascript
// Consistent service structure
class FeatureService {
  async getAll() { /* implementation */ }
  async getById(id) { /* implementation */ }
  async create(data) { /* implementation */ }
  async update(id, data) { /* implementation */ }
  async delete(id) { /* implementation */ }
}
```

## 🎨 Styling Strategy (`styles/`)

### Global Styles
- ✅ `globals.css` - CSS custom properties, base styles, utilities
- `variables.css` - Design tokens and CSS variables  
- `components.css` - Global component styles

### Component Styles
- **Co-located**: CSS files next to their components
- **CSS Modules**: Scoped styling to prevent conflicts
- **Utility Classes**: Common styles for spacing, colors, etc.

### Theme Support
- `themes/light.css` - Light theme variables
- `themes/dark.css` - Dark theme variables
- CSS custom properties for easy theme switching

## 📦 State Management Strategy

### Context Providers (`contexts/`)
- ✅ `AuthContext.js` - User authentication state
- `ThemeContext.js` - UI theme management
- `NotificationContext.js` - App-wide notifications

### Local State
- Component-specific state with `useState`
- Form state with custom `useForm` hook
- API data with `useApi` hook

## 🚀 Development Workflow

### Adding New Features
1. **Create page component** in appropriate feature folder
2. **Add service methods** for API integration
3. **Create reusable components** as needed
4. **Add custom hooks** for complex logic
5. **Update types** if using TypeScript
6. **Add constants** for configuration

### Component Development
1. Start with functional component
2. Add TypeScript types for props
3. Implement with composition patterns
4. Add CSS Module for styling
5. Create Storybook story (if applicable)
6. Write unit tests

### Best Practices
- **Single Responsibility**: One component, one purpose
- **Composition over Inheritance**: Prefer composing components
- **Props Interface**: Clear, typed component APIs
- **Error Boundaries**: Graceful error handling
- **Accessibility**: ARIA labels and semantic HTML

## 📋 Future Enhancements

### Planned Structure Additions
- `__tests__/` - Test files directory
- `stories/` - Storybook component stories
- `docs/` - Component documentation
- `locales/` - Internationalization files

### Advanced Patterns (Future)
- **Microfrontends**: Module federation setup
- **Design System**: Comprehensive component library
- **Performance**: Code splitting and lazy loading
- **PWA**: Progressive web app capabilities

---

*This structure provides a solid foundation for scaling the AYA-UIS frontend while maintaining code quality and developer productivity.*