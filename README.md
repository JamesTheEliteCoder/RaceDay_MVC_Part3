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


## 8. Event Category Management

Category management allows organisers to configure the different race categories available within an event.

The event management page displays the categories associated with the selected event, including their category type and associated route.

Organisers can create two types of categories:

- **Age categories:** Allow participants to be grouped according to minimum and maximum age requirements.
- **Distance categories:** Allow participants to enter a race category based on the selected route distance, without requiring age restrictions.

A dedicated category creation form was developed with fields for the category name, category type, associated route, minimum age, and maximum age.

The route selection dropdown is populated using route information retrieved from the API. This ensures that organisers can associate a category with an existing route belonging to the selected event.

The API remains responsible for validating category rules and ensuring that the selected route belongs to the correct event.

Both Age and Distance category creation have been tested successfully. The resulting categories are displayed on the event management page together with their associated routes and relevant age information.

## 9. Testing and Version Control

Each major development milestone is tested before moving forward.

Testing so far has included:

- Successful user login and logout.
- Role-based dashboard access.
- Retrieving organiser-specific events from the API.
- Creating new events and confirming that they are saved.
- Validating required fields and event information.
- Retrieving existing event details for editing.
- Updating events and confirming that changes persist.
- Retrieving event routes from the API.
- Creating routes and confirming that they appear on the event management page.
- Retrieving event categories from the API.
- Creating Age categories with minimum and maximum age requirements.
- Creating Distance categories without age restrictions.
- Confirming that categories are associated with the correct event routes.
- Building the MVC solution successfully after implementation changes.
- Retrieving event enrolments through the API.
- Confirming that participant and category details are included in the API response.
- Displaying participant enrolments in the organiser interface.
- Confirming that enrolment dates, categories, and statuses appear correctly.
- Retrieving the signed-in participant's personal enrolments through the API.
- Confirming that the participant enrolment endpoint returns only records belonging to the authenticated participant.
- Displaying personal event entries in the MVC My Events page.
- Confirming that event names, dates, venues, categories, enrolment dates, and statuses are displayed correctly.
- Testing navigation between the participant dashboard, My Events page, and event details page.

Git and GitHub are used throughout development to track progress and maintain a history of meaningful changes. I commit features as they are completed rather than waiting until the entire application is developed.



## 10. Organiser Enrolment Management

Organisers can now view the participants who have enrolled in their events through a dedicated enrolment management page.

The functionality builds on the existing enrolment endpoint in the Part 2 API. The API response was enhanced to include participant and category information alongside the original enrolment details.

The updated API response includes:

- Enrolment ID.
- Participant ID and full name.
- Participant email address.
- Event ID.
- Selected category ID and category name.
- Enrolment date.
- Enrolment status.

The API retrieves the relevant enrolment, participant, and category information using Entity Framework Core. It also verifies that the signed-in organiser owns the event before returning its enrolments.

On the MVC side, a dedicated enrolment view model and API client method were introduced to retrieve the information.

Organisers can access the enrolment page directly from the event management page.

The page displays participants in a responsive Bootstrap table, showing their names, email addresses, selected categories, enrolment dates, and current statuses.

A participant count is also displayed, together with an informative message when an event has no enrolments.





## 11. Participant My Events

The participant interface now includes a dedicated My Events page, allowing participants to view the race events they have enrolled in.

This functionality builds on a new endpoint introduced in the Part 2 API, which retrieves enrolments belonging specifically to the currently authenticated participant.

The API uses the participant ID stored in the API session to retrieve the relevant enrolment records. It also joins the associated event and category information so that participants can see more than just their enrolment IDs.

The information returned by the API includes:

- Enrolment ID.
- Event ID and event name.
- Event date.
- Venue and city.
- Selected category name.
- Enrolment date.
- Enrolment status.

On the MVC side, a dedicated participant enrolment view model and API client method were introduced to retrieve the participant's personal enrolment information.

A new My Events page was developed to display the information using responsive Bootstrap cards. Each card shows the event details, selected category, enrolment date, and current enrolment status.

Participants can also navigate from their personal event entries to the corresponding event details page.

The participant dashboard was updated to include a dedicated My Events navigation link, making it easier for participants to access their race entries.

The API endpoint was tested successfully, returning only the enrolments belonging to the authenticated participant. The MVC implementation was also built successfully, and the My Events page was verified through the participant interface.





The complete enrolment management process has been tested successfully. Existing participant enrolment information was retrieved from the API and displayed correctly in the MVC application.


## 13. Participant Event Discovery and Details

Enhanced the participant interface with event banners retrieved from Azure Blob Storage through the Part 2 API.

### Features
- Displays available race events in a card-based layout on the Discover Events page.
- Displays each event's Azure-hosted banner image where one is available.
- Provides a placeholder when an event does not have a banner.
- Displays a larger banner on the individual Event Details page.
- Shows event information, including the event name, description, date, distance, venue and location.
- Displays the categories available for each event.
- Allows participants to enrol in their chosen event category.
- Retrieves private event banner images through the MVC image proxy, which communicates with the Part 2 API.
- Does not expose Azure Blob Storage credentials or require direct MVC access to Azure Blob Storage.

