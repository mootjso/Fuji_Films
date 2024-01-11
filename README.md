# ShipsCinema

## Description
ShipsCinema is a dynamic theatre application showcasing a diverse collection of movies. It provides a seamless user experience, enabling functionalities like account registration and login, movie selection, showtime scheduling, seat booking, checkout, and reservation overview.

### Key Directories ###

### ShipsCinema Folder
This primary directory houses all `.CS` and `Json` files essential for running the application.

### ShipsCinemaTest Folder
Dedicated to Unit Tests, this folder is crucial for ensuring application stability and performance.

### Essential Files
For the application to function properly, verify the presence of the following files:
- `ShipsCinema > Datasources > ads`
- `ShipsCinema > Datasources > movies`
- `ShipsCinema > Datasources > theaters`
- `ShipsCinema > Datasources > UserAccounts`

**Important Note:** The `UserAccounts` file properties is or must be set to "Copy if New." Updates to accounts are reflected only in `ShipsCinema/Bin/Debug/Net7.0/Datasources/UserAccounts`. The original `UserAccounts` file persists in its location to retain an admin account post any solution cleaning or rebuilding.

## Launching the Application
Initiate the application by opening `ShipsCinema.sln`. A default admin account is pre-configured for immediate access:
- Email: admin@shipscinema.com
- Password: Admin123

## Admin Account Creation
For assigning admin roles, follow these manual steps:
1. Add a new account in `ShipsCinema > Datasources > UserAccounts` with all necessary attributes.
2. Change the "IsAdmin" attribute from `false` to `true` in `ShipsCinema/Datasources/UserAccounts`.
3. Save changes and restart the solution to apply them.

----
