# Slotify - Frontend

This is the frontend of the Slotify Appointment Management System, built with [Next.js](https://nextjs.org/).

## Tech Stack
- **Framework**: Next.js 15+ (App Router)
- **Styling**: Tailwind CSS
- **State Management**: TanStack Query (React Query)
- **Auth**: Custom JWT-based context provider
- **Icons**: Lucide React

## Getting Started

First, install dependencies:

```bash
npm install
```

Then, run the development server:

```bash
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

## Structure
- `/app`: Root layout, providers, and route groups `(auth)` and `(dashboard)`.
- `/components`: Reusable UI components.
- `/context`: Authentication context.
- `/lib`: API client and utility functions.
- `/types`: TypeScript definitions.

## Learn More
To learn more about Next.js, take a look at the [Next.js Documentation](https://nextjs.org/docs).
