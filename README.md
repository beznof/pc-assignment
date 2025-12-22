# Desks Reservation Web Application

## About

This is a solution for the **Present Connection** technical assignment.

The web application revolves around viewing and managing reservations for shared desks.

***

## File structure

```
├── backend                             # Backend application  
│   ├── Controllers                       # Controllers
│   ├── DTOs                              # Data Transfer Objects
│   ├── Data                              # Database definition and seeder
│   ├── Enums                             # Enumerators
│   ├── Models                            # EF Core entities
│   ├── Repositories                      # Data access layer
│   └── Services                          # Business logic 
│  
└── frontend                            # Frontend application  
    └── src
        ├── assets                        # Static files (SVGs)  
        ├── components                    # Components  
        ├── contexts                      # React contexts  
        └── routes                        # Application routes  
```

## Technologies used

- **React** (**TypeScript**, bundled via **Vite**)
    - **Tailwind CSS** for styles 
    - **Radix UI** for components
- **ASP.NET Core (.NET 9)**

***

## Prerequisites
- **.NET 9.0**
- **Node.js** (v18+)

## Usage
1. Clone the repository:
```bash
  git clone https://github.com/beznof/pc-assignment.git
```
2. Launch backend application  
    2.1. Navigate to the corresponding directory
    ```bash
      cd backend
    ```  
    2.2. Install packages
    ```bash
      dotnet restore
    ```  
    2.3. Build & run the application
    ```bash
      dotnet run
    ```  
3. Launch frontend application  
    3.1. Navigate to the corresponding directory (via separate terminal)
    ```bash
      cd frontend
    ```  
    3.2. Install packages
    ```bash
      npm install
    ```  
    3.3. Run the application
    ```bash
      npm run dev
    ```  

&nbsp;  

After completing these steps, the following services should be reachable:
- **Frontend** application: `http://localhost:5173`
- **Backend** application: `http://localhost:5273`
    - Endpoints explorable via **Swagger UI**:  
    `http://localhost:5273/swagger`

Note: Both applications are run in <ins>development</ins> mode.