### Testing
- Verified that Community 5K and Personal Record Breaker banners display correctly on the Discover Events page.
- Verified that the corresponding banners display correctly on both Event Details pages.
- Confirmed that banner images are retrieved through the MVC image proxy.
- Confirmed that existing event information and category enrolment functionality remain operational.
- Verified that events without banners can display a placeholder instead.



## 14. Participant Profile Management

Implemented participant profile viewing, editing and profile-picture management.

### Features
- Participants can view their personal account information.
- Participants can update their first name, last name, phone number and date of birth.
- Participants can upload or replace their profile picture.
- Profile pictures are uploaded through the Part 2 API and stored in Azure Blob Storage.
- Profile-picture URLs are saved in the database.
- Private profile pictures are retrieved through authenticated API endpoints.
- Profile pictures are displayed on the participant profile page.
- Profile-picture uploads support JPEG, PNG and WebP images, with a maximum file size of 12 MB.
- Profile management is restricted to authenticated participants.

### Testing
- Successfully retrieved participant profile information through the MVC application.
- Verified profile information updates through the Part 2 API.
- Successfully uploaded profile pictures through the MVC application.
- Verified profile pictures are stored in Azure Blob Storage.
- Confirmed profile-picture URLs are persisted in the database.
- Successfully retrieved and displayed private profile pictures.
- Verified that the 10 MB upload limit is enforced.
- Confirmed that profile-picture functionality works through the API rather than direct database or Blob Storage access from MVC.

  ## 15. Docker Deployment and Containerisation

The RaceDay MVC application has been containerised using Docker, allowing it to run independently of the local Visual Studio development environment.

### Docker Configuration
- Created a multi-stage Dockerfile using the .NET 10 SDK and ASP.NET Core runtime images.
- Created a `.dockerignore` file to exclude unnecessary development and build files.
- Configured the application to listen on port 8080 inside the container.
- Configured the API base URL through a Docker environment variable.
- Used `host.docker.internal` to allow the MVC container to communicate with the Part 2 API running on the host machine.


16. Personal Findings and Experiences
Introduction
Developing the RaceDay MVC application has been one of the more challenging, but also rewarding, experiences of my programming journey. What initially seemed like a straightforward task of building an MVC application that consumed an existing API turned into a much bigger learning experience involving API integration, authentication, session management, cloud storage, Docker, interface design and troubleshooting.

Throughout the development process, I realised that building an application is about much more than simply writing code that works. It requires planning, patience, problem-solving and, perhaps most importantly, the willingness to go back and correct things when they don't work as expected.

There were times when I felt frustrated, particularly when something that seemed relatively simple took much longer than anticipated. However, each challenge presented an opportunity to understand the technologies better and become more confident in my ability to work through problems independently.
Planning and Application Architecture
One of the first important lessons I learned was the value of planning before jumping straight into development.

Having already completed Part 2, I initially thought that building the MVC application would mostly involve creating views and connecting them to the API. However, I quickly realised that I needed to understand the existing API properly before I could build anything around it.

We spent time examining the database structure, understanding the relationships between entities, reviewing the available endpoints and planning how the MVC application would communicate with the API.

This helped me appreciate the importance of separating responsibilities between different application layers. The MVC application was not supposed to communicate directly with the database or recreate the business logic already implemented in the API. Instead, it needed to act as the presentation layer through which users could interact with the existing system.

I also learned that taking the time to plan the file structure and implementation approach made the development process considerably easier. Rather than rushing ahead and creating files without understanding their purpose, I wanted to be involved in the structure and understand why each component was necessary.
API Integration and Authentication
Integrating the MVC application with the existing RaceDay API was one of the more important parts of the project.

I learned how to use an API client service to centralise HTTP communication and how to retrieve, submit, update and delete information through API endpoints. This included working with different HTTP methods and handling responses returned by the API.

Authentication was another area that required careful consideration. I needed to ensure that users could register, log in and log out while maintaining their authenticated state as they navigated through the application.

Managing sessions and preserving the API authentication information between requests was particularly important. The application also needed to distinguish between Organisers and Participants, ensuring that each role had access to the appropriate pages and functionality.

One thing I took away from this experience was that authentication is not simply about checking whether someone has logged in. It is also about managing access properly throughout the application and ensuring that users cannot access functionality intended for another role.
Azure Blob Storage Integration
Integrating Azure Blob Storage introduced me to another aspect of application development: cloud-based file storage.

The application needed to support event banner uploads and participant profile pictures. Rather than storing image files directly in the database, I implemented Azure Blob Storage through the existing API.

I learned how uploaded files could be validated, stored in a cloud storage container and associated with database records through their corresponding URLs.

Another important consideration was that the storage container was private. This meant that displaying uploaded images required the MVC application to retrieve them through the API rather than relying on publicly accessible image URLs.

