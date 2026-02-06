# ArreglaYa React Native App

This is a React Native application built with Expo and TypeScript for the ArreglaYa monorepo.

## Getting Started

### Prerequisites

- Node.js (v14 or higher)
- npm or yarn

### Installation

```bash
cd react-native-app
npm install
```

### Running the App

```bash
# Start the Expo development server
npm start

# Run on Android
npm run android

# Run on iOS (requires macOS)
npm run ios

# Run on Web
npm run web
```

## Installed Dependencies

- **Expo**: React Native framework
- **TypeScript**: Type-safe JavaScript
- **React Navigation**: Navigation library for React Native
  - `@react-navigation/native`
  - `@react-navigation/native-stack`
  - `react-native-screens`
  - `react-native-safe-area-context`
- **TanStack Query**: Data fetching and state management
  - `@tanstack/react-query`
- **Flowbite**: CSS framework
  - `flowbite`

## Project Structure

```
react-native-app/
├── App.tsx           # Main application component
├── index.ts          # Entry point
├── app.json          # Expo configuration
├── package.json      # Dependencies and scripts
├── tsconfig.json     # TypeScript configuration
└── assets/           # Images and static assets
```

## Learn More

- [Expo Documentation](https://docs.expo.dev/)
- [React Native Documentation](https://reactnative.dev/)
- [React Navigation Documentation](https://reactnavigation.org/)
- [TanStack Query Documentation](https://tanstack.com/query/latest)
- [Flowbite Documentation](https://flowbite.com/)
