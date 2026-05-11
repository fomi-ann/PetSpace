# PetSpace

PetSpace is a prototype web-based veterinary information system designed to improve communication between pet owners, veterinarians, and clinics. The project focuses on centralized pet health management, appointment handling, and shared access to medical records.

---

# Features
- User registration and authentication

## Pet Owners
- Pet creation and management
- Shared pet ownership between multiple users
- Appointment booking
- Health history viewing
- Appointment management

## Veterinarians
- Verified veterinarian profiles
- Appointment request management
- Appointment approval/rejection
- Medical record creation
- Access to pet health history

## Clinics
- Veterinarian verification system
- Vet request management
- Clinic profile management

---

# Technologies Used

## Frontend
- React.js

## Backend
- ASP.NET Core Web API
- Entity Framework Core

## Database
- Microsoft SQL Server

---

# How to Run the Project Locally

## 0. Prerequisites
- Visual Studio 2022
- .NET 9 SDK
- Node.js
- Microsoft SQL Server
- SQL Server Management Studio

---

## 1. Clone the Repository

## 2. Configure the Database
Open appsettings.json and connect to the database using the connection string:
```
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=PetSpaceDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

## 3. Apply Database Migrations
Open Package Manager Console in Visual Studio and run:
```
Update-Database
```

## 4. Seed Test Data
The project includes seed data for testing. When the application starts, the database is filled with mock users, clinics, veterinarians, pets, appointments, and medical records. If you want to reset the test database, delete the existing database and run the project again with seed data enabled.
## 5. Run the Project
Open PetSpace.Server in the terminal and run:
```
dotnet run
```

## Test Accounts
All test accounts use the same password: Test123!
| Role      | Email                                   |
| --------- | --------------------------------------- |
| Pet Owner | owner1@test.ee |
| Pet Owner | owner2@test.ee |
| Pet Owner | owner3@test.ee |

| Role | Email                                       | Name            | Clinic              | Status       |
| ---- | ------------------------------------------- | --------------- | ------------------- | ------------ |
| Vet  | ace@test.ee          | Ace Ventura     | Happy Paws Clinic   | Verified     |
| Vet  | dolittle@test.ee | Doctor Dolittle | Happy Paws Clinic   | Verified     |
| Vet  | house@test.ee   | Gregory House   | Crazy Pets Hospital | Verified     |
| Vet  | watson@test.ee     | Dr. Watson      | Happy Paws Clinic   | Not verified |


| Role   | Email                                     | Clinic              |
| ------ | ----------------------------------------- | ------------------- |
| Clinic | clinic1@test.ee | Happy Paws Clinic   |
| Clinic | clinic2@test.ee | Crazy Pets Hospital |