Getting this integration working helped me understand how cloud storage can be incorporated into a real application and how different services can work together to provide functionality that would otherwise be difficult to manage locally.
Docker and Troubleshooting
Docker was probably one of the most frustrating parts of the development process.

I initially encountered problems with Docker Desktop and its interaction with WSL. What should have been a relatively straightforward process of building and running a container turned into a much longer troubleshooting exercise.

My WSL environment became unresponsive, and I eventually had to investigate the underlying problem, update WSL and enable the necessary Windows Subsystem for Linux feature.

Getting Docker Desktop and its WSL2 environment working again was a relief, but that was not the end of the challenges.

During container testing, I encountered another issue involving recursive client creation that resulted in a StackOverflowException. This was particularly frustrating because the application had already been working, and I now had to investigate why the containerised version was behaving differently.

After correcting the issue, rebuilding the application and testing it again, I was finally able to get the container running successfully.

This experience taught me that an application working on my development machine does not automatically mean it will work in another environment. Configuration, networking, environment variables and dependencies all need to be considered.

More importantly, I learned not to give up when something breaks, even when the problem initially seems bigger than expected.
UI/UX Design and Refinement
One of the most enjoyable stages of the project was the visual redesign of the application.

Initially, my main priority was getting the required functionality working. I wanted to make sure that users could register, log in, manage events, enrol in races, view results and update their profiles before spending too much time on the appearance of the application.

Once the functionality had been implemented and tested, I felt that the application needed something more. Although the different features worked, I wanted the overall experience to feel more like an actual race-management platform rather than a collection of ordinary web pages.

I decided on a dark, sport-performance-inspired design with an orange accent colour. The intention was to create something that felt energetic, modern and appropriate for an application centred around running, walking and cycling events.

We then worked through the application page by page, redesigning the organiser and participant interfaces. This included dashboards, event cards, event details, enrolment pages, result management, profile pages and the various forms used to create and manage events, routes and categories.

I particularly enjoyed seeing how the application gradually changed as the new styling was introduced. Features that had previously been presented in fairly basic Bootstrap layouts started to feel much more cohesive and professional.

The process also taught me that good interface design is not simply about choosing attractive colours or adding visual effects. Consistency, spacing, readability, navigation and making important information easy to identify are equally important.

I also learned the importance of preserving existing functionality during visual changes. Every redesigned page needed to retain its original model bindings, validation, form submissions and navigation behaviour.

Seeing the final result was genuinely satisfying because it felt like the application had developed an identity of its own.
Testing and Quality Assurance
Testing was an important part of the entire development process, rather than something that was only considered at the end.

After implementing individual features, I tested them to make sure they behaved as expected. This included testing authentication, role-based access, event management, route and category creation, participant enrolments, results management and profile updates.

I also tested the application after the visual redesigns to ensure that changing the appearance had not affected the underlying functionality.

One thing I learned was the importance of testing the complete user journey rather than simply checking whether an individual page loads successfully.

For example, creating an event is only one part of the process. The event must also appear correctly in the relevant dashboards, allow routes and categories to be associated with it, support participant enrolments and eventually allow results to be recorded and viewed.

Working through these scenarios gave me greater confidence in the application and helped me appreciate the importance of testing the interaction between different components.
Git and Version Control
Throughout the project, I used GitHub for version control and maintained a history of the development process through regular commits.

I learned the importance of making meaningful commits after completing individual features or significant changes. This made it easier to track progress and provided a way to recover from mistakes when necessary.

There were instances where I made changes to the wrong file or needed to undo an unintended modification. Having Git available made these situations considerably less stressful.

I also implemented a GitHub Actions workflow to automate the build process, which introduced me to the importance of continuous integration and checking that the application can build successfully outside my immediate development environment.
Lessons Learned and Final Reflection
Looking back, this project has taught me a great deal about developing an application that brings multiple technologies together.

I have gained practical experience working with ASP.NET Core MVC, REST APIs, authentication, session management, Azure Blob Storage, Docker, GitHub Actions and Bootstrap-based interface design.

However, I think the biggest lesson has been learning how to approach problems.

There were moments when I became frustrated because something wasn't working, or when I thought I had finally completed a task only to discover another issue that needed attention. There were also times when I was tired and had to step back, rethink the approach and continue later.

But I learned that making mistakes is part of the process. Understanding why something went wrong is often just as valuable as successfully implementing the feature in the first place.

I am particularly proud of the fact that I did not simply focus on meeting the minimum functional requirements. I wanted to understand the application, take ownership of the implementation and improve the overall experience wherever possible.

The visual redesign was a good example of this. It would have been easy to leave the original pages as they were once the functionality was complete, but I wanted to take the additional step of making the application feel polished and consistent.

Most importantly, this project has improved my confidence in approaching larger software development tasks. I now have a better understanding of how different components fit together, how to troubleshoot problems and why planning, testing and incremental development matter.

Overall, RaceDay has been much more than an assignment. It has been an opportunity to put what I have learned into practice, work through real development challenges and produce an application that I can genuinely be proud of.
