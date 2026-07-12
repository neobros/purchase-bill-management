# Purchase Bill Management — Frontend

Angular frontend for the Purchase Bill Management assignment: a login page backed by
a company POS API (via the backend), and a Purchase Bill entry page.

## Stack

- Angular 19 (standalone components, signals)
- TypeScript, Reactive Forms
- SCSS with a shared design-token file (`src/_variables.scss`)

## Project structure

```
src/app/
  core/
    models/       # Auth + purchase bill TypeScript interfaces
    services/     # AuthService (session state, login API call)
    guards/       # authGuard protecting /purchase-bill
  features/
    login/            # Login page
    purchase-bill/     # Purchase Bill page + its sub-components
      components/
        item-entry-form/   # Item/Batch/Cost/Price/Qty entry row + Add
        item-table/        # Added items table
        summary-panel/     # Item/Financial/Tax summary + Net Total
  shared/
    components/
      autocomplete-input/  # Reusable autocomplete (Item field)
      select-dropdown/      # Reusable dropdown (Batch field)
```

## Setup

```bash
npm install
npm start        # ng serve, http://localhost:4200
```

Configure the backend API base URL in `src/environments/environment.ts`
(`apiBaseUrl`). The frontend currently expects the backend to expose:

- `POST /api/auth/login` — proxies the company POS API (`GetLoginData`), persists
  `Location_Details`, and returns `{ email, locations, loggedInAt }`.
- (Purchase bill submission endpoint — to be wired once the backend is available.)

## Current status

- Login page and Purchase Bill page UI are built and match the provided design.
- Purchase Bill item entry, table, and live summary/net-total calculations work
  fully client-side.
- Backend integration (real login call, saving purchase bills) is pending — the
  `AuthService` is already structured to call the backend once it's available.
