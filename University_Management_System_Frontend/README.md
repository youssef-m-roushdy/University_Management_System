# University Management System Frontend

A modern React application for the AYA University Information System, designed with scalability and maintainability in mind.

## 🏗️ Project Structure

```
src/
├── components/              # Reusable UI components
│   ├── common/             # Generic components (Button, Modal, etc.)
│   ├── layout/             # Layout components (Header, Sidebar, etc.)
│   └── forms/              # Form-specific components
├── pages/                  # Page-level components
│   ├── auth/               # Authentication pages (Login, Register)
│   ├── dashboard/          # Dashboard pages
│   ├── departments/        # Department management pages
│   ├── courses/            # Course management pages
│   ├── fees/               # Fee management pages
│   └── schedules/          # Schedule management pages
├── hooks/                  # Custom React hooks
├── services/               # API service layer
├── contexts/               # React contexts for state management
├── utils/                  # Utility functions and helpers
├── styles/                 # Global styles and themes
├── types/                  # TypeScript type definitions
├── constants/              # Application constants
└── assets/                 # Static assets
    ├── images/             # Images and graphics
    └── icons/              # Icon files
```

## 🚀 Architecture Principles

### Component Organization
- **Atomic Design**: Components organized from simple to complex
- **Feature-Based Structure**: Pages grouped by functionality
- **DRY Principle**: Reusable components in `common/` directory

### State Management
- **React Context**: For global application state
- **Custom Hooks**: For stateful logic encapsulation
- **Local State**: Component-specific state with useState

### Code Organization
- **Services Layer**: API calls abstracted into service functions
- **Type Safety**: TypeScript interfaces and types
- **Constants**: Centralized configuration and constants

## 🎯 Development Guidelines

### Naming Conventions
- **Components**: PascalCase (`UserProfile.jsx`)
- **Hooks**: camelCase with `use` prefix (`useAuth.js`)
- **Services**: camelCase with descriptive names (`authService.js`)
- **Constants**: UPPER_SNAKE_CASE (`API_BASE_URL`)

### File Structure
- Each component in its own file
- Index files for clean imports
- Co-locate styles with components when component-specific

### Code Standards
- Use functional components with hooks
- Implement proper error handling
- Follow React best practices
- Maintain consistent code formatting

## 🔧 Available Scripts

In the project directory, you can run:

### `npm start`
Runs the app in development mode. Open [http://localhost:3000](http://localhost:3000) to view it in your browser.

### `npm test`
Launches the test runner in interactive watch mode.

### `npm run build`
Builds the app for production to the `build` folder.

### `npm run lint`
Runs ESLint to check for code quality issues.

### `npm run format`
Formats code using Prettier.

## 🔗 Integration with Backend

The frontend communicates with the University Management System .NET Core API:
- **Base URL**: `http://localhost:5282` (development)
- **Authentication**: JWT token-based
- **API Format**: RESTful JSON API

### Service Layer Structure
```javascript
// Example service structure
src/
├── services/
│   ├── api.js              # Base API configuration
│   ├── authService.js      # Authentication services
│   ├── departmentService.js# Department CRUD operations
│   ├── courseService.js    # Course management services
│   ├── feeService.js       # Fee management services
│   └── scheduleService.js  # Schedule management services
```

## 🎨 UI/UX Approach

### Design System
- Consistent color palette and typography
- Reusable component library
- Responsive design principles
- Accessibility considerations

### User Experience
- Intuitive navigation
- Fast loading times
- Error handling and user feedback
- Progressive web app capabilities

## 📱 Responsive Design

The application is designed to work across devices:
- **Desktop**: Full-featured interface
- **Tablet**: Adapted layouts
- **Mobile**: Touch-optimized interface

## 🚀 Future Enhancements

### Phase 1 (Current)
- ✅ Project structure setup
- ⏳ Authentication implementation
- ⏳ Department management
- ⏳ Course management

### Phase 2 (Planned)
- Advanced reporting dashboard
- Real-time notifications
- Dark mode support
- Progressive Web App features

### Phase 3 (Future)
- Mobile app (React Native)
- Advanced analytics
- Internationalization
- Performance optimizations

## 🔧 Development Setup

1. **Prerequisites**
   ```bash
   Node.js 18+ and npm
   ```

2. **Install dependencies**
   ```bash
   npm install
   ```

3. **Environment setup**
   ```bash
   cp .env.example .env.local
   # Edit .env.local with your configuration
   ```

4. **Start development server**
   ```bash
   npm start
   ```

## 🧪 Testing Strategy

### Testing Approach
- **Unit Tests**: Component testing with React Testing Library
- **Integration Tests**: API integration testing
- **E2E Tests**: User workflow testing (planned)

### Testing Commands
```bash
npm test                    # Run all tests
npm run test:watch         # Run tests in watch mode
npm run test:coverage      # Run tests with coverage report
```

## 📦 Build and Deployment

### Production Build
```bash
npm run build              # Create production build
npm run preview            # Preview production build locally
```

### Deployment Options
- Static hosting (Netlify, Vercel)
- Docker containerization
- CI/CD pipeline integration

---

*Built with React ⚛️ for modern university management*
