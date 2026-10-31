# RaceDay_MVC_Part3

## Project Overview

RaceDay is a race event management application developed using ASP.NET Core. The purpose of the application is to provide a central platform where event organisers can manage sporting events, while participants can browse events and take part in races.

This project forms the third part of the RaceDay development process. It builds on the RESTful Web API developed in Part 2 by introducing a user-friendly Model-View-Controller (MVC) interface.

Rather than communicating directly with the database, the MVC application uses HTTP requests to communicate with the existing RaceDay API. This keeps the application's responsibilities separate and ensures that data operations and business rules remain managed by the API.

##The application supports two types of users:
- Organisers: Responsible for creating and managing race events, configuring event categories and routes, managing participants, and recording race results.
- Participants: Able to browse available events, enrol in races, view their enrolment information, and access their results.
The aim is to provide both user types with a straightforward and accessible experience while keeping the underlying application structure organised and maintainable.
Technologies Used
The following technologies and tools are being used in the development of RaceDay Part 3:
- C# – Primary programming language.
- ASP.NET Core MVC – Used to develop the web application.
- ASP.NET Core Web API – Handles communication with the application's backend and database.
- Entity Framework Core – Used by the API for database operations.
- Microsoft SQL Server – Database management system.
- Bootstrap – Used to create a responsive and user-friendly interface.
- HTML and CSS – Used to structure and style the application's pages.
- JavaScript – Supports client-side functionality and form validation.
- Visual Studio 2026 – Development environment.
- Git and GitHub – Source control and project version management.
Development Progress
Development has been approached in stages, with individual features implemented, tested, and committed to GitHub before moving on to the next milestone.

## 1. Project Setup and API Integration
The first step involved creating a separate ASP.NET Core MVC project for Part 3.

The MVC application was configured to communicate with the existing Part 2 API through an HTTP client service. This service acts as the communication layer between the MVC controllers and the backend API.

The API's base address is stored in the application's configuration file, allowing the address to be changed without modifying the application code.

This approach ensures that the MVC application does not access the database directly or duplicate the business logic already implemented in the API.
# 2. User Authentication and Session Management
The authentication functionality was implemented using the existing login and registration endpoints provided by the Part 2 API.

When a user logs in successfully, the MVC application stores the relevant user information, including their user ID, first name, and role, in an ASP.NET Core session.

The API also maintains its own session. To preserve the authenticated state when making subsequent API requests, the MVC application securely retains the API session cookie on the server and forwards it with authenticated requests.

This allows users to navigate between different pages without having to log in repeatedly.

A logout feature was also implemented. When a user logs out, the MVC application communicates with the API's logout endpoint and clears its own session information.

# 3. Role-Based Navigation and Dashboards
RaceDay supports separate interfaces for organisers and participants.

After logging in, users are directed to the appropriate dashboard according to their assigned role.

Role-based checks are implemented in the MVC controllers to prevent users from accessing pages intended for another type of account.

The navigation menu also adapts to the signed-in user's role, helping keep the application organised and easy to navigate.

# 4. Organiser Dashboard
The organiser dashboard provides a central location from which organisers can manage their race events.

The dashboard retrieves event information directly from the API and displays the organiser's own events in individual Bootstrap cards.

Each event card currently displays:
- Event name and description.
- Event type (Run, Walk, or Cycle).
- Race distance.
- Event date.
- Venue and location information.
An empty-state message is also displayed when an organiser has not yet created any events.

This ensures that the dashboard remains informative even when there is no event data to display.

# 5. Event Creation
The event creation functionality allows organisers to add new race events through the MVC interface.

A dedicated form was developed with fields for the event name, description, date, venue, city, province, distance, and event type.

Data validation is implemented using C# data annotations. This helps prevent incomplete or invalid information from being submitted to the API.

Once the form has been validated, the MVC application sends the event information to the API, which handles the creation and database persistence.

Successful event creation redirects the organiser back to the dashboard, where the newly created event is displayed.

# 6. Event Editing
The event editing functionality builds on the event creation process by allowing organisers to update existing events.

Each event displayed on the organiser dashboard now has an Edit Event link.

When selected, the application retrieves the event information from the API and populates an editing form with the existing details.

Organisers can then make changes and submit the updated information.

The implementation includes:
- Pre-populating the form with existing event information.
- Reusing validation rules to ensure that updated information is valid.
- Sending changes to the API through an HTTP PUT request.
- Checking that the event belongs to the currently signed-in organiser.
- Handling common API errors, including unauthorised requests and unsuccessful updates.
The API remains responsible for enforcing event ownership, ensuring that organisers cannot modify events belonging to other organisers.

The complete event editing process has been tested successfully, including confirming that updated information is persisted and displayed on the dashboard.

# 7. Testing and Version Control
Each major development milestone is tested before moving forward.

Testing so far has included:
- Successful user login and logout.
- Role-based dashboard access.
- Retrieving organiser-specific events from the API.
- Creating new events and confirming that they are saved.
- Validating required fields and event information.
- Retrieving existing event details for editing.
- Updating events and confirming that changes persist.
- Building the MVC solution successfully after implementation changes.
Git and GitHub are used throughout development to track progress and maintain a history of meaningful changes. Features are committed as they are completed rather than waiting until the entire application has been developed.
