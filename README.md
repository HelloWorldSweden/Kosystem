# Kösystem / Queuing service

Generic enqueuing service for letting users enqueue and have all other users
see who's first in queue.

> ![Screenshot](docs/screenshot01.png)

Useful when holding lectures by allowing participants to enter a "help queue",
where the leader/teacher can easily see who's next in queue to get help, all
digitally. This way the participants does not need to hold up their hands,
which is both tiresome and disabling the participants ability to work,
especially when their task requires two hands (such as accessing a computer).

## How to use: As a user

- You enter the room ID (ex: `#1234`).
- You enter your chosen display name.
- Once in a room, you may choose to enqueue to get into the list.
- When you've received help, you leave the queue.

## How to use: As a moderator

- You log in to the admin panel. Where you can:
  - Create new rooms.
  - Delete existing rooms.
  - Enter a room as a moderator.
- Once logged in and entered a room as a moderator, you may:
  - ~~Kick users~~ *Not yet available.*
  - ~~Rename users~~ *Not yet available.*
  - ~~Dequeue users~~ *Not yet available.*

## Screenshots

> ![Creating a room](docs/screenshot02.png)

> ![Inside an empty room, without having a name specified](docs/screenshot03.png)

> ![Entering a room](docs/screenshot04.png)

> ![Being inside a room, dequeued](docs/screenshot05.png)

## Running from Visual Studio

1. You need Visual Studio with .NET 5.0 *(or greater)*, i.e.:

   - Visual Studio 2019 16.8.0 *(or greater)*
   - Visual Studio 2019 for Mac v8.8 *(or greater)*

2. You need the following workloads: *(via the Visual Studio Installer)*

   - ASP.NET and web development

3. In the tab "Induvidual components", ensure you install the following:

   - .NET 5.0 Runtime
   - .NET SDK

4. Open the solution file `Kosystem.sln` in Visual Studio

5. Start the project named simply `Kosystem`.

   1. Right click the project `Kosystem` in the Solution Explorer
   2. Click "Set as Startup Project"
   3. Run the project. For example by clicking <kbd>F5</kbd>

## Running from command line

1. Install .NET 5.0 SDK *(or greater)*

2. Run the command

   ```sh
   # The arguments "-v m" enables minimal logging of the build process
   dotnet run -p Kosystem -v m
   ```

## How it works

Based on server-side [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
where the application sends prerendered HTML documents to the client.
When the client has finished loading their web page, the client then connects
up the the web server through [SignalR](https://dotnet.microsoft.com/apps/aspnet/signalr)
(one of the components of Blazor), to preferably via Web Sockets establish a
two way connection between the client and the server.

With said connection, the server is able to update the clients web pages
without the need for reloading the page. This allows live updates of the page
whenever a user joins a room, leaves a room, enqueues, etc.

The state of the application is currently stored via the
[SQLite Database Provider](https://docs.microsoft.com/en-us/ef/core/providers/sqlite/?tabs=dotnet-core-cli)
(for [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)) on
disk. This may be replaced with a remote server in the future to allow the
state (created rooms, enqueued users, etc.) to persist even after updates or
systematic restarts.

## Architecture

In the effort to keep the project clean, the following architecture is used:

### Kosystem/Kosystem.csproj

The Blazor part of the whole solution. It's important that as soon as some part
of this project becomes too vast, to instead extract it into a separate
project. See the other projects as examples of this.

This is just a frontend project. General rule is that this project shall not
contain any buisness logic. If you spot violations of this, please feel
inclined in suggesting ways of extracting said logic, or create pull requests
with your ideas directly.

This project is dependant (directly or indirectly) on all of the other
projects in this solution.

### Kosystem.Shared/Kosystem.Shared.csproj

Models (classes, records, structs) that are used throughout all of the
projects to pass around generic data, such as the `PersonModel` and `RoomModel`
records.

This project shall have no dependencies.

### Kosystem.Utility/Kosystem.Utility.csproj

Utilities that are not strictly related to displaying data to the frontend, nor
to calculate any buisness logic. This is a project containing solely
"nice to have" features, such as string manipulation utils.

This project shall have no dependencies, not even on the `Kosystem.Shared`
project.

### Kosystem.Events/Kosystem.Events.csproj

Event argument models as well as event listener and sender interfaces and basic
implementations. Everything regarding event handling is put in here.

This project is used to coordinate the events such as when a person joins a
room, and easy to use interfaces to subscribe to those events when needed.

This project may only be dependant on the bare projects such as
`Kosystem.Shared` and `Kosystem.Utility`. The `Kosystem.Repository` project
may not be referenced here, as that will induce a circular dependency chain.

### Kosystem.Repository/Kosystem.Repository.csproj

Mostly abstract types regarding the repositories. If you need to work with one
of the repositories (ex: the `IPersonRepository`) then use the interfaces from
this project instead of the concrete types from some of the other projects
(namely `Kosystem.Repository.EF`).

If you're writing tests, you shall create test doubles for these interfaces
instead of using the provided concrete types.

This project may depend on any project, except for frontend projects
(ex: `Kosystem`) nor any concrete implementations of the repositories
(ex: `Kosystem.Repository.EF`).

### Kosystem.Repository.EF/Kosystem.Repository.EF.csproj

A concrete implementation of the `IRoomRepository` and `IPersonRepository`
provided by the `Kosystem.Repository` project, using
[Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/).

It's important to keep this project agnostic to any database provider, such as
MySql, MsSql, Sqlite, etc. This means that no SQL queries may be written here.
The `Startup.cs` file from the `Kosystem` project is the one declaring to use
Sqlite and the migrations. This is an important distinction for us to
abstracting out the database so we have the option of switching database
provider.

This project will by definition depend on the `Kosystem.Repository` project.
Any other dependencies are allowed as well, except for a frontend project
(ex: `Kosystem`), nor any future other repository implementations (such as if
we added a `Kosystem.Repository.Dapper`).
